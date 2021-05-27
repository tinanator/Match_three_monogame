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
    enum GamePlayState { 
        play,
        stop
    }
    class MainScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public GamePlayState playState = GamePlayState.play;
        public ScreenState State { get; set; }
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        private Dictionary<ItemTypes, Texture2D> atlas = new Dictionary<ItemTypes, Texture2D>();
        private GameField gameField;
        private SpriteFont font;
        private GameOverDialog gameOver;
        private Texture2D okBtnTexture;
        private Texture2D gameOverTexture;

        const float countDuration = 60000f;
        float currentTime = countDuration;

        public MainScreen(Game1 game) : base(game) {
            State = ScreenState.Active;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Score");
            atlas[ItemTypes.square] = Content.Load<Texture2D>("square");
            atlas[ItemTypes.circle] = Content.Load<Texture2D>("circle");
            atlas[ItemTypes.star] = Content.Load<Texture2D>("star");
            atlas[ItemTypes.romb] = Content.Load<Texture2D>("romb");
            atlas[ItemTypes.triangle] = Content.Load<Texture2D>("triangle");

            gameField = new GameField(atlas, font);

            atlas = new Dictionary<ItemTypes, Texture2D>();

            gameOverTexture = Content.Load<Texture2D>("gameOver");

            okBtnTexture = Content.Load<Texture2D>("okBtn");

            gameOver = new GameOverDialog(gameOverTexture, okBtnTexture,  new Rectangle(300, 300, 300, 100));
        }

        private MouseState lastMouseState = new MouseState();


        public override void Update(GameTime gameTime)
        {
            if (State == ScreenState.Hidden)
            {
                return;
            }

            Debug.WriteLine(playState);
            gameField.Update(gameTime);
            if (playState == GamePlayState.play)
            {
                currentTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (currentTime < 0)
                {
                    playState = GamePlayState.stop;

                }

                MouseState state = Mouse.GetState();


                // TODO: Add your update logic here
                if (state.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    Debug.WriteLine(state.X + ' ' + state.Y);
                    gameField.onClick(state.X, state.Y);
                }
                lastMouseState = state;


            }
            else {
                gameOver.Update(gameTime);
                if (gameOver.IsPressed) {
                    State = ScreenState.TransitionOn;
                    playState = GamePlayState.play;
                }
            }
           

        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(new Color(16, 139, 204));

            _spriteBatch.Begin();

            Texture2D _gridTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _spriteBatch.DrawString(font, "Time: " + (int)(currentTime / 1000), new Vector2(730, 30), Color.Black);

            gameField.drawField(_spriteBatch, _gridTexture, _graphics);

            if (playState == GamePlayState.stop) {
                gameOver.Draw(_spriteBatch);
            }
            _spriteBatch.End();
        }
    }
}
