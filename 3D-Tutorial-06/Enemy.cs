using Microsoft.Xna.Framework;
using System;

namespace Tutorial_06
{
    internal class Enemy : Entity
    {
        private const float COLLIDER_SCALE = 30.0f;
        private BoundingBox boundingBox;

        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }

        public Enemy(Vector3 position) : base()
        {
            this.position = position;
            scale = new Vector3(2f, 2f, 2f);
        }

        private BoundingBox CreateBoundingBox()
        {
            Vector3 min = new Vector3(-0.5f * COLLIDER_SCALE, -0.5f * COLLIDER_SCALE, -0.5f * COLLIDER_SCALE);
            Vector3 max = new Vector3(0.5f * COLLIDER_SCALE, 0.5f * COLLIDER_SCALE, 0.5f * COLLIDER_SCALE);
            min = Vector3.Transform(min, world);
            max = Vector3.Transform(max, world);
            return new BoundingBox(min, max);
        }

        public override void Update(double dt)
        {
            base.Update(dt);
            boundingBox = CreateBoundingBox();
        }

        public void SetRandomPosition()
        {
            Random random = new Random();
            position = new Vector3(random.Next(-100, 100), random.Next(-100, 100), -500);
        }
    }
}
