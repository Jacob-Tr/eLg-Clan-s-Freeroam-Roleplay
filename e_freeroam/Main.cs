using System;
using System.IO;
using GTANetworkAPI;
using e_freeroam.Utilities.ServerUtils;
using e_freeroam.Objects;
using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using System.Globalization;
using System.Threading;
using e_freeroam.Server_Properties;
using System.Diagnostics;

namespace e_freeroam
{
    public class Main : Script
    {
		// Recursion issue somewhere in NumberUtils causing Stack Overflow.

        public Main()
        {
			ServerData.toggleServerDebug(true);

			NAPI.Server.SetGlobalServerChat(false);
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

			ServerData.initializeServerDataLists();
			PlayerDataInfo.initializePlayerList();

			ServerData.loadServerData();
            ServerData.loadVehicles();

			Stopwatch timer = new Stopwatch();
			string integer = "5948392";
			bool failed = false;

			ServerData.logToConsole($"Input: {integer}");
			timer.Start();
			for(ushort i = 0; i < ushort.MaxValue; i++) {int.Parse(integer);}
			ServerData.logToConsole($"int.Parse takes {timer.ElapsedTicks/ushort.MaxValue}");
			timer.Restart();
			for(ushort i = 0; i < ushort.MaxValue; i++) {NumberUtils.parseInt(integer, ref failed);}
			ServerData.logToConsole($"Int takes {timer.ElapsedTicks/ushort.MaxValue}");
			timer.Stop();

			GeneralTimer.runTimer(500);
        }

        [ServerEvent(Event.ResourceStart)]
        public void OnGamemodeInit()
        {
			ushort maxCPs = (ushort) (Business.getMaxBusinesses() + Organization.getMaxOrgs() + House.getMaxHouses() + Store.getMaxStores());
			Checkpoint2.updateMaxCheckpoints(maxCPs);

			AppDomain.CurrentDomain.ProcessExit += (_, __) => this.OnGamemodeExit();
        }

        [ServerEvent(Event.ResourceStop)]
        public void OnGamemodeExit()
        {
			ServerData.logToConsole("Gamemode exit called.");
        }

