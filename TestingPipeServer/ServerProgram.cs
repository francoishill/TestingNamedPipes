using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using SharedClasses;

class PipeServer
{
	static void Main()
	{
		Console.WriteLine("Server application");

		NamedPipesInterop.NamedPipeServer server1 = new NamedPipesInterop.NamedPipeServer(
			NamedPipesInterop.APPMANAGER_PIPE_NAME,
			ActionOnError: (e) => { Console.WriteLine("Error: " + e.GetException().Message); },
			ActionOnMessageReceived: (m, server) =>
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
			},
			ActionOnClientRegistered: (c) =>
			{
				Console.WriteLine("Client registered");
			}).Start();
	}
}