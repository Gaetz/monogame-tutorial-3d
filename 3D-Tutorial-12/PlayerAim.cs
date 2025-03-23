using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Tutorial_12
{
    internal class PlayerAim : Entity
    {
        private Quad quad;
        GraphicsDevice device;

        public PlayerAim() : base()
        {
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

        public override void Update(double dt)
        {
            MouseState mouse = Mouse.GetState();
            position.X = (device.Viewport.Width / 2 - mouse.X) * -10.04f;
            position.Y = (device.Viewport.Height / 2 - mouse.Y) * 10.04f;

            base.Update(dt);
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            quad.Draw(device, world, view, projection);
        }
    }
}
