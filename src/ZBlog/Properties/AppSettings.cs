namespace ZBlog
{
    public class AppSettings
    {
        public string SiteTitle { get; set; }

        public UserInfo UserInfo { get; set; }
    }

    public struct UserInfo
    {
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}