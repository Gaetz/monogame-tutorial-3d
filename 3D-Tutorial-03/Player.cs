using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Tutorial_03
{
    internal class Player
    {
        private Model model;
        private Vector3 position;
        private Quaternion orientation;
        private Vector3 scale;
        private Matrix world;
        private PlayerAim playerAim;

        const float ACCELERATION_RATE = 4000.0f;
        const float DECELERATION_RATE = 0.85f;
        const float MAX_SPEED = 350.0f;
        Rectangle BOUNDS = new Rectangle(-200, -140, 400, 280);

        private float speedX = 0.0f;
        private float speedY = 0.0f;

        public Player(PlayerAim playerAim)
        {
            this.playerAim = playerAim;
        }

        public void Load(ContentManager content)
        {
            model = content.Load<Model>("Ship");
            position = new Vector3(0, 0.0f, -250.0f);
            orientation = Quaternion.Identity;
            scale = new Vector3(2f, 2f, 2f);
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
            direction.Normalize();

            Vector3 xAxis = Vector3.Cross(Vector3.Up, direction);
            xAxis.Normalize();

            Vector3 yAxis = Vector3.Cross(direction, xAxis);
            yAxis.Normalize();

            Matrix aim = Matrix.Identity;
            aim.M11 = xAxis.X;
            aim.M21 = yAxis.X;
            aim.M31 = direction.X;

            aim.M12 = xAxis.Y;
            aim.M22 = yAxis.Y;
            aim.M32 = direction.Y;

            aim.M13 = xAxis.Z;
            aim.M23 = yAxis.Z;
            aim.M33 = direction.Z;

            orientation = Quaternion.CreateFromRotationMatrix(aim);
        }

        public void Update(double dt)
        {
            HandlingInput(dt);
            HandleAiming();

            world = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(orientation) * Matrix.CreateTranslation(position);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }

    }
}
