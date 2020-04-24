using iSTMC.Common;
using iSTMC.Kernel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
   /// Page0150.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0150", Description = "上傳證件-核對資料")]
   public partial class Page0150 : Page
   {
      private Page0150ViewModel _pageVM;
      private readonly IKernelService _kernelService;

      public Page0150(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0150ViewModel(kernelService, "Page0150");

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0150_Loaded;
      }

      private void Page0150_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.IDConfirmData))
         {
            string json = _kernelService.TransactionDataCache[KioskDataCacheKey.IDConfirmData].ToString();

            if (!string.IsNullOrEmpty(json))
            {
               try
               {
                  var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                  _pageVM.CustName = jsonObj["custName"];
                  _pageVM.Birthday = jsonObj["birthday"];
                  _pageVM.IssueDate = jsonObj["iDIssueDt"];
                  _pageVM.IssueLoc = jsonObj["iDIssueLoc"];
                  _pageVM.IssueFlag = jsonObj["iDIssueFlag"];
                  _pageVM.HasPhoto = jsonObj["iDPicFlag"];
                  _pageVM.PID = jsonObj["custId"];
                  _pageVM.SecondIDType = jsonObj["secCert"];
                  _pageVM.SecondIDSN = jsonObj["secCertId"];
                  _pageVM.OtherID = jsonObj["secCertType"];
               }
               catch (Exception ex)
               {
                  _kernelService.Logger.Error(ex, $"Page0150_Loaded: {ex.Message}");
               }
            }
         }
         else
         {
            if (_kernelService.TransactionDataCache.ContainsKey("__IDImgOCRResult"))
            {
               string json = _kernelService.TransactionDataCache["__IDImgOCRResult"].ToString();

               if (!string.IsNullOrEmpty(json))
               {
                  try
                  {
                     var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                     DateTime dob;
                     if (!DateTime.TryParse(jsonObj["Birthday"], out dob))
                        dob = DateTime.MinValue;

                     DateTime issueDate;
                     if (!DateTime.TryParse(jsonObj["IssueDate"], out issueDate))
                        issueDate = DateTime.MinValue;

                     _pageVM.CustName = jsonObj["CustName"];
                     _pageVM.Birthday = dob != DateTime.MinValue ? string.Format("{0:D3}{1:D2}{2:D2}", dob.Year - 1911, dob.Month, dob.Day) : "";
                     _pageVM.IssueDate = issueDate != DateTime.MinValue ? string.Format("{0:D3}{1:D2}{2:D2}", issueDate.Year - 1911, issueDate.Month, issueDate.Day) : "";
                     _pageVM.IssueLoc = jsonObj["IssueLoc"];
                     _pageVM.IssueFlag = jsonObj["IssueFlag"];
                     _pageVM.HasPhoto = jsonObj["HasPhoto"];
                     _pageVM.PID = jsonObj["PID"];
                     _pageVM.SecondIDType = jsonObj["SecondIDType"];
                     _pageVM.SecondIDSN = jsonObj["SecondIDSN"];

                     if (_pageVM.Birthday == "" || _pageVM.PID == "")
                     {
                        if (_kernelService.TransactionDataCache.ContainsKey("OCRConfirmData"))
                        {
                           string jsonD = _kernelService.TransactionDataCache["OCRConfirmData"].ToString();
                           var jsonDObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonD);

                           if (jsonDObj.ContainsKey("PID") && _pageVM.PID == "")
                              _pageVM.PID = jsonDObj["PID"];

                           if (jsonDObj.ContainsKey("BirthYear") && jsonDObj.ContainsKey("BirthMonth") && jsonDObj.ContainsKey("BirthDay"))
                              _pageVM.Birthday = jsonDObj["BirthYear"] + jsonDObj["BirthMonth"] + jsonDObj["BirthDay"];
                        }
                     }
                  }
                  catch (Exception ex)
                  {
                     _kernelService.Logger.Error(ex, $"Page0150_Loaded: {ex.Message}");
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

         DateTime? dtIssueDate = _pageVM.ConvertIssuedDate();
         if (!dtIssueDate.HasValue)
            errorList.Add("發證日期格式不合法!");

         if(_pageVM.SecondIDType == "O" && string.IsNullOrEmpty(_pageVM.OtherID))
            errorList.Add("其他證件必須填寫!");

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
               custName = _pageVM.CustName,
               custId = _pageVM.PID,
               birthday = _pageVM.Birthday,
               iDIssueDt = _pageVM.IssueDate,
               iDIssueLoc = _pageVM.IssueLoc,
               iDIssueFlag = _pageVM.IssueFlag,
               iDPicFlag = _pageVM.HasPhoto,
               secCert = _pageVM.SecondIDType,
               secCertId = _pageVM.SecondIDSN,
               secCertType = _pageVM.SecondIDType == "O" ? _pageVM.OtherID : ""
            };

            string json = JsonConvert.SerializeObject(model);
            _kernelService.TransactionDataCache.Set(KioskDataCacheKey.IDConfirmData, json);

            _pageVM.Clear();

            PageResult result = new PageResult("Confirm", KioskDataCacheKey.IDConfirmData);
            _kernelService.NextPage(result);
         }
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
      }
   }

   public class DropdownItem : PropertyChangedNotification
   {
      public string Name
      {
         get { return GetValue(() => Name); }
         set { SetValue(() => Name, value); }
      }

      public string Value
      {
         get { return GetValue(() => Value); }
         set { SetValue(() => Value, value); }
      }

      public DropdownItem(string name, string value)
      {
         Name = name;
         Value = value;
      }
   }

   public class Page0150ViewModel : PageViewModel
   {
      public Page0150ViewModel(IKernelService kernelService, string pageName)
          : base(kernelService, pageName)
      {
         IssueFlags = new ObservableCollection<DropdownItem>();
         IssueFlags.Add(new DropdownItem("初發", "1"));
         IssueFlags.Add(new DropdownItem("補發", "2"));
         IssueFlags.Add(new DropdownItem("換發", "3"));

         HasPhotos = new ObservableCollection<DropdownItem>();
         HasPhotos.Add(new DropdownItem("有", "Y"));
         HasPhotos.Add(new DropdownItem("無", "N"));

         SecondIDTypes = new ObservableCollection<DropdownItem>();
         SecondIDTypes.Add(new DropdownItem("健保卡", "H"));
         SecondIDTypes.Add(new DropdownItem("駕照", "D"));
         SecondIDTypes.Add(new DropdownItem("學生證", "S"));
         SecondIDTypes.Add(new DropdownItem("其他", "O"));

         Clear();
      }

      public ObservableCollection<DropdownItem> IssueFlags { get; set; }
      public ObservableCollection<DropdownItem> HasPhotos { get; set; }
      public ObservableCollection<DropdownItem> SecondIDTypes { get; set; }

      public string CustName
      {
         get { return GetValue(() => CustName); }
         set { SetValue(() => CustName, value); }
      }

      public string Birthday
      {
         get { return GetValue(() => Birthday); }
         set { SetValue(() => Birthday, value); }
      }

      [Required(ErrorMessage = "發證日期為必填欄位")]
      public string IssueDate
      {
         get { return GetValue(() => IssueDate); }
         set { SetValue(() => IssueDate, value); }
      }

      [Required(ErrorMessage = "發證地點為必填欄位")]
      public string IssueLoc
      {
         get { return GetValue(() => IssueLoc); }
         set { SetValue(() => IssueLoc, value); }
      }

      public string IssueFlag
      {
         get { return GetValue(() => IssueFlag); }
         set { SetValue(() => IssueFlag, value); }
      }

      public string HasPhoto
      {
         get { return GetValue(() => HasPhoto); }
         set { SetValue(() => HasPhoto, value); }
      }

      public string PID
      {
         get { return GetValue(() => PID); }
         set { SetValue(() => PID, value); }
      }

      public string SecondIDType
      {
         get { return GetValue(() => SecondIDType); }
         set
         {
            SetValue(() => SecondIDType, value);
            OtherIDEnabled = value == "O";
            
            if(value != "O")
               OtherID = "";
         }
      }

      [Required(ErrorMessage = "第二證件卡號為必填欄位")]
      public string SecondIDSN
      {
         get { return GetValue(() => SecondIDSN); }
         set { SetValue(() => SecondIDSN, value); }
      }

      public string OtherID
      {
         get { return GetValue(() => OtherID); }
         set { SetValue(() => OtherID, value); }
      }

      public bool OtherIDEnabled
      {
         get { return GetValue(() => OtherIDEnabled); }
         set { SetValue(() => OtherIDEnabled, value); }
      }

      public DateTime? ConvertIssuedDate()
      {
         if (string.IsNullOrEmpty(IssueDate) || IssueDate.Length != 7)
            return null;

         int year, month, day;
         if (!int.TryParse(IssueDate.Substring(0, 3), out year))
            return null;
         if (!int.TryParse(IssueDate.Substring(3, 2), out month))
            return null;
         if (!int.TryParse(IssueDate.Substring(5, 2), out day))
            return null;

         string datestr = string.Format("{0}/{1}/{2}", year+1911, month, day);
         DateTime dt;
         if (!DateTime.TryParse(datestr, out dt))
            return null;

         return dt;
      }

      public void Clear()
      {
         CustName = "";
         Birthday = "";
         IssueDate = "";
         IssueLoc = "";
         IssueFlag = "";
         HasPhoto = "";
         PID = "";
         SecondIDType = "";
         SecondIDSN = "";
         OtherID = "";
         OtherIDEnabled = false;
      }
   }
}
