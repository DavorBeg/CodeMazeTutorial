using Application.Companies.Notifications;
using Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Companies.Handlers
{
	internal sealed class EmailHandler : INotificationHandler<CompanyDeletedNotification>
	{
		private readonly ILoggerManager _logger;

        public EmailHandler(ILoggerManager logger)
        {
			_logger = logger;
        }

		public Task Handle(CompanyDeletedNotification notification, CancellationToken cancellationToken)
		{
			_logger.LogWarn($"Company with ID: {notification.Id} have been deleted at time {DateTime.Now.ToString("g", CultureInfo.CurrentCulture)}");
			return Task.CompletedTask;
		}
	}
}
