using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;

namespace ViewMobile.Pediddo.Core.Configuration
{
    public class Configuration
    {
        private static int codeButtonTrue;
        private static int codeButtonFalse;
        private static string _p12FilePassword;
        private static string text_MESSAGE_ACORD;
        private static string keyGoogle;
        private static string version;
        private static string sSmtpServer;
        private static string logFile;

        static Configuration()
        {
            sSmtpServer = ConfigurationManager.AppSettings["SMTP"];
            codeButtonTrue = Convert.ToInt32(ConfigurationManager.AppSettings["CODE_BUTTON_TRUE"]);
            codeButtonFalse = Convert.ToInt32(ConfigurationManager.AppSettings["CODE_BUTTON_FALSE"]);
            _p12FilePassword = ConfigurationManager.AppSettings["p12FilePassword"];
            text_MESSAGE_ACORD = ConfigurationManager.AppSettings["TEXT_MESSAGE_ACORD"];
            keyGoogle = ConfigurationManager.AppSettings["KEY_GOOGLE"];
            version = ConfigurationManager.AppSettings["Version"];
            logFile = ConfigurationManager.AppSettings["LogFile"];
        }

        /// <summary>
        /// Gets the current Smtp Server address
        /// </summary>
        public static string SmtpServer
        {
            get { return sSmtpServer; }
        }

        public static int CodeButtonTrue
        {
            get { return codeButtonTrue; }
        }

        public static int CodeButtonFalse
        {
            get { return codeButtonFalse; }
        }

        public static string p12FilePassword
        {
            get { return _p12FilePassword; }
        }

        public static string TEXT_MESSAGE_ACORD
        {
            get { return text_MESSAGE_ACORD; }
        }

        public static string KEY_GOOGLE
        {
            get { return keyGoogle; }
        }

        public static string VERSION
        {
            get { return version; }
        }

        public static string GetLastCaracteres(string text, int NumberCaracter)
        {
            string ReturnText = string.Empty;

            if (text.Length > NumberCaracter)
            {
                ReturnText = text.Substring(text.Length - NumberCaracter);
            }
            return ReturnText;
        }

        public static bool ValidUDID(string udid)
        {
            if (udid.Length == 40)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string LogFile
        {
            get { return logFile; }
        }

        /// <summary>
        /// Get Ticket Invoice template path
        /// </summary>
        /// <returns></returns>
        public static string GetTicketInvoiceTemplateTXT()
        {
            return ConfigurationManager.AppSettings["PATH_TICKET_INVOICE_TEMPLATE_TXT"];
        }
        /// <summary>
        /// Get Email Integration template path used in Email/Fax Integration
        /// </summary>
        /// <returns></returns>
        public static string GetTicketReceiptTemplateTXT()
        {
            return ConfigurationManager.AppSettings["PATH_TICKET_RECEIPT_TEMPLATE_TXT"];
        }
        /// <summary>
        /// Get Email Integration template path used in Email/Fax Integration
        /// </summary>
        /// <returns></returns>
        public static string GetEmailIntegrationTemplateTXT()
        {
            return ConfigurationManager.AppSettings["PATH_TEMPLATE_EMAIL_INTEGRATION_TXT"];
        }
        /// <summary>
        /// Get Email Integration Backup template path used in Email/Fax Integration
        /// </summary>
        /// <returns></returns>
        public static string GetEmailIntegrationUserTemplate()
        {
            return ConfigurationManager.AppSettings["PATH_TEMPLATE_EMAIL_INTEGRATION_USER"];
        }
        /// <summary>
        /// Get Email Integration Temporary File path used in Email/Fax Integration
        /// </summary>
        /// <returns></returns>
        public static string GetTemporaryFilePath()
        {
            return ConfigurationManager.AppSettings["PATH_TEMPORARYFILE"];
        }

        /// <summary>
        /// Gets the <see cref="SmtpDeliveryMethod"/>.
        /// </summary>
        /// <returns>The <see cref="SmtpDeliveryMethod"/>.</returns>
        public static SmtpDeliveryMethod GetSmtpDeliveryMethod()
        {
            return ServiceGetSmtpDeliveryMethod();
        }

        public static SmtpDeliveryMethod ServiceGetSmtpDeliveryMethod()
        {
            SmtpDeliveryMethod method = SmtpDeliveryMethod.Network;

            System.Net.Configuration.SmtpSection section = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as System.Net.Configuration.SmtpSection;

            if (null != section)
            {
                method = section.DeliveryMethod;
            }

            return method;
        }

    }
}
