﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Tutorial_06
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

            enemies.Add(new Enemy(new Vector3(0, 0, -500)));
            enemies[0].Load(Content, "Saucer");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            double dt = gameTime.ElapsedGameTime.TotalSeconds;
            if (dt > 0.1) dt = 0.1;

            playerAim.Update(dt);
            player.Update(dt);

            UpdateProjectiles(dt);
            UpdateEnemies(dt);

            // Power up test
            powerUpTimer -= (float)dt;
            if (powerUpTimer <= 0)
            {
                powerUps.Add(new PowerUp(new Vector3(0, 0, 0), 200.0f));
                powerUps[powerUps.Count - 1].Load(Content, "BeachBall");
                powerUps[powerUps.Count - 1].SetRandomPosition();
                powerUpTimer = POWER_UP_TIME;
            }
            UpdatePowerUps(dt);

            base.Update(gameTime);
        }

        private void UpdateProjectiles(double dt)
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update(dt);
                // Remove projectiles that are out of bounds
                if (projectiles[i].Position.Z < -10000)
                {
                    projectiles.RemoveAt(i);
                    continue;
                }
                // Collision with enemies
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.BoundingBox.Intersects(projectiles[i].BoundingBox)
                        && projectiles[i].FromPlayer)
                    {
                        enemy.SetRandomPosition();
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
            var newProjectile = new Projectile(position, orientation, speed, fromPlayer);
            newProjectile.Load(Content, "Cube");
            projectiles.Add(newProjectile);
        }
    }
}
