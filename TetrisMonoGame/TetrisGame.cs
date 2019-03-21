using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Input.InputListeners;
using System;
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
        Song themeSong;
        Color lineColor = new Color(5, 10, 40);
        Color lineColorB = new Color(26, 38, 109);
        TetrisBoard tetrisBoard;
        SpriteFont eightyFour;
        SpriteFont eightyFourSmall;
        int inTimer = 500;
        int presses = 0;
        
        public TetrisGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 32 * 20,
                PreferredBackBufferWidth = 32 * 10 + 150
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
                if (args.Key == Keys.Up && !tetrisBoard.IsOver)
                    tetrisBoard.Rotate();
                else if (args.Key == Keys.Space && !tetrisBoard.IsOver)
                    tetrisBoard.PlaceDown();
                else if (args.Key == Keys.C && !tetrisBoard.IsOver)
                    tetrisBoard.Hold();
                else if (args.Key == Keys.OemMinus)
                    MediaPlayer.Volume -= 0.1f;
                else if (args.Key == Keys.OemPlus)
                    MediaPlayer.Volume += 0.1f;
                else if (args.Key == Keys.M)
                    MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
                else if (args.Key == Keys.R && tetrisBoard.IsOver)
                {
                    tetrisBoard = new TetrisBoard();
                    inTimer = 500;
                    presses = 0;
                }
            };

            gamePadListener.ButtonDown += (sender, args) =>
            {
                if (!tetrisBoard.IsOver)
                {
                    if (args.Button == Buttons.DPadUp || args.Button == Buttons.Y)
                        tetrisBoard.Rotate();
                    if (args.Button == Buttons.B)
                        tetrisBoard.PlaceDown();
                    if (args.Button == Buttons.RightShoulder || args.Button == Buttons.LeftShoulder)
                        tetrisBoard.Hold();
                }
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

            themeSong = Content.Load<Song>("loopingtetris");
            MediaPlayer.Play(themeSong);
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.IsRepeating = true;

            eightyFour = Content.Load<SpriteFont>("EightyFour");
            eightyFourSmall = Content.Load<SpriteFont>("EightyFourSmall");
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

            if (!tetrisBoard.IsOver)
            {
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
            }

            base.Update(gameTime);
        }

        private void DrawBlocks(ref SpriteBatch sb, Block[] blocks, int X, int Y, float scale = 1.0f, bool center = false)
        {
            int x, y;
            x = X;
            y = Y;

            if (center)
            {
                x -= Convert.ToInt32(new Piece(blocks[0].Color, blocks).Width * scale * 32) / 2;
                y -= Convert.ToInt32(new Piece(blocks[0].Color, blocks).Height * scale * 32) / 2;
            }

            foreach (Block b in blocks)
            {
                sb.Draw(tetrisBlocks[b.Color], new Vector2(b.X * (32 * scale) + x, b.Y * (32 * scale) + y), scale: new Vector2(scale, scale));
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(5, 5, 20));

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                
            for (int y = 1; y < 20; y++)
            {
                spriteBatch.DrawLine(0, y*32, 32*10, y*32, lineColor, 2f);
            }

            for (int x = 1; x < 10; x++)
            {
                spriteBatch.DrawLine(x*32, 0, x*32, 32*20, lineColor, 2f);
            }

            spriteBatch.DrawLine(10 * 32 + 2, 0, 10 * 32 + 2, Window.ClientBounds.Height, lineColor, 4f);

            Piece nextPiece = tetrisBoard.PieceQueue[0].Normalize();
            // TODO: Center pieces.
            DrawBlocks(ref spriteBatch, nextPiece.Normalize().Blocks, 32 * 10 + 65, 30, 0.7f, true);
            DrawBlocks(ref spriteBatch, tetrisBoard.PieceQueue[1].Normalize().Blocks, 32 * 10 + 65, 90, 0.5f, true);
            DrawBlocks(ref spriteBatch, tetrisBoard.PieceQueue[2].Normalize().Blocks, 32 * 10 + 65, 140, 0.5f, true);
            DrawBlocks(ref spriteBatch, tetrisBoard.PieceQueue[3].Normalize().Blocks, 32 * 10 + 65, 190, 0.5f, true);
            DrawBlocks(ref spriteBatch, tetrisBoard.PieceQueue[4].Normalize().Blocks, 32 * 10 + 65, 240, 0.5f, true);
            DrawBlocks(ref spriteBatch, tetrisBoard.PieceQueue[5].Normalize().Blocks, 32 * 10 + 65, 290, 0.5f, true);

            spriteBatch.DrawRectangle(new Vector2(32 * 10 + 20, 365), new Size2(110, 110), lineColorB, 4);
            if (tetrisBoard.HeldPiece.HasValue)
            {
                DrawBlocks(ref spriteBatch, tetrisBoard.HeldPiece.Value.Normalize().Blocks, 32 * 10 + 65, 410, 0.6f, true);
            }

            Piece ghostPiece = tetrisBoard.FallLocation();
            foreach (Block b in ghostPiece.Blocks)
                spriteBatch.Draw(tetrisBlocks[b.Color], new Vector2((b.X + ghostPiece.X) * 32, (b.Y + ghostPiece.Y) * 32), new Color(Color.White, 0.3f));

            DrawBlocks(ref spriteBatch, tetrisBoard.GetBlocks().ToArray(), 0, 0);

            if (tetrisBoard.IsOver)
            {
                spriteBatch.FillRectangle(new Vector2(0, 0), new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height), new Color(5, 5, 20, 128));
                DrawStringCenter(ref spriteBatch, eightyFour, "Game Over", new Vector2(Window.ClientBounds.Width/2, Window.ClientBounds.Height/2), Color.White);
                DrawStringCenter(ref spriteBatch, eightyFourSmall, "R to Restart", new Vector2(Window.ClientBounds.Width/2, Window.ClientBounds.Height/2 + 50), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawStringCenter(ref SpriteBatch sb, SpriteFont font, String text, Vector2 pos, Color color)
        {
            Vector2 s = font.MeasureString(text);
            Vector2 newPos = (pos - s/2).Round();
            sb.DrawString(font, text, newPos, color);
        }
    }
}
