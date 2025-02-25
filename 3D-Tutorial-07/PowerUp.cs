using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_06
{
    internal class PowerUp : Entity
    {
        private float speed;
        private BoundingBox boundingBox;

        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }

        public PowerUp(Vector3 position, float speed)
        {
            this.position = position;
            this.speed = speed;
            scale = new Vector3(10f, 10f, 10f);
        }

        public override void Update(double dt)
        {
            position.Z += speed * (float)dt;
            base.Update(dt);
            boundingBox = CreateBoundingBox();
        }

        private BoundingBox CreateBoundingBox()
        {
            Vector3 min = new Vector3(-2, -2, -2);
            Vector3 max = new Vector3(2, 2, 2);
            min = Vector3.Transform(min, world);
            max = Vector3.Transform(max, world);
            return new BoundingBox(min, max);
        }

        public void SetRandomPosition()
        {
            Random random = new Random();
            position = new Vector3(random.Next(-200, 200), random.Next(-100, 100), -1000);
        }
    }
}
