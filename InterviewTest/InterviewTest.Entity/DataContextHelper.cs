using InterviewTest.Entity.Abstractions;
using InterviewTest.Entity.DataContext;
using InterviewTest.Entity.Implementation;
using Microsoft.EntityFrameworkCore;

namespace InterviewTest.Entity
{
    public class DataContextHelper : IDataContextHelper
    {
        private DbContext db;
        public DataContextHelper()
        {
            db = new EFDBContext();
        }

        private IImageRepository _ImageRepo;
        public IImageRepository ImageRepo
        {
            get
            {
                if (_ImageRepo == null)
                    _ImageRepo = new ImageRepository(db);

                return _ImageRepo;
            }
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }
    }
}
