using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ForumsPorject.Repository.Entites
{
    [Table("categories")]
    public partial class Category
    {
        public Category()
        {
            Forums = new HashSet<Forum>();
        }

        [Key]
        [Column("categorie_id")]
        public int CategorieId { get; set; }
        [Column("titre_categorie")]
        [StringLength(255)]
        public string TitreCategorie { get; set; } = null!;
        [Column("description_categorie")]
        public string DescriptionCategorie { get; set; } = null!;

        [InverseProperty(nameof(Forum.Categorie))]
        public virtual ICollection<Forum> Forums { get; set; }
    }
}
