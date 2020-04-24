using iSTMC.Common;
using iSTMC.ImageOCRProtocol;
using iSTMC.Kernel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iSTMC.PageView
{
   /// <summary>
   /// Page0025.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0025", Description = "開戶認證-核對資料中")]
   public partial class Page0025 : Page
   {
      private Page0025ViewModel _pageVM;
      private readonly IKernelService _kernelService;

      public Page0025(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0025ViewModel(kernelService, "Page0025");

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0025_Loaded;
      }

      private void Page0025_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();

         if(_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.OCRConfirmData))
         {
            string json = _kernelService.TransactionDataCache[KioskDataCacheKey.OCRConfirmData].ToString();

            try
            {
               var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

               foreach (var key in jsonObj.Keys)
               {
                  switch (key)
                  {
                     case "ChineseName":
                        _pageVM.ChineseName = jsonObj[key];
                        break;
                     case "EnglishName":
                        _pageVM.EnglishName = jsonObj[key];
                        break;
                     case "PID":
                        _pageVM.PID = jsonObj[key];
                        break;
                     case "BirthYear":
                        _pageVM.BirthYear = jsonObj[key];
                        break;
                     case "BirthMonth":
                        _pageVM.BirthMonth = jsonObj[key];
                        break;
                     case "BirthDay":
                        _pageVM.BirthDay = jsonObj[key];
                        break;
                     default:
                        break;
                  }
               }
            }
            catch (Exception ex)
            {
               _kernelService.Logger.Error(ex, ex.Message);
            }
         }
      }
   }

   public class Page0025ViewModel : PageViewModel
   {
      public Page0025ViewModel(IKernelService kernelService, string pageName)
          : base(kernelService, pageName)
      {
      }

      [Required(ErrorMessage = "中文姓名為必填欄位")]
      [MaxLength(100, ErrorMessage = "中文姓名超過允許長度")]
      public string ChineseName
      {
         get { return GetValue(() => ChineseName); }
         set { SetValue(() => ChineseName, value); }
      }

      [MaxLength(100, ErrorMessage = "英文姓名超過允許長度")]
      public string EnglishName
      {
         get { return GetValue(() => EnglishName); }
         set { SetValue(() => EnglishName, value); }
      }

      [TWPIDValid(ErrorMessage = "身分證號不合法")]
      public string PID
      {
         get { return GetValue(() => PID); }
         set { SetValue(() => PID, value); }
      }

      [MinLength(3, ErrorMessage = "出生年須為3碼，不足位數第一碼請補0")]
      public string BirthYear
      {
         get { return GetValue(() => BirthYear); }
         set { SetValue(() => BirthYear, value); }
      }

      [MinLength(2, ErrorMessage = "出生月須為2碼，不足位數第一碼請補0")]
      public string BirthMonth
      {
         get { return GetValue(() => BirthMonth); }
         set { SetValue(() => BirthMonth, value); }
      }

      [MinLength(2, ErrorMessage = "出生日須為2碼，不足位數第一碼請補0")]
      public string BirthDay
      {
         get { return GetValue(() => BirthDay); }
         set { SetValue(() => BirthDay, value); }
      }

      public void Clear()
      {
         ChineseName = "";
         EnglishName = "";
         PID = "";
         BirthYear = "";
         BirthMonth = "";
         BirthDay = "";
      }
   }
}
