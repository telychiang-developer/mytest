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
   /// Page0187.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0187", Description = "資料填寫-申請項目-同意聯徵規範與共同行銷條款")]
   public partial class Page0187 : Page
   {
      private Page0187ViewModel _pageVM;
      private readonly IKernelService _kernelService;

      public Page0187(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0187ViewModel(kernelService, "Page0187");

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0187_Loaded;
      }

      private void Page0187_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData187))
         {
            string json = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData187].ToString();
            var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            _pageVM.AgreeTerm = jsonObj["AgreeTerm"] == "Y";
            _pageVM.PromotionTermAgree = jsonObj["PromotionTerm"] == "Y";
            _pageVM.PromotionTermDisagree = jsonObj["PromotionTerm"] == "N";
         }

         _pageVM.ResetTimer();
         _pageVM.StartTimer();
      }

      private void btnPrevious_Click(object sender, RoutedEventArgs e)
      {
         _pageVM.StopTimer();

         var model = new
         {
            AgreeTerm = _pageVM.AgreeTerm ? "Y" : "N",
            ApplyUniPayment = _pageVM.AgreeTerm ? "Y" : "N",             
            PromotionTerm = (_pageVM.PromotionTermAgree && !_pageVM.PromotionTermDisagree) ? "Y" : "N",  
         };

         string json = JsonConvert.SerializeObject(model);
         _kernelService.TransactionDataCache.Set(KioskDataCacheKey.PersonalData187, json);

         _pageVM.Clear();

         PageResult result = new PageResult("Previous", KioskDataCacheKey.PersonalData187);
         _kernelService.NextPage(result);
      }

      private void btnConfirm_Click(object sender, RoutedEventArgs e)
      {
         List<string> errorList = new List<string>();
         PageViewUtils.GetErrors(errorList, grdForm);

         if (!_pageVM.PromotionTermAgree && !_pageVM.PromotionTermDisagree)
            errorList.Add("共同行銷條款必須勾選");

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
               AgreeTerm = _pageVM.AgreeTerm ? "Y" : "N",             
               PromotionTerm = (_pageVM.PromotionTermAgree && !_pageVM.PromotionTermDisagree) ? "Y" : "N",  

               PromotionTermDesc = _pageVM.PromotionTermAgree ? "同意" : "不同意"
            };

            string json = JsonConvert.SerializeObject(model);
            _kernelService.TransactionDataCache.Set(KioskDataCacheKey.PersonalData187, json);

            var model2 = new
            {
               JCreditFlag = _pageVM.AgreeTerm ? "Y" : "N"
            };

            string json2 = JsonConvert.SerializeObject(model2);
            _kernelService.TransactionDataCache.Set(KioskDataCacheKey.SaveJointCredit, json2);

            PageResult result = new PageResult("Confirm", KioskDataCacheKey.PersonalData187);
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

   public class Page0187ViewModel : PageViewModel
   {
      public Page0187ViewModel(IKernelService kernelService, string pageName)
          : base(kernelService, pageName)
      {
         Clear();
      }

      public bool AgreeTerm
      {
         get { return GetValue(() => AgreeTerm); }
         set
         {
            SetValue(() => AgreeTerm, value);
         }
      }

      public bool PromotionTermAgree
      {
         get { return GetValue(() => PromotionTermAgree); }
         set
         {
            SetValue(() => PromotionTermAgree, value);
         }
      }

      public bool PromotionTermDisagree
      {
         get { return GetValue(() => PromotionTermDisagree); }
         set
         {
            SetValue(() => PromotionTermDisagree, value);
         }
      }

      public void Clear()
      {
         AgreeTerm = false;                   
         PromotionTermAgree = false;          
         PromotionTermDisagree = true;        
      }
   }
}