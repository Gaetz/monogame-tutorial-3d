using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;

namespace Tutorial_18
{
    internal class SceneMenu : Scene
    {
        private Game1 game;
        private Camera camera = new Camera(new Vector3(-200, 100, -300), new Vector3(100, -100, 0), Vector3.Up, 45f);

        private ShiftingTexture ground;
        private ShiftingTexture sky;
        private Entity ship;
        private Quaternion SHIP_DEFAULT_ORIENTATION = Quaternion.CreateFromAxisAngle(Vector3.Up, MathF.PI);
        double timeCounter = 0;

        Texture2D playButton;
        Texture2D quitButton;
        Texture2D playButtonSelected;
        Texture2D quitButtonSelected;
        int menuItem = 0;

        InputManager inputManager = new InputManager();
        private Song titleSong;

        public SceneMenu(Game1 game)
        {
            this.game = game;
        }

        public void Load(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            ground = new ShiftingTexture(new Vector3(0, -150, 1000),
                Quaternion.CreateFromAxisAngle(Vector3.Right, -MathF.PI / 2),
                new Vector2(3000f, 3000f),
                new Vector2(0, -0.2f));
            ground.Load(contentManager, graphicsDevice, "Grid");

            sky = new ShiftingTexture(new Vector3(0, 150, 1000),
                Quaternion.CreateFromAxisAngle(Vector3.Right, MathF.PI / 2),
                new Vector2(3000f, 3000f),
                new Vector2(0, 0.2f));
            sky.Load(contentManager, graphicsDevice, "Grid");

            ship = new Entity();
            ship.Load(contentManager, "Ship");
            ship.Position = new Vector3(0, 0, 0);
            ship.Orientation = SHIP_DEFAULT_ORIENTATION;
            ship.Scale = new Vector3(2.0f, 2.0f, 2.0f);

            playButton = contentManager.Load<Texture2D>("PlayButton");
            quitButton = contentManager.Load<Texture2D>("QuitButton");
            playButtonSelected = contentManager.Load<Texture2D>("PlayButtonSelected");
            quitButtonSelected = contentManager.Load<Texture2D>("QuitButtonSelected");

            titleSong = contentManager.Load<Song>("CatNoisettes-BertrandToupet");
            MediaPlayer.Play(titleSong);
        }

        public void Update(double dt, GraphicsDevice graphicsDevice)
        {
            inputManager.Update();

            ground.Update(dt);
            sky.Update(dt);

            timeCounter += dt;
            float x = (float)Math.Cos(timeCounter);
            ship.Position = new Vector3(x * 50f, 0, 0);
            ship.Orientation = Quaternion.CreateFromAxisAngle(Vector3.Forward, x * -0.2f) * SHIP_DEFAULT_ORIENTATION;
            ship.Update(dt);

            if (inputManager.IsDownActionPressed())
            { 
                menuItem = (menuItem + 1) % 2;
            }
            if (inputManager.IsUpActionPressed())
            {
                menuItem = (menuItem - 1) % 2;
            }
            if (inputManager.IsValidationActionPressed())
            {
                if (menuItem == 0)
                {
                    game.Start();
                }
                else
                {
                    game.Exit();
                }
            }

            inputManager.EndUpdate();
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            Color bgColor = new Color(30, 0, 50);
            graphicsDevice.Clear(bgColor);

            ground.Draw(camera.View, camera.Projection);
            sky.Draw(camera.View, camera.Projection);
            ship.Draw(camera.View, camera.Projection);

            spriteBatch.Begin();
            if (menuItem == 0)
            {
                spriteBatch.Draw(playButtonSelected, new Vector2(300, 300), Color.White);
                spriteBatch.Draw(quitButton, new Vector2(300, 380), Color.White);
            }
            else
            {
                spriteBatch.Draw(playButton, new Vector2(300, 300), Color.White);
                spriteBatch.Draw(quitButtonSelected, new Vector2(300, 380), Color.White);
            }
            spriteBatch.End();

            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
