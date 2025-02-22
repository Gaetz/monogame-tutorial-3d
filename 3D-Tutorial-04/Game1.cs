using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Tutorial_04
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player;
        private PlayerAim playerAim;

        private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 100), new Vector3(0, 0, 0), Vector3.UnitY);
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 1f, 10000f);

        private List<Projectile> projectiles = new List<Projectile>();

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

            playerAim = new PlayerAim();
            playerAim.Load(Content, GraphicsDevice);

            player = new Player(playerAim, this);
            player.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            double dt = gameTime.ElapsedGameTime.TotalSeconds;
            playerAim.Update(dt);
            player.Update(dt);
            foreach (Projectile projectile in projectiles)
            {
                projectile.Update(dt);
                if (projectile.Position.Z > 10000)
                {
                    projectiles.Remove(projectile);
                    break;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.BlendState = BlendState.Opaque;
            player.Draw(view, projection);

            foreach (Projectile projectile in projectiles)
            {
                projectile.Draw(view, projection);
            }

            GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            playerAim.Draw(view, projection);

            base.Draw(gameTime);
        }

        public void AddProjectile(Vector3 position, Quaternion orientation, float speed, bool fromPlayer = true)
        {
            var newProjectile = new Projectile(position, orientation, speed, fromPlayer);
            newProjectile.Load(Content);
            projectiles.Add(newProjectile);
        }
    }
}
