using NetworkMan;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hereness
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			new Login().ShowDialog();
			InitializeComponent();
		}

		private void button_send_Click(object sender, RoutedEventArgs e)
		{
			Login.SendMessage(PacketId.Message, this.text_send.Text);
			text_send.Clear();
			DisplayMessage("default", "#000000", Login.GetMessage());
		}
		public void DisplayMessage(string Username, string UserColor, string Message)
		{
			Bold item = new Bold(new Run(Username))
			{
				Foreground = (Brush)new BrushConverter().ConvertFromString(UserColor)
			};
			text_chatlog.Inlines.Add(item);
			text_chatlog.Inlines.Add(string.Format("{0} {1}{2}", new object[3] { ":", Message, "\n" }));
			richtextbox.ScrollToEnd();
		}
	}
}