using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Tutorial_17
{
    public class SceneGame : Scene
    {
        private Game1 game;
        private ContentManager content;

        private Player player;
        private PlayerAim playerAim;

        private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 100), new Vector3(0, 0, 0), Vector3.UnitY);
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 1f, 10000f);

        private List<Projectile> projectiles = new List<Projectile>();
        private List<Enemy> enemies = new List<Enemy>();
        private List<PowerUp> powerUps = new List<PowerUp>();
        private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

        private Tutorial_Data.WaveData[] levelData;
        private List<Wave> waves = new List<Wave>();
        private int currentWave = 0;
        private float waveTimer = 0.0f;

        private SoundEffect explosionSound;
        private SoundEffect bonusSound;
        private SoundEffect smallExplosionSound;
        private SoundEffect smallExplosionEnemySound;

        private ShiftingTexture ground;
        private ShiftingTexture sky;

        private DialogBox dialogBox;
        private Tutorial_Data.MessageData[] levelMessagesData;
        private List<Message> messages = new List<Message>();
        private int currentMessage = 0;
        private float messageTimer = 0f;

        internal Player Player
        {
            get { return player; }
        }

        public SceneGame(Game1 game)
        {
            this.game = game;
        }

        public void Load(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            content = contentManager;

            playerAim = new PlayerAim();
            playerAim.Load(content, graphicsDevice);

            player = new Player(playerAim, this);
            player.Load(content, "Ship");

            levelData = content.Load<Tutorial_Data.WaveData[]>("Level0");
            waves = LoadWaves(levelData);

            explosionSound = content.Load<SoundEffect>("Explosion");
            bonusSound = content.Load<SoundEffect>("PickupBonus");
            smallExplosionSound = content.Load<SoundEffect>("SmallExplosion");
            smallExplosionEnemySound = content.Load<SoundEffect>("SmallExplosionEnemy");

            ground = new ShiftingTexture(new Vector3(0, -150, -1000),
                Quaternion.CreateFromAxisAngle(Vector3.Right, -MathF.PI / 2),
                new Vector2(3000f, 3000f),
                new Vector2(0, -0.2f));
            ground.Load(content, graphicsDevice, "Grid");

            sky = new ShiftingTexture(new Vector3(0, 150, -1000),
                Quaternion.CreateFromAxisAngle(Vector3.Right, MathF.PI / 2),
                new Vector2(3000f, 3000f),
                new Vector2(0, 0.2f));
            sky.Load(content, graphicsDevice, "Grid");

            dialogBox = new DialogBox();
            dialogBox.Load(content);
            levelMessagesData = content.Load<Tutorial_Data.MessageData[]>("Level0Messages");
            messages = LoadMessages(levelMessagesData);
        }

        List<Wave> LoadWaves(Tutorial_Data.WaveData[] levelData)
        {
            List<Wave> waves = new List<Wave>();
            foreach (Tutorial_Data.WaveData waveData in levelData)
            {
                Wave wave = new Wave(waveData.time, waveData.id, waveData.elementNumber);
                for (int i = 0; i < waveData.elementNumber; i++)
                {
                    switch (i)
                    {
                        case 0:
                            if (waveData.element0Type.Equals("none")) continue;
                            wave.elements.Add(new WaveElement(waveData.element0Type, waveData.element0EnterSide, waveData.element0ExitSide,
                                waveData.element0X, waveData.element0Y, waveData.element0Z, waveData.element0Duration));
                            break;
                        case 1:
                            if (waveData.element1Type.Equals("none")) continue;
                            wave.elements.Add(new WaveElement(waveData.element1Type, waveData.element1EnterSide, waveData.element1ExitSide,
                                waveData.element1X, waveData.element1Y, waveData.element1Z, waveData.element1Duration));
                            break;
                        case 2:
                            if (waveData.element2Type.Equals("none")) continue;
                            wave.elements.Add(new WaveElement(waveData.element2Type, waveData.element2EnterSide, waveData.element2ExitSide,
                                waveData.element2X, waveData.element2Y, waveData.element2Z, waveData.element2Duration));
                            break;
                        case 3:
                            if (waveData.element3Type.Equals("none")) continue;
                            wave.elements.Add(new WaveElement(waveData.element3Type, waveData.element3EnterSide, waveData.element3ExitSide,
                                waveData.element3X, waveData.element3Y, waveData.element3Z, waveData.element3Duration));
                            break;
                        case 4:
                            if (waveData.element4Type.Equals("none")) continue;
                            wave.elements.Add(new WaveElement(waveData.element4Type, waveData.element4EnterSide, waveData.element4ExitSide,
                                waveData.element4X, waveData.element4Y, waveData.element4Z, waveData.element4Duration));
                            break;
                    }
                }
                waves.Add(wave);
            }
            return waves;
        }

        private List<Message> LoadMessages(Tutorial_Data.MessageData[] levelMessagesData)
        {
            foreach (Tutorial_Data.MessageData messageData in levelMessagesData)
            {
                messages.Add(new Message(messageData.id, messageData.time, messageData.duration, messageData.message, messageData.portrait));
            }
            return messages;
        }

        public void Update(double dt, GraphicsDevice graphicsDevice)
        {
            playerAim.Update(dt);
            player.Update(dt);

            UpdateProjectiles(dt, graphicsDevice);
            UpdateEnemies(dt);
            UpdatePowerUps(dt);
            UpdateWaves(dt);
            UpdateParticleSystems(dt);
            UpdateMessages(dt);

            ground.Update(dt);
            sky.Update(dt);
            dialogBox.Update(dt);
        }

        private void UpdateProjectiles(double dt, GraphicsDevice graphicsDevice)
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
                    particleSystems.Add(new ParticleSystem(graphicsDevice, player.Position, 5f, 0.5f, 200f, Color.Orange, Color.Red));
                    player.RemoveHp();
                    projectiles.RemoveAt(i);
                    smallExplosionSound.Play();
                    player.Flash(Color.Red, 0.5f);
                    continue;
                }
                // Collision with enemies
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.BoundingBox.Intersects(projectiles[i].BoundingBox)
                        && projectiles[i].FromPlayer)
                    {
                        Vector3 enemyPosition = enemy.Position; // Keep enemy position if the enemy is dead
                        particleSystems.Add(new ParticleSystem(graphicsDevice, enemyPosition, 5f, 0.5f, 200f, Color.LightGreen, Color.Green));
                        enemy.RemoveHp();
                        if (enemy.IsDead)
                        {
                            particleSystems.Add(new ParticleSystem(graphicsDevice, enemyPosition, 10f, 1.5f, 500f, Color.Orange, new Color(100, 0, 0)));
                            explosionSound.Play();
                        }
                        else
                        {
                            enemy.Flash(Color.Red, 0.5f);
                            smallExplosionEnemySound.Play();
                        }
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
                    bonusSound.Play();
                    break;
                }
                if (powerUps[i].Position.Z > 500)
                {
                    powerUps.RemoveAt(i);
                    break;
                }
            }
        }

        private void UpdateWaves(double dt)
        {
            waveTimer += (float)dt;
            if (currentWave < waves.Count && waveTimer >= waves[currentWave].time)
            {
                waves[currentWave].Launch(this);
                currentWave++;
            }
        }

        private void UpdateParticleSystems(double dt)
        {
            for (int i = particleSystems.Count - 1; i >= 0; i--)
            {
                particleSystems[i].Update(dt);
                if (!particleSystems[i].Active)
                {
                    particleSystems.RemoveAt(i);
                }
            }
        }

        private void UpdateMessages(double dt)
        {
            messageTimer += (float)dt;
            if (currentMessage < messages.Count && messageTimer >= messages[currentMessage].time)
            {
                messages[currentMessage].Launch(this);
                currentMessage++;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            Color bgColor = new Color(30, 0, 50);
            graphicsDevice.Clear(bgColor);

            graphicsDevice.BlendState = BlendState.Opaque;

            ground.Draw(view, projection);
            sky.Draw(view, projection);
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

            graphicsDevice.BlendState = BlendState.NonPremultiplied;
            playerAim.Draw(view, projection);
            foreach (ParticleSystem particles in particleSystems)
            {
                particles.Draw(view, projection);
            }

            spriteBatch.Begin();
            dialogBox.Draw(spriteBatch);
            spriteBatch.End();

            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        public void AddProjectile(Vector3 position, Quaternion orientation, float speed, bool fromPlayer = true)
        {
            string modelName = fromPlayer ? "Cube" : "CubeRed";
            var newProjectile = new Projectile(position, orientation, speed, fromPlayer);
            newProjectile.Load(content, modelName);
            projectiles.Add(newProjectile);
        }

        public void GameOver()
        {
            game.GameOver();
        }

        public void AddEnemy(Vector3 position, ScreenSide enterSide, ScreenSide exitSide, float mainPhaseDuration, string modelName)
        {
            Enemy enemy = new Enemy(position, this, enterSide, exitSide, mainPhaseDuration);
            enemy.Load(content, modelName);
            enemies.Add(enemy);
        }

        public void AddPowerUp(Vector3 position)
        {
            PowerUp powerUp = new PowerUp(position, 100f);
            powerUp.Load(content, "BeachBall");
            powerUps.Add(powerUp);
        }

        public void DisplayMessage(string message, DisplayedCharacter character, float duration)
        {
            dialogBox.DisplayMessage(message, character, duration);
        }
    }
}
