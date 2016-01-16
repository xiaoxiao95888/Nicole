namespace Nicole.Service
{
    public class BaseService
    {
        public readonly NicoleDataContext DbContext;

        public BaseService(NicoleDataContext dbContext)
        {
            DbContext = dbContext;
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
