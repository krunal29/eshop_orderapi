using eshop_orderapi.Business.Helpers;
using eshop_orderapi.Interfaces.Background;
using eshop_orderapi.Mail.Models;
using System;

namespace eshop_orderapi.Mail
{
    public class BackgroundMailerJobs : IBackgroundMailerJobs
    {
        #region Properties

        private static readonly object MailServiceLock = new object();

        #endregion Properties

        #region Constructor

        public BackgroundMailerJobs()
        {
        }

        #endregion Constructor

        public void SendWelcomeEmail()
        {
            var welcomeEmailModel = new WelcomeEmail
            {
                RecipientMail = "krunal.ifour@gmail.com",
                DisplayName = "Krunal" + " " + "Patel",
            };
            var mail = new Mail<WelcomeEmail>("WelcomeEmail", welcomeEmailModel);
            lock (MailServiceLock)
            {
                var sentMailData = mail.Send(welcomeEmailModel.RecipientMail, "Welcome to iFour network");                
            }
        }

        public void ForgotPassword(string emailId, string passwordReset)
        {
            var model = new ForgotPassword()
            {
                Link = $"{new AppSettings().FrontEndUrl}/Person/ForgotPassword?resetId={Convert.ToString(passwordReset)}"
            };
            var mail = new Mail<ForgotPassword>("ForgotPassword", model);

            lock (MailServiceLock)
            {
                mail.Send(emailId, "ForgotPassword");
            }
        }
    }
}