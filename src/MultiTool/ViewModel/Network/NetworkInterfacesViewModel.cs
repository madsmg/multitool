using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;

namespace MultiTool.ViewModel.Network
{
   public class NetworkInterfacesViewModel : IDisposable
   {
      private ObservableCollection<NetworkInterfaceViewModel> _adapters = new ObservableCollection<NetworkInterfaceViewModel>();
      private DispatcherTimer _timer;
      private object m_SyncObject = new object();

      #region Properties

      public ICollectionView AdaptersView { get; set; }

      public string Filter
      {
         get
         {
            return _filter;
         }
         set
         {
            if (_filter != value)
            {
               _filter = value;

               AdaptersView.Refresh();
            }
         }
      }

      private string _filter;

      public bool HideDown
      {
         get
         {
            return _hideDown;
         }
         set
         {
            if (_hideDown != value)
            {
               _hideDown = value;
               AdaptersView.Refresh();
            }
         }
      }

      private bool _hideDown = false;

      public bool HideLoopBack
      {
         get
         {
            return _hideLoopBack;
         }
         set
         {
            if (_hideLoopBack != value)
            {
               _hideLoopBack = value;
               AdaptersView.Refresh();
            }
         }
      }

      private bool _hideLoopBack = true;

      public bool HideIpV6Addresses
      {
         get
         {
            return _hideIpV6Addresses;
         }
         set
         {
            if (_hideIpV6Addresses != value)
            {
               _hideIpV6Addresses = value;
               RefreshNetworkInterfaces();
            }
         }
      }

      private bool _hideIpV6Addresses = true;

      #endregion

      public NetworkInterfacesViewModel()
      {
         AdaptersView = new CollectionViewSource() { Source = _adapters }.View;

         AdaptersView.Filter = nic =>
         {
            var adapter = nic as NetworkInterfaceViewModel;

            if (adapter != null)
            {
               if (HideDown && adapter.NetworkInterface.OperationalStatus == OperationalStatus.Down)
               {
                  return false;
               }

               if (HideLoopBack && adapter.NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
               {
                  return false;
               }

               return string.IsNullOrEmpty(Filter) ||
                  adapter.NetworkInterface.Name.IndexOf(Filter, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                  adapter.NetworkInterface.Description.IndexOf(Filter, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                  adapter.NetworkInterface.GetIPProperties().UnicastAddresses.Any(a => a.Address.ToString().IndexOf(Filter, StringComparison.InvariantCultureIgnoreCase) >= 0);
            }
            else
            {
               return false;
            }
         };

         _timer = new DispatcherTimer();
         _timer.Interval = TimeSpan.FromSeconds(2);
         _timer.Tick += (o, e) =>
         {
            RefreshNetworkInterfaces();
         };

         RefreshNetworkInterfaces();
      }

      public void StartRefresh()
      {
         _timer.Start();
         Console.WriteLine("Started!");
      }

      public void StopRefresh()
      {
         _timer.Stop();
         Console.WriteLine("Stopped!");
      }

      public void RefreshNetworkInterfaces()
      {
         var currentSelectedItem = AdaptersView.CurrentItem;

         var adapters = NetworkInterface.GetAllNetworkInterfaces();

         foreach( var removed in _adapters.Where(p => !adapters.Any(p2 => p2.Id == p.NetworkInterface.Id)).ToList())
         {
            _adapters.Remove(removed);
         }

         foreach (NetworkInterface adapter in adapters)
         {
            var adapterToUpdate = _adapters.FirstOrDefault(a => a.NetworkInterface.Id == adapter.Id);

            if (adapterToUpdate != null)
            {
               adapterToUpdate.NetworkInterface = adapter;
               adapterToUpdate.HideIpV6Addresses = HideIpV6Addresses;
            }
            else
            {
               _adapters.Add(new NetworkInterfaceViewModel(adapter) { HideIpV6Addresses = HideIpV6Addresses } );
            }
         }

         AdaptersView?.Refresh();
         AdaptersView?.MoveCurrentTo(currentSelectedItem);
      }

      #region IDisposable members

      /// <summary>
      /// Finalizer for waitable queue as part of IDisposable pattern.
      /// </summary>
      ~NetworkInterfacesViewModel()
      {
         Cleanup();
      }

      /// <summary>
      /// Gets whether the object has been disposed.
      /// </summary>
      private bool Disposed
      {
         get
         {
            lock (m_SyncObject)
            {
               return m_Disposed;
            }
         }
      }
      bool m_Disposed = false;

      /// <summary>
      /// Disposes object.
      /// </summary>
      public void Dispose()
      {
         lock (m_SyncObject)
         {
            if (m_Disposed == false)
            {
               Cleanup();
               m_Disposed = true;
               GC.SuppressFinalize(this);
            }
         }
      }

      /// <summary>
      /// Performs the necessary clean-up of disposable members.
      /// </summary>
      private void Cleanup()
      {
         if (_timer != null)
         {
            _timer.Stop();
            _timer = null;
         }
      }

      #endregion
   }
}