        [ServerEvent(Event.PlayerConnected)]
        public void OnPlayerConnect(Player player)
        {
			string ipAddress = NAPI.Player.GetPlayerAddress(player);
			if(ServerData.isIPBanned(ipAddress))
			{
				ServerData.logToConsole($"{PlayerDataInfo.getPlayerName(player)} attempted to login with banned IP Address: {ipAddress}");
				player.Kick();
				return;
			}

            PlayerData data = new PlayerData(player);
            PlayerDataInfo.addPlayerData(data);

			data.setPlayerColor(ServerData.COLOR_YELLOW);

            byte adminLevel = data.getAdminLevel();

			data.freezePlayer(true);

			if(data.isPlayerRegistered()) 
			{
				Dialog login = new Dialog(DialogID.DIALOG_LOGIN, DialogType.PASSWORD, "Login to your account", "Type your password below to login to your account.", "", new string[] {"Login", "Cancel"});
				login.showDialogForPlayer(player);

				ChatUtils.sendClientMessage(player, ServerData.COLOR_WHITE, $"Welcome back {PlayerDataInfo.getPlayerName(player)}. Login to your account to play!");
			}
			else
			{
				Dialog register = new Dialog(DialogID.DIALOG_REGISTER, DialogType.PASSWORD, "Register an Account", "You must register an account to play in this server.<br>Type a password below to register an account.", "", new string[] {"Confirm", "Cancel"});
				register.showDialogForPlayer(player);

				ChatUtils.sendClientMessage(player, ServerData.COLOR_WHITE, $"Account {PlayerDataInfo.getPlayerName(player)} isn't registered. Register your account below to play.");
			}
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnect(Player player, DisconnectionType type, string reason)
        {
            PlayerData data = PlayerDataInfo.getPlayerData(player);

            Vector3 pos = player.Position;
            float angle = player.Heading;

            data.updatePlayerHealth((byte) player.Health);
            data.updatePlayerArmor((byte) player.Armor);
            data.updatePlayerPos(pos, angle);

            data.savePlayerData();

            PlayerDataInfo.removePlayerData(data);
        }

        [ServerEvent(Event.PlayerSpawn)]
        public void OnPlayerSpawn(Player player)
        {
			PlayerData data = PlayerDataInfo.getPlayerData(player);
			if(!data.isPlayerLoggedIn()) return;

			NAPI.Player.SetPlayerName(player, ChatUtils.colorString(PlayerDataInfo.getPlayerName(player), ChatUtils.getColorAsHex(data.getPlayerColor())));
			NAPI.Player.SetPlayerArmor(player, (int) data.getPlayerArmor());
			NAPI.Player.SetPlayerHealth(player, (int) data.getPlayerHealth());
        }

        [ServerEvent(Event.PlayerDeath)]
        public void OnPlayerDeath(Player player, Player Killer, uint reason)
        {
			PlayerData data = PlayerDataInfo.getPlayerData(player);

			NAPI.Player.RemoveAllPlayerWeapons(player);
			data.updatePlayerArmor(0);
			data.updatePlayerHealth(100);
			data.resetTempData();
        }

        [ServerEvent(Event.PlayerEnterVehicleAttempt)]
        public void OnPlayerEnterVehicleAttempt(Player player, Vehicle veh, sbyte seatID)
        {
            Vehicle2 vehicle = ServerData.getVehicleObject(veh);
			vehicle.updateVehicleSpeedValue(NAPI.Entity.GetEntityVelocity(veh.Handle));
        }

        [ServerEvent(Event.PlayerEnterVehicle)]
        public void OnPlayerEnterVehicle(Player player, Vehicle veh, sbyte seatID)
        {
            Vehicle2 vehicle = ServerData.getVehicleObject(veh);

            if(seatID == 0)
            {
                if(vehicle.getEngineStatus()) vehicle.startEngine();
                else vehicle.stopEngine();
            }
        }

        [ServerEvent(Event.PlayerExitVehicle)]
        public void OnPlayerExitVehicle(Player player, Vehicle veh)
        {
            Vehicle2 vehicle = ServerData.getVehicleObject(veh);

            if(player.VehicleSeat == 0)
            {
                if(vehicle.getEngineStatus()) vehicle.startEngine();
                else vehicle.stopEngine();
            }
        }

		[ServerEvent(Event.ChatMessage)]
		public void onPlayerText(Player player, string text)
		{
			PlayerData data = PlayerDataInfo.getPlayerData(player);

			if(!data.isPlayerRegistered())
			{
				ChatUtils.sendClientMessage(player, ServerData.COLOR_RED, "You must register to play! Type /register to continue.");
				return;
			}

			if(!data.isPlayerLoggedIn())
			{
				ChatUtils.sendClientMessage(player, ServerData.COLOR_RED, "You must login to play! Type /login to continue.");
				return;
			}

			if(data.isMuted()) 
			{
				ChatUtils.sendClientMessage(player, ServerData.COLOR_RED, "Error: You are muted smart ass!");
				return;
			}

			string output = $"{ChatUtils.colorString($"{PlayerDataInfo.getPlayerName(player)}:", ChatUtils.getColorAsHex(data.getPlayerColor()), ChatUtils.getColorAsHex(ServerData.COLOR_WHITE))} {text}";
			ChatUtils.sendClientMessageToAll(ServerData.COLOR_WHITE, output);

			data.updateFloodTimer((byte) (data.getFloodTimer() + 1));

			if(data.getFloodTimer() >= 5)
			{
				ChatUtils.sendClientMessage(player, ServerData.COLOR_RED, "~ You have been auto-muted for flooding.");
				ChatUtils.sendClientMessageToAll(ServerData.COLOR_RED, $"~ {PlayerDataInfo.getPlayerName(player)} has been auto-muted for 60 seconds (flooding in main chat).");
							
				data.mutePlayer(true, 60);
			}
		}

        [ServerEvent(Event.VehicleDamage)]
        public void OnVehicleTakeDamage(Vehicle veh, float bodyHealthLoss, float engineHealthLoss)
        {
            Vehicle2 vehicle = ServerData.getVehicleObject(veh);
        }

        [ServerEvent(Event.VehicleDeath)]
        public void OnVehicleDeath(Vehicle veh)
        {
            Vehicle2 vehicle = ServerData.getVehicleObject(veh);
        }

		[ServerEvent(Event.PlayerEnterColshape)]
		public void PlayerEnterColshape(ColShape shape, Player player)
		{
			PlayerData data = PlayerDataInfo.getPlayerData(player);
			if(!data.isPlayerLoggedIn()) return;

			data.updatePlayerCurrentColshape(shape);

			object owner = ServerData.getColShapeOwner(shape);
			Checkpoint2 ownerCP = null;

			if(owner is Organization)
			{
				Organization org = (Organization) owner;

				ownerCP = org.getCheckpoint2();
				ChatUtils.sendClientMessage(player, org.getColor(), $"Organization: {org.getName()}.");

				string info = (org.isPlayerInOrg(player)) ? "Collect Wage\nCollect Weapons\nResign\nMembers\nNotes\nTerritories\nEnter" : "Request\nMembers\nNotes\nTerritories\nEnter";
				
				Dialog orgDialog = new Dialog(DialogID.DIALOG_ORG_MENU, DialogType.LIST, org.getName(), "", info, new string[] {"Select", "Cancel"});
				orgDialog.showDialogForPlayer(player);
			}

			if(owner is Business)
			{
				ChatUtils.sendClientMessage(player, ServerData.COLOR_RED, "Work in progress");
			}
			if(owner is Store)
			{
				ChatUtils.sendClientMessage(player, ServerData.COLOR_RED, "Work in progress");
			}
			if(owner is GasStation)
			{
				if(!NAPI.Player.IsPlayerInAnyVehicle(player)) ChatUtils.sendClientMessage(player, ServerData.COLOR_RED, "You must be the driver of a vehicle.");
				else ChatUtils.sendClientMessage(player, ServerData.COLOR_RED, "Work in progress");
			}
		}

		[ServerEvent(Event.PlayerExitColshape)]
		public void OnPlayerLeaveColshape(ColShape shape, Player player)
		{
			PlayerData data = PlayerDataInfo.getPlayerData(player);
			if(!data.isPlayerLoggedIn()) return;

			object owner = ServerData.getColShapeOwner(shape);

			data.updatePlayerCurrentColshape(null);
			data.updatePlayerLastColshape(shape);
		}

		[RemoteEvent("TogglePlayerTyping")]
		public void togglePlayerTyping(Player player, bool value)
		{
			PlayerData data = PlayerDataInfo.getPlayerData(player);
			data.updateTypingStatus(value);
		}

        [RemoteEvent("OnKeyPress")]
        public void KeyPressEvent(Player player, int key) 
		{
			if(player == null || !NAPI.Player.IsPlayerConnected(player)) return;
			PlayerData data = PlayerDataInfo.getPlayerData(player);

			if(!data.isPlayerLoggedIn()) return;

			if(data.getPlayerDialog() != ushort.MaxValue && key == ServerData.getKeyValue(KeyRef.ESC_KEY)) NAPI.ClientEventThreadSafe.TriggerClientEvent(player.Handle, "DestroyDialog");
			if(key == ServerData.getKeyValue(KeyRef.TWO_KEY)) NAPI.ClientEventThreadSafe.TriggerClientEvent(player.Handle, "ToggleUICursor");

			bool typing = data.isPlayerTyping();
			if(!typing && key == ServerData.getKeyValue(KeyRef.T_KEY)) data.updateTypingStatus(true);
			if(typing && key == ServerData.getKeyValue(KeyRef.ENTER_KEY)) data.updateTypingStatus(false);
		}

        [RemoteEvent("OnKeyRelease")]
        public void KeyReleaseEvent(Player player, int key)
        {
			PlayerData data = PlayerDataInfo.getPlayerData(player);
			if(!data.isPlayerLoggedIn()) return;

            if(key == ServerData.getKeyValue(KeyRef.N_KEY))
            {
                if((!PlayerDataInfo.getPlayerData(player).isPlayerTyping()) && player.IsInVehicle && player.VehicleSeat == 0)
                {
                    Vehicle playerVeh = player.Vehicle;
                    Vehicle2 vehicle = ServerData.getVehicleObject(playerVeh);

                    bool engineStatus = vehicle.getEngineStatus();

                    string output = null;
                    Color color;

                    if(!engineStatus && vehicle.getVehicleFuel() > 0)
                    {
                        vehicle.startEngine();

                        output = "~ Engine turned on.";
                        color = ServerData.COLOR_GREEN;
                    }
                    else
                    {
                        vehicle.stopEngine();

                        output = "~ Engine turned off.";
                        color = ServerData.COLOR_WHITE;
                    }

                    Utilities.ChatUtils.sendClientMessage(player, color, output);
                }

                return;
            }
        }

		[RemoteEvent("SyncVehicleDataForClient")]
		public void getVehicleRotSyncDataForClient(Player player, Vehicle veh) 
		{
			Vehicle2 vehicle = ServerData.getVehicleObject(veh);

			NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "SyncVehicleRot", veh, veh.Heading);
			NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "ToggleEngine", veh.Value, vehicle.getEngineStatus());
		}

