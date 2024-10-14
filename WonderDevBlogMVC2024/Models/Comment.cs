﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Enums;

namespace WonderDevBlogMVC2024.Models
{
    
    public class Comment
    {
        public int Id { get; set; }
        public string? AuthorId { get; set; }
        public int PostId {  get; set; }
        public string? ModeratorId { get; set; }
        public string? CommentatorId { get; set; }

        [Display(Name ="Comment")]
        public string? Body {  get; set; }

        [Display(Name = "Created Date")]
        public DateTime Created { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime? Updated { get; set; }


        [Display(Name = "Date Moderated")]
        public DateTime? Moderated { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? Deleted { get; set; }

        [Display(Name = "Moderated Comment")]
        public string? ModeratedBody { get; set; }

        public ModerationReason ModerationReason { get; set; }


        //Navigation properties
        public virtual Post? Post { get; set; }

        public virtual ApplicationUser? Author { get; set; }

        public virtual ApplicationUser? Commentator { get; set; }
        public virtual ApplicationUser? Moderator { get; set; }

        


    }
}
