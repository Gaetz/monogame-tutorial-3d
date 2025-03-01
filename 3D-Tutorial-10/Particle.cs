using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_10
{
    internal class Particle : Entity
    {
        private Quad quad;
        GraphicsDevice device;

        Vector3 velocity = Vector3.Zero;
        float lifeTime;
        float age = 0;
        Color startColor;
        Color endColor;

        public bool IsDead
        {
            get { return age >= lifeTime; }
        }

        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Particle(GraphicsDevice device, Vector3 position, float scale, float lifeTime, Color startColor, Color endColor) : base()
        {
            this.device = device;
            
            BasicEffect effect = new BasicEffect(device);
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = new Texture2D(device, 1, 1);
            effect.Texture.SetData(new Color[] { Color.White });
            quad = new Quad(Vector3.Zero, -Vector3.Forward, Vector3.Up, scale, scale, effect);

            this.position = position;
            this.lifeTime = lifeTime;
            this.startColor = startColor;
            this.endColor = endColor;
        }

        public override void Update(double dt)
        {
            float dtf = (float)dt;
            age += dtf;
            velocity *= 0.98f;
            velocity.Y -= 10f * dtf;
            position += velocity * dtf;
            float lerpAmount = age / lifeTime;
            Color color = Color.Lerp(startColor, endColor, lerpAmount);
            quad.Effect.DiffuseColor = color.ToVector3();
            base.Update(dt);
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            quad.Draw(device, world, view, projection);
        }
    }
}
