using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using U.Game.Feedback.Domain.Contracts;
using U.Game.Feedback.Domain.Entities;

namespace U.Game.Feedback.Repository.Implementations
{
    public class UserFeedbackRepository : IRepositoryBase<UserFeedback>
    {
        private readonly RepositoryDbContext context;

        public UserFeedbackRepository(RepositoryDbContext context)
        {
            this.context = context;
        }

        public async Task<ActionResultMessage> AddAsync(UserFeedback data)
        {
            dynamic result = new System.Dynamic.ExpandoObject();

            if (data?.User == null || data?.User.Id == Guid.Empty)
                return new ActionResultMessage(System.Net.HttpStatusCode.InternalServerError, "The User Id field cannot be empty.");

            var userFeedbackWithSameSessionId = await GetFilteredAsync(uf => uf.SessionId.Equals(data.SessionId) && uf.User.Id.Equals(data.User.Id));
            if (userFeedbackWithSameSessionId != null)
                return new ActionResultMessage(System.Net.HttpStatusCode.InternalServerError,
                    "Thank you, but you alaready submited a previous feedback, We will revievew it and give you an answer soon.");

            var newUserFeedback = new UserFeedback(data.Id, data.User, data.SessionId, data.Rating, data.Comments);
            this.context.Users.Attach(data.User);
            this.context.UserFeedbacks.Attach(newUserFeedback);

            await this.context.SaveChangesAsync();

            return new ActionResultMessage(System.Net.HttpStatusCode.OK, $"Feedback Created, Id: {newUserFeedback.Id}");
        }

        public async Task<IEnumerable<UserFeedback>> GetFilteredListAsync(Func<UserFeedback, bool> filter, int? totalRecords = 15)
        {
            var userFeedbacks = await this.context.UserFeedbacks
                .Include(p => p.User)
                .ToListAsync();

            return userFeedbacks
                    .Where(filter)
                    .OrderByDescending(uf => uf.CreatedDate)
                    .Take(totalRecords.Value);
        }

        public async Task<IEnumerable<UserFeedback>> GetListAsync(int totalRecords = 15)
        {
            var userFeedbacks = await this.context.UserFeedbacks
                .Include(p => p.User)
                .ToListAsync();

            return userFeedbacks
                    .OrderByDescending(uf => uf.CreatedDate)
                    .Take(totalRecords);
        }

        public async Task<UserFeedback> GetFilteredAsync(Func<UserFeedback, bool> filter)
        {
            var userFeedbacks = await GetListAsync();
            return userFeedbacks.FirstOrDefault(filter);
        }

        public UserFeedback Get(Guid id)
        {
            return this.context.UserFeedbacks.Find(id);
        }
    }
}
