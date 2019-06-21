using InterviewTest.Entity;
using InterviewTest.Entity.Models;
using InterviewTest.Models;
using InterviewTest.BLL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using InterviewTest.BLL;
using InterviewTest.BLL.AppConfig;
using InterviewTest.BLL.HelperClasses;
using System;
using System.Diagnostics;
using System.IO;

namespace InterviewTest.Controllers
{
    public class HomeController : Controller
    {
        private DataContextHelper _context;
        IHostingEnvironment env;
        private NumberToWord objNumberToWord;
        private string _externalKey;
        public HomeController(IHostingEnvironment _env)
        {
            this._context = new DataContextHelper();
            env = _env;
            this.objNumberToWord = new NumberToWord();
            _externalKey = AppConfiguration.GetConfiguration("ExternalKey");
        }

        /// <summary>
        /// Index method for Initial Page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// It's method runs on ajax call from view, It store Image on Azure storage and in Database and returned with Image URL.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ImageTagResponse Upload()
        {
            ImageTagResponse responseModel = new ImageTagResponse();
            try
            {
                var files = Request.Form.Files;
                if (files == null && files.Count <= 0)
                    return null;

                var extension = Path.GetExtension(files[0].FileName);
                var fileName = string.Format(@"{0}" + extension, Guid.NewGuid());
                string mimeType = files[0].ContentType;
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    files[0].CopyTo(ms);
                    fileBytes = ms.ToArray();
                }

                BlobStorageService objBlobService = new BlobStorageService();
                Image image = new Image
                {
                    Path = objBlobService.UploadFileToBlob(fileName, fileBytes, mimeType),
                    Name = fileName,
                    CreateDate = DateTime.Now,
                    LastModified = DateTime.Now,
                    ExternalKey = _externalKey
                };

                string tagResult = objBlobService.GenerateTags(image.Path);
                image.Tags = tagResult;
                _context.ImageRepo.Add(image);
                _context.SaveChanges();
                var resultId = image.ImageId;

                if (!string.IsNullOrWhiteSpace(tagResult))
                    responseModel = JsonConvert.DeserializeObject<ImageTagResponse>(tagResult);
                if (responseModel != null)
                    responseModel.ImageId = resultId;

            }
            catch (Exception ex)
            {
                Logger.writelog(ex.Message);
                responseModel = null;
            }
            return responseModel;
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
