using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MultiTool.Views.Converters
{
   public class NetworkInterfaceToIPAddressListConverter : IMultiValueConverter
   {
      public object Convert(object[] values, Type targetType,
            object parameter, CultureInfo culture)
      {
         if (values[0] is NetworkInterface)
         {
            var sb = new StringBuilder();
            var nic = values[0] as NetworkInterface;
            bool hideIpV6 = (bool)values[1];

            foreach (var ip in nic.GetIPProperties().UnicastAddresses)
            {
               //if ((nic.OperationalStatus == OperationalStatus.Up)
               //    && (ip.Address.AddressFamily == AddressFamily.InterNetwork))
               {
                  if (hideIpV6 && ip.Address.AddressFamily == AddressFamily.InterNetworkV6 )
                  {
                  }
                  else
                  {
                     sb.AppendLine(ip.Address.ToString());
                  }
               }
            }

            return sb.ToString().Trim();
         }
         else
         {
            return "";
         }
      }

      public object[] ConvertBack(object value, Type[] targetTypes,
            object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}

