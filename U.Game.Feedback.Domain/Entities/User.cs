using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace U.Game.Feedback.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }

        [StringLength(150)]
        public string NickName { get; private set; }

        [StringLength(150)]
        public string Name { get; private set; }

        [StringLength(250)]
        public string Email { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public ICollection<UserFeedback> UserFeedbacks { get; set; }

        protected User() { }

        public User(Guid id, string nickName, string name, string email)
        {
            this.Id = id;
            this.NickName = nickName;
            this.Name = name;
            this.Email = email;
            this.CreatedDate = DateTime.UtcNow;
        }
    }
}
