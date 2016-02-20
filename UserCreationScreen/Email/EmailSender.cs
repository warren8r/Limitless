using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace UserCreationScreen.Email
{
    public class EmailSender
    {
        // Public Constants

        // Public Variables
        public string smtpAddress = "smtp.mail.yahoo.com";
        public int portNumber = 587;
        public bool enableSSL = true;

        public string emailFrom
        {
            get
            {
                if ((_emailFrom == null) || (_emailFrom == string.Empty))
                    return null;
                else
                    return "Email Exists";
            }

            set { _emailFrom = value; }
        }

        public string password
        {
            get
            {
                if ((_password == null) || (_password == string.Empty))
                    return null;
                else
                    return "Email Exists";
            }

            set { _password = value; }
        }

        // Private Constants

        // Private Variables
        string _emailFrom = "aptestemail@yahoo.com";
        string _password = "T3stT3st";

        /// <summary>
        /// Sends an email to the provided email containg the provided message.
        /// </summary>
        /// <param name="emailTo">The email to send the email to</param>
        /// <param name="subject">The text to go into the email's subject bar</param>
        /// <param name="message">The email's message</param>
        /// <param name="isMessageHtml">Whether the email uses html (true) or plain text (false)</param>
        /// <returns></returns>
        public bool SendEmail(string emailTo, string subject, string message, bool isMessageHtml = true)
        {
            bool toReturn = true;

            // Begin sending the email
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = isMessageHtml;
                // Can set to false, if you are sending pure text.

                try
                {
                    // Create the client to send through
                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(_emailFrom, _password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }
                catch // Make sure that we know that the mail didn't send properly
                {
                    toReturn = false;
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Check whether the provided string is the emailFrom
        /// </summary>
        /// <param name="toCheck">The string to check</param>
        /// <returns>Whether toCheck equals EmailFrom</returns>
        public bool EqualsEmailFrom(string toCheck)
        {
            return _emailFrom == toCheck;
        }

        /// <summary>
        /// Check whether the provided string is the password
        /// </summary>
        /// <param name="toCheck">The string to check</param>
        /// <returns>Whether toCheck equals password</returns>
        public bool EqualsPassword(string toCheck)
        {
            return _password == toCheck;
        }
    }
}