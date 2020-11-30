using AutoMapper;
using System;
using System.Collections.Generic;
using U.Game.Feedback.Domain.Contracts;
using U.Game.Feedback.Domain.Entities;
using U.Game.Feedback.Repository;
using U.Game.Feedback.Repository.Implementations;

namespace U.Game.Feedback.Api.Profiles
{
    public class MappingProfile : Profile
    {
        private readonly IRepositoryBase<User> userRepository;
        private readonly RepositoryDbContext context;

        public MappingProfile()
        {
            CreateMap<Domain.Entities.UserFeedback, Models.FeedbackModel>()
                .ForMember(dest => dest.UserId,
                           opt => opt.MapFrom(src => src.User.Id));            
            CreateMap<Models.FeedbackModel, Domain.Entities.UserFeedback>()
                .ForMember(dest => dest.User,
                           opt => opt.MapFrom(src => new User(src.UserId, string.Empty, string.Empty, string.Empty)));

            CreateMap<Domain.Entities.User, Models.UserModel>();
            CreateMap<Models.UserModel, Domain.Entities.User>();
        }
    }
}
