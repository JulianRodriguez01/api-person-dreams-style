namespace ApiPersons.Tests
{
    using NUnit.Framework;
    using Moq;
    using System.Net;
    using System.Net.Mail;
    using ApiPersons.Utilities;

    [TestFixture]
    public class MailHelperTests
    {

        [Test]
        public void SendPasswordResetEmail_ShouldSendEmailSuccessfully()
        {
            var mailHelper = new MailHelper();
            var smtpClientMock = new Mock<SmtpClient>();
            mailHelper.SetSmtpClient(smtpClientMock.Object); 

            string toAddress = "julian.rodriguez21@uptc.edu.co";
            string username = "Julian";
            string resetLink = "https://example.com/reset-password";

            mailHelper.SendEmail(toAddress, username, resetLink);
            smtpClientMock.Verify(
                client => client.Send(
                    It.Is<MailMessage>(message =>
                        message.To.ToString() == toAddress &&
                        message.Subject == "Restablecimiento de contraseña" &&
                        message.Body.Contains(username) &&
                        message.Body.Contains(resetLink)
                    )
                ),
                Times.Once
            );
        }
    }

}
