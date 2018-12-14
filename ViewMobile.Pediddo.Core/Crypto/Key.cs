using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace ViewMobile.Pediddo.Core.Crypto
{
    /// <summary>
    ///  Note que esta dll é implantada tanto para site sellers quanto para softwares disponibilizados, não existe nada com case sensitive aqui.
    /// </summary>
    public class Key
    {

        // You can add all the feature sets you want here or even use this enumerator to differentiate different products
        // If you want to make old keys in the marketplace not work with new versions you can simply change the integer and obsolete them
        /// <summary>
        /// Você pode adicionar todos os recursos que quiser aqui ou mesmo usar esse enumerater para diferenciar produtos diferentes
        /// Se você quiser fazer Chaves antigas no mercado nao trabalhando com as novas versões, você pode simplismente trocar o inteiro e Obsoletos
        /// </summary>
        public enum ProductOrProductFeatureSet
        {
            None = 0,
            Product1_PaidVersion = 10,
            Product1_Free30DayTrialVersion = 11,
            Product2_PaidFullVersion = 20,
            Product2_PaidLiteVersion = 21,
            Product2_Free30DayTrialFullVersion = 22,
            Product2_Free30DayTrialLiteVersion = 23
        }

        // Returns a HEX string from the given byte array
        /// <summary>
        /// Retorna um texto HEX do array de bytes da entrada
        /// </summary>
        /// <param name="parmData">Array de bytes</param>
        /// <returns>Texto em Hex</returns>
        public string GetHexStringFromBytes(byte[] parmData)
        {
            StringBuilder sBuilder = new StringBuilder();
            int i;
            ///16
            for (i = 0; i <= parmData.Length - 1; i++)
            {
                if (i > 0 && i % 2 == 0)
                {
                    sBuilder.Append("-");
                }
                sBuilder.Append(parmData[i].ToString("x2").ToUpper());
            }
            return sBuilder.ToString();
        }

        // Returns a byte array from the given HEX string
        /// <summary>
        /// Retorna um array de bytes do texto HEX de entrada
        /// </summary>
        /// <param name="hexString">Texto HEX</param>
        /// <returns>Array de bytes</returns>
        public byte[] GetBytesFromHexString(string hexString)
        {
            string newString = "";
            char c;
            // remove all none A-F, 0-9, characters
            /// remove todos mesnos A-F, 0-9 e caracteres
            for (int i = 0; i < hexString.Length; i++)
            {
                c = hexString[i];
                newString += c;
            }
            // if odd number of characters, discard last character
            /// se o número de caracteres for ímpar, descartar o último caractere
            if (newString.Length % 2 != 0)
            {
                newString = newString.Substring(0, newString.Length - 1);
            }

            int byteLength = newString.Length / 2;
            byte[] locBytes = new byte[byteLength]; ;


            ///int i;
            int j = 0;
            for (int i = 0; i <= hexString.Length - 2; i += 2)
            {
                locBytes[j] = byte.Parse(hexString.Substring(i, 2), NumberStyles.HexNumber);
                j += 1;
            }
            return locBytes;
        }

    }
}
