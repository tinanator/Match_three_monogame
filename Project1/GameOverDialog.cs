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
    class GameOverDialog
    {
        private Texture2D _texture;
        private Rectangle _rectangle;
        private PlayButton closeButton;
        public bool IsPressed => closeButton.IsPressed();
        public GameOverDialog(Texture2D _texture, Texture2D _btnTexture, Rectangle _rectangle) {
            this._texture = _texture;
            this._rectangle = _rectangle;
            closeButton = new PlayButton(new Rectangle(_rectangle.X, _rectangle.Y + _rectangle.Height + 30, _rectangle.Width, 50), _btnTexture);
        }
        public void Draw(SpriteBatch _spriteBatch) {
            _spriteBatch.Draw(_texture, _rectangle, Color.White);
            closeButton.Draw(_spriteBatch);
            

        }

        public void Update(GameTime gameTime) {
            MouseState state = Mouse.GetState();
            closeButton.Update(state);
        }
    }
}
