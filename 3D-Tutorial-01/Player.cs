using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tutorial_01
{
    internal class Player
    {
        private Model model;
        private Vector3 position;
        private Quaternion orientation;
        private Vector3 scale;
        private Matrix world;

        public void Load(ContentManager content)
        {
            model = content.Load<Model>("Ship");
            position = new Vector3(0, 0.0f, -250.0f);
            orientation = Quaternion.Identity;
            scale = new Vector3(2f, 2f, 2f);
        }

        public void Update(double dt)
        {
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
