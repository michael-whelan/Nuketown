using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Nuketown_Savior
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Nuke[] nuke;
        Menu menu;
        Texture2D background;
        Rectangle backRec;
        Texture2D menuBack;
        SpriteFont spriteFont;
        Information info;
        enum gameState { menu, gamePlay } // holds the game state
        gameState theGameState = gameState.menu;
        bool pause = false;
        int timer;
        int sTimer;
        bool gameOver = false;
        DisplayOrientation ori;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            menu = new Menu(this);
            nuke = new Nuke[10];
            for (int i = 0; i < 10; i++)
            {
                nuke[i] = new Nuke(this);
            }
            info = new Information(this);
            //set resolution
            DisplayOrientation ori = DisplayOrientation.LandscapeRight;
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = 400;
            graphics.PreferredBackBufferWidth = 666;

        }

        protected override void Initialize()
        {
            for (int i = 0; i < 10; i++)
            {
                nuke[i].Initialize();
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            menuBack = this.Content.Load<Texture2D>("Background");
            background = this.Content.Load<Texture2D>("Background2");
            //menu
            menu.playOn = this.Content.Load<Texture2D>("menu/Play");
            menu.quitOn = this.Content.Load<Texture2D>("menu/Quit");
            menu.resumeOn = this.Content.Load<Texture2D>("menu/resume");

            for (int i = 0; i < 10; i++)
            {
                nuke[i].texbomb = this.Content.Load<Texture2D>("nuke");
                nuke[i].animation.spriteSheet = this.Content.Load<Texture2D>("explosion");
            }
            spriteFont = this.Content.Load<SpriteFont>("SpriteFont1");


        }

        protected override void UnloadContent()
        {
        }

        public void Pause()
        {
            if (timer >= 10)
            {
                if (!pause)
                {
                    pause = true;
                }
                else if (pause)
                {
                    pause = false;
                }
                timer = 0;
            }
        }

        int scoreTime=0;
        public void UpdateScore() 
        {
            scoreTime++;
            if (scoreTime >= 30)
            {
                info.Score++;
                scoreTime = 0;
            }
        }

        public void UpdateDifficulty()
        {
            for (int i = 0; i < 10; i++)
            {
                if (info.Score >= 40)
                {
                    nuke[i].maxSpeed = 5;
                }

                if (info.Score >= 80)
                {
                    nuke[i].maxSpeed = 6;
                }

                if (info.Score >= 110)
                {
                    nuke[i].maxSpeed = 7;
                }

                if (info.Score >= 160)
                {
                    nuke[i].maxSpeed = 8;
                }
                if (info.Score >= 300)
                {
                    nuke[i].maxSpeed = 9;
                }
                if (info.Score >= 400)
                {
                    nuke[i].maxSpeed = 10;
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            // Allows the game to exit
            if (theGameState == gameState.gamePlay)
            {
                timer++;//for pause
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || ks.IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                { Pause(); }
                if (!pause)
                {
                    UpdateDifficulty();
                    for (int i = 0; i < 10; i++)
                    {
                        nuke[i].Update();
                        //off screen
                        if (nuke[i].Y >= 500)
                        {
                            if (!nuke[i].explode && !nuke[i].miss)
                            {
                                info.Missed++;
                                nuke[i].miss = true;
                            }
                        }
                        foreach (TouchLocation tl in TouchPanel.GetState())
                        {
                            if (nuke[i].Rec.Contains((int)tl.Position.X, (int)tl.Position.Y))
                            {
                                nuke[i].explode = true;
                              
                            }
                        }
                    }

                    if (info.Missed >= 2)
                    {
                        EndGame();
                    }
                    else
                    {
                        info.EndGame = false;
                    }

                    if(!gameOver)
                    UpdateScore();
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
                        theGameState = gameState.menu;
                    }
                }
                else if (pause)
                {
                    menu.Update(pause);

                }
            }
            else if (theGameState == gameState.menu)
            {
                menu.Update(pause);
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    this.Exit();
                }
            }
            UpdateGameState(ks);
            base.Update(gameTime);
        }

        public void UpdateGameState(KeyboardState ks)
        {
            if (theGameState == gameState.menu)
            {
                sTimer++;

                foreach (TouchLocation tl in TouchPanel.GetState())
                {
                    if (menu.playRec.Contains((int)tl.Position.X, (int)tl.Position.Y))
                    {
                        theGameState = gameState.gamePlay;
                        gameOver = false;
                        info.Score = 0;
                        info.Missed = 0; for (int i = 0; i < 10; i++)
                        {
                            nuke[i].Initialize();
                        }
                    }
                    else if (menu.quitRec.Contains((int)tl.Position.X, (int)tl.Position.Y))
                    {
                        {
                            if (sTimer >= 10)
                            {
                                this.Exit();
                                sTimer = 0;
                            }
                        }
                    }
                }
                pause = false;
            }
            if (theGameState == gameState.gamePlay)
            {
                if (pause == true)
                {
                    foreach (TouchLocation tl in TouchPanel.GetState())
                    {
                        sTimer = 0;
                        if (menu.playRec.Contains((int)tl.Position.X, (int)tl.Position.Y))
                        {

                            pause = false;
                        }

                        else if (menu.quitRec.Contains((int)tl.Position.X, (int)tl.Position.Y))
                        {
                            theGameState = gameState.menu;
                        }

                    }
                }
            }
        }

        public void EndGame()
        {
            pause = true;
            gameOver = true;
            info.EndGame = true;
        }

        protected override void Draw(GameTime gameTime)
        {
            backRec = new Rectangle(0, 0, (int)GraphicsDevice.Viewport.Width, (int)GraphicsDevice.Viewport.Height);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (theGameState == gameState.gamePlay)
            {
                spriteBatch.Draw(background, backRec, Color.AntiqueWhite);

                for (int i = 0; i < 10; i++)
                {
                    nuke[i].Draw(spriteBatch,gameTime);
                }

                if (pause && !gameOver)
                {
                    menu.Draw(spriteBatch);
                }
                else if (gameOver)
                {
                    menu.GameOver(spriteBatch);
                }
                info.Draw(spriteBatch, spriteFont);
            }
            else if (theGameState == gameState.menu)
            {
                spriteBatch.Draw(menuBack, backRec, Color.AntiqueWhite);
                menu.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
