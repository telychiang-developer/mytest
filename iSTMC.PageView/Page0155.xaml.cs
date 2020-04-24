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
   /// Page0155.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0155", Description = "上傳證件-核對資料中")]
   public partial class Page0155 : Page
   {
      private Page0155ViewModel _pageVM;
      private readonly IKernelService _kernelService;

      public Page0155(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0155ViewModel(kernelService, "Page0155");

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0155_Loaded;
      }

      private void Page0155_Loaded(object sender, RoutedEventArgs e)
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
                  _kernelService.Logger.Error(ex, $"Page0155_Loaded: {ex.Message}");
               }
            }
         }
      }
   }

   public class Page0155ViewModel : PageViewModel
   {
      public Page0155ViewModel(IKernelService kernelService, string pageName)
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

      public string IssueDate
      {
         get { return GetValue(() => IssueDate); }
         set { SetValue(() => IssueDate, value); }
      }

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
         }
      }

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
