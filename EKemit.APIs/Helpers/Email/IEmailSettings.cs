using EKemit.Core.Entities.Identity;
namespace EKemit.APIs.Helpers.Email
{
    public interface IEmailSettings
    {
        public void SendEmail(Emaill email);
    }
}
