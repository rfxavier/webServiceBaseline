using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ViewMobile.Pediddo.Core.Crypto
{
    /// <summary>
    /// Funcionalidades Basicas de Emcriptação/Decriptação
    /// </summary>
    public class Deck
    {
        #region enums, constants & fields
        /// <summary>
        ///  Tipos de Encriptação Semantica
        /// </summary>
        public enum CryptoTypes
        {
            encTypeDES = 0,
            encTypeRC2,
            encTypeRijndael,
            encTypeTripleDES
        }

        private const string CRYPT_DEFAULT_PASSWORD = "abcd!@#";
        private const CryptoTypes CRYPT_DEFAULT_METHOD = CryptoTypes.encTypeRijndael;

        private byte[] mKey = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        private byte[] mIV = { 65, 110, 68, 26, 69, 178, 200, 219 };
        private byte[] SaltByteArray = { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
        private CryptoTypes mCryptoType = CRYPT_DEFAULT_METHOD;
        private string mPassword = CRYPT_DEFAULT_PASSWORD;
        #endregion

        #region Constructors

        public Deck()
        {
            calculateNewKeyAndIV();
        }

        public Deck(CryptoTypes CryptoType)
        {
            this.CryptoType = CryptoType;
        }
        #endregion

        #region Props

        /// <summary>
        ///	Tipo de Encriptação/Decriptação usados
        /// </summary>
        public CryptoTypes CryptoType
        {
            get
            {
                return mCryptoType;
            }
            set
            {
                if (mCryptoType != value)
                {
                    mCryptoType = value;
                    calculateNewKeyAndIV();
                }
            }
        }

        /// <summary>
        ///		Propriedade Chave Password.
        ///		A chave de Passwork quando Encriptando/Decriptando
        /// </summary>
        public string Password
        {
            get
            {
                return mPassword;
            }
            set
            {
                if (mPassword != value)
                {
                    mPassword = value;
                    calculateNewKeyAndIV();
                }
            }
        }
        #endregion

        #region Encryption

        /// <summary>
        ///		Encripta um Texto
        /// </summary>
        /// <param name="inputText">Texto a Encriptar</param>
        /// <returns>Um Texto Encriptado</returns>
        public string Encrypt(string inputText)
        {
            //Declarando um Novo Encoder
            UTF8Encoding UTF8Encoder = new UTF8Encoding();
            //Pegando uma Representação em Bites do Texto
            byte[] inputBytes = UTF8Encoder.GetBytes(inputText);

            //Convertendo novamente em Texto
            return Convert.ToBase64String(EncryptDecrypt(inputBytes, true));
        }

        /// <summary>
        ///		Encripta um Text com a Senha definida pelo Usuario
        /// </summary>
        /// <param name="inputText">Texto para Encriptar</param>
        /// <param name="password">Senha a ser usada enquanto Encripta</param>
        /// <returns>Um Texto Encriptado</returns>
        public string Encrypt(string inputText, string password)
        {
            this.Password = password;
            return this.Encrypt(inputText);
        }

        /// <summary>
        ///		Encripta um Text acc para CryptType e com Senha definida pelo Usuario
        /// </summary>
        /// <param name="inputText">Texto a Encriptar</param>
        /// <param name="password">Senha a ser usada enquanto Encripta </param>
        /// <param name="cryptoType">Tipo da Encriptação</param>
        /// <returns>Um Text Encriptado</returns>
        public string Encrypt(string inputText, string password, CryptoTypes cryptoType)
        {
            mCryptoType = cryptoType;
            return this.Encrypt(inputText, password);
        }

        /// <summary>
        ///		Encripta um Texto acc para um Cryptotype
        /// </summary>
        /// <param name="inputText">Text para Encriptar</param>
        /// <param name="cryptoType">Tipo da Encriptação</param>
        /// <returns>Um Texto Encriptado</returns>
        public string Encrypt(string inputText, CryptoTypes cryptoType)
        {
            this.CryptoType = cryptoType;
            return this.Encrypt(inputText);
        }

        #endregion

        #region Decryption

        /// <summary>
        ///		Decripta uma Texto
        /// </summary>
        /// <param name="inputText">Testo para Decriptar</param>
        /// <returns>Um Text Decriptado</returns>
        public string Decrypt(string inputText)
        {
            //Declara um novo Encoder
            UTF8Encoding UTF8Encoder = new UTF8Encoding();
            //Pega um Representação em byte do Texto
            byte[] inputBytes = Convert.FromBase64String(inputText);

            //Converte novamente a Texto
            return UTF8Encoder.GetString(EncryptDecrypt(inputBytes, false));
        }

        /// <summary>
        ///		Decripta um texto usando uma Senha definida pelo Usuario
        /// </summary>
        /// <param name="inputText">Texto a ser decriptado</param>
        /// <param name="password">Password a ser usado enquanto decripta</param>
        /// <returns>Um Texto Decriptado</returns>
        public string Decrypt(string inputText, string password)
        {
            this.Password = password;
            return Decrypt(inputText);
        }

        /// <summary>
        ///		Decripta um Texto acc para Tipo de Decriptação e com uma Senha definida pelo Usuario
        /// </summary>
        /// <param name="inputText">Text a ser decriptado</param>
        /// <param name="password">A Senha a ser usada na decriptação</param>
        /// <param name="cryptoType">Typo de Decriptação</param>
        /// <returns>Um Texto Decriptado</returns>
        public string Decrypt(string inputText, string password, CryptoTypes cryptoType)
        {
            mCryptoType = cryptoType;
            return Decrypt(inputText, password);
        }

        /// <summary>
        ///		Decripta um texto acc para um tipo de decriptação
        /// </summary>
        /// <param name="inputText">Texto a ser decriptado</param>
        /// <param name="cryptoType">Tipo de decriptação</param>
        /// <returns>Um texto decriptado</returns>
        public string Decrypt(string inputText, CryptoTypes cryptoType)
        {
            this.CryptoType = cryptoType;
            return Decrypt(inputText);
        }
        #endregion

        #region Symmetric Engine

        /// <summary>
        ///		Realiza a enc/dec real.
        /// </summary>
        /// <param name="inputBytes">Entrada de Array de Bytes</param>
        /// <param name="Encrpyt">Se é ou não Encriptação</param>
        /// <returns>Saída de array de byte</returns>
        private byte[] EncryptDecrypt(byte[] inputBytes, bool Encrpyt)
        {
            ///pega a transformação correta
            ICryptoTransform transform = getCryptoTransform(Encrpyt);

            ///memory string de saida
            MemoryStream memStream = new MemoryStream();

            try
            {
                ///Cria a Cryptação - Saída escrita para memstream
                CryptoStream cryptStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);

                ///escreve as informações para o Mecanismo de Crypatação
                cryptStream.Write(inputBytes, 0, inputBytes.Length);

                ///Terminando
                cryptStream.FlushFinalBlock();

                ///Pegando Resultados
                byte[] output = memStream.ToArray();

                ///Terminando o Mecanismo, então fechando o Stream
                cryptStream.Close();

                return output;
            }
            catch (Exception e)
            {
                ///Arremessa um erro
                throw new Exception("Erro: " + e.Message, e);
            }
        }

        /// <summary>
        ///     retorna o mecanismo simetrico e cria o 
        ///		returns the symmetric engine and creates the encyptor/decryptor
        /// </summary>
        /// <param name="encrypt">whether to return a encrpytor or decryptor</param>
        /// <returns>ICryptoTransform</returns>
        private ICryptoTransform getCryptoTransform(bool encrypt)
        {
            SymmetricAlgorithm SA = selectAlgorithm();
            SA.Key = mKey;
            SA.IV = mIV;
            if (encrypt)
            {
                return SA.CreateEncryptor();
            }
            else
            {
                return SA.CreateDecryptor();
            }
        }
        /// <summary>
        ///		returns the specific symmetric algorithm acc. to the cryptotype
        /// </summary>
        /// <returns>SymmetricAlgorithm</returns>
        private SymmetricAlgorithm selectAlgorithm()
        {
            SymmetricAlgorithm SA;
            switch (mCryptoType)
            {
                case CryptoTypes.encTypeDES:
                    SA = DES.Create();
                    break;
                case CryptoTypes.encTypeRC2:
                    SA = RC2.Create();
                    break;
                case CryptoTypes.encTypeRijndael:
                    SA = Rijndael.Create();
                    break;
                case CryptoTypes.encTypeTripleDES:
                    SA = TripleDES.Create();
                    break;
                default:
                    SA = TripleDES.Create();
                    break;
            }
            return SA;
        }

        /// <summary>
        ///		calculates the key and IV acc. to the symmetric method from the password
        ///		key and IV size dependant on symmetric method
        /// </summary>
        private void calculateNewKeyAndIV()
        {
            //use salt so that key cannot be found with dictionary attack
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(mPassword, SaltByteArray);
            SymmetricAlgorithm algo = selectAlgorithm();
            mKey = pdb.GetBytes(algo.KeySize / 8);
            mIV = pdb.GetBytes(algo.BlockSize / 8);
        }

        #endregion
    }

    /// <summary>
    /// Classe de Hash. Somente membros estaticos entao a mesma nao tem 
    /// necessidade de ser instanciada.
    /// </summary>
    public class Hashing
    {
        #region enum, constants and fields
        //Tipos de hash disponiveis
        public enum HashingTypes
        {
            SHA, SHA256, SHA384, SHA512, MD5
        }
        #endregion

        #region static members
        public static string Hash(String inputText)
        {
            return ComputeHash(inputText, HashingTypes.MD5);
        }

        public static string Hash(String inputText, HashingTypes hashingType)
        {
            return ComputeHash(inputText, hashingType);
        }

        /// <summary>
        ///     retorna verdade se o texto de entrada é igual ao texto em hash
        /// </summary>
        /// <param name="inputText">Um texto nao hash para testar</param>
        /// <param name="hashText">Um texto já em hash</param>
        /// <returns>boleando verdadeiro ou falso</returns>
        public static bool isHashEqual(string inputText, string hashText)
        {
            return (Hash(inputText) == hashText);
        }

        public static bool isHashEqual(string inputText, string hashText, HashingTypes hashingType)
        {
            return (Hash(inputText, hashingType) == hashText);
        }
        #endregion

        #region Hashing Engine

        /// <summary>
        ///     calcula o hash code e converte para texto
        /// </summary>
        /// <param name="inputText">texto de entrada para ser transformado em hash</param>
        /// <param name="hashingType">tipo de hash a ser utilizado</param>
        /// <returns>texto em hash</returns>
        private static string ComputeHash(string inputText, HashingTypes hashingType)
        {
            HashAlgorithm HA = getHashAlgorithm(hashingType);

            //declara um novo encoder
            UTF8Encoding UTF8Encoder = new UTF8Encoding();
            //pegar uma representação em byte do texto

            byte[] inputBytes = UTF8Encoder.GetBytes(inputText);


            //Cria um hash com o entrada de Array de bytes
            byte[] output = HA.ComputeHash(inputBytes);

            //Converte a saida para um Array de bytes
            return Convert.ToBase64String(output);
        }

        /// <summary>
        ///		retorna um algoritimo especifico de hash
        /// </summary>
        /// <param name="hashingType">tipo de hash utilizado</param>
        /// <returns>Algoritimo de hash</returns>
        private static HashAlgorithm getHashAlgorithm(HashingTypes hashingType)
        {
            switch (hashingType)
            {
                case HashingTypes.MD5:
                    return new MD5CryptoServiceProvider();
                case HashingTypes.SHA:
                    return new SHA1CryptoServiceProvider();
                case HashingTypes.SHA256:
                    return new SHA256Managed();
                case HashingTypes.SHA384:
                    return new SHA384Managed();
                case HashingTypes.SHA512:
                    return new SHA512Managed();
                default:
                    return new MD5CryptoServiceProvider();
            }
        }
        #endregion

    }
}
