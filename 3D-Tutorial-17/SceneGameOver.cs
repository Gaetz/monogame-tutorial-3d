using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        }

        public void Update(double dt, GraphicsDevice graphicsDevice)
        {
            KeyboardState state = Keyboard.GetState();
            inputManager.Update(state);
            if (inputManager.IsKeyPressed(Keys.Enter) || inputManager.IsKeyPressed(Keys.Space))
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
