using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tutorial_15
{
    enum DisplayedCharacter
    {
        None,
        Pilot,
        Robot
    }

    internal class DialogBox
    {
        private string message = "";
        private DisplayedCharacter currentCharacter = DisplayedCharacter.None;
        private bool isVisible = false;
        private SpriteFont font;
        private float displayTimer = 0.0f;

        private Texture2D dialogBoxTexture;
        private Texture2D pilotTexture;
        private Texture2D robotTexture;

        const int DIALOG_BOX_X = 20;
        const int DIALOG_BOX_Y = 332;
        const int PORTRAIT_SIZE = 128;
        const int TEXT_START_X = DIALOG_BOX_X + PORTRAIT_SIZE + 50;
        const float DISPLAY_TIME = 3.0f;


        public void Load(ContentManager content)
        {
            dialogBoxTexture = content.Load<Texture2D>("DialogBox");
            pilotTexture = content.Load<Texture2D>("PilotDialog");
            robotTexture = content.Load<Texture2D>("RobotDialog");
            font = content.Load<SpriteFont>("Arial");
        }

        public void DisplayMessage(string message, DisplayedCharacter character = DisplayedCharacter.None)
        {
            this.message = message;
            currentCharacter = character;
            isVisible = true;
        }

        public void Update(double dt)
        {
            if (!isVisible)
            {
                return;
            }
            displayTimer += (float)dt;
            if (displayTimer > DISPLAY_TIME)
            {
                isVisible = false;
                displayTimer = 0.0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                DrawDialogBox(spriteBatch);
                DrawCharacter(spriteBatch);
                DrawMessage(spriteBatch);
            }
        }

        private void DrawDialogBox(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(dialogBoxTexture, new Vector2(DIALOG_BOX_X, DIALOG_BOX_Y), Color.White);
        }

        private void DrawCharacter(SpriteBatch spriteBatch)
        {
            Texture2D texture = null;
            switch (currentCharacter)
            {
                case DisplayedCharacter.Pilot:
                    texture = pilotTexture;
                    break;
                case DisplayedCharacter.Robot:
                    texture = robotTexture;
                    break;
            }
            if (texture != null)
            {
                spriteBatch.Draw(texture, new Vector2(DIALOG_BOX_X, DIALOG_BOX_Y), Color.White);
            }
        }

        private void DrawMessage(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, message, new Vector2(TEXT_START_X, DIALOG_BOX_Y + 20), Color.White);
        }
    }
}
