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

namespace U.Game.Feedback.Api.Tests
{
    public class UserControllerTests : TestsBase
    {
        readonly UserController userController;
        readonly Mock<IRepositoryBase<User>> userRepositoryMock;

        public UserControllerTests()
        {
            this.userRepositoryMock = new Mock<IRepositoryBase<User>>();
            this.userController = new UserController(this.mapper, userRepositoryMock.Object);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(null)]
        public async Task Get_List_Async_Success(int? totalRecords = 15)
        {
            var users = this.userDataMocks.UsersMock().Take(totalRecords != null ? totalRecords.Value : 15);

            //Arrange
            this.userRepositoryMock.Setup(ufr => ufr.GetListAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(users));

            //Act
            var callbackResult = await this.userController.List();
            var okObjectResult = callbackResult as OkObjectResult;

            //Asserts
            callbackResult.Should().NotBeNull();
            callbackResult.Should().BeOfType<OkObjectResult>();
            okObjectResult.StatusCode.Value.Should().Be(200);

            var petResponseDtoResult = mapper.Map<IEnumerable<UserModel>>(users);
            callbackResult.Should()
                .BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(petResponseDtoResult);

            var totalItems = okObjectResult.Value as IEnumerable<UserModel>;
            totalItems.Count().Should().BeGreaterOrEqualTo(0);
        }

        [Fact]
        public async Task Create_New_User_Async_Success()
        {
            var user = this.userDataMocks.UserMock();

            //Arrange
            this.userRepositoryMock.Setup(ufr => ufr.AddAsync(It.IsAny<User>()))
                .Returns(Task.FromResult(new ActionResultMessage(System.Net.HttpStatusCode.OK, $"User Created, Id: {this.userDataMocks.userId}")));

            //Act
            var callbackResult = await this.userController.Post(
               new UserModel
               {
                   Email = user.Email,
                   Name = user.Name,
                   NickName = user.NickName
               }
            );

            //Asserts
            callbackResult.Should().NotBeNull();
            callbackResult.Should().BeOfType<OkObjectResult>();

            var objectResult = callbackResult as OkObjectResult;
            objectResult.StatusCode.Should().Be(200);

            objectResult.Value.Should().BeOfType<ActionResultMessage>();
            var actionResultMessage = (ActionResultMessage)objectResult.Value;
            actionResultMessage.Message.Should().Be($"User Created, Id: {this.userDataMocks.userId}");
        }
    }
}
