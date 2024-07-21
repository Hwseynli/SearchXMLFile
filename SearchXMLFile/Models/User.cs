using System;
using System.ComponentModel.DataAnnotations;

namespace SearchXMLFile.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FatherName { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        public string FullName;
    }
}