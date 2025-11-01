namespace Tutorial_18
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

        public void Launch(SceneGame game)
        {
            game.DisplayMessage(message, displayedCharacter, duration);
        }
    }
}
