using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
namespace Project1
{
    class PlayButton
    {

        private Rectangle _rectangle;
        private Texture2D _texture;
        private bool isPressed = false;

        public PlayButton(Rectangle rectangle, Texture2D texture)
        {
            _rectangle = rectangle;
            _texture = texture;
        }
  
        
        

        public void Update(MouseState mouseState)
        {
            if (_rectangle.Contains(mouseState.X, mouseState.Y))
            {
                if (mouseState.LeftButton == ButtonState.Pressed) {
                    isPressed = true;
                }
                    
                
            }
        }

        public bool IsPressed() {
            return isPressed;
        }

        // Make sure Begin is called on s before you call this function
        public void Draw(SpriteBatch s)
        {
            s.Draw(_texture, _rectangle, Color.White);
        }
    }
}
