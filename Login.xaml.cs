using NetworkMan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
		public ChatClient client = new ChatClient();
		public static UdpClient u;
		public static string username;
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
				SendMessage(u, PacketId.Login, text_username.Text + " " + text_pass.Password);
				if (ChatClient.Retrieve(u, "SUCCESS"))
				{
					username = text_username.Text;
					Close();
				}
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
				u = client.Init(host, 8000);
				u.Send(BitConverter.GetBytes((int)PacketId.Status), 4);
				connected = true;
			}
			catch { }
			finally
			{
				if (connected)
				{
					if (ChatClient.Retrieve(u, "SUCCESS"))
					{
						label_status.Content = "Status: connected";
					}
				}
			}
		}

		/// <summary>
		/// Listen for the Login class's instance of UdpClient a "SUCCESS" message.
		/// </summary>
		/// <returns>Did it receive a proper message.</returns>
		public static bool Success()
		{
			IPEndPoint end = new IPEndPoint(IPAddress.Any, 0);
			return Encoding.UTF8.GetString(u.Receive(ref end), 0, "SUCCESS".Length) == "SUCCESS";
		}

		/// <summary>
		/// Listen for a "SUCCESS" message.
		/// </summary>
		/// <param name="u">The UdpClient to use.</param>
		/// <returns>Did it receive a proper message.</returns>
		public static bool Success(UdpClient u)
		{
			IPEndPoint end = new IPEndPoint(IPAddress.Any, 0);
			return Encoding.UTF8.GetString(u.Receive(ref end), 0, "SUCCESS".Length) == "SUCCESS";
		}

		/// <summary>
		/// Send the Login class's UdpClient instance a textual message.
		/// </summary>
		/// <param name="id">The packet enum.</param>
		/// <param name="message">Proper text message to send.</param>
		public static void SendMessage(PacketId id, string message)
		{
			byte[] buffer = Packet.ConstructPacketData((int)id, message: message);
			u.Send(buffer, buffer.Length);
		}

		public static void SendMessage(UdpClient client, PacketId id, string message)
		{
			byte[] buffer = Packet.ConstructPacketData((int)id, message: message);
			client.Send(buffer, buffer.Length);
		}

		public static string GetMessage()
		{
			IPEndPoint end = new IPEndPoint(IPAddress.Any, 0);
			return Encoding.UTF8.GetString(u.Receive(ref end));
		}

		public static byte[] ReceiveData()
		{
		//  | Based on packet being sent respond with that differently here; |
		//  | Currently defautls to sending "Message" packet.				 |
		//	v  																 v
			IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            return u.Receive(ref RemoteIpEndPoint);
		//	DEBUG
		//	Console.WriteLine("Received: " + new Packet(receiveBytes).GetMessage());
		}

		public static byte[] ReceiveData(UdpClient client)
		{
		//  | Based on packet being sent respond with that differently here; |
		//  | Currently defautls to sending "Message" packet.				 |
		//	v  																 v
			IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            return client.Receive(ref RemoteIpEndPoint);
		//	DEBUG
		//	Console.WriteLine("Received: " + new Packet(receiveBytes).GetMessage());
		}
	}
}
