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

    public class Nuke : Microsoft.Xna.Framework.GameComponent
    {
        public Nuke(Game game)
            : base(game)
        {
            width = 50;
            height = 60;
            animation = new Animation(game);
            Initialize();
        }

        public Texture2D texbomb;
        public Texture2D texplosion;// :)
        int xPos, yPos, width, height;
        Rectangle rec;
        static Random rand = new Random();
        int speed;
        int timer, timerMax;
        bool fall = false;
        public Animation animation;
        public bool explode = false;
        int animationTimer = 0;
        public int maxSpeed=4;
        public bool miss = false;

        public int Speed
        {
            get { return speed; }
        }

        public int Y
        {
            get { return yPos; }
            set { yPos = value; }
        }

        public Rectangle Rec
        {
            get { return rec; }
        }

        public void Initialize()
        {
            fall = false;
            timer = 0;
            timerMax = rand.Next(30, 1000);
            xPos = rand.Next(10, 600);
            yPos = -100;
            speed = rand.Next(1, maxSpeed);
        }

        public void Update()
        {
            timer++;
            animation.SetAnimation(80, 12);
            if (timer >= timerMax)
            {
                fall = true;
            }
            if (fall == true)
            {
                yPos += speed;
            }

            animation.Place(new Vector2(xPos-40, yPos));

            if (explode)
            {
                fall = false;
                animationTimer++;
                if (animationTimer >= 35)
                {
                    explode = false;
                    animationTimer = 0;
                    Initialize();
                }
            }
            else if (miss)
            {
                    Initialize();
            }
            else { fall = true; }

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            rec = new Rectangle(xPos, yPos, width, height);

            if (!explode)
            {
                spriteBatch.Draw(texbomb, rec, Color.AntiqueWhite);
            }
            else
            {
                animation.Draw(gameTime, spriteBatch, Color.AntiqueWhite);
            }
        }
    }
}
