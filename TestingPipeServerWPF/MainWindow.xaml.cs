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
using System.IO;
using System.IO.Pipes;
using System.Diagnostics;

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

			server = new NamedPipesInterop.NamedPipeServer(
			NamedPipesInterop.APPMANAGER_PIPE_NAME,
			ActionOnError: (e1) => { Console.WriteLine("Error: " + e1.GetException().Message); },
			ActionOnMessageReceived: (m, serv) =>
			{
				if (m.MessageType == PipeMessageTypes.ClientRegistrationRequest)
					AppendMessage("Client registered: " + m.AdditionalText);
			}).Start();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			server.Stop();
		}

		private void ButtonClose_Click(object sender, RoutedEventArgs e)
		{
			server.SendMessageToClient(PipeMessageTypes.Close, "TestingPipeClient");
			server.SendMessageToClient(PipeMessageTypes.Close, "TestingPipeClient.vshost");
		}
	}
}
