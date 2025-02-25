using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Tutorial_04
{
    internal class Projectile : Entity
    {
        private float speed;
        private bool fromPlayer;

        public Projectile(Vector3 position, Quaternion orientation, float speed, bool fromPlayer = true) : base()
        {
            this.position = position;
            this.orientation = orientation;
            this.speed = speed;
            this.fromPlayer = fromPlayer;
        }

        public override void Load(ContentManager content, string modelName)
        {
            base.Load(content, modelName);
            scale = new Vector3(5f, 5f, 50f);
        }

        public override void Update(double dt)
        {
            Vector3 direction = Vector3.Transform(-Vector3.Forward, orientation);
            position += direction * speed * (float)dt;
            base.Update(dt);
        }
    }
}
