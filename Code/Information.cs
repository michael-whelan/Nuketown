using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Nuketown_Savior
{

    public class Information : Microsoft.Xna.Framework.GameComponent
    {
        public Information(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }


        int score;
        int missed;
        bool gameOver = false;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public int Missed
        {
            get { return missed; }
            set { missed = value; }
        }

        public bool EndGame
        {
            get { return gameOver; }
            set { gameOver = value; }
        }

        public void GameOver(SpriteBatch spritebatch, SpriteFont spriteFont)
        {
            gameOver = true;
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont spriteFont)
        {
            if (gameOver == false)
            {
                Vector2 stringPos = new Vector2(10, 10);
                string output = "Survival Time: " + score + "\n" + "Times Hit: " + missed;
                spritebatch.DrawString(spriteFont, output, stringPos, Color.Red);
            }
            else
            {
                Vector2 stringPos = new Vector2(100, 100);
                string output = "Survival Time:" + score;
                spritebatch.DrawString(spriteFont, output, stringPos, Color.Red);
            }
        }
    }
}
