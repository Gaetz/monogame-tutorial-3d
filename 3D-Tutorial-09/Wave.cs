using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_09
{
    public class Wave
    {
        public float time;
        public int id;
        public int enemyNumber;
        public List<WaveEnemy> elements = new List<WaveEnemy>();
    }

    public class WaveEnemy
    {
        public string type;
        public ScreenSide screenSideEnter;
        public ScreenSide screenSideExit;
        public Vector3 position;
        public float mainPhaseDuration;
    }
}
