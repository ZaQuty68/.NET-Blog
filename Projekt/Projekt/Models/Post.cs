using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class Post
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        [MinLength(3)]
        [Required]
        [Display(Name ="Title")]
        public string Title { get; set; }
        [MaxLength(2555)]
        [MinLength(3)]
        [Required]
        [Display(Name ="Post content")]
        public string Content { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        [Display(Name ="User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}