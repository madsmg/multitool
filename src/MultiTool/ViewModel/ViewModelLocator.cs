/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:DoManagerMui"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System;
using Autofac;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MultiTool.ViewModel.Network;
using MultiTool.ViewModel.SystemInfo;

namespace MultiTool.ViewModel
{

   /// <summary>
   /// This class contains static references to all the view models in the
   /// application and provides an entry point for the bindings.
   /// </summary>
   public class ViewModelLocator
   {
      private IContainer _container;

      /// <summary>
      /// Initializes a new instance of the ViewModelLocator class.
      /// </summary>
      public ViewModelLocator()
      {

         var builder = new ContainerBuilder();
         builder.RegisterType<MainViewModel>();
         builder.RegisterType<WelcomeViewModel>();
         builder.RegisterType<NetworkScanViewModel>();
         builder.RegisterType<NetworkInterfacesViewModel>();
         builder.RegisterType<SystemInfoViewModel>();
         _container = builder.Build();
      }


      public MainViewModel Main
      {
         get
         {
            return _container.Resolve<MainViewModel>();
         }
      }

      public WelcomeViewModel Welcome
      {
         get
         {
            return _container.Resolve<WelcomeViewModel>();
         }
      }

      public NetworkScanViewModel NetworkScan
      {
         get
         {
            return _container.Resolve<NetworkScanViewModel>();
         }
      }

      public NetworkInterfacesViewModel NetworkInterfaces
      {
         get
         {
            return _container.Resolve<NetworkInterfacesViewModel>();
         }
      }

      public SystemInfoViewModel SystemInfo
      {
         get
         {
            return _container.Resolve<SystemInfoViewModel>();
         }
      }

      public static void Cleanup()
      {
         // TODO Clear the ViewModels
      }
   }

}
