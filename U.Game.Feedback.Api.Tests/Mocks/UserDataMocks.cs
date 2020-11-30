using System;
using System.Collections.Generic;
using U.Game.Feedback.Domain.Entities;

namespace U.Game.Feedback.Api.Tests.Mocks
{
    public class UserDataMocks
    {
        public readonly Guid userId;

        public UserDataMocks()
        {
            this.userId = Guid.NewGuid();
        }

        public User UserMock(string nickname = "UserTest2020", string name = "User Test", string email = "user.test@someemailaccount.com")
        {
            return new User(this.userId, nickname, name, email);
        }

        public IEnumerable<User> UsersMock()
        {
            var users = new List<User>();

            for (int i = 0; i < 30; i++)
            {
                var newUser = new User(Guid.NewGuid(), $"nickname_{i}", $"name_test_{i}", $"email.test_{i}@sometestaccount.com");
                users.Add(newUser);
            }

            return users;
        }
    }
}
