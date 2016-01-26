namespace ZBlog
{
    public class AppSettings
    {
        /// <summary>
        /// Site title
        /// </summary>
        public string SiteTitle { get; set; }

        /// <summary>
        /// About me in short/a word
        /// </summary>
        public string AboutMeInShort { get; set; }

        /// <summary>
        /// User info
        /// </summary>
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