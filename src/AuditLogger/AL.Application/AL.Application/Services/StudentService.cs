using AL.Application.Repositories.Interfaces;
using AL.Application.Services.Interfaces;

namespace AL.Application.Services
{
    public class StudentService : IStudentService
    {
        private IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository) { this._studentRepository = studentRepository; }
        public void SaveStudentAuto()
        {
            try
            {

                _studentRepository.SaveStudentAuto();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
