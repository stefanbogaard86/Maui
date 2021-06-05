using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Controls = Microsoft.Maui.Controls;

namespace CommunityToolkit.Maui.Sample.Views.Base
{
    class BaseNavigationPage : Controls.NavigationPage
    {
        public BaseNavigationPage(Controls.Page root)
            : base(root)
        {
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);
            On<iOS>().DisableTranslucentNavigationBar();
            On<iOS>().SetHideNavigationBarSeparator(true);
        }
    }
}