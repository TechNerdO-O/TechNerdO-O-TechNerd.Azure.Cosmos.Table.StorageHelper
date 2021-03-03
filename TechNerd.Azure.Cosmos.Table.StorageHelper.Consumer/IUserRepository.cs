using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Consumer
{
    public interface IUserRepository
    {
        Task<bool> DeleteUserAsync(UserEntity entity);
        Task<bool> UpdateUserAsync(UserEntity entity);
        Task<bool> CreateUserAsync(UserEntity entity);
        Task<UserEntity> GetUserDetailsAsync(UserEntity entity);

        Task<IEnumerable<UserEntity>> GetAllUserDetailsAsync(string query);
    }
}
