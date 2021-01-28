using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekt.ViewModels
{
    public class PostViewModel
    {
        public PostViewModel(Post post, User user, List<CommentViewModel> comments)
        {
            this.Post = post;
            this.Comments = comments;
            this.User = user;

        }
        public Post Post { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public User User { get; set; }
        public Comment Comment { get; set; }
    }
}