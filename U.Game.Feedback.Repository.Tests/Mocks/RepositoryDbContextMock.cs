using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using U.Game.Feedback.Domain.Entities;

namespace U.Game.Feedback.Repository.Tests.Mocks
{
    public class RepositoryDbContextMock
    {
        private int totalItemsToMock = 30;

        public readonly DbContextOptions<RepositoryDbContext> options;
        public readonly RepositoryDbContext dbContextMock;
        public IList<User> usersMock;
        public IList<UserFeedback> userFeedbacksMock;

        public RepositoryDbContextMock()
        {
            this.options = new DbContextOptionsBuilder<RepositoryDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            this.dbContextMock = new RepositoryDbContext(options);

            this.usersMock = new List<User>();
            CreateUsersMock();

            this.userFeedbacksMock = new List<UserFeedback>();
            CreateFeedBacksMock();

            SaveDataMock();            
        }

        public void CreateUsersMock()
        {            
            for (int i = 0; i < totalItemsToMock; i++)
            {
                var user = new User(Guid.NewGuid(), $"nickName_test_{i}", $"name_test_{i}", $"email_test_{i}@emailtestunit.com");
                this.usersMock.Add(user);
            }
        }

        public void CreateFeedBacksMock()
        {
            for (int i = 0; i < totalItemsToMock; i++)
            {
                var randomRating = new Random().Next(1, 5);
                var userFeedback = new UserFeedback(Guid.NewGuid(), this.usersMock[i], Guid.NewGuid().ToString(), randomRating, $"Comment about something {i}");
                this.userFeedbacksMock.Add(userFeedback);
            }
        }

        public void SaveDataMock()
        {
            if(this.usersMock != null && this.usersMock.Any())
                dbContextMock.Users.AddRange(this.usersMock);

            if (this.userFeedbacksMock != null && this.userFeedbacksMock.Any())
                dbContextMock.UserFeedbacks.AddRange(this.userFeedbacksMock);

            dbContextMock.SaveChanges();
        }
    }
}
