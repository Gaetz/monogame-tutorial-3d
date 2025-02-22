using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            position = new Vector3(0, 0, -500);

            this.device = device;
            BasicEffect effect = new BasicEffect(device);
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = content.Load<Texture2D>("Crosshair");
            quad = new Quad(Vector3.Zero, -Vector3.Forward, Vector3.Up, 100, 100, effect);
        }

        public void Update(double dt)
        {
            MouseState mouse = Mouse.GetState();
            position.X = (device.Viewport.Width / 2 - mouse.X) * -1.04f;
            position.Y = (device.Viewport.Height / 2 - mouse.Y) * 1.04f;

            world = Matrix.CreateTranslation(position);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            quad.Draw(device, world, view, projection);
        }
    }
}
