using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace flappyBird
{
    class Pipes
    {

        public Sprite TopPipe { get; private set; }
        public Sprite BottomPipe { get; private set; }


        public Vector2 Position
        {
            get { return TopPipe.Position; }
            set { TopPipe.Position = value; BottomPipe.Position = value; }
        }

        public float X
        {
            get { return Position.X; }
            set { TopPipe.Position.X = value; BottomPipe.Position.X = value; }
        }

        public float Y
        {
            get { return Position.Y; }
            set { TopPipe.Position.Y = value; BottomPipe.Position.Y = value; }
        }

        public int width;
        public float gap
        {
            get { return Position.Y + TopPipe.Hitbox.Height + 75; }
        }
        public bool isPassed = false;

        public Pipes(float scale, Texture2D texture, Vector2 position, Color color, float rotation)
        {
            TopPipe = new Sprite(texture, position, rotation, color, scale);
            BottomPipe = new Sprite(texture, new Vector2(position.X, (position.Y + scale * (texture.Height)) + 150), rotation, color, scale);
            BottomPipe.spriteEffects = SpriteEffects.FlipVertically;
            width = TopPipe.Hitbox.Width;
        }

        public bool CheckIntersects(Rectangle hitbox)
        {
            return TopPipe.Hitbox.Intersects(hitbox) || BottomPipe.Hitbox.Intersects(hitbox);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            TopPipe.Draw(spriteBatch);
            BottomPipe.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            X -= 5;
        }

    }
}
