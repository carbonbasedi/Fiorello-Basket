namespace Fiorello.Utilities.EmailService.EmailSender.Abstract
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
