using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using U.Game.Feedback.Api.Controllers;
using U.Game.Feedback.Api.Models;
using U.Game.Feedback.Domain.Contracts;
using U.Game.Feedback.Domain.Entities;
using Xunit;
using System.Collections.Generic;
using System.Text.Json;

namespace U.Game.Feedback.Api.Tests
{
    public class FeedbackControllerTests : TestsBase
    {
        readonly FeedbackController feedbackController;
        readonly Mock<IRepositoryBase<UserFeedback>> userFeedbackRepositoryMock;
        readonly Mock<IRepositoryBase<User>> userRepositoryMock;

        public FeedbackControllerTests()
        {
            this.userFeedbackRepositoryMock = new Mock<IRepositoryBase<UserFeedback>>();
            this.userRepositoryMock = new Mock<IRepositoryBase<User>>();

            this.feedbackController = new FeedbackController(this.mapper, userFeedbackRepositoryMock.Object, userRepositoryMock.Object);
        }

        [Theory]
        [InlineData(2, 10)]
        [InlineData(3, 5)]
        [InlineData(4, null)]
        [InlineData(5, 15)]
        [InlineData(null, 15)]
        public async Task Get_Filtered_List_Async_Success(int? rating, int? totalRecords)
        {
            var userFeedbacks = this.feedbackDataMocks.UserFeedbacksMock().Where(ufb => (rating != null ? ufb.Rating == rating.Value : ufb.Rating >= 1))
                .Take(totalRecords != null ? totalRecords.Value : 15);

            //Arrange
            this.userFeedbackRepositoryMock.Setup(ufr => ufr.GetFilteredListAsync(It.IsAny<Func<UserFeedback, bool>>(), totalRecords))
                .Returns(Task.FromResult(userFeedbacks));

            //Act
            var callbackResult = await this.feedbackController.List(rating, totalRecords);
            var okObjectResult = callbackResult as OkObjectResult;

            //Asserts
            callbackResult.Should().NotBeNull();
            callbackResult.Should().BeOfType<OkObjectResult>();
            okObjectResult.StatusCode.Value.Should().Be(200);

            var petResponseDtoResult = mapper.Map<IEnumerable<FeedbackModel>>(userFeedbacks);
            callbackResult.Should()
                .BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(petResponseDtoResult);

            var totalItems = okObjectResult.Value as IEnumerable<FeedbackModel>;
            totalItems.Count().Should().BeGreaterOrEqualTo(0);
        }

        [Fact]
        public async Task Send_Feedback_Async_No_Session_Id_Sent()
        {
            var userFeedback = this.feedbackDataMocks.UserFeedbackMock(this.feedbackDataMocks.sessionId, 5, "Test comments");

            //Act
            var callbackResult = await this.feedbackController.Post(
                string.Empty,
                this.userDataMocks.userId,
                new FeedbackModel
                {
                    Comments = userFeedback.Comments,
                    Rating = userFeedback.Rating
                }
            );

            //Asserts
            callbackResult.Should().NotBeNull();
            callbackResult.Should().BeOfType<BadRequestObjectResult>();

            var objectResult = callbackResult as BadRequestObjectResult;
            objectResult.StatusCode.Should().Be(400);
            objectResult.Value.Should().Be("The parameter Session Id needs to be supply it in the QueryString.");
        }

        [Fact]
        public async Task Send_Feedback_Async_No_User_Id_In_Headers()
        {
            var userFeedback = this.feedbackDataMocks.UserFeedbackMock(this.feedbackDataMocks.sessionId, 5, "Test comments");

            //Act
            var callbackResult = await this.feedbackController.Post(
                userFeedback.SessionId,
                Guid.Empty,
                new FeedbackModel
                {
                    Comments = userFeedback.Comments,
                    Rating = userFeedback.Rating
                }
            );

            //Asserts
            callbackResult.Should().NotBeNull();
            callbackResult.Should().BeOfType<BadRequestObjectResult>();

            var objectResult = callbackResult as BadRequestObjectResult;
            objectResult.StatusCode.Should().Be(400);
            objectResult.Value.Should().Be("The parameter User Id needs to be supply it in the Headers.");
        }

        [Fact]
        public async Task Send_Feedback_Async_User_With_User_Id_Doesnt_exists()
        {
            var userFeedback = this.feedbackDataMocks.UserFeedbackMock(this.feedbackDataMocks.sessionId, 5, "Test comments");

            //Arrange
            this.userRepositoryMock.Setup(ufr => ufr.Get(this.userDataMocks.userId))
                .Returns(Task.FromResult<User>(null).Result);

            //Act
            var callbackResult = await this.feedbackController.Post(
               userFeedback.SessionId,
               this.userDataMocks.userId,
               new FeedbackModel
               {
                   Comments = userFeedback.Comments,
                   Rating = userFeedback.Rating
               }
            );

            //Asserts
            callbackResult.Should().NotBeNull();
            callbackResult.Should().BeOfType<NotFoundObjectResult>();

            var objectResult = callbackResult as NotFoundObjectResult;
            objectResult.StatusCode.Should().Be(404);
            objectResult.Value.Should().Be($"A user with the ID: {this.userDataMocks.userId} doesn't exists in our database.");
        }

        [Theory]
        [InlineData(5, "I love it")]
        [InlineData(4, "Test Comment almost love it")]
        [InlineData(3, "Not bad, but could be improve it")]
        [InlineData(2, "mmm, too many toxic players")]
        [InlineData(1, "I cannot win nothing it's very hard :(!")]
        public async Task Send_Feedback_Async_Success(int rating, string comments)
        {
            var userFeedback = this.feedbackDataMocks.UserFeedbackMock(this.feedbackDataMocks.sessionId, rating, comments);

            //Arrange
            this.userRepositoryMock.Setup(ufr => ufr.Get(this.userDataMocks.userId))
                .Returns(this.userDataMocks.UserMock());

            this.userFeedbackRepositoryMock.Setup(ufr => ufr.AddAsync(It.IsAny<UserFeedback>()))
                .Returns(Task.FromResult(new ActionResultMessage(System.Net.HttpStatusCode.OK, $"Feedback Created, Id: {this.feedbackDataMocks.feedbackId}")));

            //Act
            var callbackResult = await this.feedbackController.Post(
               this.feedbackDataMocks.sessionId,
               this.userDataMocks.userId,
               new FeedbackModel
               {
                   Comments = userFeedback.Comments,
                   Rating = userFeedback.Rating
               }
            );

            //Asserts
            callbackResult.Should().NotBeNull();
            callbackResult.Should().BeOfType<OkObjectResult>();

            var objectResult = callbackResult as OkObjectResult;
            objectResult.StatusCode.Should().Be(200);
            objectResult.Value.Should().BeOfType<ActionResultMessage>();
            var actionResultMessage = (ActionResultMessage)objectResult.Value;
            actionResultMessage.Message.Should().Be($"Feedback Created, Id: {this.feedbackDataMocks.feedbackId}");
        }
    }
}
