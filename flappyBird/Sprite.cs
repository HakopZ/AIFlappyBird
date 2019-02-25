﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flappyBird
{
    class Sprite
    {
        public Texture2D Texture;
        public Vector2 Position;
        public float rotation;
        public Color color;
        public float scale;
        public SpriteEffects spriteEffects;

        public virtual Vector2 Origin
        {
            get
            {
                return Vector2.Zero;
            }
        }
        public Vector2 Size
        {
            get
            {
                return new Vector2(Texture.Width * scale, Texture.Height * scale);
            }
        }
        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }
        public Sprite(Texture2D texture, Vector2 Position, float rotation, Color color, float scale)
        {
            Texture = texture;
            this.Position = Position;
            this.rotation = rotation;
            this.color = color;
            this.scale = scale;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, color, rotation, Origin, scale, spriteEffects, 0f);
        }
        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
