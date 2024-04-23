namespace backend.Controllers
{
    public class UserInfo
    {
        public int AccountID { get; set; }
        public int RoleID { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool? BanAccount { get; set; }
        public int? CreatorID { get; set; }
    }
}
