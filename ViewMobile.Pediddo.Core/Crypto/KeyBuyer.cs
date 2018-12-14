using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace ViewMobile.Pediddo.Core.Crypto
{
    // Note this dll is only deployed to your software product (not your selling web site)
    /// <summary>
    /// Esta dll é somente utilizada no seu software (não no seu site de vendas)
    /// </summary>
    public sealed class KeyBuyer
    {
        // Your company-wide cipher key
        /// <summary>
        /// Sua chave de codificação de toda a empresa
        /// </summary>
        private byte[] _CipherKey;
        private byte[] _CipherIV;

        /// <summary>
        /// Biblioteca comum
        /// </summary>
        private Key _KeyLib;

        // Object must be instantiated with your company secret cipher keys
        /// <summary>
        /// Os objetos precisam ser instanciados com sua chave secreta de codificção da empresa
        /// </summary>
        /// <param name="parmCipherKey"></param>
        /// <param name="parmCipherIV"></param>
        public KeyBuyer(byte[] parmCipherKey, byte[] parmCipherIV)
        {
            if (parmCipherKey.Length != 24)
            {
                throw new Exception("O comprimento da chave deve ser de 24");
            }
            if (parmCipherIV.Length != 8)
            {
                throw new Exception("O comprimento da chave deve ser de 8");
            }
            _CipherKey = parmCipherKey;
            _CipherIV = parmCipherIV;
            _KeyLib = new Key();
        }

        // Allows the customer to unlock their copy of the software
        // All parameters are required, the customer only needs to enter their name and the key
        // Your software should already know your company name and its own serial number
        // Only one possible key can unlock the software and that is the hash created by the provided seller name, serial and buyer name here
        /// <summary>
        /// Permite que o Cliente destravare as copias do software deles
        /// Todos os parâmetros são requeridos, o cliente somente precisa entar com o nome e chave
        /// Seu software pode já pode reconhecer o nome da sua compania e seu proprio número serial
        /// comento um chave pode desbloquear o software e ela é um hash criado pelo nome so vendedor, serial e nome do comprador aqui
        /// </summary>
        /// <param name="parmSellersName">Nome do Vendedor</param>
        /// <param name="parmSoftwareSerialNumber">Chave serial</param>
        /// <param name="parmBuyersName">Nome do comprador</param>
        /// <param name="Key">Chave</param>
        /// <returns></returns>
        public Key.ProductOrProductFeatureSet BuyKey(string parmSellersName, string parmSoftwareSerialNumber, string parmBuyersName, string Key)
        {
            if (parmBuyersName.Trim().Length == 0)
            {
                throw new Exception("Chave inválida. Voce voce fornecer a chave correta");
            }
            if (Key.Trim().Length != 59)
            {
                throw new Exception("Chave inválida. Você deve fornecer uma chave de destravamento 59 caracteres no formato xxxx-xxxx-xxxx-xxxx-xxxx-xxxx-xxxx-xxxx-xxxx-xxxx-xxxx-xxxx.");
            }
            else
            {
                Regex _regex = new Regex(@"[A-F0-9.-]{59}");
                Match match = _regex.Match(Key);
                if (!match.Success)
                {
                    throw new Exception("Chave inválida. Você deve fornecer uma chave válida para desbloquear.");
                }
                //or
                //for (Match match = Regex.Match(Key, "[A-F0-9.-]{59}"); match.Success; match = match.NextMatch())
                //{
                //    if (!match.Success)
                //    {
                //        throw new Exception("Chave inválida. Você deve fornecer uma chave válida para desbloquear.");
                //    }
                //}
            }
            string locFullKey = Key.Replace("-", "");
            string locHashKey = locFullKey.Substring(0, 32);
            byte[] locFeatureKey = _KeyLib.GetBytesFromHexString(locFullKey.Substring(32, 16));
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            string locHashThis = parmSellersName + "-" + parmSoftwareSerialNumber + "-" + parmBuyersName;
            locHashThis = locHashThis.ToLower().Trim().Replace(" ", "");
            byte[] data = md5Hasher.ComputeHash(Encoding.ASCII.GetBytes(locHashThis));
            string hashOfInput = _KeyLib.GetHexStringFromBytes(data).Replace("-", "");
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, locHashKey))
            {
                string locFeature = Decrypt(locFeatureKey, _CipherKey, _CipherIV);
                if (Char.IsNumber(locFeature, 0))
                {
                    if (Enum.IsDefined(typeof(Key.ProductOrProductFeatureSet), Convert.ToInt32(locFeature)))
                    {
                        return (Key.ProductOrProductFeatureSet)Convert.ToInt32(locFeature);
                    }
                    else
                    {
                        throw new Exception("Produto inválida. Você deve fornecer uma chave de produto que ainda é suportado.");
                    }
                }
                else
                {
                    throw new Exception("Produto inválida. Você deve entrar com a chave corretamente.");
                }
            }
            else
            {
                throw new Exception("Produto inválida. Verifique se você digitou o seu nome exatamente do jeito que você fez quando gerou a chave.");
            }
        }

        // Decrypts the feature set portion of the delivered key
        /// <summary>
        /// Decripta o conjunto de recursos parte da chave entregue 
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        private string Decrypt(byte[] Data, byte[] Key, byte[] IV)
        {
            System.IO.MemoryStream msDecrypt = new System.IO.MemoryStream(Data);
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, new TripleDESCryptoServiceProvider().CreateDecryptor(Key, IV), CryptoStreamMode.Read);
            byte[] fromEncrypt = new byte[Data.Length];
            csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
            return new ASCIIEncoding().GetString(fromEncrypt);
        }

    }
}
