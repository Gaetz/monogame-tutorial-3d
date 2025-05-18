using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Tutorial_16
{
    internal class Entity
    {
        protected Model model;
        protected Vector3 position;
        protected Quaternion orientation;
        protected Vector3 scale;
        protected Matrix world;

        protected float flashTimer = 0.0f;
        protected float flashDuration = 0.5f;
        protected Color currentFlashColor = Color.Transparent;
        protected Color flashColor = Color.Red;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Quaternion Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public Vector3 Scale { 
            get { return scale; }
            set { scale = value; }
        }

        public Entity()
        {
            position = Vector3.Zero;
            orientation = Quaternion.Identity;
            scale = Vector3.One;
            world = Matrix.Identity;
        }

        public virtual void Load(ContentManager content, string modelName)
        {
            model = content.Load<Model>(modelName);
        }

        public virtual void Update(double dt)
        {
            UpdateFlash(dt);
            world = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(orientation) * Matrix.CreateTranslation(position);
        }

        public virtual void Draw(Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.FogEnabled = true;
                    effect.FogStart = 1000;
                    effect.FogEnd = 2000;
                    effect.FogColor = new Color(30, 0, 50).ToVector3();
                }
                
                mesh.Draw();
            }
        }

        protected void UpdateFlash(double dt)
        {
            if (flashTimer > 0.0f)
            {
                currentFlashColor = Color.Lerp(Color.Transparent, flashColor, flashTimer/flashDuration);
                flashTimer -= (float)dt;
                if (flashTimer <= 0.0f)
                {
                    currentFlashColor = Color.Transparent;
                }
            }
        }

        public void Flash(Color color, float duration)
        {
            flashColor = color;
            flashTimer = duration;
            flashDuration = duration;
        }
    }
}
