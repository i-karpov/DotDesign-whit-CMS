using DotDesign.Domain.Entities;
using System;
using System.Data.Entity;

namespace DotDesign.DataLayer
{
    public class DotDesignCmsContext : DbContext, IDisposable
    {
        public DbSet<Admin> Admins
        {
            get;
            set;
        }

        public DbSet<Page> Pages
        {
            get;
            set;
        }
        
        /// <summary>
        /// Must contain 1 row as maximum, that indicates
        /// wich Page should be treated as Home Page.
        /// </summary>
        public DbSet<HomePage> HomePages
        {
            get;
            set;
        }

        public DbSet<Category> Categories
        {
            get;
            set;
        }

        public DbSet<Image> Images
        {
            get;
            set;
        }

        public DbSet<Subscriber> Subscribers
        {
            get;
            set;
        }

        public DbSet<PageLargeSlideshowImage> PageLargeSlideshowImages
        {
            get;
            set;
        }

        public DbSet<PageSmallSlideshowImage> PageSmallSlideshowImages
        {
            get;
            set;
        }

        public DbSet<TwitterUpdate> TwitterUpdates
        {
            get;
            set;
        }

        public DotDesignCmsContext(String connectionString)
            : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigureAdmins(modelBuilder);
            ConfigurePages(modelBuilder);
            ConfigureCategories(modelBuilder);
            ConfigureImages(modelBuilder);
            ConfigurePageLargeSlideshowImages(modelBuilder);
            ConfigurePageSmallSlideshowImages(modelBuilder);
            ConfigureTwitterUpdates(modelBuilder);
            ConfigureHomePages(modelBuilder);
            ConfigureSubscribers(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureAdmins(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().Property(a => a.Username).IsRequired();
            modelBuilder.Entity<Admin>().Property(a => a.Username).HasMaxLength(100);
            modelBuilder.ComplexType<AdminProfile>();
            modelBuilder.ComplexType<AdminProfile>().Property(ap => ap.FirstName).HasMaxLength(100);
            modelBuilder.ComplexType<AdminProfile>().Property(ap => ap.LastName).HasMaxLength(100);
            modelBuilder.ComplexType<AdminProfile>().Property(ap => ap.PhoneNumber).HasMaxLength(100);
            modelBuilder.ComplexType<AdminProfile>().Property(ap => ap.Email).HasMaxLength(320);
        }

        private void ConfigureHomePages(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HomePage>().
                HasRequired(hp => hp.Page).
                WithOptional().
                WillCascadeOnDelete();
        }

        private void ConfigureSubscribers(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscriber>().Property(s => s.Name).IsRequired();
            modelBuilder.Entity<Subscriber>().Property(s => s.Name).HasMaxLength(100);
            modelBuilder.Entity<Subscriber>().Property(s => s.Email).IsRequired();
            modelBuilder.Entity<Subscriber>().Property(s => s.Email).HasMaxLength(100);
            modelBuilder.Entity<Subscriber>().Property(s => s.CreateDate).IsRequired();
        }

        private void ConfigurePageLargeSlideshowImages(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PageLargeSlideshowImage>().
                HasRequired(pi => pi.Image).
                WithMany(i => i.PageLargeSlideshowImages).
                HasForeignKey(pi => pi.ImageId);
            modelBuilder.Entity<PageLargeSlideshowImage>().
                HasRequired(pi => pi.Page).
                WithMany(p => p.PageLargeSlideshowImages).
                HasForeignKey(pi => pi.PageId);
        }

        private void ConfigurePageSmallSlideshowImages(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PageSmallSlideshowImage>().
                HasRequired(pi => pi.Image).
                WithMany(i => i.PageSmallSlideshowImages).
                HasForeignKey(pi => pi.ImageId);
            modelBuilder.Entity<PageSmallSlideshowImage>().
                HasRequired(pi => pi.Page).
                WithMany(p => p.PageSmallSlideshowImages).
                HasForeignKey(pi => pi.PageId);
        }

        private void ConfigurePages(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Page>().Property(p => p.CreateDate).IsRequired();
            modelBuilder.Entity<Page>().Property(p => p.Url).IsRequired();
            modelBuilder.Entity<Page>().Property(p => p.Url).HasMaxLength(100);
            modelBuilder.Entity<Page>().Property(p => p.Title).IsRequired();
            modelBuilder.Entity<Page>().Property(p => p.Title).HasMaxLength(100);
        }

        private void ConfigureCategories(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().Property(c => c.Url).IsRequired();
            modelBuilder.Entity<Category>().Property(c => c.Url).HasMaxLength(100);
            modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<Category>().Property(c => c.Name).HasMaxLength(100);
            modelBuilder.Entity<Category>().Property(c => c.CreateDate).IsRequired();
            modelBuilder.Entity<Category>().Property(c => c.IsPublic).IsRequired();
        }

        private void ConfigureTwitterUpdates(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TwitterUpdate>().Property(tu => tu.Text).IsRequired();
            modelBuilder.Entity<TwitterUpdate>().Property(tu => tu.CreateDate).IsRequired();
        }

        private void ConfigureImages(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>().Property(i => i.Url).IsRequired();
            modelBuilder.Entity<Image>().Property(i => i.Url).HasMaxLength(100);
            modelBuilder.Entity<Image>().Property(i => i.ServerFilePath).IsRequired();
            modelBuilder.Entity<Image>().Property(i => i.CreateDate).IsRequired();
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }
    }
}
