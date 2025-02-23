﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_06
{
    internal class PowerUp
    {
        private Model model;
        private Vector3 position;
        private Quaternion orientation;
        private Vector3 scale;
        private Matrix world;
        private float speed;

        public BoundingBox BoundingBox
        {
            get { return CreateBoundingBox(); }
        }

        public Vector3 Position 
        { 
            get { return position; } 
        }

        public PowerUp(Vector3 position, float speed)
        {
            this.position = position;
            this.speed = speed;
            orientation = Quaternion.Identity;
            scale = new Vector3(10f, 10f, 10f);
        }

        private BoundingBox CreateBoundingBox()
        {
            Vector3 min = new Vector3(-2, -2, -2);
            Vector3 max = new Vector3(2, 2, 2);
            min = Vector3.Transform(min, world);
            max = Vector3.Transform(max, world);
            return new BoundingBox(min, max);
        }

        public void Load(ContentManager content)
        {
            model = content.Load<Model>("BeachBall");
        }

        public void Update(double dt)
        {
            position.Z += speed * (float)dt;
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
        public void SetRandomPosition()
        {
            Random random = new Random();
            position = new Vector3(random.Next(-200, 200), random.Next(-100, 100), -1000);
        }
    }
}
