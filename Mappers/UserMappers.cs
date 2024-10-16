using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Mappers;
public static class UserMappers
{
    public static UserResponseDTO UserToUserResponseDTO(this User user)
    {
        return new UserResponseDTO
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.UserName,
            Bio = user.Bio,
            Create_at = user.Create_at,
            Profile_img = user.Profile_img,
            Date_of_birth = (DateTime)user.Date_of_birth,
            Gender = user.Gender,
            PhoneNumber = user.PhoneNumber
        };
    }

    public static User ToUserFromUserResponseDTO(this UserResponseDTO user)
    {
        return new User
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.Username,
            Bio = user.Bio,
            Create_at = user.Create_at,
            Profile_img = user.Profile_img,
            Date_of_birth = (DateTime)user.Date_of_birth,
            Gender = user.Gender,
            PhoneNumber = user.PhoneNumber
        };
    }
}