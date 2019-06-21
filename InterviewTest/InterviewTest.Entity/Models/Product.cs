using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterviewTest.Entity.Models
{
    [Table("Product", Schema = "Core")]
    public class Product
    {
        public Product()
        {
            CreatedDate = DateTime.Now;
        }

        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Please Enter Description")]
        [Column(TypeName = "varchar(500)")]
        public string Description { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string ImageName { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string ImagePath { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string TagResult { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}
