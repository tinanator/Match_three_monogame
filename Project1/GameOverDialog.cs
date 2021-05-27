using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project1
{
    class GameOverDialog
    {
        private Texture2D texture;
        private Rectangle rectangle;
        private PlayButton closeButton;
        public bool IsPressed => closeButton.IsPressed();
        public GameOverDialog(Texture2D texture, Texture2D btnTexture, Rectangle rectangle) {
            this.texture = texture;
            this.rectangle = rectangle;
            closeButton = new PlayButton(new Rectangle(rectangle.X, rectangle.Y + rectangle.Height + 30, rectangle.Width, 50), btnTexture);
        }
        public void Draw(SpriteBatch _spriteBatch) {
            _spriteBatch.Draw(texture, rectangle, Color.White);
            closeButton.Draw(_spriteBatch);
            

        }

        public void Update(GameTime gameTime) {
            MouseState state = Mouse.GetState();
            closeButton.Update(state);
        }
    }
}
