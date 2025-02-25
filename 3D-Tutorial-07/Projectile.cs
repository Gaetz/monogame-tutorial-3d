using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Tutorial_07
{
    internal class Projectile : Entity
    {
        private float speed;
        private bool fromPlayer;
        private BoundingBox boundingBox;

        public bool FromPlayer
        {
            get { return fromPlayer; }
        }

        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }

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
            boundingBox = CreateBoundingBox();
        }

        private BoundingBox CreateBoundingBox()
        {
            Vector3[] vertices = {
                new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, -0.5f)
            };
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Vector3.Transform(vertices[i], world);
            }
            return BoundingBox.CreateFromPoints(vertices);
        }
    }
}
