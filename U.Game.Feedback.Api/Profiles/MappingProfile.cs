using AutoMapper;

namespace U.Game.Feedback.Api.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Entities.UserFeedback, Models.FeedbackModel>()
                .ForMember(dest => dest.UserId,
                           opt => opt.MapFrom(src => src.User.Id));            
            CreateMap<Models.FeedbackModel, Domain.Entities.UserFeedback>()
                .ForMember(dest => dest.User,
                           opt => opt.MapFrom(src => new Domain.Entities.User(src.UserId, string.Empty, string.Empty, string.Empty)));

            CreateMap<Domain.Entities.User, Models.UserModel>();
            CreateMap<Models.UserModel, Domain.Entities.User>();
        }
    }
}
