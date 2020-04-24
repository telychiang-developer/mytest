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
   /// Page0020.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0020", Description = "開戶認證-核對資料")]
   public partial class Page0020 : Page
   {
      private Page0020ViewModel _pageVM;
      private readonly IKernelService _kernelService;

      public Page0020(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0020ViewModel(kernelService, "Page0020");

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0020_Loaded;
      }

      private void Page0020_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.OCRConfirmData))
         {
            string json = _kernelService.TransactionDataCache[KioskDataCacheKey.OCRConfirmData].ToString();
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
         else
         {
            if (_kernelService.TransactionDataCache.ContainsKey("__OCRResult"))
            {
               var result = _kernelService.TransactionDataCache["__OCRResult"] as IDImgOCRResult;

               if (result != null)
               {
                  _pageVM.ChineseName = result.Name;
                  _pageVM.PID = result.ID;

                  try
                  {
                     DateTime? dtBirth = _pageVM.GetBirthday(result.DateOfBirth);
                     if (dtBirth.HasValue)
                     {
                        _pageVM.BirthYear = string.Format("{0:D3}", dtBirth.Value.Year - 1911);
                        _pageVM.BirthMonth = string.Format("{0:D2}", dtBirth.Value.Month);
                        _pageVM.BirthDay = string.Format("{0:D2}", dtBirth.Value.Day);
                     }
                  }
                  catch (Exception ex)
                  {
                     _kernelService.Logger.Error(ex, "GetBirthday error with: {0}", ex.Message);
                  }
               }
            }

            if (_kernelService.TransactionDataCache.ContainsKey("__HostResultData"))
            {
               string json = _kernelService.TransactionDataCache["__HostResultData"].ToString();
               var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

               foreach (var key in jsonObj.Keys)
               {
                  switch (key)
                  {
                     case "custEnName":
                        _pageVM.EnglishName = jsonObj[key];
                        break;
                     default:
                        break;
                  }
               }
            }
         }

         _pageVM.ResetTimer();
         _pageVM.StartTimer();
      }

      private void btnConfirm_Click(object sender, RoutedEventArgs e)
      {
         List<string> errorList = new List<string>();
         PageViewUtils.GetErrors(errorList, grdForm);

         if(_pageVM.EnglishName.Contains("*"))
            errorList.Add("英文姓名內容有誤，請重新確認!");

         DateTime dtBirth = DateTime.MinValue;
         int numOfYear;
         if(int.TryParse(_pageVM.BirthYear, out numOfYear))
         {
            string dob = string.Format("{0}/{1}/{2}", numOfYear + 1911, _pageVM.BirthMonth, _pageVM.BirthDay);
            if (!DateTime.TryParse(dob, out dtBirth))
               errorList.Add("出生日期格式不合法!");
         }
         else
            errorList.Add("出生日期格式不合法!");

         if(dtBirth != DateTime.MinValue)
         {
            DateTime dtToday = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
            DateTime dtCompare = dtBirth.AddYears(20);

            if (dtToday < dtCompare)
               errorList.Add("未滿20歲不具開戶資格!");
         }

         if (errorList.Count > 0)
         {
            ScalingModal sm = new ScalingModal(MainGrid, InnerGrid);
            var popup = new ValidationErrorDialog(errorList.ToArray(), sm);
            sm.Expand(popup, ScalingModalExpandCollapseAnimation.SlideB);
         }
         else
         {
            _pageVM.StopTimer();

            var model = new
            {
               _pageVM.ChineseName,
               _pageVM.EnglishName,
               _pageVM.PID,
               DateOfBirth = DateTime.Parse(string.Format("{0:D3}/{1:D2}/{2:D2}", numOfYear + 1911, _pageVM.BirthMonth, _pageVM.BirthDay)),
               BirthYear = string.Format("{0:D3}", _pageVM.BirthYear),  
               BirthMonth = string.Format("{0:D2}", _pageVM.BirthMonth),
               BirthDay = string.Format("{0:D2}", _pageVM.BirthDay)
            };

            _pageVM.Clear();

            string json = JsonConvert.SerializeObject(model);
            _kernelService.TransactionDataCache.Set(KioskDataCacheKey.OCRConfirmData, json);

            PageResult result = new PageResult("Confirm", KioskDataCacheKey.OCRConfirmData);
            _kernelService.NextPage(result);
         }
      }

      private void btnCancel_Click(object sender, RoutedEventArgs e)
      {
         ScalingModal sm = new ScalingModal(MainGrid, InnerGrid);
         var popup = new ConfirmBackHome(sm);

         sm.Closed += ScalingModal_Closed;
         sm.Expand(popup, ScalingModalExpandCollapseAnimation.SlideB);
      }

      private void btnHome_Click(object sender, RoutedEventArgs e)
      {
         ScalingModal sm = new ScalingModal(MainGrid, InnerGrid);
         var popup = new ConfirmBackHome(sm);

         sm.Closed += ScalingModal_Closed;
         sm.Expand(popup, ScalingModalExpandCollapseAnimation.SlideB);
      }

      private void ScalingModal_Closed(object sender, EventArgs e)
      {
         ConfirmBackHome popup = sender as ConfirmBackHome;

         if (popup.IsConfirmed)
         {
            _pageVM.StopTimer();

            PageResult result = new PageResult("Home");
            _kernelService.NextPage(result);
         }
         else
         {
            _pageVM.ResetTimer();
         }
      }
   }

   public class Page0020ViewModel : PageViewModel
   {
      public Page0020ViewModel(IKernelService kernelService, string pageName)
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

      [Required(ErrorMessage = "英文姓名為必填欄位")]
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

      public DateTime? GetBirthday(string dateOfBirth)
      {
         if (dateOfBirth.Length < 3)
            return null;

         string s = dateOfBirth.Substring(2);

         int i = s.IndexOf("年");
         if (i == -1)
            return null;

         string year = s.Substring(0, i);
         s = s.Substring(year.Length + 1);

         i = s.IndexOf("月");
         if (i == -1)
            return null;

         string month = s.Substring(0, i);
         s = s.Substring(month.Length + 1);

         i = s.IndexOf("日");

         if (i == -1)
            return null;

         string day = s.Substring(0, i);
         s = s.Substring(day.Length + 1);

         int y;
         if (!int.TryParse(year, out y))
            y = -1;

         int numOfYear = y > 0 ? y + 1911 : -1;

         if (numOfYear > 0)
            return DateTime.Parse($"{numOfYear}/{month}/{day}");
         else
            return null;
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
