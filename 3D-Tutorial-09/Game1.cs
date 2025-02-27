using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Tutorial_09
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

        private const float POWER_UP_TIME = 5.0f;
        private float powerUpTimer = 1;

        private Tutorial_Data.WaveData[] levelData;
        private List<Wave> waves = new List<Wave>();
        private int currentWave = 0;
        private float waveTimer = 0.0f;

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

            levelData = Content.Load<Tutorial_Data.WaveData[]>("Level0");
            waves = LoadWaves(levelData);
        }

        List<Wave> LoadWaves(Tutorial_Data.WaveData[] levelData)
        {
            List<Wave> waves = new List<Wave>();
            foreach (Tutorial_Data.WaveData waveData in levelData)
            {
                Wave wave = new Wave(waveData.time, waveData.id, waveData.enemyNumber);
                for (int i = 0; i < waveData.enemyNumber; i++)
                {
                    switch(i)
                    {
                        case 0:
                            if (waveData.enemy0Type.Equals("none")) continue;
                            wave.elements.Add(new WaveElement(waveData.enemy0Type, waveData.enemy0EnterSide, waveData.enemy0ExitSide,
                                waveData.enemy0X, waveData.enemy0Y, waveData.enemy0Z, waveData.enemy0Duration));
                            break;
                        case 1:
                            if (waveData.enemy1Type.Equals("none")) continue;
                            wave.elements.Add(new WaveElement(waveData.enemy1Type, waveData.enemy1EnterSide, waveData.enemy1ExitSide,
                                waveData.enemy1X, waveData.enemy1Y, waveData.enemy1Z, waveData.enemy1Duration));
                            break;
                        case 2:
                            if (waveData.enemy2Type.Equals("none")) continue;
                            wave.elements.Add(new WaveElement(waveData.enemy2Type, waveData.enemy2EnterSide, waveData.enemy2ExitSide,
                                waveData.enemy2X, waveData.enemy2Y, waveData.enemy2Z, waveData.enemy2Duration));
                            break;
                        case 3:
                            if (waveData.enemy3Type.Equals("none")) continue;
                            wave.elements.Add(new WaveElement(waveData.enemy3Type, waveData.enemy3EnterSide, waveData.enemy3ExitSide,
                                waveData.enemy3X, waveData.enemy3Y, waveData.enemy3Z, waveData.enemy3Duration));
                            break;
                        case 4:
                            if (waveData.enemy4Type.Equals("none")) continue;
                            wave.elements.Add(new WaveElement(waveData.enemy4Type, waveData.enemy4EnterSide, waveData.enemy4ExitSide,
                                waveData.enemy4X, waveData.enemy4Y, waveData.enemy4Z, waveData.enemy4Duration));
                            break;
                    }
                }
                waves.Add(wave);
            }
            return waves;
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
            UpdateWaves(dt);

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
            
        public void UpdateWaves(double dt)
        {
            waveTimer += (float)dt;
            if (currentWave < waves.Count && waveTimer >= waves[currentWave].time)
            {
                waves[currentWave].Launch(this);
                currentWave++;
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

        public void AddEnemy(Vector3 position, ScreenSide enterSide, ScreenSide exitSide, float mainPhaseDuration, string modelName)
        {
            Enemy enemy = new Enemy(position, this, enterSide, exitSide, mainPhaseDuration);
            enemy.Load(Content, modelName);
            enemies.Add(enemy);
        }
    }
}
