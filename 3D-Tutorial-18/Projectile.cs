using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Tutorial_18
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

        public static Quaternion CreateQuaternionFromDirection(Vector3 direction)
        {
            direction.Normalize();

            Vector3 xAxis = Vector3.Cross(Vector3.Up, direction);
            xAxis.Normalize();

            Vector3 yAxis = Vector3.Cross(direction, xAxis);
            yAxis.Normalize();

            Matrix aim = Matrix.Identity;
            aim.M11 = xAxis.X;
            aim.M21 = yAxis.X;
            aim.M31 = direction.X;

            aim.M12 = xAxis.Y;
            aim.M22 = yAxis.Y;
            aim.M32 = direction.Y;

            aim.M13 = xAxis.Z;
            aim.M23 = yAxis.Z;
            aim.M33 = direction.Z;

            return Quaternion.CreateFromRotationMatrix(aim);
        }
    }
}
