using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace Tutorial_04
{
    internal class Entity
    {
        protected Model model;
        protected Vector3 position;
        protected Quaternion orientation;
        protected Vector3 scale;
        protected Matrix world;

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
    }
}
