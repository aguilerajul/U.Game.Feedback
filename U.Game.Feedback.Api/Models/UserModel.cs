using System;

namespace U.Game.Feedback.Api.Models
{
    public class UserModel
    {
        public Guid Id { get; private set; }
                
        public string NickName { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