		[RemoteEvent("OnDialogResponse")]
		public void onDialogResponse(Player player, int dialogid, string name, int response, object info)
		{
			PlayerData data = PlayerDataInfo.getPlayerData(player);
			bool responseCheck = (response == 0) ? true : false;

			switch(dialogid)
			{
				case (int) DialogID.DIALOG_LOGIN:
				{
					if(!responseCheck) return;
					data.loadAccount(NAPI.Util.GetHashSha256((string) info));
					break;
				}
				case (int) DialogID.DIALOG_REGISTER:
				{
					if(!responseCheck) return;
					string pass = NAPI.Util.GetHashSha256((string) info);
					data.setPlayerPassword(pass);

					Dialog confirm = new Dialog(DialogID.DIALOG_CONFIRM, DialogType.PASSWORD, "Register an account", "Please confirm your password.", "", new string[] {"Register", "Cancel"});
					confirm.showDialogForPlayer(player);
					break;
				}
				case (int) DialogID.DIALOG_CONFIRM:
				{
					if(!responseCheck) return;
					if(!data.getPlayerPassword().Equals(NAPI.Util.GetHashSha256((string) info)))
					{
						data.setPlayerPassword(null);

						Dialog register = new Dialog(DialogID.DIALOG_REGISTER, DialogType.PASSWORD, "Register an Account", "You must register an account to play in this server.\n\nType a password below to register an account.", "", new string[] {"Confirm", "Cancel"});
						register.showDialogForPlayer(player);

						ChatUtils.sendClientMessage(player, ServerData.COLOR_RED, "The passwords didn't match.");
						return;
					}

					data.createAccount(data.getPlayerPassword());
					break;
				}

				case (int) DialogID.DIALOG_ORG_MENU:
				{
					if(!responseCheck) return;

					int listItem =  (int) info;
					ColShape orgCP = data.getPlayerCurrentColshape();

					if(orgCP == null) return;
					if(!(ServerData.getColShapeOwner(orgCP) is Organization)) return;

					Organization org = (Organization) ServerData.getColShapeOwner(orgCP);

					if(!org.isPlayerInOrg(player))
					{
						switch(listItem)
						{
							case (byte) 0:
							{
								Player coLeader = null;
								byte leadersOnline = 0;
								for(byte i = 1; i <= 3; i++)
								{
									if(i == 1)
									{
										coLeader = org.getLeader();
										if(coLeader == null) continue;
										else leadersOnline++;
									}
									coLeader = org.getColeader(i);
									if(coLeader == null) continue;
									else leadersOnline++;
								}

								if(leadersOnline == 0)
								{
									ChatUtils.sendClientMessage(player, ServerData.COLOR_WHITE, "Sorry the leader(s) of this organization are currently offline.");
									return;
								}

								data.updateRequestToJoinOrg((sbyte) org.getID());
								ChatUtils.sendMessageToOrgMembers(org, ServerData.COLOR_SILVER, $"{PlayerDataInfo.getPlayerName(player)} is requesting to join your organization.");
								break;
							}
							case 1:
							{
								byte count = org.getOrgMemberCount();
								string infoStr = "", addStr = null, memberName = null;
								for(byte i = 1; i <= count; i++)
								{
									if(i == 1)
									{
										memberName = org.getLeaderName();
										if(memberName == null) 
										{
											ChatUtils.sendClientMessage(player, ServerData.COLOR_SILVER, "This organization does not have any members.");
											return;
										}
										else addStr = $"{memberName}\tLeader\n";
									}
									if(i <= 3)
									{
										memberName = org.getColeaderName(i);
										if(memberName != null) addStr = $"{addStr}{memberName}\tColeader\n";
									}
									memberName = org.getMemberName(i);
									if(memberName != null) addStr = $"{addStr}{memberName}\n";

									infoStr = $"{infoStr}{addStr}";
								}

								Dialog memberDialog = new Dialog(DialogID.DIALOG_ORG_MEMBERS, DialogType.LIST, "Members", "", infoStr, new string[] {"Select", "Cancel"});
								memberDialog.showDialogForPlayer(player);
								break;
							}
						}
					}
					break;
				}

				case (int) DialogID.DIALOG_ORG_MEMBERS:
				{
					if(!responseCheck) return;

					int listItem =  (int) info;
					ColShape orgCP = data.getPlayerCurrentColshape();

					if(orgCP == null) return;
					if(!(ServerData.getColShapeOwner(orgCP) is Organization)) return;

					Organization org = (Organization) ServerData.getColShapeOwner(orgCP);
					if(!org.isPlayerColeader(player)) return;
					break;
				}
			}
		}
    }
}