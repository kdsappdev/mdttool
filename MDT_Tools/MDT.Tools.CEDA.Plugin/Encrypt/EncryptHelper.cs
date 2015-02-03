using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.IO.Compression;
using Atf.Common;
using System.Windows.Forms;
using Ats.Foundation.Message;
using MDT.Tools.Core.Utils;

namespace Atf.Common.Encrypt
{
    public class EncryptHelper
    {
        private static readonly String ATSPublicKeyPath = Application.StartupPath + "./resources/ValidationServer.key";
        private static string publicKey = "";
        public static Dictionary<string, object> getLoginInfo(string loginInfo)
        {
            InitPublicKey();
            Dictionary<string, object> loginDic = new Dictionary<string, object>();
            MD5 md5 = new MD5CryptoServiceProvider();
            Random r = new Random();
            string a = r.NextDouble().ToString();
            byte[] rData = md5.ComputeHash(Encoding.UTF8.GetBytes(a));
            loginDic.Add("param1", EncryptByRSA(Convert.ToBase64String(rData), publicKey));
            loginDic.Add("param2", EncodeInfo(rData, loginInfo));
            return loginDic;
        }

        private static string EncodeInfo(byte[] rData, string info)
        {
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(info);
            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = rData,
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray);
        }

        private static string EncryptByRSA(string msg, string publicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            byte[] enBytes = rsa.Encrypt(UTF8Encoding.UTF8.GetBytes(msg), false);
            return Convert.ToBase64String(enBytes);
        }

        private static void InitPublicKey()
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                publicKey = "";
                FileStream fs = new FileStream(ATSPublicKeyPath, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string line;
                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (!string.IsNullOrEmpty(line))
                        {
                            publicKey += line;
                        }
                    }
                    if (!string.IsNullOrEmpty(publicKey))
                    {
                        publicKey = Encoding.UTF8.GetString(DeCompressData(Convert.FromBase64String(publicKey)));
                        Dictionary<string, string> dic = MsgHelper.Deserialize<Dictionary<string, string>>(publicKey);
                        RSACertContent rkey = MsgHelper.Deserialize<RSACertContent>(dic["Content"]);
                        if (rkey != null)
                        {
                            publicKey = rkey.Extra.ContainsKey("cspublickey") ? rkey.Extra["cspublickey"] : "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
                finally
                {
                    try
                    {
                        sr.Close();
                        fs.Close();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                    }
                }
            }
        }

        /// <summary>  
        /// 解压  
        /// </summary>  
        /// <param name="param"></param>  
        /// <returns></returns>  
        private static byte[] DeCompressData(byte[] sourceByte)
        {
            MemoryStream memStream = new MemoryStream(sourceByte);
            memStream.Position = 0;

            GZipStream zipStream = new GZipStream(memStream, CompressionMode.Decompress);
            memStream = new MemoryStream();

            byte[] buff = new byte[512];
            while (true)
            {
                int size = zipStream.Read(buff, 0, 512);
                memStream.Write(buff, 0, size);
                if (size < 512)
                    break;
            }

            byte[] tempByte = memStream.ToArray();
            memStream.Close();
            return tempByte;
        }
    }
}
