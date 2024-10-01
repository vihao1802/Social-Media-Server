namespace SocialMediaServer.Models;
public class Login
{
    public int LoginId { get; set; }
    public required string Username { get; set; }
    public required string Hash_password { get; set; }

    public DateTime Create_at {get;set;} = DateTime.Now;
    public bool Is_disabled {get;set;} = false;

    public required int UserId {get;set;}

    public required User User {get;set;}
}