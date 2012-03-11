using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Text;
using SharedClasses;

class PipeClient
{
	static void Main(string[] args)
	{
		Console.WriteLine("Client application");

		NamedPipesInterop.NamedPipeClient client = new NamedPipesInterop.NamedPipeClient();
		client.OnError += (s, e) => { Console.WriteLine("Error occured: " + e.GetException().Message); };
		client.OnMessageReceived += (s, m) =>
		{
			if (m.MessageType == PipeMessageTypes.AcknowledgeClientRegistration)
				Console.WriteLine("Client successfully registered.");
			else
			{
				Console.WriteLine(m.MessageType.ToString() + ": " + (m.AdditionalText ?? ""));
			}
		};
		client.Start();
	}
}