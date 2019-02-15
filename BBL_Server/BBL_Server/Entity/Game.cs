using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBL_Server.Entity
{
    public class Game
    {
        private Dictionary<int, Player> players = new Dictionary<int, Player>();
        private Dictionary<int, Player> waiting = new Dictionary<int, Player>();
        public bool running = false;

        public Dictionary<int, Player> GetPlayers()
        {
            return players;
        }

        public Player GetPlayer(int accountId)
        {
            if (players.TryGetValue(accountId, out Player value))
                return value;
            return null;
        }

        public bool AddPlayer(Player player)
        {
            if (running)
                return false;
            waiting.Add(player.accountId, player);
            return true;
        }

        public bool AcceptPlayer(Player player)
        {
            if (running)
                return false;
            if (waiting.TryGetValue(player.accountId, out Player value))
            {
                players.Add(player.accountId, player);
                waiting.Remove(player.accountId);
                return true;
            }
            return false;
        }

        public int GetPlayerCount()
        {
            return players.Count;
        }

        public void Start()
        {
            /*
            running = true;
            foreach(KeyValuePair<int, Player> player in players)
            {
               
            }
            */
        }
    }
}
