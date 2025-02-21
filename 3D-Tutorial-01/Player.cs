using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_01
{
    internal class Player
    {
        private Model model;
        private Vector3 position;
        private Quaternion orientation;
        private Matrix world;

        public void Load(ContentManager content)
        {
            model = content.Load<Model>("Ship");
            position = new Vector3(0, 0.0f, 250.0f);
            orientation = Quaternion.Identity;
        }

        public void Update(double dt)
        {
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
