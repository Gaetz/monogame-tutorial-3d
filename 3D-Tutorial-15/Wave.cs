using System;
using System.Collections.Generic;
using System.Numerics;

namespace Tutorial_15
{
    public class Wave
    {
        public float time;
        public int id;
        public int elementNumber;
        public List<WaveElement> elements = new List<WaveElement>();

        public Wave(float time, int id, int enemyNumber)
        {
            this.time = time;
            this.id = id;
            this.elementNumber = enemyNumber;
        }

        public void Launch(Game1 game)
        {
            foreach (WaveElement element in elements)
            {
                switch (element.type)
                {
                    case "enemy":
                        game.AddEnemy(element.position, element.screenSideEnter, element.screenSideExit, element.mainPhaseDuration, "BeachBall");
                        break;
                    case "powerup":
                        game.AddPowerUp(element.position);
                        break;
                }
            }
        }
    }

    public class WaveElement
    {
        public string type;
        public ScreenSide screenSideEnter;
        public ScreenSide screenSideExit;
        public Vector3 position;
        public float mainPhaseDuration;

        public WaveElement(string type, string screenSideEnter, string screenSideExit, float x, float y, float z, float mainPhaseDuration)
        {
            this.type = type;
            this.screenSideEnter = (ScreenSide)Enum.Parse(typeof(ScreenSide), screenSideEnter);
            this.screenSideExit = (ScreenSide)Enum.Parse(typeof(ScreenSide), screenSideExit);
            this.position = new Vector3(x, y, z);
            this.mainPhaseDuration = mainPhaseDuration;
        }
    }
}
