using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tutorial_18
{
    public enum SceneType
    {
        Game,
        Menu,
        GameOver
    }

    public interface Scene
    {
        public void Load(ContentManager contentManager, GraphicsDevice graphicsDevice);
        public void Update(double dt, GraphicsDevice graphicsDevice);
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice);

    }
}
