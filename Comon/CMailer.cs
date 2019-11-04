using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common.Interfaces;
using Common.Utils;


using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Common
{
    public class CMailer : CBaseFunctional
    {
        private string _companyName = "Crypto Trading Company";

        private string _smtpServer;
        private string _user;
        private string _mailDomain;
        private string _password;
        private bool _bUseEamilAsUserName;
        private int _portSMTP;

        private bool _bEnableSSL;


        public CMailer(IAlarmable alarmer,
                       string smtpServer,
                       int portSMTP,
                       string user,
                       string mailDomain,
                       string password,
                       bool bUseEamilAsUserName,
                       bool bEnableSSL)
            
            : base(alarmer)
        {

            _smtpServer = smtpServer;
            _user = user;
            _mailDomain = mailDomain;
            _password = password;

            _bUseEamilAsUserName = bUseEamilAsUserName;

            _bEnableSSL = bEnableSSL;

            _portSMTP = portSMTP;

		}


        public void SendMail(string toAddress,
                             string title,
                             string stContent,
                             string fileName
                             
                                    )
        {



            Log("Sending " + title);
            Log("Sending to");


            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(_smtpServer);           
           
            mail.Headers["Precedence"] = "bulk";
            mail.From = new MailAddress(String.Format("{0}@{1}", _user, _mailDomain));
            mail.To.Add(toAddress);
            mail.Subject = _companyName + ". " + title;
            mail.Body = "Здравствуйте. \n\n";
            mail.Body += stContent;
            mail.Body += "\n\n\n ";
            mail.Body += "С уважением, " + _companyName;

            //with html message marked as spam
            /*
             try
             {
                 string path = String.Format(@"{0}\Common\ctc_logo_narrow.jpg", Environment.GetEnvironmentVariable("DATA_PATH"));

                 LinkedResource LinkedImage = new LinkedResource(path);
                 LinkedImage.ContentId = "LogoId";
                 LinkedImage.ContentType = new ContentType(MediaTypeNames.Image.Jpeg);


                 string htmlSt = "<img src=cid:LogoId><BR>Здравствуйте. <BR>";
                 htmlSt += stContent;
                 htmlSt += "<BR><BR><BR>";
                 htmlSt += "С уважением, " + _companyName;

                 AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                     htmlSt,
                     null, "text/html");
                 htmlView.LinkedResources.Add(LinkedImage);
                 mail.AlternateViews.Add(htmlView);

                  mail.IsBodyHtml = true;

             }
             catch (Exception exc)
             {
                 Error("SendMail. Error gen html",exc);
             }
             */

            mail.Attachments.Add(new Attachment(fileName));
            SmtpServer.Port =  _portSMTP;

            if (_bUseEamilAsUserName)
                SmtpServer.Credentials = new System.Net.NetworkCredential(String.Format("{0}@{1}", _user, _mailDomain), _password);           
            else
                SmtpServer.Credentials = new System.Net.NetworkCredential(_user, _password);



            SmtpServer.EnableSsl = _bEnableSSL;



            try
            {
                SmtpServer.Send(mail);
                Log("Message was sent successfully");
            }
            catch (Exception e)
            {
                Error("SendMail error",e);
            }

        }




    }
}
