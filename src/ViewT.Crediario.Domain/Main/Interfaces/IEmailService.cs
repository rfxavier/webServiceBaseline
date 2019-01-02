using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewT.Crediario.Domain.Main.Interfaces
{
    public interface IEmailService
    {
        void SendEmailForgotPassword(string email, string password);
    }
}
