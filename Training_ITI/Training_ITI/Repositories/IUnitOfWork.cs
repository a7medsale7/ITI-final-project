using Training_ITI.Models;

namespace Training_ITI.Repositories
{
    public interface IUnitOfWork
    {
        IGenericRepository<Course> Courses { get; }
        IGenericRepository<Session> Sessions { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<Grade> Grades { get; }
        Task SaveAsync();
    }
}
