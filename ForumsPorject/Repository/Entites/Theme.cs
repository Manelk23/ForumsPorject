using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ForumsPorject.Repository.Entites
{
    [Table("themes")]
    [Index(nameof(Forumid), Name = "IX_themes_forumid")]
    public partial class Theme
    {
        public Theme()
        {
            Discussions = new HashSet<Discussion>();
        }

        [Key]
        [Column("theme_id")]
        public int ThemeId { get; set; }
        [Column("titre_theme")]
        [StringLength(255)]
        public string TitreTheme { get; set; } = null!;
        [Column("dateCreation_theme", TypeName = "date")]
        public DateTime DateCreationTheme { get; set; }
        [Column("forumid")]
        public int Forumid { get; set; }

        [ForeignKey(nameof(Forumid))]
        [InverseProperty("Themes")]
        public virtual Forum Forum { get; set; } = null!;
        [InverseProperty(nameof(Discussion.Theme))]
        public virtual ICollection<Discussion> Discussions { get; set; }
    }
}
