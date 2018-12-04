using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mom_Blog.Services;

namespace Mom_Blog.Tests.Fakes
{
    class FakeEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }
}
