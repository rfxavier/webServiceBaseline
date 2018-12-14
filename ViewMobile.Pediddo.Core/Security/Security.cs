using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ViewMobile.Pediddo.Core.Security
{
    /// <summary>
    /// Security contains general purpose functions
    /// </summary>
    [Serializable]
    public class Security
    {
        #region Methods

        /// <summary>
        /// Creates a MD5 hash using the string's characters' ascii codes
        /// </summary>
        /// <param name="pString">The string to hash</param>
        /// <returns></returns>
        public string MD5(string pString)
        {
            string strHash = String.Empty;

            for (int j = 0, len = pString.Length; j < len; j++)
            {
                strHash += (int)pString[j];
            }

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash = Encoding.UTF8.GetBytes(strHash);
            byte[] result = md5.ComputeHash(hash);
            strHash = String.Empty;
            for (int i = 0, len = result.Length; i < len; i++)
            {
                strHash += result[i].ToString("x2");
            }

            return strHash;
        }

        public string NewPasswordRandom()
        {
            Random rdmNewPass = new Random();

            string sNewPass = string.Empty;

            while (sNewPass.Length < 10)
            {
                sNewPass += rdmNewPass.Next();
            }

            return sNewPass;
        }

        public bool isValidCPF(string cpf)
        {
            try
            {
                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                string tempCpf;
                string digito;
                int soma;
                int resto;

                cpf = cpf.Trim();
                cpf = cpf.Replace(".", "").Replace("-", "");

                if (cpf.Length != 11)
                    return false;

                tempCpf = cpf.Substring(0, 9);
                soma = 0;
                for (int i = 0; i < 9; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = resto.ToString();

                tempCpf = tempCpf + digito;

                soma = 0;
                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = digito + resto.ToString();

                return cpf.EndsWith(digito);
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
