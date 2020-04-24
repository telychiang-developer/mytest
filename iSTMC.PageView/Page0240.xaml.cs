using iSTMC.Common;
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
   /// Page0240.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0240", Description = "開戶設定-密碼設定-臨櫃通提密碼")]
   public partial class Page0240 : Page
   {
      private Page0240ViewModel _pageVM;
      private readonly IKernelService _kernelService;

      public Page0240(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0240ViewModel(kernelService, "Page0240");

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0240_Loaded;
      }

      private void Page0240_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.NewAccountType))
         {
            string json = _kernelService.TransactionDataCache[KioskDataCacheKey.NewAccountType].ToString();
            var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            _pageVM.AccountType = jsonObj["accType"];
         }

         _pageVM.ResetTimer();
         _pageVM.StartTimer();
      }

      private void btnConfirm_Click(object sender, RoutedEventArgs e)
      {
         List<string> errorList = new List<string>();
         PageViewUtils.GetErrors(errorList, grdForm);

         if (_pageVM.UniPassword.Length < 4)
            errorList.Add("通提密碼長度必須為4-6碼!");

         int pswd;
         if (int.TryParse(_pageVM.UniPassword, out pswd))
         {
            if(pswd == 0)
               errorList.Add("通提密碼不可全部為0!");
         }
         else
         {
            errorList.Add("通提密碼必須全為數字");
         }

         if (_pageVM.UniPassword != _pageVM.ConfirmPassword)
         errorList.Add("通提密碼與確認密碼不相符!");

         if (errorList.Count > 0)
         {
            ScalingModal sm = new ScalingModal(MainGrid, InnerGrid);
            var popup = new ValidationErrorDialog(errorList.ToArray(), sm);
            sm.Expand(popup, ScalingModalExpandCollapseAnimation.SlideB);

            _pageVM.Clear();
         }
         else
         {
            _pageVM.StopTimer();

            byte[] bytPswd = Encoding.ASCII.GetBytes(_pageVM.ConfirmPassword);

            var model = new
            {
                takePwd = _pageVM.AccountType == "1" || _pageVM.AccountType == "3" ? Convert.ToBase64String(bytPswd) : "",
                ftakePwd = _pageVM.AccountType == "2" || _pageVM.AccountType == "3" ? Convert.ToBase64String(bytPswd) : "",
            };

            _pageVM.Clear();

            string json = JsonConvert.SerializeObject(model);
            _kernelService.TransactionDataCache.Set(KioskDataCacheKey.UniPassword, json);

            PageResult result = new PageResult("Confirm", KioskDataCacheKey.UniPassword);
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

   public class Page0240ViewModel : PageViewModel
   {
      public Page0240ViewModel(IKernelService kernelService, string pageName)
          : base(kernelService, pageName)
      {
      }

      [Required(ErrorMessage = "通提密碼為必填欄位")]
      public string UniPassword
      {
         get { return GetValue(() => UniPassword); }
         set { SetValue(() => UniPassword, value); }
      }

      [Required(ErrorMessage = "確認密碼為必填欄位")]
      public string ConfirmPassword
      {
         get { return GetValue(() => ConfirmPassword); }
         set { SetValue(() => ConfirmPassword, value); }
      }

      public string AccountType
      {
         get { return GetValue(() => AccountType); }
         set
         {
            SetValue(() => AccountType, value);
         }
      }

      public void Clear()
      {
         UniPassword = "";
         ConfirmPassword = "";
      }
   }
}
