using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;


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

        public SpriteBatch spriteBatch;

        public ScreenState State { get; set; }
        public StartScreen(Game1 game) : base(game) 
        {
            State = ScreenState.Active;
        }

        private Texture2D texture;
        private Rectangle rectangle;
        public override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("btn");
            rectangle = new Rectangle(150, 150, 500, 200);
            playBtn = new PlayButton(rectangle, texture);
        }

        public override void Update(GameTime gameTime)
        {
            if (State == ScreenState.Hidden)
            {
                return;
            }

            MouseState state = Mouse.GetState();
            playBtn.Update(state);
            if (playBtn.IsPressed) {
                State = ScreenState.TransitionOn;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            playBtn.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
