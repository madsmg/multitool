using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MultiTool.ViewModel.Network
{
   public class NetworkInterfaceViewModel : INotifyPropertyChanged
   {
      #region Properties 

      public NetworkInterface NetworkInterface
      {
         get
         {
            return _networkInterface;
         }
         set
         {
            _networkInterface = value;

            NotifyPropertyChanged(nameof(NetworkInterface));
            NotifyPropertyChanged(nameof(IpAddresses));
            NotifyPropertyChanged(nameof(IpSubnetMasks));
            NotifyPropertyChanged(nameof(IpDefaultGateways));
            NotifyPropertyChanged(nameof(IpDnsServers));
            NotifyPropertyChanged(nameof(IsDhcp));
         }
      }

      private NetworkInterface _networkInterface;

      public bool HideIpV6Addresses { get; set; } = true;

      public string IpAddresses
      {
         get
         {
            var sb = new StringBuilder();

            foreach (var ip in NetworkInterface.GetIPProperties().UnicastAddresses)
            {
               if ( !HideIpV6Addresses || !(HideIpV6Addresses&& ip.Address.AddressFamily == AddressFamily.InterNetworkV6 ) )
               {
                  sb.AppendLine(ip.Address.ToString());
               }
            }

            return sb.ToString().Trim();
         }
      }

      public string IpSubnetMasks
      {
         get
         {
            var sb = new StringBuilder();

            foreach (var ip in NetworkInterface.GetIPProperties().UnicastAddresses)
            {
               if (!HideIpV6Addresses || !(HideIpV6Addresses && ip.Address.AddressFamily == AddressFamily.InterNetworkV6))
               {
                  sb.AppendLine(ip.IPv4Mask.ToString());
               }
            }

            return sb.ToString().Trim();
         }
      }

      public string IpDefaultGateways
      {
         get
         {
            var sb = new StringBuilder();

            foreach (var gwy in NetworkInterface.GetIPProperties().GatewayAddresses)
            {
               if (!HideIpV6Addresses || !(HideIpV6Addresses && gwy.Address.AddressFamily == AddressFamily.InterNetworkV6))
               { 
                  sb.AppendLine(gwy.Address.ToString());
               }
            }

            return sb.ToString().Trim();
         }
      }
      public string IpDnsServers
      {
         get
         {
            var sb = new StringBuilder();

            foreach (var ip in NetworkInterface.GetIPProperties().DnsAddresses)
            {
               if (!HideIpV6Addresses || !(HideIpV6Addresses && ip.AddressFamily == AddressFamily.InterNetworkV6))
               {
                  sb.AppendLine(ip.ToString());
               }
            }

            return sb.ToString().Trim();
         }
      }

      public string IsDhcp
      {
         get
         {
            try
            {
               if (NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel ) // GetIpV4Properties not supported on tunnel interfaces!
               {
                  return NetworkInterface.GetIPProperties().GetIPv4Properties().IsDhcpEnabled ? "Yes" : "No";
               }

               return "";

            }
            catch
            {
               return "";
            }
         }
      }

      public string MACAddress
      {
         get
         {
            return BitConverter.ToString(NetworkInterface.GetPhysicalAddress().GetAddressBytes() );
         }
      }

      #endregion 

      public NetworkInterfaceViewModel( NetworkInterface nic )
      {
         NetworkInterface = nic;
      }

      #region INotifyPropertyChanged Members

      public event PropertyChangedEventHandler PropertyChanged;

      private void NotifyPropertyChanged(string propertyName = "")
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      #endregion

   }
}
