using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Tutorial_08
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
        private List<Enemy> enemies = new List<Enemy>();
        private List<PowerUp> powerUps = new List<PowerUp>();

        const float POWER_UP_TIME = 5.0f;
        float powerUpTimer = 1;

        internal Player Player
        {
            get { return player; }
        }

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
            player.Load(Content, "Ship");

            enemies.Add(new Enemy(new Vector3(0, 0, -500), this));
            enemies[0].Load(Content, "BeachBall");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            double dt = gameTime.ElapsedGameTime.TotalSeconds;

            playerAim.Update(dt);
            player.Update(dt);

            UpdateProjectiles(dt);
            UpdateEnemies(dt);
            UpdatePowerUps(dt);

            base.Update(gameTime);
        }

        private void UpdateProjectiles(double dt)
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update(dt);
                // Remove projectiles that are out of bounds
                if (projectiles[i].Position.Z < -10000 || projectiles[i].Position.Z > 1000)
                {
                    projectiles.RemoveAt(i);
                    continue;
                }
                // Collision with player
                if (projectiles[i].BoundingBox.Intersects(player.BoundingBox)
                    && !projectiles[i].FromPlayer)
                {
                    player.RemoveHp();
                    projectiles.RemoveAt(i);
                    continue;
                }
                // Collision with enemies
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.BoundingBox.Intersects(projectiles[i].BoundingBox)
                        && projectiles[i].FromPlayer)
                    {
                        enemy.RemoveHp();
                        projectiles.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void UpdateEnemies(double dt)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(dt);
                // Remove dead enemies
                if (enemies[i].IsDead)
                {
                    enemies.RemoveAt(i);
                }
            }
        }

        private void UpdatePowerUps(double dt)
        {
            for (int i = powerUps.Count - 1; i >= 0; i--)
            {
                powerUps[i].Update(dt);
                if (player.BoundingBox.Intersects(powerUps[i].BoundingBox))
                {
                    player.PowerUp();
                    powerUps.RemoveAt(i);
                    break;
                }
                if (powerUps[i].Position.Z > 500)
                {
                    powerUps.RemoveAt(i);
                    break;
                }
            }
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

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(view, projection);
            }

            foreach (PowerUp powerUp in powerUps)
            {
                powerUp.Draw(view, projection);
            }

            GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            playerAim.Draw(view, projection);

            base.Draw(gameTime);
        }

        public void AddProjectile(Vector3 position, Quaternion orientation, float speed, bool fromPlayer = true)
        {
            string modelName = fromPlayer ? "Cube" : "CubeRed";
            var newProjectile = new Projectile(position, orientation, speed, fromPlayer);
            newProjectile.Load(Content, modelName);
            projectiles.Add(newProjectile);
        }

        public void GameOver()
        {
            Exit();
        }
    }
}
