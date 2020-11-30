using System;

namespace U.Game.Feedback.Api.Models
{
    public class FeedbackModel
    {
        public string SessionId { get; private set; }

        public Guid UserId { get; private set; }
                
        public int Rating { get; set; }

        public string Comments { get; set; }
    }
}
