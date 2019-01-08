using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using WebAPI_Sistema_Cenfotec.Models;

namespace WebAPI_Sistema_Cenfotec.Controllers.Logic
{
    public class Email
    {
        private static MailMessage mail = new MailMessage();
        private static SmtpClient smtp = new SmtpClient();
        private static Email email;
        private DBContext db = new DBContext();

        private Email() { }

        public static Email getInstance()
        {
            if (email == null) return new Email();
            return email;
        }

        public bool send(List<usuario> users, plantilla plantilla, evaluacione evaluacion)
        {
            string from = System.Configuration.ConfigurationManager.AppSettings["email"];
            string password = System.Configuration.ConfigurationManager.AppSettings["password"];
            string port = System.Configuration.ConfigurationManager.AppSettings["endpoint"];
            try
            {
                mail.Subject = "Evaluacion ";
                mail.From = new MailAddress(from);
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(from, password);
                mail.IsBodyHtml = true;

                foreach (var user in users)
                {
                    mail.To.Add(new MailAddress(user.correo));
                    mail.Body = "<h4>Buenas puedes ingresar a realizar la evaluacion de " + evaluacion.usuario.nombre + " "
                        + evaluacion.usuario.apellido + "</h4>";
                    mail.Body = "<h2>" + port +  AES256.encryptPassword(evaluacion.id_evaluacion.ToString()) + "/" +
                        AES256.encryptPassword(user.id_usuario.ToString()) + " - " + evaluacion.producto.nombre + "</h2>";
                    smtp.Send(mail);
                }
                
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return false;
            }
        }
    }
}