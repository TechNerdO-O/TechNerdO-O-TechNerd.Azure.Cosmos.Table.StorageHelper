using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Interfaces;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Consumer
{
    public class UserRepository : IUserRepository
    {
        private readonly ITableStorage<string, UserEntity> _userTableStorage;
        public UserRepository(ITableStorage<string, UserEntity> userTableStorage)
        {
            _userTableStorage = userTableStorage;
        }
        public async Task<bool> CreateUserAsync(UserEntity entity)
        {
            var result = await _userTableStorage.CreateAsync(entity);
            return result.IsSuccess;
        }

        public async Task<bool> DeleteUserAsync(UserEntity entity)
        {
            var result = await _userTableStorage.DeleteByIdAsync(entity.Id, entity.Id);
            return result.IsSuccess;
        }

        public async Task<IEnumerable<UserEntity>> GetAllUserDetailsAsync(string query)
        {
            var result = await _userTableStorage.ReadByQueryAsync(query);
            return result.Entity;
        }

        public async Task<UserEntity> GetUserDetailsAsync(UserEntity entity)
        {
            var result = await _userTableStorage.ReadByIdAsync(entity.Id, entity.Id);
            return result.Entity;
        }

        public async Task<bool> UpdateUserAsync(UserEntity entity)
        {
            var result = await _userTableStorage.Update(entity);
            return result.IsSuccess;
        }
    }
}
