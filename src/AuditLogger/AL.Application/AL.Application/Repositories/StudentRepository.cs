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
                for (int i = 0; i < 1000; i++)
                {
                    _db.Students.Add(new Student()
                    {
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = "Test",
                        Name = string.Format("user - {0}", i.ToString())
                    });

                    Student student = _db.Students.FirstOrDefault(x => x.Id == i);
                    if (student != null && i % 5 == 0)
                    {

                        student.Name = "test";
                    }


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
