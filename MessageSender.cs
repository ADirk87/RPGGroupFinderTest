using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace GroupFinderCapstone
{
    public class MessageSender
    {

        private const string FromAddress = "RPGGroupFinder@gmail.com";
        private const string Password = "szkecyhwiygyxmjr";
    
        public static void SendEmail(string subject, string body, params string[] toAddresses)
        {
                using (var client = new SmtpClient())
                using (var msg = new MailMessage())
                {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(FromAddress, Password);
                client.EnableSsl = true;
                client.Port = 587;
                client.Host = "smtp.gmail.com";

                msg.From = new MailAddress(FromAddress);
                foreach (var address in toAddresses)
                {
                    msg.To.Add(address);
                }

                msg.Body = body;
                msg.Subject = subject;

                client.Send(msg);

                }
            }
        }
    }
