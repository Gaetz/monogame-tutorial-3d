using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text;

namespace Tutorial_18
{
    public enum DisplayedCharacter
    {
        None,
        Pilot,
        Robot
    }

    internal class DialogBox
    {
        private string message = "";
        private bool isVisible = false;
        private SpriteFont font;
        private float displayTimer = 0.0f;
        float displayDuration = 3.0f;

        private Texture2D dialogBoxTexture;
        private Texture2D pilotTexture;
        private Texture2D robotTexture;
        private Texture2D portraitTexture;

        const int DIALOG_BOX_X = 20;
        const int DIALOG_BOX_Y = 332;
        const int PORTRAIT_SIZE = 128;
        const int TEXT_START_X = DIALOG_BOX_X + PORTRAIT_SIZE + 50;
        const int MAX_LINE_CHARACTERS = 70;
        const int LINE_SEPARATION = 30;


        public void Load(ContentManager content)
        {
            dialogBoxTexture = content.Load<Texture2D>("DialogBox");
            pilotTexture = content.Load<Texture2D>("PilotDialog");
            robotTexture = content.Load<Texture2D>("RobotDialog");
            font = content.Load<SpriteFont>("Arial");
        }

        public void DisplayMessage(string message, DisplayedCharacter character, float duration)
        {
            this.message = message;
            displayDuration = duration;
            isVisible = true;
            switch(character)
            {
                case DisplayedCharacter.Pilot:
                    portraitTexture = pilotTexture;
                    break;
                case DisplayedCharacter.Robot:
                    portraitTexture = robotTexture;
                    break;
                default:
                    portraitTexture = null;
                    break;
            }

        }

        public void Update(double dt)
        {
            if (!isVisible)
            {
                return;
            }
            displayTimer += (float)dt;
            if (displayTimer > displayDuration)
            {
                isVisible = false;
                displayTimer = 0.0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isVisible)
            {
                return;
            }
            DrawDialogBox(spriteBatch);
            DrawCharacter(spriteBatch);
            DrawMessage(spriteBatch);
        }

        private void DrawDialogBox(SpriteBatch spriteBatch)
        {
            Color transparentColor = new Color(255, 255, 255, 150);
            spriteBatch.Draw(dialogBoxTexture, new Vector2(DIALOG_BOX_X, DIALOG_BOX_Y), transparentColor);
        }

        private void DrawCharacter(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(portraitTexture, new Vector2(DIALOG_BOX_X, DIALOG_BOX_Y), Color.White);
        }

        private void DrawMessage(SpriteBatch spriteBatch)
        {
            // Divide the message in lines
            string[] words = message.Split(' ');
            List<string> lines = new List<string>();
            StringBuilder line = new StringBuilder();
            foreach (string word in words)
            {
                if (line.Length + word.Length < MAX_LINE_CHARACTERS)
                {
                    line.Append(word + " ");
                }
                else
                {
                    lines.Add(line.ToString());
                    line.Clear();
                    line.Append(word + " ");
                }
            }
            // The last line is added
            lines.Add(line.ToString());
            // Display each line
            for (int i = 0; i < lines.Count; i++)
            {
                spriteBatch.DrawString(font, lines[i], new Vector2(TEXT_START_X, DIALOG_BOX_Y + 20 + i * LINE_SEPARATION), Color.White);
            }
        }
    }
}
