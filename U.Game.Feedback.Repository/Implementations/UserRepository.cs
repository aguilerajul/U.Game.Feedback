using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using U.Game.Feedback.Domain.Contracts;
using U.Game.Feedback.Domain.Entities;

namespace U.Game.Feedback.Repository.Implementations
{
    public class UserRepository : IRepositoryBase<User>
    {
        private readonly RepositoryDbContext context;

        public UserRepository(RepositoryDbContext context)
        {
            this.context = context;
        }

        public async Task<ActionResultMessage> AddAsync(User data)
        {
            var existingUser = await GetFilteredAsync(u => u.Email.Equals(data.Email) || u.NickName.Equals(data.NickName));
            if (existingUser != null)
                return new ActionResultMessage(System.Net.HttpStatusCode.InternalServerError, "This user is already registered in our database.");
            
            var newUser = new User(data.Id, data.Name, data.NickName, data.Email);
            this.context.Users.Attach(newUser);
            await this.context.SaveChangesAsync();

            return new ActionResultMessage(System.Net.HttpStatusCode.OK, $"User Created, Id: {newUser.Id}");
        }

        public async Task<IEnumerable<User>> GetFilteredListAsync(Func<User, bool> filter, int? totalRecords = 15)
        {
            return await Task.Run(() => this.context.Users.Where(filter).Take(totalRecords.Value));
        }

        public async Task<IEnumerable<User>> GetListAsync(int totalRecords = 15)
        {
            return await Task.Run(() => this.context.Users.Take(totalRecords));
        }

        public async Task<User> GetFilteredAsync(Func<User, bool> filter)
        {
            return await Task.Run(() => this.context.Users.FirstOrDefault(filter));
        }

        public User Get(Guid id)
        {
            return this.context.Users.Find(id);
        }
    }
}
