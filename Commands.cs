using System;
using GTANetworkServer;
using GTANetworkShared;
using System.Collections.Generic;

namespace ServerCore
{
    class Commands : Script
    {

        private List<Client> helpList = new List<Client>();
        private List<Vehicle> spawned = new List<Vehicle>();

        [Command("chamarModerador", "UTILIZAÇÃO: /chamarModerador ou /cm", Alias = "cm")]
        public void callModerator(Client sender, Client helping = null)
        {

            string moderatorGroup = "Moderator";
            string senderGroup = API.getPlayerAclGroup(sender);

            if (helping == null || !senderGroup.Equals(moderatorGroup))
            {

                if (senderGroup.Equals(moderatorGroup))
                {

                    API.sendChatMessageToPlayer(sender, "~r~ Não podes pedir ajuda como moderador!");
                    return;

                }

                if (helpList.Contains(sender))
                {

                    API.sendChatMessageToPlayer(sender, "~y~ Já chamaste um moderador. Por favor aguarda pela resposta.");
                    return;

                }

                string clientName = sender.name;
                bool isOnline = false;

                foreach (Client player in API.getAllPlayers())
                {

                    string playerGroup = API.getPlayerAclGroup(player);

                    if (playerGroup.Equals(moderatorGroup))
                    {

                        API.sendChatMessageToPlayer(player, "~y~ -- O JOGADOR " + clientName + " PEDIU AJUDA --");
                        isOnline = true;

                    }

                }

                if (!isOnline)
                {

                    API.sendChatMessageToPlayer(sender, "~r~ Não existe nenhum moderador online no momento.");

                }
                else
                {

                    API.sendChatMessageToPlayer(sender, "~y~ Acabaste de chamar um moderador. Por favor aguarda pela resposta.");
                    helpList.Add(sender);

                }

                return;

            }

            if (helping == null || !helpList.Contains(helping))
            {

                API.sendChatMessageToPlayer(sender, "~r~ O jogador não está na lista de ajuda!");
                return;

            }

            helpList.Remove(helping);
            API.setEntityPosition(sender.handle, API.getEntityPosition(helping.handle));

        }

        [Command("eu", GreedyArg = true)]
        public void meCommand(Client sender, string message)
        {

            foreach (Client player in API.getPlayersInRadiusOfPlayer(30, sender))
            {

                API.sendChatMessageToPlayer(player, "~p~ *" + sender.name + " " + message);

            }

        }

        [Command("coords")]
        public void gps(Client sender)
        {

            Vector3 coords = API.getEntityPosition(sender.handle);
            sender.sendChatMessage("~r~GPS: ~w~x" + coords.X + " y" + coords.Y + " z" + coords.Z);

        }

        [Command("spawn", ACLRequired = true)]
        public void spawn(Client sender, string name)
        {

            VehicleHash hash = API.vehicleNameToModel(name);
            Vehicle veh = API.createVehicle(hash, sender.position, sender.rotation, 36, 36);
            API.setPlayerIntoVehicle(sender, veh.handle, -1);
            spawned.Add(veh);

        }

        [Command("killent")]
        public void killent(Client sender)
        {

            foreach (Vehicle veh in spawned)
            {

                veh.delete();

            }

        }

    }
}
