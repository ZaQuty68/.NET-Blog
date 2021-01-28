using Projekt.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekt.Models
{
    public class User : IValidatableObject
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        [Display(Name ="Username")]
        public string Username { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string Password { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(30, MinimumLength = 3)]
        [Display(Name ="Email")]
        public string Email { get; set; }
        [Required]
        [PhoneNumberAttribute]
        [StringLength(20, MinimumLength =0)]
        [Display(Name ="Phone number")]
        public string PhoneNumber { get; set; }
        public Boolean Admin { get; set; } = false;
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Comment> Comments { get; set; } = new List<Comment>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Username.Equals(Password))
            {
                yield return new ValidationResult("Username and password need to be difrent!");
            }
        }
    }
}