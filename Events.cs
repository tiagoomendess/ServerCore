using System;
using GTANetworkServer;
using GTANetworkShared;

namespace ServerCore
{
    public class Events : Script
    {

        public Events()
        {

            API.onPlayerConnected += onPlayerConnected;
            API.onChatMessage += onChatMessage;
            API.onPlayerDeath += onPlayerDeath;

        }

        public void onPlayerConnected(Client player)
        {

            API.sendNotificationToPlayer(player, "Bem-vindo ao GTA V IMPÉRIO!");

        }

        public void onChatMessage(Client player, string message, CancelEventArgs e)
        {

            e.Cancel = true;

            string group = API.getPlayerAclGroup(player);
            string constructedUsername = "~c~[PLEBEU] " + player.name;

            if (group.Equals("Admin"))
            {

                constructedUsername = "~o~[ADMIN] " + player.name;

            }
            else if (group.Equals("Moderator"))
            {

                constructedUsername = "~g~[MODERADOR] " + player.name;

            }

            API.sendChatMessageToAll(constructedUsername, message);

        }

        public void onPlayerDeath(Client player, NetHandle entityKiller, int weapon)
        {

            Client killer = API.getPlayerFromHandle(entityKiller);

            if (killer != null)
            {

                API.sendNotificationToAll(killer.name + " matou " + player.name);

            }
            else
            {

                API.sendNotificationToAll(player.name + " morreu");

            }

        }

    }
}
