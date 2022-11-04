using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileSystem2.Managers.Statics;

namespace TileSystem2.Base.Sprites
{
    public class Sprite
    {
        public Texture2D texture;
        public Vector2 position, size;
        public Color color;
        public Rectangle? sourceRect;
        public bool shouldDraw = true;

        public Rectangle DrawRect {
            get => new(position.ToPoint(), size.ToPoint());
            set {
                position = value.Location.ToVector2();
                size = value.Size.ToVector2();
            }
        }

        public bool Visible {
            get => DrawRect.Top < ScreenManager.ScreenSize.Y && DrawRect.Bottom > 0
                && DrawRect.Left < ScreenManager.ScreenSize.X && DrawRect.Right > 0;
            set { 
                shouldDraw = value;
                if(!value) return;
                if(position.Y >= ScreenManager.ScreenSize.Y) position.Y = ScreenManager.ScreenSize.Y - 1;
                else if(position.Y < -DrawRect.Height) position.Y = -DrawRect.Height + 1;
                if(position.X >= ScreenManager.ScreenSize.X) position.X = ScreenManager.ScreenSize.X - 1;
                else if(position.X < -DrawRect.Width) position.X = -DrawRect.Height + 1; 
            }
        }

        public Sprite(Texture2D texture, Rectangle? sourceRect = null, Vector2 size = default, Color color = default)
        {
            this.texture = texture;
            this.sourceRect = sourceRect;
            this.size = size == default ? Vector2.One * 50 : size;
            this.color = color == default ? Color.White : color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(!shouldDraw) return;
            if(texture == null) throw new("texture was null");
            spriteBatch.Draw(texture, DrawRect, sourceRect, color);
        }
    }
}