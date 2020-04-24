using NSKiosk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NSKiosk.PageView
{
    public class PageViewBase : Page
    {
        private ConfirmBackHome _popup;

        protected virtual void OnModalClosed(bool bConfirmed)
        {

        }

        public PageViewBase()
        {
        }

        public void InitializeScalingModal(Panel parentPanel, Panel panelToBeDisabled)
        {
            ScalingModal.Instance.Initialize(parentPanel, panelToBeDisabled);
            ScalingModal.Instance.Animation = ScalingModalExpandCollapseAnimation.SlideB;
            ScalingModal.Instance.BackgroundOpacity = 0.7;
        }

        public void GoHome()
        {
            _popup = new ConfirmBackHome();
            ScalingModal.Instance.Closed += Instance_Closed;
            ScalingModal.Instance.Expand(_popup);
        }

        private void Instance_Closed(object sender, EventArgs e)
        {
            ScalingModal.Instance.Closed -= Instance_Closed;

            OnModalClosed(_popup.IsConfirmed);

            if (_popup.IsConfirmed)
            {
                PageResult result = new PageResult("Home");
                _kernelService.NextPage(result);
            }
        }
    }
}
