using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace Tutorial_05
{
    internal class Projectile
    {
        private Model model;
        private Vector3 position;
        private Quaternion orientation;
        private Vector3 scale;
        private float speed;
        private bool fromPlayer;
        private Matrix world;

        public Vector3 Position
        {
            get { return position; }
        }
        
        public bool FromPlayer
        {
            get { return fromPlayer; }
        }

        public BoundingBox BoundingBox
        {
            get { return CreateBoundingBox(); }
        }

        public Projectile(Vector3 position, Quaternion orientation, float speed, bool fromPlayer = true)
        {
            this.position = position;
            this.orientation = orientation;
            this.speed = speed;
            this.fromPlayer = fromPlayer;
        }

        private BoundingBox CreateBoundingBox()
        {
            Vector3[] vertices = { 
                new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, -0.5f)
            };
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Vector3.Transform(vertices[i], world);
            }
            return BoundingBox.CreateFromPoints(vertices);
        }

        public void Load(ContentManager content)
        {
            model = content.Load<Model>("Cube");
            scale = new Vector3(5f, 5f, 50f);
        }

        public void Update(double dt)
        {
            Vector3 direction = Vector3.Transform(-Vector3.Forward, orientation);
            position += direction * speed * (float)dt;
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
