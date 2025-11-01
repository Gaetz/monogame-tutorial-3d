using Microsoft.Xna.Framework;

namespace Tutorial_18
{
    internal class Camera
    {
        private Matrix view;
        private Matrix projection;

        public Matrix View => view;
        public Matrix Projection => projection;

        private Vector3 position;
        private Vector3 target;
        private Vector3 up;
        private float fieldOfView;
        private float aspectRatio;
        private float nearPlane;
        private float farPlane;


        public Camera(Vector3 position, Vector3 target, Vector3 up, float fieldOfViewDegrees, float aspectRatio = 800f / 480f, float nearPlane = 1f, float farPlane = 10000f)
        {
            this.position = position;
            this.target = target;
            this.up = up;
            this.fieldOfView = MathHelper.ToRadians(fieldOfViewDegrees);
            this.aspectRatio = aspectRatio;
            this.nearPlane = nearPlane;
            this.farPlane = farPlane;

            UpdateMatrices();
        }

        private void UpdateMatrices()
        {
            view = Matrix.CreateLookAt(position, target, up);
            projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlane, farPlane);
        }

        public float FieldOfView
        {
            get => MathHelper.ToDegrees(fieldOfView);
            set
            {
                fieldOfView = MathHelper.ToRadians(value);
                UpdateMatrices();
            }
        }

        public Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                UpdateMatrices();
            }
        }

        public Vector3 Target
        {
            get => target;
            set
            {
                target = value;
                UpdateMatrices();
            }
        }

        public Vector3 Up
        {
            get => up;
            set
            {
                up = value;
                UpdateMatrices();
            }
        }

        public float AspectRatio
        {
            get => aspectRatio;
            set
            {
                aspectRatio = value;
                UpdateMatrices();
            }
        }

        public float NearPlane
        {
            get => nearPlane;
            set
            {
                nearPlane = value;
                UpdateMatrices();
            }
        }

        public float FarPlane
        {
            get => farPlane;
            set
            {
                farPlane = value;
                UpdateMatrices();
            }
        }

    }
}
