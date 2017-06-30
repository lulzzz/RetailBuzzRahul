using System.Text;
using CodeFirstServices;
using CodeFirstServices.Interfaces;
using CodeFirstData.EntityRepositories;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace CodeFirstServices.Services
{
    public class UtilityService:IUtilityService
    {
        private readonly IUnitOfWork _unitOfwork;
        public UtilityService(IUnitOfWork unitOfwork)
        {
            this._unitOfwork = unitOfwork;
        }
        
          public string getName(string name, int length, int value)
        {
            string finalName = string.Empty;
            switch (length)
            {
                case 1:
                    finalName = name + "00000" + value;
                    break;
                case 2:
                    finalName = name + "0000" + value;
                    break;
                case 3:
                    finalName = name + "000" + value;
                    break;
                case 4:
                    finalName = name + "00" + value;
                    break;
                case 5:
                    finalName = name + "0" + value;
                    break;
                case 6:
                    finalName = name + value;
                    break;

            }
            return finalName;
        }


          public string Encryptdata(string password)
          {
              string strmsg = string.Empty;
              byte[] encode = new
              byte[password.Length];
              encode = Encoding.UTF8.GetBytes(password);
              strmsg = Convert.ToBase64String(encode);
              return strmsg;
          }
          public string Decryptdata(string encryptpwd)
          {
              string decryptpwd = string.Empty;
              UTF8Encoding encodepwd = new UTF8Encoding();
              System.Text.Decoder utf8Decode = encodepwd.GetDecoder();
              byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
              int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
              char[] decoded_char = new char[charCount];
              utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
              decryptpwd = new String(decoded_char);
              return decryptpwd;
          }

    }
}
