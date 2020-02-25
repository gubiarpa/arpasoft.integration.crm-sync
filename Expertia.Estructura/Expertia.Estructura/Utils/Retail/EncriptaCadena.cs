using System;
using Microsoft.VisualBasic;
using System.Security.Cryptography;

namespace Expertia.Estructura.Utils
{
    public class EncriptaCadena
    {
        public enum TIPO_KEY : short
        {
            KEY_DEFAULT = 0,
            KEY_ENCRIPTA_PDF_COMPROBANTES = 1,
            KEY_ENCRIPTA_REP_GRAL_VTA_DET_OTROS = 2,
            KEY_ENCRIPTA_NRO_PEDIDO_PAGO_ONLINE = 3
        }

        private byte[] DES_IV = {0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };        

        public string DES_Decrypt(string pStrCadenaToDecrypt, string pStrKeyEncripta)
        {
            byte[] inputByteArray = new byte[pStrCadenaToDecrypt.Length + 1];
            try
            {
                if (string.IsNullOrEmpty(pStrKeyEncripta))
                    return null;
                else
                {
                    byte[] DES_key = System.Text.Encoding.UTF8.GetBytes(Strings.Left(pStrKeyEncripta, 8));
                    DESCryptoServiceProvider objDESCryptoServiceProvider = new DESCryptoServiceProvider();
                    // inputByteArray = Convert.FromBase64String(pStrCadenaToDecrypt)
                    inputByteArray = HexStringToByteArray(pStrCadenaToDecrypt);
                    System.IO.MemoryStream objMemoryStream = new System.IO.MemoryStream();
                    CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDESCryptoServiceProvider.CreateDecryptor(DES_key, DES_IV), CryptoStreamMode.Write);
                    objCryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                    objCryptoStream.FlushFinalBlock();
                    System.Text.Encoding objEncoding = System.Text.Encoding.UTF8;
                    return objEncoding.GetString(objMemoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public string DES_Encrypt(string pStrCadenaToEncrypt, string pStrKeyEncripta)
        {
            string strCadenaEncriptada = "";
            try
            {
                byte[] DES_key = System.Text.Encoding.UTF8.GetBytes(Strings.Left(pStrKeyEncripta, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = System.Text.Encoding.UTF8.GetBytes(pStrCadenaToEncrypt);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(DES_key, DES_IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                byte[] aBytes = ms.ToArray();
                System.Text.StringBuilder sbHexa = new System.Text.StringBuilder(aBytes.Length * 2);
                foreach (byte bytX in aBytes)
                    sbHexa.AppendFormat("{0:x2}", bytX);
                strCadenaEncriptada = sbHexa.ToString();
                return strCadenaEncriptada.ToUpper();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
               
        public string GetKEY(TIPO_KEY pTipoKey)
        {
            switch (pTipoKey)
            {
                case TIPO_KEY.KEY_ENCRIPTA_PDF_COMPROBANTES:
                    {
                        return System.Configuration.ConfigurationManager.AppSettings["STR_KEY_ENCRIPTA_PDF_COMPROBANTES"];
                    }

                case TIPO_KEY.KEY_ENCRIPTA_REP_GRAL_VTA_DET_OTROS:
                    {
                        return System.Configuration.ConfigurationManager.AppSettings["STR_ENCRIPTA_REP_GRAL_VTA_DET_OTROS"];
                    }

                case TIPO_KEY.KEY_ENCRIPTA_NRO_PEDIDO_PAGO_ONLINE:
                    {
                        return System.Configuration.ConfigurationManager.AppSettings["STR_KEY_ENCRIPTA_NRO_PEDIDO_PAGO_ONLINE"];
                    }

                default:
                    {
                        return System.Configuration.ConfigurationManager.AppSettings["STR_KEY_ENCRIPTA_DEFAULT"];
                    }
            }
        }

        private byte[] HexStringToByteArray(string aHex)
        {
            if (string.IsNullOrEmpty(aHex))
                return new byte[] { };
            int hexl = aHex.Length;
            if (hexl - (hexl / 2) * 2 != 0)
                throw new Exception("Hex string cannot have an odd number of digits."); // this is instead of MOD, as in all my tests a-(a\b)*b is faster than a mod b
            char[] hexar = aHex.ToCharArray(); // this is also faster than going after characters using Stirng(index)
            byte[] ar = new byte[(hexl >> 1) - 1 + 1];
            for (int i = 0; i <= ar.Length - 1; i++)
            {
                int ti = i << 1;
                int v1 = Strings.AscW(hexar[ti]);
                int v2 = Strings.AscW(hexar[ti + 1]);
                v1 -= (v1 < 58 ? 48 : (v1 < 97 ? 55 : 87));
                v2 -= (v2 < 58 ? 48 : (v2 < 97 ? 55 : 87));
                v1 <<= 4;
                ar[i] = System.Convert.ToByte(v1 + v2);
            }
            return ar;
        }
    }
}