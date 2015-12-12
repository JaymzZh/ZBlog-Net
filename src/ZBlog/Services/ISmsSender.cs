using System.Threading.Tasks;

namespace ZBlog.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
