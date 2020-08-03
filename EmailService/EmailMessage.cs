using System.Collections.Generic;
using System.Linq;
using MimeKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace EmailTrigger
{
	public class EmailMessage
	{
		public List<MailboxAddress> Reciever { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }

		public IFormFileCollection Attachments { get; set; }

        public List<FileContentResult> DataFiles { get; set; }

        public EmailMessage(IEnumerable<string> to, string subject, string content,
                            IFormFileCollection attachments, List<FileContentResult> dataFiles)
        {
            Reciever = new List<MailboxAddress>();
            Reciever.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            Content = content;
            Attachments = attachments;
            DataFiles = dataFiles;
        }
	}
}
