using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class GameState
    {
        public static GameState instance;
        public long hiPoints = 0;

        public static GameState getInstance()
        {
            if (instance == null)
            {
                instance = new GameState();
            }

            return instance;
        }
    }
}
