using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ViewMobile.Pediddo.Core.Crypto
{
    // Note this dll is only deployed to the sellers web site (not the deployed software)
    /// <summary>
    /// Esse dll é somente implementada para o site vendedor (não para implementação do software)
    /// </summary>
    public sealed class KeySeller
    {

        // Your company-wide cipher key
        /// <summary>
        /// Sua chave de Codificação para toda a Empresa
        /// </summary>
        private byte[] _CipherKey;
        private byte[] _CipherIV;

        // Common library
        /// <summary>
        /// Biblioteca comum
        /// </summary>
        private Key _KeyLib;

        // Object must be instantiated with your company secret cipher keys
        /// <summary>
        /// O objeto precisa ser instanciado junto com a chave codificada da empresa
        /// </summary>
        /// <param name="parmCipherKey"></param>
        /// <param name="parmCipherIV"></param>
        public KeySeller(byte[] parmCipherKey, byte[] parmCipherIV)
        {
            if (parmCipherKey.Length != 24)
            {
                //throw new Exception("Cipher key length must be 24");
                throw new Exception("A tamanho da chave codificada precisa ser de 24 caracteres");
            }
            if (parmCipherIV.Length != 8)
            {
                //throw new Exception("Cipher IV length must be 8");
                throw new Exception("O tamanho do IV codificado precisar ser de 8 caracteres");
            }
            _CipherKey = parmCipherKey;
            _CipherIV = parmCipherIV;
            _KeyLib = new Key();
        }

        // Returns a software unlock key that ties together these 4 items:
        // 1) Your company name
        // 2) The serial number of the software thing you just sold (allowed customer to download)
        // 3) The name of the buyer/customer
        // 4) The feature set you want to activate/unlock in the software
        // The key will unlock a specific feature set, in a specific serial numbered downloaded copy, for a specific named customer
        // This key will not unlock any other software products other than the targeted downloaded copy and customer as long as you include the serial number on this method and stamp your downloads with a matching serial number
        /// <summary>
        /// Retorna a chave desbloquiadora do software que vai juntos com os proximos 4 item:
        /// 1) Nome da sua Compania
        /// 2) O número serial do software que você vendeu (habilitado para o cliente baixar)
        /// 3) O nome do comprator/Cliente
        /// 4) O Conjunto de Recursos que vc quer para ativar/bloquiar no software
        /// A chave irá desbloquear um conjunto de recursos especificos, em uma série específica numerada da copia baixada, para um cliente específico nomeado
        /// Essa chave não vai desbloquear qualquer outros produtos do software que não seja a cópia baixada e orientada ao cliente,
        /// desde que você inclua o número de série sobre esse método e carimbe seus downloads com um número de série correspondentee 
        /// </summary>
        /// <param name="parmSellersName">Nome do Vendedor</param>
        /// <param name="parmSoftwareSerialNumber">Número serial</param>
        /// <param name="parmBuyersName">Nome do Comprador</param>
        /// <param name="parmFeatures">Recursos</param>
        /// <returns></returns>
        public string SellKey(string parmSellersName, string parmSoftwareSerialNumber, string parmBuyersName, Key.ProductOrProductFeatureSet parmFeatures)
        {
            if (!Enum.IsDefined(typeof(Key.ProductOrProductFeatureSet), (int)parmFeatures))
            {
                throw new Exception("Este recurso não é para utlizar, ou esta obsoleto");
            }
            if (parmBuyersName.Trim().Length == 0)
            {
                throw new Exception("Deve ser fornecido uma chave, a fim de gerar a chave do produto.");
            }
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            string locHashThis = parmSellersName + "-" + parmSoftwareSerialNumber + "-" + parmBuyersName;
            locHashThis = locHashThis.ToLower().Trim().Replace(" ", "");
            byte[] dataKey = md5Hasher.ComputeHash(Encoding.ASCII.GetBytes(locHashThis));
            TripleDESCryptoServiceProvider tDESalg = new TripleDESCryptoServiceProvider();
            byte[] dataFeatures = Encrypt(parmFeatures, _CipherKey, _CipherIV);
            string locKey = _KeyLib.GetHexStringFromBytes(dataKey) + "-" + _KeyLib.GetHexStringFromBytes(dataFeatures);

            return locKey;
        }

        // Encrypts the feature set portion of the delivered key
        /// <summary>
        /// Encripta uma seção conjuntos de recursos da chave entregue
        /// </summary>
        /// <param name="Data">Data</param>
        /// <param name="Key">Chave</param>
        /// <param name="IV">IV</param>
        /// <returns></returns>
        private byte[] Encrypt(Key.ProductOrProductFeatureSet Data, byte[] Key, byte[] IV)
        {
            System.IO.MemoryStream mStream = new System.IO.MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, new TripleDESCryptoServiceProvider().CreateEncryptor(Key, IV), CryptoStreamMode.Write);
            byte[] toEncrypt = new ASCIIEncoding().GetBytes(Convert.ToInt32(Data).ToString());
            cStream.Write(toEncrypt, 0, toEncrypt.Length);
            cStream.FlushFinalBlock();
            byte[] ret = mStream.ToArray();
            cStream.Close();
            mStream.Close();
            return ret;
        }

    }
}
