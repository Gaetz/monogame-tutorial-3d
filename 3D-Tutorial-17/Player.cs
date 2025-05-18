using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Tutorial_17
{
    internal class Player : Entity
    {
        private SceneGame game;
        private PlayerAim playerAim;
        private BoundingBox boundingBox;

        const float ACCELERATION_RATE = 4000.0f;
        const float DECELERATION_RATE = 0.85f;
        const float MAX_SPEED = 350.0f;
        Rectangle BOUNDS = new Rectangle(-200, -140, 400, 280);
        const float COOLDOWN = 0.25f;
        const float PROJECTILES_RADIUS = 25.0f;

        private float speedX = 0.0f;
        private float speedY = 0.0f;
        private float cooldownTimer = 0.0f;
        private int projectileNumber = 1;

        private int hp = 10;
        bool isDead = false;
        private SoundEffect shootSound;

        private Entity laserAim;
        float laserAimDefaultLength = 0;

        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }

        public Player(PlayerAim playerAim, SceneGame game)
        {
            this.playerAim = playerAim;
            this.game = game;
        }

        public override void Load(ContentManager content, string modelName)
        {
            base.Load(content, modelName);
            position = new Vector3(0, 0.0f, -250.0f);
            scale = new Vector3(2f, 2f, 2f);
            shootSound = content.Load<SoundEffect>("Laser0");
            laserAim = new Entity();
            laserAim.Load(content, "Cube");
            laserAimDefaultLength = (playerAim.Position - position).Length();
        }

        private void HandlingInput(double dt)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Z))
            {
                speedY += ACCELERATION_RATE * (float)dt;
            }
            if (state.IsKeyDown(Keys.S))
            {
                speedY -= ACCELERATION_RATE * (float)dt;
            }
            if (MathF.Abs(speedY) > MAX_SPEED)
            {
                speedY = MathF.Sign(speedY) * MAX_SPEED;
            }


            if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Q))
            {
                speedX -= ACCELERATION_RATE * (float)dt;
            }
            if (state.IsKeyDown(Keys.D))
            {
                speedX += ACCELERATION_RATE * (float)dt;
            }
            if (MathF.Abs(speedX) > MAX_SPEED)
            {
                speedX = MathF.Sign(speedX) * MAX_SPEED;
            }


            position += new Vector3((float)(speedX * dt), (float)(speedY * dt), 0);
            if (position.X < BOUNDS.Left)
            {
                position.X = BOUNDS.Left;
                speedX = 0;
            } 
            else if (position.X > BOUNDS.Right)
            {
                position.X = BOUNDS.Right;
                speedX = 0;
            }
            if (position.Y < BOUNDS.Top)
            {
                position.Y = BOUNDS.Top;
                speedY = 0;
            }
            else if (position.Y > BOUNDS.Bottom)
            {
                position.Y = BOUNDS.Bottom;
                speedY = 0;
            }

            speedX *= DECELERATION_RATE;
            speedY *= DECELERATION_RATE;
        }

        private void HandleAiming()
        {
            Vector3 direction = playerAim.Position - position;
            orientation = Projectile.CreateQuaternionFromDirection(direction);
            ComputeLaserAimTransform(direction, orientation);
        }

        private void ComputeLaserAimTransform(Vector3 direction, Quaternion orientation)
        {
            laserAim.Position = (playerAim.Position - position) / 2f + position;
            laserAim.Orientation = orientation;
            laserAim.Scale = new Vector3(1f, 1f, 2400f * direction.Length() / laserAimDefaultLength);
        }

        public override void Update(double dt)
        {
            HandlingInput(dt);
            HandleAiming();

            // Handle shooting
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed && cooldownTimer <= 0)
            {
                shootSound.Play();
                if (projectileNumber == 1)
                {
                    game.AddProjectile(position, orientation, 1000.0f);
                }
                else
                {
                    CreateProjectiles();
                }
                cooldownTimer = COOLDOWN;
            }
            cooldownTimer -= (float)dt;

            base.Update(dt);
            boundingBox = CreateBoundingBox();
            laserAim.Update(dt);
        }

        public void PowerUp()
        {
            projectileNumber++;
            Flash(new Color(0, 255, 0), 1.0f);
        }

        private void CreateProjectiles()
        {
            List<Projectile> projectiles = new List<Projectile>();
            for (int i = 0; i < projectileNumber; i++)
            {
                Vector3 offset = new Vector3(0, 0, 0);
                float projectileAngle = 2 * MathHelper.Pi / projectileNumber * i;
                offset.X = PROJECTILES_RADIUS * MathF.Cos(projectileAngle);
                offset.Y = PROJECTILES_RADIUS * MathF.Sin(projectileAngle);
                game.AddProjectile(position + offset, orientation, 1000.0f);
            }
        }

        private BoundingBox CreateBoundingBox()
        {
            Vector3[] vertices = {
                new Vector3(-64f, -7f, -67f), new Vector3(64f, -7f, -67f),
                new Vector3(-64f, 25f, -67f), new Vector3(64f, 25f, -67f),
                new Vector3(-64f, -7f, 77f), new Vector3(64f, -7f, 77f),
                new Vector3(-64f, 25f, 77f), new Vector3(64f, 25f, 77f)
            };
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Vector3.Transform(vertices[i], world);
            }
            return BoundingBox.CreateFromPoints(vertices);
        }

        public void RemoveHp()
        {
            hp--;
            if (hp <= 0)
            {
                isDead = true;
                game.GameOver();
            }
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EmissiveColor = currentFlashColor.ToVector3();
                }
            }
            base.Draw(view, projection);
            laserAim.Draw(view, projection);
        }
    }
}
