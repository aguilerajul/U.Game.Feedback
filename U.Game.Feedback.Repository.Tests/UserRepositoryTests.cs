using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using U.Game.Feedback.Domain.Contracts;
using U.Game.Feedback.Domain.Entities;
using U.Game.Feedback.Repository.Implementations;
using U.Game.Feedback.Repository.Tests.Mocks;
using Xunit;

namespace U.Game.Feedback.Repository.Tests
{
    public class UserRepositoryTests
    {
        private readonly IRepositoryBase<User> userRepository;
        private readonly RepositoryDbContextMock repositoryDbContextMock;

        private DateTime createdDate;

        public UserRepositoryTests()
        {
            this.repositoryDbContextMock = new RepositoryDbContextMock();
            this.userRepository = new UserRepository(this.repositoryDbContextMock.dbContextMock);
            this.createdDate = DateTime.UtcNow.AddDays(-1);
        }

        [Fact]
        public void Get_User_By_Id_Success()
        {
            var user = this.repositoryDbContextMock.usersMock.FirstOrDefault();

            //Act
            var userFromRepository = this.userRepository.Get(user.Id);

            //Asserts
            userFromRepository.Should().NotBeNull();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(3)]
        public async Task Get_User_List_Success(int? totalRecords = 15)
        {
            var users = this.repositoryDbContextMock.userFeedbacksMock
                .OrderByDescending(uf => uf.CreatedDate)
                .Take(totalRecords.Value);

            //Act
            var userFromRepository = await this.userRepository.GetListAsync(totalRecords.Value);

            //Asserts
            userFromRepository.Should().NotBeNull();
            userFromRepository.Count().Should().BeGreaterOrEqualTo(0);
        }

        [Fact]
        public async Task Get_User_Filtered_By_CreatedDate_Success()
        {
            var users = this.repositoryDbContextMock.userFeedbacksMock
                .Where(uf => uf.CreatedDate >= this.createdDate)
                .OrderByDescending(uf => uf.CreatedDate);

            //Act
            var userFromRepository = await this.userRepository.GetFilteredAsync(f => f.CreatedDate >= createdDate);

            //Asserts
            userFromRepository.Should().NotBeNull();
        }

        [Theory]
        [InlineData("name_test_0", "user test", "email_test_0@emailtestunit.com")]
        public async Task Create_User_Duplicated_Message(string nickName, string name, string email)
        {
            var userMock = this.repositoryDbContextMock.usersMock.FirstOrDefault();

            //Act
            var actionResultMessage = await this.userRepository.AddAsync(
                    new User(
                        userMock.Id,
                        nickName,
                        name,
                        email)
                    );

            //Asserts
            actionResultMessage.Should().NotBeNull();
            actionResultMessage.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            actionResultMessage.Message.Should().Be("This user is already registered in our database.");
        }

        [Theory]
        [InlineData("user_nick_test_1", "user test", "user@userunittest.com")]
        [InlineData("coolNickName", "Tester", "user2312330asd@userunittest.com")]
        public async Task Create_User_Success(string nickName, string name, string email)
        {
            var userMock = this.repositoryDbContextMock.usersMock.FirstOrDefault();

            //Act
            var actionResultMessage = await this.userRepository.AddAsync(
                    new User(
                        Guid.NewGuid(),
                        nickName,
                        name,
                        email)
                    );

            //Asserts
            actionResultMessage.Should().NotBeNull();
            actionResultMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
