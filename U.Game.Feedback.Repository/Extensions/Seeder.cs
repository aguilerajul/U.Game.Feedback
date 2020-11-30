using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using U.Game.Feedback.Domain.Entities;

namespace U.Game.Feedback.Repository.Extensions
{
    public static class Seeder
    {
        public static void Initialize(RepositoryDbContext context)
        {
            context.Database.EnsureCreated();

            CreateUsers(context);
        }

        private static void CreateUsers(RepositoryDbContext context)
        {
            var users = new List<User>();
            var currentUsers = context.Users.ToList();
            for (int i = 0; i < 10; i++)
            {
                var newUser = new User(Guid.NewGuid(), $"Nickname_{i}", $"User{i}", $"user.{i}@testUsers.com");
                var existingUser = currentUsers.FirstOrDefault(cu => cu.NickName.Equals(newUser.NickName) && cu.Name.Equals(newUser.Name));
                if (existingUser == null)
                    users.Add(newUser);
            }

            if (users.Any() && users.Count > 0)
            {
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}
