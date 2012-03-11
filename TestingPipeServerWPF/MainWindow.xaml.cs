using System;
using System.Collections.Generic;
using System.Linq;
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
using SharedClasses;
using System.Threading;

namespace TestingPipeServerWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		NamedPipesInterop.NamedPipeServer server;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void AppendMessage(string mess)
		{
			Dispatcher.BeginInvoke(
			(Action)delegate
			{
				textBox1.Text += (textBox1.Text.Length > 0 ? Environment.NewLine : "") + mess;
			});
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			AppendMessage("Server application");

			server = new NamedPipesInterop.NamedPipeServer(NamedPipesInterop.APPMANAGER_PIPE_NAME);
			server.OnError += (s, er) =>
			{
				AppendMessage(er.GetException().Message);
			};
			server.OnMessageReceived += (s, m) =>
			{
				if (m.MessageType == PipeMessageTypes.ClientRegistrationRequest)
					new Timer(
						delegate
						{
							server.SendMessageToClient(PipeMessageTypes.unknown, "TestingPipeClient", "Hallo sexy");
							server.SendMessageToClient(PipeMessageTypes.unknown, "TestingPipeClient.vshost", "Hallo sexy");
						},
						null,
						TimeSpan.FromSeconds(0),
						TimeSpan.FromSeconds(2));
			};
			server.Run();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//server.Stop();
		}
	}
}
