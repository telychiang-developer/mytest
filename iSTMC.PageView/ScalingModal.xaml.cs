using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace iSTMC.PageView
{
   public enum ScalingModalExpandCollapseAnimation
   {
      None = 0,
      ScaleI,
      ScaleO,
      ScaleV,
      ScaleH,
      RotateCW,
      RotateCCW,
      SlideR,
      SlideL,
      SlideT,
      SlideB,
      Fade,
      Twist,
      Skew1,
      Skew2,
      Skew3,
      Skew4,
      Flip 
   }


   public partial class ScalingModal : UserControl
   {
      private Panel _panelParent;
      private FrameworkElement _panelToDisable;
      private FrameworkElement _innerControl;

      public ScalingModal(Panel parentPanel, Panel panelToBeDisabled)
      {
         InitializeComponent();
         IsHitTestVisible = false;

         _panelToDisable = panelToBeDisabled;
         _panelParent = parentPanel;
         _panelParent.Children.Add(this);
      }

      static ScalingModal()
      {
      }

      public event EventHandler Closed;

      public ScalingModalExpandCollapseAnimation Animation { get; set; }

      public double BackgroundOpacity
      {
         set { DisableRectangle.Opacity = value; }
      }

      public void Initialize(Panel parentPanel, Panel panelToBeDisabled)
      {
         _panelToDisable = panelToBeDisabled;
         _panelParent = parentPanel;
         _panelParent.Children.Add(this);
      }

      public void Expand(FrameworkElement innerControl, ScalingModalExpandCollapseAnimation animationToUse)
      {
         _innerControl = innerControl;

         ModalCanvas.Width = innerControl.Width;
         ModalCanvas.Height = innerControl.Height;
         ModalCanvas.Children.Clear();
         ModalCanvas.Children.Add(innerControl);

         this.IsHitTestVisible = true;

         if (_panelToDisable != null)
            _panelToDisable.IsEnabled = false;

         RunExpandAnimation(animationToUse);
         innerControl.Focus();
      }

      public void Expand(FrameworkElement innerControl)
      {
         Expand(innerControl, Animation);
      }

      public void Collapse(ScalingModalExpandCollapseAnimation animationToUse)
      {
         if (_panelToDisable != null)
            _panelToDisable.IsEnabled = true;
         this.IsHitTestVisible = false;
         RunCollapseAnimation(animationToUse);
      }

      public void Collapse()
      {
         Collapse(Animation);
      }


      private void RunExpandAnimation(ScalingModalExpandCollapseAnimation animation)
      {
         ResetTransformation();
            RunAnimation(this, animation.ToString() + "In");
      }

      private void RunAnimation(ScalingModal scalingModal, string key)
      {
         Storyboard s = Resources[key] as Storyboard;
         s.Begin(this);
      }

      private void ResetTransformation()
      {
         RunAnimation(this, "Reset");
      }

      private void RunCollapseAnimation(ScalingModalExpandCollapseAnimation animation)
      {
         ResetTransformation();
            RunAnimation(this, animation.ToString() + "Out");
      }

      private void expandingOpacityAnimation_Completed(object sender, EventArgs e)
      {
         if (_innerControl != null)
            _innerControl.Focus();
      }

      private void collapsingOpacityAnimation_Completed(object sender, EventArgs e)
      {
         Closed?.Invoke(_innerControl, e);

         _innerControl = null;
         ModalCanvas.Children.Clear();
      }
   }
}
