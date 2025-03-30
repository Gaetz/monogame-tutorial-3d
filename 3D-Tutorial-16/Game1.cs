using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Tutorial_16
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Scene currentScene;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            SwitchScene(SceneType.Game);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            double dt = gameTime.ElapsedGameTime.TotalSeconds;
            currentScene.Update(dt, GraphicsDevice);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            currentScene.Draw(_spriteBatch, GraphicsDevice);
            base.Draw(gameTime);
        }

        public void GameOver()
        {
            Exit();
        }

        private void SwitchScene(SceneType sceneType)
        {
            switch (sceneType)
            {
                case SceneType.Game:
                    currentScene = new SceneGame(this);
                    break;
                case SceneType.Menu:
                    //currentScene = new SceneMenu(this);
                    break;
                case SceneType.GameOver:
                    //currentScene = new SceneGameOver(this);
                    break;
            }
            currentScene.Load(Content, GraphicsDevice);
        }
    }
}
