using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekt.ViewModels
{
    public class CommentViewModel
    {
        public CommentViewModel(Comment comment, User user)
        {
            this.Comment = comment;
            this.User = user;
        }
        public Comment Comment { get; set; }
        public User User { get; set; }
    }
}