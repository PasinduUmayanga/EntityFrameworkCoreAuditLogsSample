

using AL.Infrastructure.Persistance.Models;

namespace AL.Application.Repositories
{
    public class BaseRepository
    {
        protected readonly AuditLogDbContext _db;
        public BaseRepository(AuditLogDbContext db)
        {
            _db = db;
        }
    }
}
