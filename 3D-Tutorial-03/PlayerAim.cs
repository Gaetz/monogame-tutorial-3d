﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Tutorial_03
{
    internal class PlayerAim
    {
        private Quad quad;
        private Vector3 position;
        private Quaternion orientation;
        private Matrix world;
        GraphicsDevice device;
        public Vector3 Position
        {
            get { return position; }
        }

        public void Load(ContentManager content, GraphicsDevice device)
        {
            position = new Vector3(0, 0, -5000);

            this.device = device;
            BasicEffect effect = new BasicEffect(device);
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = content.Load<Texture2D>("Crosshair");
            quad = new Quad(Vector3.Zero, -Vector3.Forward, Vector3.Up, 1000, 1000, effect);
        }

        public void Update(double dt)
        {
            MouseState mouse = Mouse.GetState();
            position.X = (device.Viewport.Width / 2 - mouse.X) * -10.04f;
            position.Y = (device.Viewport.Height / 2 - mouse.Y) * 10.04f;

            world = Matrix.CreateTranslation(position);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            quad.Draw(device, world, view, projection);
        }
    }
}
