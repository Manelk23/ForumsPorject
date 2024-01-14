using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ForumsPorject.Repository.Entites
{
    [Table("forums")]
    [Index(nameof(Categorieid), Name = "IX_forums_categorieid")]
    public partial class Forum
    {
        public Forum()
        {
            Themes = new HashSet<Theme>();
        }

        [Key]
        [Column("forum_id")]
        public int ForumId { get; set; }
        [Column("titre_forum")]
        [StringLength(255)]
        public string TitreForum { get; set; } = null!;
        [Column("dateCreation_forum", TypeName = "date")]
        public DateTime DateCreationForum { get; set; }
        [Column("discription_forum")]
        public string DiscriptionForum { get; set; } = null!;
        [Column("categorieid")]
        public int Categorieid { get; set; }

        [ForeignKey(nameof(Categorieid))]
        [InverseProperty(nameof(Category.Forums))]
        public virtual Category Categorie { get; set; } = null!;
        [InverseProperty(nameof(Theme.Forum))]
        public virtual ICollection<Theme> Themes { get; set; }
    }
}
