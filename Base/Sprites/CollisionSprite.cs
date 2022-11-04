using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TileSystem2.Base.Sprites
{
    public class CollisionSprite : Sprite
    {
        public Vector2 velocity;
        public Rectangle hitbox;
        public bool isStatic, shouldCollide = true, useGravity = true;
        public float bounciness = .2f, friction = .95f;
        private float[] bouncinessDirections, frictionDirections;
        public const float gravity = 500;

        public CollisionSprite(Texture2D texture, Rectangle? sourceRect = null, Rectangle hitbox = default, Color color = default, Vector2 size = default)
            : base(texture, sourceRect, size, color) => this.hitbox = hitbox == default ? DrawRect : hitbox;

        public virtual void Update(List<CollisionSprite> collisionSprites, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(useGravity) velocity.Y += gravity * deltaTime;
            Collision(collisionSprites);
            position += velocity * deltaTime;
        }

        #region Collision
        public virtual void Collision(List<CollisionSprite> collisionSprites)
        {
            bouncinessDirections = new float[] { 0, 0, 0, 0 };
            frictionDirections = new float[] { float.NaN, float.NaN };
            foreach(var sprite in collisionSprites)
                if(IsColliding(sprite.hitbox)) Collide(sprite);

            velocity.Y *= -bouncinessDirections[0] * bouncinessDirections[1];
            velocity.X *= -bouncinessDirections[2] * bouncinessDirections[3];

            velocity.Y *= float.IsNaN(frictionDirections[0]) ? 1 : frictionDirections[0];
            velocity.X *= float.IsNaN(frictionDirections[1]) ? 1 : frictionDirections[1];
        }                

        protected virtual void Collide(CollisionSprite sprite)
        {
            for(int i = 0; i < 4; i++)
                if(IsTouching(sprite.hitbox, i)) { 
                    velocity = i < 2 ? new Vector2(velocity.X, 0) : new Vector2(0, velocity.Y);
                    ResolveCollision(sprite.hitbox, i);
                    if(frictionDirections[i] < sprite.friction) frictionDirections[i] = sprite.friction;
                    if(bouncinessDirections[(int)(i * .5f)] < sprite.bounciness) 
                        bouncinessDirections[(int)(i * .5f)] = sprite.bounciness;
                }
        }

        protected virtual void ResolveCollision(Rectangle rectangle, int index)
            => position = index switch {
                0 => new(position.X, rectangle.Top - size.Y - .1f),
                1 => new(position.X, rectangle.Bottom + .1f),
                2 => new(rectangle.Left - size.X - .1f, position.Y),
                3 => new(rectangle.Right + .1f, position.Y),
                _ => throw new("index was not found")};

        public virtual bool IsColliding(Rectangle rect)
        {
            if(!shouldCollide) return false;
            if(Vector2.Distance(position, rect.Location.ToVector2()) > (size + rect.Size.ToVector2()).Length() + .01f)
                return false;

            if(rect.Location.X < position.X) if(IsTouchingRight(rect)) return true;
            if(rect.Location.X > position.X) if(IsTouchingLeft(rect)) return true;
            if(rect.Location.Y > position.Y) if(IsTouchingTop(rect)) return true;
            if(rect.Location.Y < position.Y) if(IsTouchingBottom(rect)) return true;
            return false;
        }

        public virtual bool IsTouching(Rectangle rect, int index)
            => index switch {
                0 => IsTouchingTop(rect),
                1 => IsTouchingBottom(rect),
                2 => IsTouchingLeft(rect),
                3 => IsTouchingRight(rect),
                _ => throw new("Index was not found")};

        public virtual bool IsTouchingLeft(Rectangle rect)
        {
            if(!shouldCollide) return false;

            return hitbox.Right + velocity.X > rect.Left &&
                    hitbox.Left < rect.Left &&
                    hitbox.Bottom > rect.Top &&
                    hitbox.Top < rect.Bottom;
        }

        public virtual bool IsTouchingRight(Rectangle rect)
        {
            if(!shouldCollide) return false;

            return hitbox.Left + velocity.X < rect.Right &&
                    hitbox.Right > rect.Right &&
                    hitbox.Bottom > rect.Top &&
                    hitbox.Top < rect.Bottom;
        }

        public virtual bool IsTouchingTop(Rectangle rect)
        {
            if(!shouldCollide) return false;

            return hitbox.Bottom + velocity.Y > rect.Top &&
                    hitbox.Top < rect.Top &&
                    hitbox.Right > rect.Left &&
                    hitbox.Left < rect.Right;
        }

        public virtual bool IsTouchingBottom(Rectangle rect)
        {
            if(!shouldCollide) return false;

            return hitbox.Top + velocity.Y < rect.Bottom &&
                    hitbox.Bottom > rect.Bottom &&
                    hitbox.Right > rect.Left &&
                    hitbox.Left < rect.Right;
        }
        #endregion Collision
    }
}