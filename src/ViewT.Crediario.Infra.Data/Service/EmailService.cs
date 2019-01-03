using SendGrid;
using SendGrid.Helpers.Mail;
using ViewT.Crediario.Domain.Main.Interfaces;

namespace ViewT.Crediario.Infra.Data.Service
{
    public class EmailService : IEmailService
    {
        public void SendEmailForgotPassword(string email, string password)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("contato@crediario.com.br", "Crediário");
            var subject = "CREDIÁRIO - Recuperação de senha";
            var to = new EmailAddress(email);
            var plainTextContent = "";
            var htmlContent = "Condomínio - Nova senha\n\nPara entrar você deve usar a seguinte senha temporária: " + password +
                " . Lembre-se de redefinir sua senha.";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }
    }
}
