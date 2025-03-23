using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_13
{
    internal class ShiftingTexture : Entity
    {
        private Quad quad;
        private GraphicsDevice device;
        private Vector2 size;
        private Vector2 shiftSpeed;

        public ShiftingTexture(Vector3 position, Quaternion orientation, Vector2 size, Vector2 shiftSpeed) : base()
        {
            this.position = position;
            this.orientation = orientation;
            this.size = size;
            this.shiftSpeed = shiftSpeed;
        }

        public void Load(ContentManager content, GraphicsDevice device, string textureName)
        {
            this.device = device;
            BasicEffect effect = new BasicEffect(device);
            effect.FogEnabled = true;
            effect.FogStart = 1000;
            effect.FogEnd = 2000;
            effect.FogColor = new Color(30, 0, 50).ToVector3();
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = content.Load<Texture2D>(textureName);

            quad = new Quad(Vector3.Zero, -Vector3.Forward, Vector3.Up, size.X, size.Y, effect);
        }

        public override void Update(double dt)
        {
            quad.TextureShiftSpeed = shiftSpeed * (float)dt;
            base.Update(dt);
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            quad.Draw(device, world, view, projection);
        }
    }
}
