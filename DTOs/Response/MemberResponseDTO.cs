namespace SocialMediaServer.DTOs.Response
{
    public class MemberResponseDTO
    {
        public int Id {get; set;}
        public GroupResponseDTO GroupChat { get; set; }
        public UserResponseDTO User { get; set; }
        public DateTime Join_at { get; set; } = DateTime.Now;
        public bool IsLeft { get; set; } = false;
        public bool isDelete = false;
    }
}