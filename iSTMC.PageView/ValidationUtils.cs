using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace iSTMC.PageView
{
   public static class ValidationUtils
   {
      /// <summary>
      /// 驗證台灣身分證字號合法性
      /// </summary>
      /// <param name="pid"></param>
      /// <returns></returns>
      public static bool ValidateTWPID(string pid)
      {
         if (string.IsNullOrEmpty(pid))
            return false;

         // 檢查基本格式
         Regex regex = new Regex(@"([A-Z]|[a-z])\d{9}");
         if (!regex.IsMatch(pid))
            return false;

         // 身分證數字第1碼必須為1或2
         if (pid.Length > 2 && pid[1] != '1' && pid[1] != '2')
            return false;

         var d = false;
         if (pid.Length == 10)
         {
            pid = pid.ToUpper();
            if (pid[0] >= 0x41 && pid[0] <= 0x5A)
            {
               var a = new[] { 10, 11, 12, 13, 14, 15, 16, 17, 34, 18, 19, 20, 21, 22, 35, 23, 24, 25, 26, 27, 28, 29, 32, 30, 31, 33 };
               var b = new int[11];
               b[1] = a[(pid[0]) - 65] % 10;
               var c = b[0] = a[(pid[0]) - 65] / 10;
               for (var i = 1; i <= 9; i++)
               {
                  b[i + 1] = pid[i] - 48;
                  c += b[i] * (10 - i);
               }
               if (((c % 10) + b[10]) % 10 == 0)
               {
                  d = true;
               }
            }
         }
         return d;
      }

      /// <summary>
      /// 驗證民國日期合法性
      /// </summary>
      /// <param name="date">民國年月日(不含分隔符號/-等)</param>
      /// <returns></returns>
      public static bool ValidateTWDate(string date)
      {
         if (date.Length < 6)
            return false;

         int bnum;
         if (!int.TryParse(date, out bnum))
            return false;

         int year = int.Parse(date.Substring(0, date.Length - 4));
         int month = int.Parse(date.Substring(date.Length - 4, 2));
         int day = int.Parse(date.Substring(date.Length - 2, 2));

         if (year < 1 || ((year + 1911) > DateTime.Today.Year))
            return false;

         if (month > 12 || month < 1)
            return false;

         int days = DateTime.DaysInMonth(year + 1911, month);
         if (day > days || day < 1)
            return false;

         return true;
      }

      /// <summary>
      /// 轉民國年為西元年
      /// </summary>
      /// <param name="date">民國年月日(不含分隔符號/-等)</param>
      /// <returns></returns>
      public static DateTime TWDateToCDate(string date)
      {
         DateTime m_dt = new DateTime();

         if (date.Length < 6)
            return m_dt;

         int bnum;
         if (!int.TryParse(date, out bnum))
            return m_dt;

         int year = int.Parse(date.Substring(0, date.Length - 4));
         int month = int.Parse(date.Substring(date.Length - 4, 2));
         int day = int.Parse(date.Substring(date.Length - 2, 2));

         m_dt = new DateTime((year + 1911), month, day);

         return m_dt;
      }

      /// <summary>
      /// 檢查連續數字
      /// </summary>
      /// <param name="pwd"></param>
      /// <returns></returns>
      public static bool ValidateContinueNo(string pwd)
      {
         bool m_ok = true;
         if (pwd.Length == 0) return true;

         for (int i = 0; i < (pwd.Length - 1); i++)
         {
            if (i + 2 > pwd.Length) break;
            if (IsContinueNo(pwd.Substring(i, 2)))
            {
               m_ok = false;
               break;
            }
         }
         return m_ok;
      }

      /// <summary>
      /// 密碼與使用者代號相同
      /// </summary>
      /// <param name="user_code">使用者代號</param>
      /// <param name="pwd">密碼</param>
      /// <returns>false=相同/true=不相同</returns>
      public static bool ValidateCode2id(string user_code, string pwd)
      {
         try
         {
            if (user_code.Length == 0 && pwd.Length == 0) return true;

            if (user_code.Equals(pwd))
               return false;
            else
               return true;
         }
         catch
         {
            return true;
         }
      }

      /// <summary>
      /// 檢核是否內含相同數字
      /// </summary>
      /// <param name="str">要被檢核的整個字串</param>
      /// <returns>true=含相同數字/false=不內含</returns>
      public static bool ValidateHadSameNo(string str)
      {
         bool m_ok = true;

         try
         {
            if (str.Length == 0) return true;

            string m_str1;
            for (int j = 0; j < str.Length; j++)
            {
               m_str1 = str.Substring(j, 1);
               if (ValidateDecimail(m_str1))
               {
                  for (int i = 0; i < str.Length; i++)
                  {
                     if (j != i
                         && ValidateDecimail(str.Substring(i, 1))
                         && m_str1.Equals(str.Substring(i, 1)))
                     {
                        return false;
                     }
                  }
               }
            }
         }
         catch
         {
            return false;
         }

         return m_ok;
      }

      /// <summary>
      /// 確認字串是否為英文字或數字
      /// </summary>
      /// <param name="str">要被檢核的字串</param>
      /// <returns>true=整個字串都符合</returns>
      public static bool ValidateAlphaDigit(string str)
      {
         if (str == null) str = string.Empty;
         return Regex.IsMatch(str, "^[A-Za-z0-9]+$");
      }

      #region private functions
      /// <summary>
      /// 檢核是否為連續數字(含正/反連續數字)
      /// </summary>
      /// <param name="str">要被檢核的字串(2碼/次)</param>
      /// <returns>true=含連續數字/false=不內含</returns>
      private static bool IsContinueNo(string str)
      {
         bool m_ok = false;

         string cNo = "0123456789";
         try
         {
            char[] cn2 = cNo.ToCharArray();
            for (int i = 0; i < (cNo.Length - 1); i++)
            {
               //每2碼比對(含正/反連續數字)
               if (str.Equals(cn2[i].ToString() + cn2[i + 1].ToString())
                   || str.Equals(cn2[i + 1].ToString() + cn2[i].ToString()))
               {
                  m_ok = true;
                  break;
               }
            }
         }
         catch
         {
            return true;
         }

         return m_ok;
      }

      /// <summary>
      /// 檢核是否為數字格式
      /// </summary>
      /// <param name="str">要被檢核的字串</param>
      /// <returns>整個字串都符合規定時回傳true</returns>
      private static bool ValidateDecimail(string str)
      {
         bool m_isNumeric = false;
         try
         {
            Decimal decRtn;
            m_isNumeric = Decimal.TryParse(str, out decRtn); //TryParse轉換失敗不會發生exception
         }
         catch
         {
            return false;
         }

         return m_isNumeric;
      }

      /// <summary>
      /// 驗證台灣統一編號合法性
      /// </summary>
      /// <param name="vatNo"></param>
      /// <returns></returns>
      public static bool ValidateVATNo(string vatNo)
      {
         int vatno;
         if (vatNo == null || vatNo.Trim().Length != 8)
            return false;
         if (!int.TryParse(vatNo, out vatno))
            return false;
         int[] Logic = new int[] { 1, 2, 1, 2, 1, 2, 4, 1 };
         int addition, sum = 0, j = 0, x;
         for (x = 0; x < Logic.Length; x++)
         {
            int no = Convert.ToInt32(vatNo.Substring(x, 1));
            j = no * Logic[x];
            addition = ((j / 10) + (j % 10));
            sum += addition;
         }
         if (sum % 10 == 0)
         {
            return true;
         }
         if (vatNo.Substring(6, 1) == "7")
         {
            if (sum % 10 == 9)
            {
               return true;
            }
         }
         return false;
      }
      #endregion
   }
}
