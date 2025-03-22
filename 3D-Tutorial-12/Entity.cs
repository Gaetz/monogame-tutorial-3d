﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Tutorial_11
{
    internal class Entity
    {
        protected Model model;
        protected Vector3 position;
        protected Quaternion orientation;
        protected Vector3 scale;
        protected Matrix world;

        private float flashTimer = 0.0f;
        private float flashDuration = 0.5f;
        private Color currentFlashColor = Color.White;
        private Color flashColor = Color.Red;

        public Vector3 Position
        {
            get { return position; }
        }

        public Entity()
        {
            position = Vector3.Zero;
            orientation = Quaternion.Identity;
            scale = Vector3.One;
            world = Matrix.Identity;
        }

        public virtual void Load(ContentManager content, string modelName)
        {
            model = content.Load<Model>(modelName);
        }

        public virtual void Update(double dt)
        {
            UpdateFlash(dt);
            world = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(orientation) * Matrix.CreateTranslation(position);
        }

        public virtual void Draw(Matrix view, Matrix projection)
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

        protected void UpdateFlash(double dt)
        {
            if (flashTimer > 0.0f)
            {
                currentFlashColor = Color.Lerp(Color.White, flashColor, flashTimer/flashDuration);
                flashTimer -= (float)dt;
                if (flashTimer <= 0.0f)
                {
                    currentFlashColor = Color.White;
                }
                SetDiffuseColor(currentFlashColor);
            }
        }

        public void Flash(Color color, float duration)
        {
            flashColor = color;
            flashTimer = duration;
            flashDuration = duration;
        }

        protected virtual void SetDiffuseColor(Color color)
        {

        }
    }
}
