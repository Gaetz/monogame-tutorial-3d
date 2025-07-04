using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tutorial_17
{
    internal class SceneGameOver : Scene
    {
        private Game1 game;

        Texture2D gameOverPanel;
        Rectangle panelRect = new Rectangle(0, 0, 800, 480);
        InputManager inputManager = new InputManager();

        public SceneGameOver(Game1 game)
        {
            this.game = game;
        }

        public void Load(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            gameOverPanel = contentManager.Load<Texture2D>("GameOver");
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
            }
        }

        public void Update(double dt, GraphicsDevice graphicsDevice)
        {
            inputManager.Update();

            if (inputManager.IsValidationActionPressed())
            {
                game.GoToMenu();
            }

            inputManager.EndUpdate();
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(gameOverPanel, panelRect, Color.White);
            spriteBatch.End();
        }
    }
}
