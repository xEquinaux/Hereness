using System.Configuration;
using System.Data;
using System.Windows;

namespace Hereness
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			if (!new Login().ShowDialog().Value)
			{
				Application.Current.Shutdown(0);
			}
		}
	}
}
