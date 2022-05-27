using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Painel.Core
{
    public class Email
    {
        public string templateFile { get; set; }

        private StringBuilder _templateText;
        public StringBuilder templateText
        {
            get
            {
                if (_templateText == null) { _templateText = new StringBuilder(); }
                return _templateText;
            }
            set { _templateText = value; }
        }

        private List<String> _anexos;
        public List<String> anexos
        {
            get
            {
                if (_anexos == null) { _anexos = new List<String>(); }
                return _anexos;
            }
            set { _anexos = value; }
        }

        private List<String> _destinatarios;
        public List<String> destinatarios
        {
            get
            {
                if (_destinatarios == null) { _destinatarios = new List<String>(); }
                return _destinatarios;
            }
            set { _destinatarios = value; }
        }

        private List<String> _destinatariosOcultos;
        public List<String> destinatariosOcultos
        {
            get
            {
                if (_destinatariosOcultos == null) { _destinatariosOcultos = new List<String>(); }
                return _destinatariosOcultos;
            }
            set { _destinatariosOcultos = value; }
        }

        public string assunto { get; set; }
        public string remetenteEmail { get; set; }
        public string remetenteNome { get; set; }
        public string smtpUsername { get; set; }
        public string smtpPass { get; set; }
        public string smtpServer { get; set; }

        public void Substituir(string oldText, string newText)
        {
            if (templateText.Length == 0) BuscaTemplate();
            this.templateText = this.templateText.Replace(oldText, newText);
        }

        public bool Enviar()
        {
            try
            {
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["host"]))
                    this.templateText = this.templateText.Replace("{host}", ConfigurationManager.AppSettings["host"].ToString());

                if (smtpServer == null || smtpUsername == null || smtpPass == null)
                    GetCredentials();

                if (templateText.Length == 0)
                    BuscaTemplate();

                MailMessage mail = new MailMessage();

                foreach (string cc in destinatarios)
                    mail.To.Add(cc);

                foreach (string bcc in destinatariosOcultos)
                    mail.Bcc.Add(bcc);

                foreach (var aa in anexos)
                {
                    Attachment attachment = new Attachment(aa, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(aa);
                    disposition.ModificationDate = File.GetLastWriteTime(aa);
                    disposition.ReadDate = File.GetLastAccessTime(aa);
                    disposition.FileName = Path.GetFileName(aa);
                    disposition.Size = new FileInfo(aa).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mail.Attachments.Add(attachment);
                }

                mail.From = new MailAddress(ConfigurationManager.AppSettings["email_from"].ToString());
                mail.ReplyToList.Add(new MailAddress(remetenteEmail, remetenteNome));
                mail.Subject = assunto;
                mail.Body = templateText.ToString();
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Host = smtpServer;
                smtp.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPass);
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["email_ssl"].ToString());
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["email_port"].ToString());
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtp.Send(mail);
                mail.Dispose();
                smtp.Dispose();

                return true;
            }
            catch (Exception ex)
            { throw ex; }
        }

        private bool BuscaTemplate()
        {
            try
            {
                if (File.Exists(templateFile))
                {
                    StreamReader srDescricao = new StreamReader(templateFile, System.Text.Encoding.UTF8);
                    templateText.Append(srDescricao.ReadToEnd());
                    srDescricao.Close();
                    srDescricao.Dispose();
                    return true;
                }
            }
            catch (Exception) { }
            return false;
        }

        private void GetCredentials()
        {
            this.smtpUsername = ConfigurationManager.AppSettings["email_user"].ToString();
            this.smtpPass = ConfigurationManager.AppSettings["email_pass"].ToString();
            this.smtpServer = ConfigurationManager.AppSettings["email_server"].ToString();
            if (this.remetenteEmail == null || this.remetenteEmail.Equals(string.Empty))
                this.remetenteEmail = ConfigurationManager.AppSettings["email_from"].ToString();
            if (this.remetenteNome == null || this.remetenteNome.Equals(string.Empty))
                this.remetenteNome = ConfigurationManager.AppSettings["email_from_name"].ToString();
        }
    }
}