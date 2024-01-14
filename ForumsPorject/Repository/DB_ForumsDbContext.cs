using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ForumsPorject.Repository.Entites;

namespace ForumsPorject.Repository
{
    public partial class DB_ForumsDbContext : DbContext
    {
        public DB_ForumsDbContext()
        {
        }

        public DB_ForumsDbContext(DbContextOptions<DB_ForumsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppRole> AppRoles { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Discussion> Discussions { get; set; } = null!;
        public virtual DbSet<Forum> Forums { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Theme> Themes { get; set; } = null!;
        public virtual DbSet<Utilisateur> Utilisateurs { get; set; } = null!;
        public virtual DbSet<UtilisateurRole> UtilisateurRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=PC-MANEL;Initial Catalog=DB_Forums;Integrated Security=True ; Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategorieId)
                    .HasName("categorie_pk");
            });


            

            modelBuilder.Entity<Discussion>(entity =>
            {
                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.Discussions)
                    .HasForeignKey(d => d.Themeid)
                    .HasConstraintName("FK_discussions_themes")
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.Discussions)
                    .HasForeignKey(d => d.Utilisateurid)
                    .HasConstraintName("FK_discussions_utilisateurs")
                     .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Forum>(entity =>
            {
                entity.HasOne(d => d.Categorie)
                    .WithMany(p => p.Forums)
                    .HasForeignKey(d => d.Categorieid)
                    .HasConstraintName("forum_categorie_id_fk")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MessagesId)
                    .HasName("message_pk");

                entity.HasOne(d => d.Auteur)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.AuteurId)
                    .HasConstraintName("message_auteur_id_fk")
                 .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Discussion)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.Discussionid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("message_discussion_id_fk")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Theme>(entity =>
            {
                entity.HasOne(d => d.Forum)
                    .WithMany(p => p.Themes)
                    .HasForeignKey(d => d.Forumid)
                    .HasConstraintName("thème_forum_id_fk")
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(t => t.Discussions)
                    .WithOne(d => d.Theme)
                    .HasForeignKey(d => d.Themeid)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Utilisateur>(entity =>
            {
                entity.HasKey(e => e.UtilisateurId).HasName("user_pk");

                entity.Property(e => e.Actif).HasDefaultValueSql("((0))");

                entity.HasMany(e => e.UtilisateurRoles)
                   .WithOne(ur => ur.Utilisateur)
                   .HasForeignKey(ur => ur.UtilisateurID)
                   .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<UtilisateurRole>(entity =>
            {
                entity.HasKey(e => new { e.UtilisateurID, e.AppRoleId });

                entity.HasOne(e => e.Utilisateur)
                    .WithMany(u => u.UtilisateurRoles)
                    .HasForeignKey(e => e.UtilisateurID);

                entity.HasOne(e => e.AppRole)
                    .WithMany(ar => ar.UtilisateurRoles)
                    .HasForeignKey(e => e.AppRoleId);
            });

            modelBuilder.Entity<AppRole>(entity =>
            {
                entity.HasKey(e => e.AppRoleId).HasName("PK__AppRoles__E66DD698AA10D3CE");
                entity.Property(e => e.SimpleRole).HasMaxLength(255).IsRequired();
                entity.Property(e => e.ManagerRole).HasMaxLength(255).IsRequired();
                entity.HasMany(e => e.UtilisateurRoles)
                        .WithOne(ur => ur.AppRole)
                        .HasForeignKey(ur => ur.AppRoleId)
                        .IsRequired();
            });





            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
