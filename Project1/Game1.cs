using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace Project1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;

        private MainScreen mainScreen;

        private StartScreen startScreen;

        private readonly ScreenManager screenManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            screenManager = new ScreenManager();
            Components.Add(screenManager);
        }

        private void LoadMainScreen()
        {
            mainScreen = new MainScreen(this);
            screenManager.LoadScreen(mainScreen, new FadeTransition(GraphicsDevice, Color.Black));
        }

        private void LoadStartScreen()
        {
            startScreen = new StartScreen(this);
            screenManager.LoadScreen(startScreen);
        }


        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 850;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            LoadStartScreen();

            base.Initialize();
        }

        
        protected override void LoadContent()
        {
           
        }
        
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (startScreen.State == ScreenState.TransitionOn) {
                LoadMainScreen();
                startScreen.State = ScreenState.Hidden;
            }
            if (mainScreen != null && mainScreen.State == ScreenState.TransitionOn) {
                LoadStartScreen();
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
