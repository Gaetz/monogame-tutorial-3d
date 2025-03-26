using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_15
{
    internal class Message
    {
        public int id;
        public float time;
        public float duration;
        public string message;
        public DisplayedCharacter displayedCharacter;

        public Message(int id, float time, float duration, string message, string portrait)
        {
            this.time = time;
            this.id = id;
            this.time = time;
            this.duration = duration;
            this.message = message;
            switch (portrait)
            {
                case "pilot":
                    displayedCharacter = DisplayedCharacter.Pilot;
                    break;
                case "robot":
                    displayedCharacter = DisplayedCharacter.Robot;
                    break;
                default:
                    displayedCharacter = DisplayedCharacter.None; 
                    break;    
            }
        }

        public void Launch(Game1 game)
        {
            game.DisplayMessage(message, displayedCharacter, duration);
        }
    }
}
