using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface IDataContext : IObjectContextAdapter, IDisposable
    {
        IDbSet<ProductTypeComparison> ProductTypeComparisons { get; set; }
        IDbSet<VoltageComparison> VoltageComparisons { get; set; }
        IDbSet<LevelComparison> LevelComparisons { get; set; }
        IDbSet<PitchComparison> PitchComparisons { get; set; }
        IDbSet<Sample> Samples { get; set; }
        IDbSet<SampleReview> SampleReviews { get; set; }
        IDbSet<ApplyExpense> ApplyExpenses { get; set; }
        IDbSet<ApplyExpenseType> ApplyExpenseTypes { get; set; }
        IDbSet<FaPiao> FaPiaos { get; set; }
        IDbSet<Finance> Finances { get; set; }
        IDbSet<PayPeriod> PayPeriods { get; set; }
        IDbSet<Role> Roles { get; set; }
        IDbSet<OrderReview> OrderReviews { get; set; }
        IDbSet<AuditLevel> AuditLevels { get; set; }
        IDbSet<Order> Orders { get; set; }
        IDbSet<CustomerType> CustomerTypes { get; set; }
        IDbSet<Account> Accounts { get; set; }
        IDbSet<LeftNavigation> LeftNavigations { get; set; }
        IDbSet<PositionCustomer> PositionCustomers { get; set; }
        IDbSet<Customer> Customers { get; set; }
        IDbSet<Employee> Employees { get; set; }
        IDbSet<EmployeePostion> EmployeePostions { get; set; }
        IDbSet<Enquiry> Enquiries { get; set; }
        IDbSet<Position> Positions { get; set; }
        IDbSet<Product> Products { get; set; }
        //IDbSet<ProductType> ProductTypes { get; set; }
        IDbSet<StandardCost> StandardCosts { get; set; }
       
        int SaveChanges();
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
