using iSTMC.Common;
using iSTMC.Kernel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
   /// Home.xaml 的互動邏輯
   /// </summary>
   [PageView("Home", Description = "首頁-立即開戶")]
   public partial class Home : Page
   {
      private int secretCount = 0;

      private DateTime? beginSecretClick;

      private HomePageViewModel _pageVM;
      private readonly IKernelService _kernelService;

      public Home(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new HomePageViewModel(kernelService, "Home");
         _pageVM.Clear();

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Home_Loaded;
      }

      private void Home_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();
      }

      private void btnNewAccount_Click(object sender, RoutedEventArgs e)
      {
         string accType = "";
         if (_pageVM.TaiwanCurrencyAccount && _pageVM.ForeignCurrencyAccount)
            accType = "3";
         else if (_pageVM.TaiwanCurrencyAccount && !_pageVM.ForeignCurrencyAccount)
            accType = "1";
         else if (!_pageVM.TaiwanCurrencyAccount && _pageVM.ForeignCurrencyAccount)
            accType = "2";

         var model = new
         {
            accType
         };

         string json = JsonConvert.SerializeObject(model);
         _kernelService.TransactionDataCache.Set(KioskDataCacheKey.NewAccountType, json);
         PageResult result = new PageResult("Confirm", KioskDataCacheKey.NewAccountType);
         _kernelService.NextPage(result);
      }

      private void Image_MouseDown(object sender, MouseButtonEventArgs e)
      {
         if (!_kernelService.TestMode)
            return;

         if (secretCount == 0)
            beginSecretClick = DateTime.Now;

         secretCount++;

         if (secretCount >= 5)
         {
            secretCount = 0;

            if ((DateTime.Now - beginSecretClick.Value).TotalSeconds <= 3)
            {
               if (MessageBox.Show("是否結束系統", "結束系統", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
               {
                  var window = this.Parent as NavigationWindow;
                  if (window != null)
                     window.Close();
               }
            }
         }
      }
   }

   public class HomePageViewModel : PageViewModel
   {
      public HomePageViewModel(IKernelService kernelService, string pageName)
          : base(kernelService, pageName)
      {
         TaiwanCurrencyAccount = true;
         ForeignCurrencyAccount = false;
      }

      public bool TaiwanCurrencyAccount
      {
         get { return GetValue(() => TaiwanCurrencyAccount); }
         set
         {
            SetValue(() => TaiwanCurrencyAccount, value);
            CanNewAccount = TaiwanCurrencyAccount || ForeignCurrencyAccount;
         }
      }

      public bool ForeignCurrencyAccount
      {
         get { return GetValue(() => ForeignCurrencyAccount); }
         set
         {
            SetValue(() => ForeignCurrencyAccount, value);
            CanNewAccount = TaiwanCurrencyAccount || ForeignCurrencyAccount;
         }
      }

      public bool CanNewAccount
      {
         get { return GetValue(() => CanNewAccount); }
         set { SetValue(() => CanNewAccount, value); }
      }

      public void Clear()
      {
         TaiwanCurrencyAccount = true;
         ForeignCurrencyAccount = false;
         CanNewAccount = TaiwanCurrencyAccount || ForeignCurrencyAccount;
      }
   }
}
