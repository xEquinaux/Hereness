using NetworkMan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hereness
{
	/// <summary>
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class Login : Window
	{
		public Client client = new Client();
		public string host;
		public bool connected = false;
		public Login()
		{
			InitializeComponent();
		}

		private void button_login_Click(object sender, RoutedEventArgs e)
		{
			if (connected)
			{
				Close();
			}
        }

		private void text_ip_LostFocus(object sender, RoutedEventArgs e)
		{
			host = text_ip.Text;
			try
			{
				if (Dns.GetHostAddresses(host)[0] == default && !IPAddress.TryParse(host, out IPAddress? _))
				{
					return;
				}
			}
			catch { return; }
			try
			{
				client.StartChat(host, 8000);
				connected = true;
			}
			catch { }
			finally
			{
				if (connected)
				{
					label_status.Content = "Status: connected";
				}
			}
		}
	}
}
