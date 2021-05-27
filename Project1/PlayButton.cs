using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project1
{
    class PlayButton
    {

        private Rectangle rectangle;
        private Texture2D texture;
        private bool isPressed = false;

        public PlayButton(Rectangle rectangle, Texture2D texture)
        {
            this.rectangle = rectangle;
            this.texture = texture;
        }
  
        
        public void Update(MouseState mouseState)
        {
            if (rectangle.Contains(mouseState.X, mouseState.Y))
            {
                if (mouseState.LeftButton == ButtonState.Pressed) {
                    isPressed = true;
                }
            }
        }

        public bool IsPressed() {
            return isPressed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
