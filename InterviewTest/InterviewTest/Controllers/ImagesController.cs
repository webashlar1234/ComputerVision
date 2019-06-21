using InterviewTest.Entity;
using InterviewTest.Entity.DataContext;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InterviewTest.Controllers
{
    public class ImagesController : Controller
    {
        private DataContextHelper _context;

        public ImagesController(EFDBContext context)
        {
            this._context = new DataContextHelper();
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            return View(_context.ImageRepo.GetAll());
        }

        // GET: Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = _context.ImageRepo.GetById(id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }
      
        private bool ImageExists(int id)
        {
            var Image = _context.ImageRepo.GetById(id);
            if(Image != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
