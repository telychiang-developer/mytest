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
using System.Windows.Threading;

namespace iSTMC.PageView
{
   /// <summary>
   /// ValidationError.xaml 的互動邏輯
   /// </summary>
   public partial class ValidationErrorDialog : UserControl
   {
      private ScalingModal _scalingModal;
      private DispatcherTimer _timer;

      public int CountDown
      {
         get { return (int)GetValue(CountDownProperty); }
         set { SetValue(CountDownProperty, value); }
      }

      public static readonly DependencyProperty CountDownProperty =
          DependencyProperty.Register("CountDown", typeof(int), typeof(ValidationErrorDialog), new PropertyMetadata(3));


      public string[] Errors { get; set; }

      public ValidationErrorDialog(string[] errors, ScalingModal modal)
      {
         InitializeComponent();

         Errors = errors;
         _scalingModal = modal;

         _timer = new DispatcherTimer();

         CountDown = 3;

         this.DataContext = this;

         _timer.Interval = TimeSpan.FromSeconds(1);
         _timer.Tick += (s, e) =>
         {
            this.CountDown--;
            if (this.CountDown == 0)
            {
               _timer.Stop();

               _scalingModal.Collapse(ScalingModalExpandCollapseAnimation.SlideB);
            }
         };

         this.Loaded += PopUpUserControl_Loaded;
      }

      private void PopUpUserControl_Loaded(object sender, RoutedEventArgs e)
      {
         btnClose.Focus();
         _timer.Start();
      }

      private void btnClose_Click(object sender, RoutedEventArgs e)
      {
         _timer.Stop();

         _scalingModal.Collapse(ScalingModalExpandCollapseAnimation.SlideB);
      }
   }
}
