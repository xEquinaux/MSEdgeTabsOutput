using System.Text;
using System.Timers;
using System.Windows;


//https://stackoverflow.com/questions/71252013/how-to-get-urls-and-tab-titles-from-edge
namespace MSEdgeTabsOutput
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		System.Timers.Timer timer;
		public MainWindow()
		{
			InitializeComponent();

			timer = new System.Timers.Timer(TimeSpan.FromSeconds(1));
			timer.Elapsed += (e, o) => 
			{
				int num = 0;
				List<string> result = GetTabs.GetAndChangeTabUrl();
				if (result.Count <= 0)
					return;
				string text = result[0];
				label.Dispatcher.Invoke(() => 
				{ 
					this.label.Content = text;
				});
			};
			timer.AutoReset = true;
			timer.Enabled = true;
			timer.Start();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			timer.Stop();
			timer.Dispose();
		}
	}
}