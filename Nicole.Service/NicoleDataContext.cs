using System;
using System.Data.Entity;
using Nicole.Library.Models;
using Nicole.Library.Models.Interfaces;
using Nicole.Library.Services;

namespace Nicole.Service
{    
    public class NicoleDataContext : DbContext, IDataContext
    {
        public NicoleDataContext()
            : base("NicoleDataContext")
        {
        }
        public IDbSet<Account> Accounts { get; set; }
        public IDbSet<LeftNavigation> LeftNavigations { get; set; }       

        public IDbSet<Customer> Customers { get; set; }

        public IDbSet<Employee> Employees { get; set; }

        public IDbSet<EmployeePostion> EmployeePostions { get; set; }

        public IDbSet<Enquiry> Enquiries { get; set; }

        public IDbSet<Position> Positions { get; set; }

        public IDbSet<Product> Products { get; set; }

        public IDbSet<StandardCost> StandardCosts { get; set; }

        IDbSet<TEntity> IDataContext.Set<TEntity>()
        {
            return this.Set<TEntity>();
        }

        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries<IDtStamped>();

            foreach (var dtStamped in entities)
            {
                if (dtStamped.State == EntityState.Added)
                {
                    dtStamped.Entity.CreatedTime = DateTime.Now;
                    dtStamped.Entity.UpdateTime = DateTime.Now;
                }

                if (dtStamped.State == EntityState.Modified)
                {
                    dtStamped.Entity.UpdateTime = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new CategoryMapping());
            //modelBuilder.Configurations.Add(new AvatarMapping());
            //modelBuilder.Configurations.Add(new LetterMapping());
            //modelBuilder.Configurations.Add(new RetailerMapping());
            //modelBuilder.Configurations.Add(new SiteMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
   
}
