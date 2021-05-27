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

    public enum ScreenState

    {
        TransitionOn,
        Hidden,
        Active
    }
    class StartScreen : GameScreen
    {
        private PlayButton playBtn;

        public SpriteBatch _spriteBatch;

        public ScreenState State { get; set; }
        public StartScreen(Game1 game) : base(game) {
            State = ScreenState.Active;
        }

        private Texture2D _texture;
        private Rectangle _rectangle;
        public override void LoadContent()
        {
            base.LoadContent();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _texture = Content.Load<Texture2D>("btn");
            _rectangle = new Rectangle(100, 100, 500, 200);
            playBtn = new PlayButton(_rectangle, _texture);
        }

        public override void Update(GameTime gameTime)
        {
            if (State == ScreenState.Hidden)
            {
                return;
            }

            MouseState state = Mouse.GetState();
            playBtn.Update(state);
            if (playBtn.IsPressed()) {
                State = ScreenState.TransitionOn;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            playBtn.Draw(_spriteBatch);
            _spriteBatch.End();

        }
    }
}
