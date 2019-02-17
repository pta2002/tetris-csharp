using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input.InputListeners;
using TetrisEngine;

namespace TetrisMonoGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TetrisGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D[] tetrisBlocks;
        Color lineColor = new Color(5, 10, 40);
        TetrisBoard tetrisBoard;
        int inTimer = 500;
        int presses = 0;
        
        public TetrisGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 32 * 20,
                PreferredBackBufferWidth = 32 * 10
            };
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Window.Title = "Tetris (MonoGame)";
            tetrisBoard = new TetrisBoard();

            KeyboardListener keyboardListener = new KeyboardListener(new KeyboardListenerSettings());
            GamePadListener gamePadListener = new GamePadListener(new GamePadListenerSettings());
            Components.Add(new InputListenerComponent(this, keyboardListener, gamePadListener));

            keyboardListener.KeyPressed += (sender, args) =>
            {
                if (args.Key == Keys.Up)
                    tetrisBoard.Rotate();
                else if (args.Key == Keys.Space)
                    tetrisBoard.PlaceDown();
            };

            gamePadListener.ButtonDown += (sender, args) =>
            {
                if (args.Button == Buttons.DPadUp || args.Button == Buttons.Y)
                    tetrisBoard.Rotate();
                if (args.Button == Buttons.B)
                    tetrisBoard.PlaceDown();
            };

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tetrisBlocks = new Texture2D[7];
            for (int i = 0; i < tetrisBlocks.Length; i++)
            {
                tetrisBlocks[i] = Content.Load<Texture2D>("tetrisblocks_" + i);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            tetrisBoard.Tick(gameTime.ElapsedGameTime.Milliseconds);

            bool resetTimer = true;
            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                resetTimer = false;
                if ((inTimer >= 30 && presses != 1) || (presses == 1 && inTimer >= 100))
                {
                    inTimer = 0;
                    presses++;
                    tetrisBoard.MoveLeft();
                }
                else
                {
                    inTimer += gameTime.ElapsedGameTime.Milliseconds;
                }
            }
            else if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                resetTimer = false;
                if ((inTimer >= 30 && presses != 1) || (presses == 1 && inTimer >= 100))
                {
                    inTimer = 0;
                    presses++;
                    tetrisBoard.MoveRight();
                }
                else
                {
                    inTimer += gameTime.ElapsedGameTime.Milliseconds;
                }
            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                resetTimer = false;
                if ((inTimer >= 30 && presses != 1) || (presses == 1 && inTimer >= 100))
                {
                    inTimer = 0;
                    presses++;
                    tetrisBoard.GoDown();
                }
                else
                {
                    inTimer += gameTime.ElapsedGameTime.Milliseconds;
                }
            }

            if (resetTimer)
            {
                inTimer = 500;
                presses = 0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(5, 5, 20));

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
                
            for (int y = 1; y < 20; y++)
            {
                spriteBatch.DrawLine(0, y*32, 32*10, y*32, lineColor);
            }

            for (int x = 1; x < 10; x++)
            {
                spriteBatch.DrawLine(x*32, 0, x*32, 32*20, lineColor);
            }

            Piece ghostPiece = tetrisBoard.FallLocation();
            foreach (Block b in ghostPiece.Blocks)
                spriteBatch.Draw(tetrisBlocks[b.Color], new Vector2((b.X + ghostPiece.X) * 32, (b.Y + ghostPiece.Y) * 32), new Color(Color.White, 0.3f));
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);
            foreach (Block b in tetrisBoard.GetBlocks())
            {
                spriteBatch.Draw(tetrisBlocks[b.Color], new Vector2(b.X * 32, b.Y * 32), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
