using System;
using System.Data.Entity;
using Nicole.Library.Models;
using Nicole.Library.Models.Interfaces;
using Nicole.Library.Services;


namespace Nicole.Test
{

    public class CodeFirstDbContext : DbContext, IDataContext
    {
        public IDbSet<OrderDetail> OrderDetails { get; set; }
        public IDbSet<ProductTypeComparison> ProductTypeComparisons { get; set; }
        public IDbSet<VoltageComparison> VoltageComparisons { get; set; }
        public IDbSet<LevelComparison> LevelComparisons { get; set; }
        public IDbSet<PitchComparison> PitchComparisons { get; set; }
        public IDbSet<SampleReview> SampleReviews { get; set; }
        public IDbSet<Sample> Samples { get; set; }
        public IDbSet<ApplyExpense> ApplyExpenses { get; set; }
        public IDbSet<ApplyExpenseType> ApplyExpenseTypes { get; set; }
        public IDbSet<FaPiao> FaPiaos { get; set; }
        public IDbSet<Finance> Finances { get; set; }
        public IDbSet<PayPeriod> PayPeriods { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<OrderReview> OrderReviews { get; set; }
        public IDbSet<AuditLevel> AuditLevels { get; set; }
        public IDbSet<Order> Orders { get; set; }
        public IDbSet<Account> Accounts { get; set; }
        public IDbSet<CustomerType> CustomerTypes { get; set; }
        public IDbSet<LeftNavigation> LeftNavigations { get; set; }
        public IDbSet<PositionCustomer> PositionCustomers { get; set; }

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
