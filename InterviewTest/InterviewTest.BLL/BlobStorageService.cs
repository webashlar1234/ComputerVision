using InterviewTest.BLL.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using InterviewTest.BLL.AppConfig;
using InterviewTest.BLL.HelperClasses;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace InterviewTest.BLL
{
    public class BlobStorageService
    {
        string accessKey = string.Empty;
        string container = string.Empty;
        public BlobStorageService()
        {
            this.accessKey = AppConfiguration.GetConfiguration("DefaultBlobConnection");
            this.container = AppConfiguration.GetConfiguration("ContainerKey");
        }

        /// <summary>
        /// Uploads files to Azure storage
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="fileData"></param>
        /// <param name="fileMimeType"></param>
        /// <returns></returns>
        public string UploadFileToBlob(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                var _task = Task.Run(() => this.UploadFileToBlobAsync(strFileName, fileData, fileMimeType));
                _task.Wait();
                string fileUrl = _task.Result;
                return fileUrl;
            }
            catch (Exception ex)
            {
                Logger.writelog(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// Deletes Image from Azure storage
        /// </summary>
        /// <param name="fileUrl"></param>
        public async void DeleteBlobData(string fileUrl)
        {
            try
            {
                Uri uriObj = new Uri(fileUrl);
                string BlobName = Path.GetFileName(uriObj.LocalPath);

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                string strContainerName = container;
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

                string pathPrefix = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/";
                CloudBlobDirectory blobDirectory = cloudBlobContainer.GetDirectoryReference(pathPrefix);
                // get block blob refarence  
                CloudBlockBlob blockBlob = blobDirectory.GetBlockBlobReference(BlobName);

                // delete blob from container      
                await blockBlob.DeleteAsync();
            }
            catch (Exception ex)
            {
                Logger.writelog(ex.Message);
            }
        }

        private async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                string strContainerName = container;
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
                string fileName = strFileName;

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                if (fileName != null && fileData != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                    cloudBlockBlob.Properties.ContentType = fileMimeType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }
                return "";
            }
            catch (Exception ex)
            {
                Logger.writelog(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// This method generates Tags for the Image
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string GenerateTags(string filename)
        {
            try
            {
                string resStr = string.Empty;
                ImageTagResponse response = null;
                var uri = AppConfiguration.GetConfiguration("ApiUrl");
                var imgUrl = filename;
                var objRequest = new
                {
                    url = imgUrl
                };
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AppConfiguration.GetConfiguration("ApiKey"));
                    StringContent content = new StringContent(JsonConvert.SerializeObject(objRequest), Encoding.UTF8, "application/json");
                    // HTTP POST
                    HttpResponseMessage resMsg = client.PostAsync(uri, content).Result;

                    if (resMsg.IsSuccessStatusCode)
                    {
                        resStr = resMsg.Content.ReadAsStringAsync().Result;
                        response = JsonConvert.DeserializeObject<ImageTagResponse>(resStr);
                    }
                }
                return resStr;
            }
            catch (Exception ex)
            {
                Logger.writelog(ex.Message);
                return "";
            }
        }
    }
}
