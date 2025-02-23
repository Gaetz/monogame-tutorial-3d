using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_05
{
    internal class Quad
    {
        private VertexPositionNormalTexture[] vertices;
        private int[] indices;

        private Vector3 origin;
        private Vector3 up;
        private Vector3 normal;
        private Vector3 left;

        public Vector3 upperLeft;
        public Vector3 upperRight;
        public Vector3 lowerLeft;
        public Vector3 lowerRight;

        private BasicEffect effect;


        public Quad(Vector3 origin, Vector3 normal, Vector3 up,
            float width, float height, BasicEffect effect)
        {
            vertices = new VertexPositionNormalTexture[4];
            indices = new int[6];
            this.origin = origin;
            this.normal = normal;
            this.up = up;

            // Calculate the quad corners
            left = Vector3.Cross(normal, this.up);
            Vector3 uppercenter = (this.up * height / 2) + origin;
            upperLeft = uppercenter + (this.left * width / 2);
            upperRight = uppercenter - (this.left * width / 2);
            lowerLeft = this.upperLeft - (this.up * height);
            lowerRight = this.upperRight - (this.up * height);

            FillVertices();
            this.effect = effect;
        }

        private void FillVertices()
        {
            Vector2 textureUpperLeft = new Vector2(0.0f, 0.0f);
            Vector2 textureUpperRight = new Vector2(1.0f, 0.0f);
            Vector2 textureLowerLeft = new Vector2(0.0f, 1.0f);
            Vector2 textureLowerRight = new Vector2(1.0f, 1.0f);

            for (int i = 0; i < this.vertices.Length; i++)
            {
                vertices[i].Normal = normal;
            }

            vertices[0].Position = lowerLeft;
            vertices[0].TextureCoordinate = textureLowerLeft;
            vertices[1].Position = upperLeft;
            vertices[1].TextureCoordinate = textureUpperLeft;
            vertices[2].Position = lowerRight;
            vertices[2].TextureCoordinate = textureLowerRight;
            vertices[3].Position = upperRight;
            vertices[3].TextureCoordinate = textureUpperRight;

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 2;
            indices[4] = 1;
            indices[5] = 3;
        }

        public void Draw(GraphicsDevice device, Matrix world, Matrix view, Matrix projection)
        {
            effect.World = world;
            effect.View = view;
            effect.Projection = projection;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList, vertices, 0, 4, indices, 0, 2
                );
            }
        }
    }
}