using iSTMC.Common;
using iSTMC.Kernel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
   /// Page0040.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0040", Description = "開戶認證-確認條款-開戶總約定書")]
   public partial class Page0040 : Page
   {
      private Page0040ViewModel _pageVM;
      private readonly IKernelService _kernelService;
      private Vector _totalScale = new Vector(1.0, 1.0);

      public Page0040(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0040ViewModel(kernelService, "Page0040");
         _pageVM.Clear();

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0040_Loaded;
      }

      private void Page0040_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();

         if (_kernelService.TerminalDataCache.ContainsKey("__PdImageFiles"))
         {
            List<string> imgPaths = _kernelService.TerminalDataCache["__PdImageFiles"] as List<string>;

            foreach (var path in imgPaths)
            {
               _pageVM.PageImages.Add(_pageVM.LoadImage(path));
            }
         }

         scrollViewer.ScrollToTop();

         _pageVM.ResetTimer();
         _pageVM.StartTimer();
      }

      private void btnCancel_Click(object sender, RoutedEventArgs e)
      {
         ScalingModal sm = new ScalingModal(MainGrid, InnerGrid);
         var popup = new ConfirmBackHome(sm);

         sm.Closed += ScalingModal_Closed;
         sm.Expand(popup, ScalingModalExpandCollapseAnimation.SlideB);
      }

      private void btnConfirm_Click(object sender, RoutedEventArgs e)
      {
         _pageVM.StopTimer();

         var model = new
         {
            opFlag = _pageVM.IsRead,
            fatcaFlag = _pageVM.IsNotForeginer ? "Y" : "N"
         };

         string json = JsonConvert.SerializeObject(model);
         _kernelService.TransactionDataCache.Set("__2004_8_BodyData", json);

         PageResult result = new PageResult("Confirm", "__2004_8_BodyData");
         _kernelService.NextPage(result);
      }

      private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
      {
         e.Handled = true;
      }

      private void ScrollViewer_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
         Rectangle rect = Mouse.DirectlyOver as Rectangle;
         if (rect != null)
         {
            ScrollViewer sv = sender as ScrollViewer;
            ScrollBar scrollBar = sv.Template.FindName("PART_VerticalScrollBar", sv) as ScrollBar;
            Track track = FindVisualChild<Track>(scrollBar);

            double trackHeight = track.ActualHeight;
            double factor = sv.ScrollableHeight / trackHeight;

            Point p = e.GetPosition(track);

            sv.ScrollToVerticalOffset(p.Y * factor);

         }
      }

      private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
      {
         for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
         {
            DependencyObject child = VisualTreeHelper.GetChild(obj, i);
            if (child != null && child is T)
               return (T)child;
            else
            {
               T childOfChild = FindVisualChild<T>(child);
               if (childOfChild != null)
                  return childOfChild;
            }
         }
         return null;
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

   public class Page0040ViewModel : PageViewModel
   {
      public Page0040ViewModel(IKernelService kernelService, string pageName)
          : base(kernelService, pageName)
      {
         IsRead = false;
         IsNotForeginer = false;
         IsForeginer = false;

         PageImages = new ObservableCollection<ImageSource>();
      }

      public ImageSource LoadImage(string fileName)
      {
         var image = new BitmapImage();

         using (var stream = new FileStream(fileName, FileMode.Open))
         {
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
         }

         return image;
      }

      public ObservableCollection<ImageSource> PageImages { get; set; }

      public bool IsRead
      {
         get { return GetValue(() => IsRead); }
         set
         {
            SetValue(() => IsRead, value);
            CanNewAccount = IsRead && IsNotForeginer;
         }
      }

      public bool IsNotForeginer
      {
         get { return GetValue(() => IsNotForeginer); }
         set
         {
            SetValue(() => IsNotForeginer, value);
            CanNewAccount = IsRead && IsNotForeginer;
         }
      }

      public bool IsForeginer
      {
         get { return GetValue(() => IsForeginer); }
         set
         {
            SetValue(() => IsForeginer, value);
            CanNewAccount = IsRead && IsNotForeginer;
         }
      }

      public bool CanNewAccount
      {
         get { return GetValue(() => CanNewAccount); }
         set { SetValue(() => CanNewAccount, value); }
      }

      public void Clear()
      {
         IsRead = false;
         IsNotForeginer = false;
         IsForeginer = false;
      }
   }
}
