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
   /// Page0290.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0290", Description = "開戶設定-開戶完成")]
   public partial class Page0290 : Page
   {
      private Page0290ViewModel _pageVM;
      private readonly IKernelService _kernelService;

      public Page0290(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0290ViewModel(kernelService, "Page0290");

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0290_Loaded;
      }

      private void Page0290_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData185))
         {
            string json = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData185].ToString();
            var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            _pageVM.ApplyATMCard = jsonObj["ApplyATMCard"] == "Y";
         }

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData186))
         {
            string json = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData186].ToString();
            var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            _pageVM.ApplyEBank = jsonObj["ApplyEBank"] == "Y";
            _pageVM.ApplyEMobileService = jsonObj["ApplyEMobileService"] == "Y";
         }
      }
   }

   public class Page0290ViewModel : PageViewModel
   {
      public Page0290ViewModel(IKernelService kernelService, string pageName)
          : base(kernelService, pageName)
      {
         Clear();
      }

      public bool ApplyATMCard
      {
         get { return GetValue(() => ApplyATMCard); }
         set
         {
            SetValue(() => ApplyATMCard, value);

            BrushConverter bc = new BrushConverter();

            if (value)
               ApplyEBankBKBrush = (Brush)bc.ConvertFromString("#e2e2e2");
            else
               ApplyEBankBKBrush = Brushes.Transparent;
         }
      }

      public bool ApplyEBank
      {
         get { return GetValue(() => ApplyEBank); }
         set
         {
            SetValue(() => ApplyEBank, value);

            BrushConverter bc = new BrushConverter();

            if (value && ApplyATMCard)
               ApplyEMobileServiceBKBrush = Brushes.Transparent; 
            else if(value && !ApplyATMCard)
               ApplyEMobileServiceBKBrush = (Brush)bc.ConvertFromString("#e2e2e2");
            else if(!value && !ApplyATMCard)
               ApplyEMobileServiceBKBrush = (Brush)bc.ConvertFromString("#e2e2e2");
            else if(!value && !ApplyATMCard)
               ApplyEMobileServiceBKBrush = Brushes.Transparent;
         }
      }

      public bool ApplyEMobileService
      {
         get { return GetValue(() => ApplyEMobileService); }
         set { SetValue(() => ApplyEMobileService, value); }
      }

      public Brush ApplyEBankBKBrush
      {
         get { return GetValue(() => ApplyEBankBKBrush); }
         set { SetValue(() => ApplyEBankBKBrush, value); }
      }

      public Brush ApplyEMobileServiceBKBrush
      {
         get { return GetValue(() => ApplyEMobileServiceBKBrush); }
         set { SetValue(() => ApplyEMobileServiceBKBrush, value); }
      }

      public void Clear()
      {
         ApplyATMCard = false;
         ApplyEBank = false;
         ApplyEMobileService = false;
      }
   }
}
