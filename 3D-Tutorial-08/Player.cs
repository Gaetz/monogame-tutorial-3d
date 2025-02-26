using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Tutorial_08
{
    internal class Player : Entity
    {
        private PlayerAim playerAim;
        private Game1 game;
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

        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }

        public Player(PlayerAim playerAim, Game1 game)
        {
            this.playerAim = playerAim;
            this.game = game;
        }

        public override void Load(ContentManager content, string modelName)
        {
            base.Load(content, modelName);
            position = new Vector3(0, 0.0f, -250.0f);
        }

        private void HandlingInput(double dt)
        {
            // Controls
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.W))
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


            if (state.IsKeyDown(Keys.A))
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
        }

        public void Update(double dt)
        {
            HandlingInput(dt);
            HandleAiming();

            // Handle shooting
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed && cooldownTimer <= 0)
            {
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
        }

        public void PowerUp()
        {
            projectileNumber++;
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
    }
}
