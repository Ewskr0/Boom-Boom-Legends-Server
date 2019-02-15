using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BBL_Server.Entity
{
    public class Player
    {
        public int accountId;
        public Vector2 position;
        public int maxBombCount;
        public int currentBombCount;
        public int connectionId;
        public bool alive;

        public Player(Vector2 position, int maxBombCount)
        {
            this.position = position;
            this.maxBombCount = maxBombCount;
            this.currentBombCount = maxBombCount;
            this.alive = true;
        }
    }
}
