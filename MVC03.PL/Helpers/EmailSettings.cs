using System.Net;
using System.Net.Mail;

namespace MVC03.PL.Helpers
{
    public static class EmailSettings
    {
        public static bool SendEmail(Email email)
        {

            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("sherifrawan09@gmail.com", "ojumeqqajeisxmri");
                client.Send("sherifrawan09@gmail.com", email.To, email.Subject, email.Body);
                //ojumeqqajeisxmri
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }
    }
}
