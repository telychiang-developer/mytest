using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace iSTMC.PageView
{
   public class PageViewUtils
   {
      public static BitmapImage LoadImgFromFile(string imgPath)
      {
         if (!File.Exists(imgPath))
            return null;

         MemoryStream memStream = new MemoryStream();
         using (FileStream fileStream = File.OpenRead(imgPath))
         {
            memStream.SetLength(fileStream.Length);
            fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
         }

         BitmapImage bi = new BitmapImage();
         bi.BeginInit();
         bi.CacheOption = BitmapCacheOption.OnLoad;
         bi.StreamSource = memStream;
         bi.EndInit();

         return bi;
      }

      public static void GetErrors(List<string> errlist, DependencyObject obj)
      {
         foreach (var child in LogicalTreeHelper.GetChildren(obj))
         {
            Panel panel = child as Panel;
            if (panel != null)
               GetErrors(errlist, panel);

            DependencyObject dp = child as DependencyObject;

            if (dp is TextBox || dp is RadioButton || dp is PasswordBox)
            {
               if (Validation.GetHasError(dp))
               {
                  foreach (ValidationError error in Validation.GetErrors(dp))
                  {
                     string errMsg = error.ErrorContent.ToString();

                     if (!errlist.Contains(errMsg))
                        errlist.Add(errMsg);
                  }
               }

               GetErrors(errlist, dp);
            }
         }
      }
   }
}
