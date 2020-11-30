using AutoMapper;
using U.Game.Feedback.Api.Profiles;
using U.Game.Feedback.Api.Tests.Mocks;

namespace U.Game.Feedback.Api.Tests
{
    public class TestsBase
    {
        public readonly IMapper mapper;
        public readonly FeedbackDataMocks feedbackDataMocks;
        public readonly UserDataMocks userDataMocks;

        public TestsBase()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            mapper = mockMapper.CreateMapper();

            this.feedbackDataMocks = new FeedbackDataMocks();
            this.userDataMocks = new UserDataMocks();
        }
    }
}
