mp.events.add("playerJoin", (player) =>
{
	mp.game.vehicle.defaultEngineBehaviour = false;
});

mp.events.add("playerStartEnterVehicle", (vehicle, seat) =>
{
	mp.game.vehicle.defaultEngineBehaviour = false;
});

mp.events.add("playerEnterVehicle", (vehicle, seat) => 
{
	mp.game.vehicle.defaultEngineBehaviour = false;
    mp.players.local.setConfigFlag(429, true);
});