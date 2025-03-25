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
        float displayDuration = 3.0f;

        private Texture2D dialogBoxTexture;
        private Texture2D pilotTexture;
        private Texture2D robotTexture;
        private Texture2D portraitTexture;

        const int DIALOG_BOX_X = 20;
        const int DIALOG_BOX_Y = 332;
        const int PORTRAIT_SIZE = 128;
        const int TEXT_START_X = DIALOG_BOX_X + PORTRAIT_SIZE + 50;

        public void Load(ContentManager content)
        {
            dialogBoxTexture = content.Load<Texture2D>("DialogBox");
            pilotTexture = content.Load<Texture2D>("PilotDialog");
            robotTexture = content.Load<Texture2D>("RobotDialog");
            font = content.Load<SpriteFont>("Arial");
        }

        public void DisplayMessage(string message, DisplayedCharacter character = DisplayedCharacter.None, float duration = 3f)
        {
            this.message = message;
            currentCharacter = character;
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
            spriteBatch.DrawString(font, message, new Vector2(TEXT_START_X, DIALOG_BOX_Y + 20), Color.White);
        }
    }
}
