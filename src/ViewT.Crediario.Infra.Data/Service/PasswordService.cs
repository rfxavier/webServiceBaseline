using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ViewT.Crediario.Domain.Main.Interfaces;

namespace ViewT.Crediario.Infra.Data.Service
{
    public class PasswordService : IPasswordService
    {
        public string Encrypt(string password)
        {
            string result = "";
            UnicodeEncoding encode = new UnicodeEncoding();
            byte[] textBytes = encode.GetBytes(password);
            SHA512Managed sha512 = new SHA512Managed();
            sha512.ComputeHash(textBytes).ToList().ForEach(x => result += string.Format("{0:x2}", x));
            return result;
        }
        
    }
}
