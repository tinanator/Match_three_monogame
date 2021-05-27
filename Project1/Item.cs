using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace Project1
{
    public enum ItemTypes
    {
        square,
        circle,
        star,
        triangle,
        romb
    }
    enum Direction
    {
        none, up, down, left, right
    }
    struct Position
    {
        public Position(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public void update(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public double y, x;

        public static bool operator ==(Position obj1, Position obj2)
        {
            return (obj1.x == obj2.x) && (obj1.y == obj2.y);
        }

        public Position convertToCell()
        {
            Position cell = new Position(Convert.ToInt32(this.x / Constants.CELLSIZE), Convert.ToInt32(this.y / Constants.CELLSIZE));
            return cell;
        }

        public Position convertToCoordinates()
        {
            Position pos = new Position(this.x * Constants.CELLSIZE, this.y * Constants.CELLSIZE);
            return pos;
        }

        public static bool operator !=(Position obj1, Position obj2)
        {
            return !(obj1 == obj2);
        }

        public static Position operator +(Position p, Vector2 vec)
        {
            return new Position(p.x + vec.X, p.y + vec.Y);
        }

        public static Position operator -(Position p1, Position p2)
        {
            return new Position(p1.x - p2.x, p1.y - p2.y);
        }
    }

    enum threeMatchState
    {
        horizontal, vertical, none
    }
    class Item
    {
        private Texture2D texture;
        public Item(ItemTypes itemType, Position CellPosition, Texture2D texture)
        {
            this.ItemType = itemType;
            this.CellPosition = CellPosition;
            this.texture = texture;
            this.CoordinatePosition = CellPosition.convertToCoordinates();
            this.State = threeMatchState.none;
        }

        private bool clicked = false;

        public void setClicked() {
            clicked = true;
        }

        public void setUnclicked() {
            clicked = false;
        }
        public threeMatchState State { get; set; }
        public ItemTypes ItemType { get; set; }

        public bool isToRemove = false;

        private float rotation = 0f;
        public void draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            if (clicked)
            {
                rotation += 0.3f;
            }
            else {
                rotation = 0f;
            }
            CoordinatePosition.update(CellPosition.x * Constants.CELLSIZE, CellPosition.y * Constants.CELLSIZE);
            spriteBatch.Draw(texture, new Rectangle((int)CoordinatePosition.x + Constants.CELLSIZE/2, 
                (int)CoordinatePosition.y + Constants.CELLSIZE / 2,
               Constants.CELLSIZE, Constants.CELLSIZE), null, Color.Red, rotation, new Vector2(texture.Width/2, texture.Height/2),
               SpriteEffects.None, 0f);
        }
        public Position CellPosition { get; set; }

        private Position CoordinatePosition;
    }
}
