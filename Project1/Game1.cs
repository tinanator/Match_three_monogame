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
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        private Dictionary<ItemTypes, Texture2D> atlas;
        private GameField gameField;
        private SpriteFont font;

        private MainScreen mainScreen;
        private StartScreen startScreen;

        private readonly ScreenManager _screenManager;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
            

        }

        private void LoadScreen1()
        {
            mainScreen = new MainScreen(this);
            _screenManager.LoadScreen(mainScreen, new FadeTransition(GraphicsDevice, Color.Black));
        }

        private void LoadScreen2()
        {
            startScreen = new StartScreen(this);
            _screenManager.LoadScreen(startScreen);
        }


        protected override void Initialize()
        {

            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 850;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            // TODO: Add your initialization logic here


            LoadScreen2();

            base.Initialize();
        }

        
        protected override void LoadContent()
        {
           
        }
        
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (startScreen.State == ScreenState.TransitionOn) {
                LoadScreen1();
                startScreen.State = ScreenState.Hidden;
            }
            if (mainScreen != null && mainScreen.State == ScreenState.TransitionOn) {
                LoadScreen2();
                mainScreen.State = ScreenState.Hidden;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
