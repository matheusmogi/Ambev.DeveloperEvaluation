using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Profiles;

public class AuthenticateUserProfile : Profile
{
    public AuthenticateUserProfile()
    {
        CreateMap<AuthenticateUserRequest, AuthenticateUserCommand>();
        CreateMap<AuthenticateUserResult, AuthenticateUserResponse>();
    }
}