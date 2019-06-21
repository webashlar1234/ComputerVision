using InterviewTest.Entity.Abstractions;
using InterviewTest.Entity.DataContext;
using InterviewTest.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewTest.Entity.Implementation
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        private EFDBContext context
        {
            get
            {
                return db as EFDBContext;
            }
        }

        public ImageRepository(DbContext db)
        {
            this.db = db;
        }
    }
    
}
