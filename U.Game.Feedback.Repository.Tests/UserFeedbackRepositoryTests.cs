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
    public class UserFeedbackRepositoryTests
    {
        private readonly IRepositoryBase<UserFeedback> userFeedbackRepository;
        private readonly RepositoryDbContextMock repositoryDbContextMock;

        private DateTime createdDate;

        public UserFeedbackRepositoryTests()
        {
            this.repositoryDbContextMock = new RepositoryDbContextMock();
            this.userFeedbackRepository = new UserFeedbackRepository(this.repositoryDbContextMock.dbContextMock);
            this.createdDate = DateTime.UtcNow.AddDays(-1);
        }

        [Fact]
        public void Get_User_Feedback_By_Id_Success()
        {
            var userFeedback = this.repositoryDbContextMock.userFeedbacksMock.FirstOrDefault();

            //Act
            var userFeedbackFromRepository = this.userFeedbackRepository.Get(userFeedback.Id);

            //Asserts
            userFeedbackFromRepository.Should().NotBeNull();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(3)]
        public async Task Get_User_Feedback_List_Success(int? totalRecords = 15)
        {
            var userFeedbacks = this.repositoryDbContextMock.userFeedbacksMock
                .OrderByDescending(uf => uf.CreatedDate)
                .Take(totalRecords.Value);

            //Act
            var userFeedbacksFromRepository = await this.userFeedbackRepository.GetListAsync(totalRecords.Value);

            //Asserts
            userFeedbacksFromRepository.Should().NotBeNull();
            userFeedbacksFromRepository.Count().Should().BeGreaterOrEqualTo(0);
            userFeedbacksFromRepository.Count().Should().Be(userFeedbacks.Count());
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(1)]
        public async Task Get_User_Feedback_Filtered_List_By_Rating_Success(int rating)
        {
            var userFeedbacks = this.repositoryDbContextMock.userFeedbacksMock
                .Where(uf => uf.Rating.Equals(rating))
                .OrderByDescending(uf => uf.CreatedDate);

            //Act
            var userFeedbacksFromRepository = await this.userFeedbackRepository.GetFilteredListAsync(f => f.Rating.Equals(rating));

            //Asserts
            userFeedbacksFromRepository.Should().NotBeNull();
            userFeedbacksFromRepository.Count().Should().BeGreaterOrEqualTo(0);
            userFeedbacksFromRepository.ToList()?.FirstOrDefault()?.Rating.Should().Be(rating);
            userFeedbacksFromRepository.Count().Should().Be(userFeedbacks.Count());
        }

        [Fact]
        public async Task Get_User_Feedback_Filtered_By_CreatedDate_Success()
        {
            var userFeedback = this.repositoryDbContextMock.userFeedbacksMock
                .OrderByDescending(uf => uf.CreatedDate)
                .FirstOrDefault();

            //Act
            var userFeedbacksFromRepository = await this.userFeedbackRepository.GetFilteredAsync(f => f.Id.Equals(userFeedback.Id));

            //Asserts
            userFeedbacksFromRepository.Should().NotBeNull();
            userFeedbacksFromRepository.Id.Should().Be(userFeedback.Id);
        }


        [Fact]
        public async Task Create_User_Feedback_Duplicated_By_Same_Session_Should_Throw_Error()
        {
            var userFeedback = this.repositoryDbContextMock.userFeedbacksMock
                    .OrderByDescending(uf => uf.CreatedDate)
                    .FirstOrDefault();

            //Act
            var actionResultMessage = await this.userFeedbackRepository.AddAsync(userFeedback);

            //Asserts
            actionResultMessage.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            actionResultMessage.Message.Should().Be("Thank you, but you alaready submited a previous feedback, We will revievew it and give you an answer soon.");
        }

        [Fact]
        public async Task Create_User_Feedback_No_User_Id_Sent_Throw_Error()
        {
            var userMock = new User(Guid.Empty, string.Empty, string.Empty, string.Empty);

            //Act
            var actionResultMessage = await this.userFeedbackRepository.AddAsync(
                new UserFeedback(
                    Guid.Empty,
                    userMock,
                    string.Empty,
                    1,
                    string.Empty)
                );

            //Asserts
            actionResultMessage.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            actionResultMessage.Message.Should().Be("The User Id field cannot be empty.");
        }

        [Theory]
        [InlineData(1, "69af17f5-f2cb-4472-8ba6-5cf55ca8ea4a", "mmm, It's very difficult to win")]
        [InlineData(2, "c29afd3e-f94a-4de5-92b1-0a67038449ce", "There are too many toxic players!!! please banned")]
        [InlineData(3, "50994c22-ec08-488c-92a0-e2462668f731", "Is not bad, but could be better.")]
        [InlineData(4, "2d4e49d4-d8ef-44a9-b111-201548d7e2f4", "I liked, nice story line!")]
        [InlineData(5, "f4ee3483-ee12-4d38-8e59-38d79fe90874", "The best thing that I saw in my life!!!")]
        public async Task Create_User_Feedback_Success(int rating, string sessionId, string comment)
        {
            //Act
            using(var context = new RepositoryDbContext(this.repositoryDbContextMock.options))
            {
                this.repositoryDbContextMock.usersMock = new List<User>();
                var userMock = new User(Guid.NewGuid(), "User1", "UserName", "user@usersforunittests.com");
                this.repositoryDbContextMock.usersMock.Add(userMock);

                this.repositoryDbContextMock.userFeedbacksMock = new List<UserFeedback>();

                var actionResultMessage = await this.userFeedbackRepository.AddAsync(
                        new UserFeedback(
                            Guid.NewGuid(),
                            new User(userMock.Id, userMock.NickName, userMock.Name, userMock.Email),
                            sessionId,
                            rating,
                            comment)
                        );

                //Asserts
                actionResultMessage.Should().NotBeNull();
                actionResultMessage.StatusCode.Should().Be(HttpStatusCode.OK);
                actionResultMessage.Message.Should().Contain("Feedback");
            }
        }
    }
}
