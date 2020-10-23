using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace iRacingOverlayService.Hubs
{
	public interface IStandingsHub
	{
		Task ShowTime(string dateTime);
	}

	public class StandingsHub : Hub<IStandingsHub>
	{
		public async Task SendTimeToClients(string dateTime)
		{
			await Clients.All.ShowTime(dateTime);
		}
	}
}
