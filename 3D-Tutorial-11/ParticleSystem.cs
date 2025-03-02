using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Tutorial_11
{
    internal class ParticleSystem
    {
        private GraphicsDevice device;
        private bool active = true;
        private const int MAX_PARTICLES = 100;
        private List<Particle> particles = [];
        Random random = new Random();

        public bool Active
        {
            get { return active; }
        }

        public ParticleSystem(GraphicsDevice device, Vector3 position, float scale, float lifetime, float speed, Color startColor, Color endColor) 
        { 
            this.device = device;
            particles.Capacity = MAX_PARTICLES;
            for (int i = 0; i < MAX_PARTICLES; i++)
            {
                Particle p = new Particle(device, position, scale, lifetime, startColor, endColor);
                p.Velocity = RandomVector3(-speed, speed);
                particles.Add(p);
            }
        }

        public void Update(double dt)
        {
            if (!active)
            {
                return;
            }
            if (particles.Count == 0)
            {
                active = false;
                return;
            }


            foreach (Particle p in particles)
            {
                p.Update(dt);
            }

            particles.RemoveAll(p => p.IsDead);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            if (!active)
            {
                return;
            }
            foreach (Particle p in particles)
            {
                p.Draw(view, projection);
            }
        }

        private Vector3 RandomVector3(float v1, float v2) => new(RandomFloat(v1, v2), RandomFloat(v1, v2), RandomFloat(v1, v2));

        float RandomFloat(float v1, float v2) => (float)(v1 + (v2 - v1) * random.NextDouble());
    }
}
