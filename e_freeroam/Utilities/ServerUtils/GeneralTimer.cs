using e_freeroam.Objects;
using e_freeroam.Server_Properties;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace e_freeroam.Utilities.PlayerUtils
{
	public abstract class GeneralTimer
	{
		private static byte lastSaved = 0;
		private static ushort[] lastOccupied = new ushort[Vehicle2.getMaxVehicles()];
		private static ushort[] lastPlayerSave = new ushort[NAPI.Server.GetMaxPlayers()];
		private static ushort[] busProfit = new ushort[Business.getMaxBusinesses()];
		private static bool isOdd = false, wasAnyoneOnlineLastCheck = true;

		private static Stopwatch timer = new Stopwatch();
		private static long increments = 0, averageTime = 0;

		public static void runTimer(long delay)
		{
			NAPI.Task.Run(() =>
			{
				timer.Reset();
				timer.Start();

				if(++lastSaved == 60) ServerData.saveServerData(); // Save server info timer
				isOdd = ((lastSaved % (byte) 2) != 0);
				if(lastSaved >= 120) lastSaved = 0;

				long ms = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
				if(ServerData.haveServerDataListsBeenInitialized())
				{

					ushort maxBusinesses = Business.getMaxBusinesses(), maxPlayers = (ushort) NAPI.Server.GetMaxPlayers(), maxVehicles = Vehicle2.getMaxVehicles();
					ushort iterations = (maxBusinesses > maxPlayers && maxBusinesses > maxVehicles) ? maxBusinesses : (maxPlayers > maxVehicles) ? maxPlayers : maxVehicles;

					Business bus = null;
					PlayerData data = null;
					Player player = null;
					ushort playerid = ushort.MaxValue;
					Vehicle2 vehicle = null;
					ushort vehicleid = ushort.MaxValue;
					NetHandle veh;
					Player driver = null;


					for(ushort i = 0; i < iterations; i++) // Replacing foreach loops
					{
						if(iterations < maxBusinesses)
						{
							bus = ServerData.getBusiness((byte) i);
							byte busID = bus.getBusinessID();

							busProfit[busID]++;

							if(busProfit[busID] >= 1800) // Business profit generation
							{
								bus.updateBusinessProfit((uint) (bus.getBusinessProfit() + bus.getBusinessOutput()));

								player = NAPI.Player.GetPlayerFromName(bus.getOwnerName());
								if(player !=  null) ChatUtils.sendClientMessage(player, ServerData.COLOR_LBLUE, "~ You have earned from your business; return to collect it.");
								ServerData.logToConsole($"Business profit for {bus.getBusinessName()} ({bus.getBusinessID()}) has been updated to ${bus.getBusinessProfit()}");
						
								busProfit[busID] = 0;
							}
						}

						if(!ServerData.isAnyoneOnline())
						{
							if(wasAnyoneOnlineLastCheck)
							{
								wasAnyoneOnlineLastCheck = false;
								ServerData.logToConsole("There is nobody online to iterate on the global timer.");
							}
							break;
						}
						else if(!wasAnyoneOnlineLastCheck)
						{
							wasAnyoneOnlineLastCheck = true;
							ServerData.logToConsole("There is now somebody online to iterate on the global timer.");
						}

						if(iterations < maxPlayers)
						{
							data = PlayerDataInfo.getPlayerDataFromID(i);
							if(data == null || !data.isPlayerLoggedIn()) continue;

							player = data.getPlayer();
							if(!NAPI.Player.IsPlayerConnected(player)) 
							{
								ServerData.logToConsole($"Removing player data for {PlayerDataInfo.getPlayerName(player)}.");
								PlayerDataInfo.removePlayerData(data);
								continue;
							}

							playerid = (ushort) player.Value;

							if(!isOdd) 
							{ 
								data.updatePlayerScoreCounter((ushort) (data.getPlayerScoreCounter() + 1));
								if(data.getPlayerScoreCounter() >= 3600) // Level up
								{
									data.updatePlayerScoreCounter(0);
									data.updatePlayerScore((ushort) (data.getPlayerScore() + 1));

									data.updatePlayerMoney(data.getPlayerMoney() + 1000);

									ChatUtils.sendClientMessage(player, ServerData.COLOR_WHITE, $"You have leveled up! Your new level and hours played is {data.getPlayerScore()}");
								}
								if(data.isMuted()) data.updateMutedTime((ushort) (data.getMutedTime() - 1)); // Mute time recession

								if(data.getFloodTimer() > 0) data.updateFloodTimer((byte) (data.getFloodTimer() - 1)); // Flooding detection

								if(++lastPlayerSave[playerid] % 60 == 0) // Save user data timer
								{
									data.savePlayerData();
									lastPlayerSave[playerid] = 0;
								}
							}
						}

						if(iterations < maxVehicles)
						{
							vehicle = ServerData.getVehicleFromID(i);
							if(vehicle == null) continue;

							veh = vehicle.getVehicle().Handle;
							vehicleid = vehicle.getID();
							driver = (Player) NAPI.Vehicle.GetVehicleDriver(veh);

							if(vehicle.getVehicleType() == VehicleType.SERVER_VEHICLE || vehicle.getVehicleType() == VehicleType.ORG_VEHICLE) // Respawn unoccupied server vehicles
							{
								if(NAPI.Vehicle.GetVehicleOccupants(veh).Count != 0) 
								{
									lastOccupied[vehicleid] = 0;
									if(driver != null)
									{
										if(vehicle.getVehicleFuel() <= 0)
										{
											vehicle.stopEngine();
											ChatUtils.sendClientMessage(driver, ServerData.COLOR_RED, "~ Vehicle ran out of fuel.");
										}
									}
								}
								else if(lastOccupied[vehicleid]++ >= 1200) 
								{ 
									vehicle.respawnVeh();
									lastOccupied[vehicleid] = 0;
								}
							}

							// Update gauge cluster info
							if(NAPI.Vehicle.GetVehicleDriver(veh) != null || vehicle.getEngineStatus() == true) vehicle.updateVehicleSpeedValue(NAPI.Entity.GetEntityVelocity(veh));
							if(vehicle.getEngineStatus() == true) vehicle.setVehicleFuel(vehicle.getVehicleFuel() - (float) Math.Round(0.0001F * (vehicle.getVehicleSpeed() / 5F), 4));
						}
					}
				}

				timer.Stop();
				increments++;
				averageTime = (averageTime + timer.ElapsedTicks) / 2;

				if(increments > 240)
				{
					ServerData.logToConsole($"General timer running {averageTime} ticks.");
					increments = 0;
				}

				runTimer(500 - (System.DateTimeOffset.Now.ToUnixTimeMilliseconds() - ms));
			}, delayTime: delay);
		}
	}
}