using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flappyBird
{
    public class Bird : Sprite
    {
        public Vector2 speed;
        public double gravity;
        KeyboardState lastkS;
        public Network Brain;
        public double Fitness;
        public bool HitPipe = false;
        public Bird(Vector2 speed, Color color, double rotation, double scale, Texture2D texture, Vector2 position, double gravity)
            :base(texture, position, rotation, color, scale)
        {
            this.speed = speed;
            this.gravity = gravity;
            Brain = new Network(ActivationAndErrorFunction.BinaryStepActivationFunction, 2, 2, 4, 1);
        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (!HitPipe)
            {
                speed.Y -= (float)gravity;
                Position.Y += speed.Y;
                lastkS = ks;
                Fitness++;
                base.Update(gameTime);
            }
        }

        public void Jump()
        {
            speed.Y = -9;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (HitPipe)
            {
                spritebatch.Draw(Texture, Position, null, color * 0.01f, (float)rotation, Origin, (float)scale, SpriteEffects.None, 0f);
            }
            else
            {
                spritebatch.Draw(Texture, Position, null, color, (float)rotation, Origin, (float)scale, SpriteEffects.None, 0f);
            }
        }
    }
}
