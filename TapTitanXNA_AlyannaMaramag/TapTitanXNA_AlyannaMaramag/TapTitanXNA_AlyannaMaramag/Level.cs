using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TapTitanXNA_AlyannaMaramag
{
    public class Level
    {
        public static int windowWidth = 400;
        public static int windowHeight = 500;


        #region Properties
        ContentManager content;

        Texture2D background;
        public MouseState oldMouseState;
        public MouseState mouseState;

        bool mpressed, prev_mpressed = false;
        int mouseX, mouseY;
        public int health = 10;
        int timer = 0;

        Hero hero;

        Vector2 fPos, mpos;

        SpriteFont damageStringFont;
        int damageNumber = 10;

        Button playButton;

        TimeSpan afterAttackTime = TimeSpan.FromSeconds(2);
        bool isTappedPlayButton = false;

        #endregion
        
        public Level(ContentManager content)
        {
            this.content = content;

            hero = new Hero(content, this);
        }

        public void LoadContent()
        {
            background = content.Load<Texture2D>("Background/BG01");
            damageStringFont = content.Load<SpriteFont>("Font");

            int fPositionX = (Level.windowWidth / 4);
            int fPositionY = (Level.windowHeight / 12);
            fPos = new Vector2((float)fPositionX, (float)fPositionY);
            playButton = new Button(content, "Button/buttona", new Vector2((windowWidth / 2) - 20,(windowHeight / 3) + (windowHeight / 2)));
            //attackButton = new Button(content, "Button/buttonb", new Vector2((windowWidth / 2) - 24, (windowHeight / 3) + (windowHeight / 2)));
            ally01Idle = new Button(content, "Button/buttonAlly0101", new Vector2((windowWidth / 2) - 24, (windowHeight / 4) + (windowHeight / 2)));
           
            hero.LoadContent();

        }
        public void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            mouseX = mouseState.X;
            mouseY = mouseState.Y;
            Trace.Write(mouseX + "," + mouseY + ",");
            prev_mpressed = mpressed;
            mpressed = mouseState.LeftButton == ButtonState.Pressed;

            mpos = new ((float)mouseX, (float)mouseY);

            attackButton = new Button(content, "Button/buttonb", new Vector2((float)mouseX, (float)mouseY));
            hero.Update(gameTime);

            oldMouseState = mouseState;

            timer++;

            if (attackButton.Update(gameTime, mouseX, mouseY, mpressed, prev_mpressed) && damageNumber!=0)
            {
                damageNumber -= 1;                
            }
            
            else if (damageNumber == 0 && timer >= 15)
            {
                damageNumber = 10;
            }

            bool tappedPlayButton = playButton.Update(gameTime, mouseX, mouseY, mpressed, prev_mpressed);

            if (tappedPlayButton)
            {
                hero.PlayAttackAnimation();
                isTappedPlayButton = true;
            }

            if (isTappedPlayButton)
            {
                afterAttackTime = afterAttackTime.Subtract(gameTime.ElapsedGameTime);
            }

            if (afterAttackTime.TotalMilliseconds <= 0)
            {
                hero.PlayIdleAnimation();
                afterAttackTime = TimeSpan.FromSeconds(2);
            }

        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.Draw(background,
            new Vector2(0, -5),
            null,
            Color.White,
            0.0f,
            Vector2.Zero,
            1.0f,
            SpriteEffects.None,
            0.0f);
            /*
            spriteBatch.Draw(player,
            playerPosition,
            null,
            Color.White,
            0.0f,
            Vector2.Zero,
            1.0f,
            SpriteEffects.None,
            0.0f);*/
            hero.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(damageStringFont, damageNumber+" ", fPos, Color.White);
            spriteBatch.DrawString(damageStringFont, "  /"+health, fPos, Color.White);


            playButton.Draw(gameTime, spriteBatch);
            //playButton.Draw(gameTime, spriteBatch);


        }

        public Button attackButton { get; set; }

        public Button ally01Attack { get; set; }

        public Button ally01Idle { get; set; }
    }
}
