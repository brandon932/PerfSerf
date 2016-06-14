using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using PerfSurf.Counters;

namespace PerfSurf.Hubs
{
	public class PerfHub : Hub
	{
		public PerfHub()
		{
			StartCounterCollection();
		}

		private void StartCounterCollection()
		{
			var task = Task.Factory.StartNew(async () =>
			{
				var perfService = new PerfCounterService();
				while (true)
				{
					var results = perfService.GetResults();
					Clients.All.newCounters(results);
					await Task.Delay(2000);
				}
			},TaskCreationOptions.LongRunning);
		}
		public void Send(string message)
		{
			Clients.All.newMessage(Context.User.Identity.Name +" says " + message);
		}
	}
}