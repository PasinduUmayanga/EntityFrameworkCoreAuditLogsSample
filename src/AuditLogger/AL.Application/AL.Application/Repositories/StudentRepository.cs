using AL.Application.Repositories.Interfaces;
using AL.Infrastructure.Persistance.Models;

namespace AL.Application.Repositories
{
    public class StudentRepository : BaseRepository, IStudentRepository
    {

        public StudentRepository(AuditLogDbContext db) : base(db) { }
        public void SaveStudentAuto()
        {
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    _db.Students.Add(new Student()
                    {
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = "Test user",
                        Name = string.Format("user - {0}", i.ToString())
                    });
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
