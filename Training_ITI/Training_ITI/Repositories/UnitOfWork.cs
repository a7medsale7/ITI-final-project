using Training_ITI.Data;
using Training_ITI.Models;

namespace Training_ITI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGenericRepository<Course> Courses { get; }
        public IGenericRepository<Session> Sessions { get; }
        public IGenericRepository<User> Users { get; }
        public IGenericRepository<Grade> Grades { get; }

        public UnitOfWork(
            AppDbContext context,
            IGenericRepository<Course> courses,
            IGenericRepository<Session> sessions,
            IGenericRepository<User> users,
            IGenericRepository<Grade> grades)
        {
            _context = context;
            Courses = courses;
            Sessions = sessions;
            Users = users;
            Grades = grades;
        }

        public Task SaveAsync() => _context.SaveChangesAsync();
    }
}
