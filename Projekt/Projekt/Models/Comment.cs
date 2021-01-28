using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(2555)]
        [MinLength(3)]
        [Required]
        [Display(Name ="Comment content")]
        public string Content { get; set; }
        [Display(Name ="Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }
        [Display(Name = "User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}