using FirstFloor.ModernUI.Windows;
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
using FirstFloor.ModernUI.Windows.Navigation;
using MultiTool.ViewModel.Network;

namespace MultiTool.Views.Network
{
   /// <summary>
   /// Interaction logic for NetworkInterfacesView.xaml
   /// </summary>
   public partial class NetworkInterfacesView : UserControl, IContent
   {
      public NetworkInterfacesView()
      {
         InitializeComponent();
      }

      public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
      {
      }

      public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
      {
      }

      public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
      {
         var dc = DataContext as NetworkInterfacesViewModel;
         dc?.StartRefresh();
      }

      public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
      {
         var dc = DataContext as NetworkInterfacesViewModel;
         dc?.StopRefresh();
      }
   }
}
