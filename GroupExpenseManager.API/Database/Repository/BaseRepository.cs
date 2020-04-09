using System.Threading.Tasks;
using GroupExpenseManager.API.Database.Context;
using Microsoft.Extensions.Logging;

namespace GroupExpenseManager.API.Database.Repository
{
    public class BaseRepository
    {
        private readonly ILogger _logger;
        private readonly DataContext _context;

        public BaseRepository(DataContext context, ILogger logger)
        {
            _logger = logger;
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        protected async Task<bool> SaveAll()
        {
            _logger.LogDebug("Saving database changes");
            return await _context.SaveChangesAsync() > 0;
        }

        protected void LogProperties<T>(T value)
        {
            var properties = typeof(T).GetProperties();
            foreach(var property in properties)
            {
                _logger.LogInformation($"{property.Name} : {property.GetValue(value)}");
            }
        }
    }
}