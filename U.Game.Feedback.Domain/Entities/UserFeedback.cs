using System;
using System.ComponentModel.DataAnnotations;

namespace U.Game.Feedback.Domain.Entities
{
    public class UserFeedback
    {
        public Guid Id { get; private set; }
        public User User { get; private set; }
        public string SessionId { get; private set; }

        [Range(1,5)]
        public int Rating { get; private set; }

        [StringLength(512)]
        public string Comments { get; private set; }

        public DateTime CreatedDate { get; private set; }

        protected UserFeedback() { }

        public UserFeedback(Guid id, User user, string sessionId, int rating, string comments)
        {
            this.Id = id;
            this.User = user;
            this.SessionId = sessionId;
            this.Rating = rating;
            this.Comments = comments;
            this.CreatedDate = DateTime.UtcNow;
        }
    }
}
