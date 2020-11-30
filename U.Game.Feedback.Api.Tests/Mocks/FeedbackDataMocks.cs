using System;
using System.Collections.Generic;
using U.Game.Feedback.Domain.Entities;

namespace U.Game.Feedback.Api.Tests.Mocks
{
    public class FeedbackDataMocks
    {
        public readonly Guid feedbackId;
        public readonly string sessionId;

        public FeedbackDataMocks()
        {
            this.feedbackId = Guid.NewGuid();
            this.sessionId = Guid.NewGuid().ToString();
        }

        public UserFeedback UserFeedbackMock(string sessionId, int rating, string comments)
        {
            var user = new User(Guid.NewGuid(), "UserTest2020", "User Test", "user.test@someemailaccount.com");
            return new UserFeedback(this.feedbackId, user, sessionId, rating, comments);
        }

        public IEnumerable<UserFeedback> UserFeedbacksMock()
        {
            var userFeedbacks = new List<UserFeedback>();

            for (int i = 0; i < 30; i++)
            {
                var newUser = new User(Guid.NewGuid(), $"nickname_{i}", $"name_test_{i}", $"email.test_{i}@sometestaccount.com");
                var randomRating = new Random().Next(1, 5);
                userFeedbacks.Add(new UserFeedback(Guid.NewGuid(), newUser, this.sessionId, randomRating, $"Comment test number {i}"));
            }

            return userFeedbacks;
        }
    }
}
