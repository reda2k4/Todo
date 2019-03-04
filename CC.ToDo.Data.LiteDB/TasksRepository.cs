using CC.ToDo.Domain;
using System.Threading.Tasks;
using LiteDB;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CC.ToDo.Data.LiteDB
{
    public interface IUserTaskRepository
    {
        Task AddAsync(ToDoTask task);
        Task UpdateAsync(ToDoTask task);
        Task<IEnumerable<ToDoTask>> GetToDoListAsync(Guid userId);
        Task<User> GetUserAsync(Guid userId);
        Task StoreAsync(User user);
    }

    public class UserTaskRepository : IUserTaskRepository
    {
        private LiteRepository _repository;

        public UserTaskRepository(LiteRepository repository)
        {
            _repository = repository;
        }
        public async Task AddAsync(ToDoTask task)
        {
            await Task.Run(() => _repository.Insert(task));
        }

        public async Task UpdateAsync(ToDoTask task)
        {
            await Task.Run(() => _repository.Update(task));
        }

        public async Task<IEnumerable<ToDoTask>> GetToDoListAsync(Guid userId)
        {
            var user = await Task.Run(() => _repository.Fetch<User>(x => x.Id == userId).FirstOrDefault());

            return user?.Tasks;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            var user = await Task.Run(() => _repository.Fetch<User>(x => x.Id == userId).FirstOrDefault());

            return user;
        }

        public async Task StoreAsync(User user)
        {
            await Task.Run(() => _repository.Upsert(user));
        }
    }

}
