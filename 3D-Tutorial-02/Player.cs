using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_02
{
    internal class Player
    {
        private Model model;
        private Vector3 position;
        private Quaternion orientation;
        private Matrix world;

        const float ACCELERATION_RATE = 4000.0f;
        const float DECELERATION_RATE = 0.85f;
        const float MAX_SPEED = 350.0f;
        Rectangle BOUNDS = new Rectangle(-200, -140, 400, 280);

        private float speedX = 0.0f;
        private float speedY = 0.0f;

        public void Load(ContentManager content)
        {
            model = content.Load<Model>("Ship");
            position = new Vector3(0, 0.0f, 250.0f);
            orientation = Quaternion.Identity;
        }

        private void HandlingInput(double dt)
        {
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
                speedX += ACCELERATION_RATE * (float)dt;
            }
            if (state.IsKeyDown(Keys.D))
            {
                speedX -= ACCELERATION_RATE * (float)dt;
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

        public void Update(double dt)
        {
            HandlingInput(dt);
            world = Matrix.CreateFromQuaternion(orientation) * Matrix.CreateTranslation(position);
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
