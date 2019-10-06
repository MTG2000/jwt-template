using System.Threading.Tasks;
using JwtTemplate.Domain.Models;

namespace JwtTemplate.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<string> LoginUser(User user);
    }
}