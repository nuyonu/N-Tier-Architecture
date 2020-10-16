using System.Collections.Generic;

namespace N_Tier.Application.Common.Email
{
    public class EmailMessage
    {
        public string ToAddress { get; private set; }

        public string Body { get; private set; }

        public string Subject { get; private set; }

        public List<EmailAttachment> Attachments { get; private set; }

        private EmailMessage() { }

        public static EmailMessage Create(string toAddress, string body, string subject, List<EmailAttachment> attachments = null)
        {
            attachments ??= new List<EmailAttachment>();

            return new EmailMessage()
            {
                ToAddress = toAddress,
                Body = body,
                Subject = subject,
                Attachments = attachments,
            };
        }

        public static EmailMessage Create(string toAddress, string body, string subject, EmailAttachment attachment)
        {
            return new EmailMessage()
            {
                ToAddress = toAddress,
                Body = body,
                Subject = subject,
                Attachments = new List<EmailAttachment>() { attachment },
            };
        }

    }
}
