using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailTrigger.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private IEmailSender _emailSender;
		private EmailConfiguration _emailConfig;
		List<Referral> referrals = new List<Referral>
		{
			new Referral { Id = 1, FirstName = "Joydip", LastName = "Kanjilal" },
			new Referral { Id = 2, FirstName = "Steve", LastName = "Smith" },
			new Referral { Id = 3, FirstName = "Anand", LastName = "Narayaswamy"}
		};

		public WeatherForecastController(IEmailSender emailSender, EmailConfiguration emailConfig)
		{
			_emailSender = emailSender;
			_emailConfig = emailConfig;
		}

		[HttpGet]
		public async Task<string> Get()
		{
			var dataFile = new List<FileContentResult>() { GenerateExcelDocument() };
			string[] to = new string[] { _emailConfig.Reciever };
			string subject = "Mail Trigger With Export";
			string messageBody = "This mail contains Exported Excel Data.";
			var message = new EmailMessage(to, subject, messageBody, null, dataFile);

			//_emailSender.SendEmail(message);
			await _emailSender.SendEmailAsync(message);

			return "Mail Sent Successfully.";
		}

		[HttpPost]
		public async Task<string> Post()
		{

			var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
			string[] to = new string[] { _emailConfig.Reciever };
			string subject = "Mail Trigger With Attachment";
			string messageBody = "Trigger mail with Attachments.";
			var message = new EmailMessage(to, subject , messageBody, files, null);
			await _emailSender.SendEmailAsync(message);

			return "Mail with attachment sent Successfully.";
		}

		private FileContentResult GenerateExcelDocument()
		{
			string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

			string fileName = "Referrals.xlsx";
			try
			{
				using (var workbook = new XLWorkbook())
				{
					IXLWorksheet worksheet = workbook.Worksheets.Add("Referrals");
					worksheet.Cell(1, 1).Value = "Id";
					worksheet.Cell(1, 2).Value = "FirstName";
					worksheet.Cell(1, 3).Value = "LastName";
					for (int index = 1; index <= referrals.Count; index++)
					{
						worksheet.Cell(index + 1, 1).Value = referrals[index - 1].Id;
						worksheet.Cell(index + 1, 2).Value = referrals[index - 1].FirstName;
						worksheet.Cell(index + 1, 3).Value = referrals[index - 1].LastName;
					}
					using (var stream = new MemoryStream())
					{
						workbook.SaveAs(stream);
						var content = stream.ToArray();
						return File(content, contentType, fileName);
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
