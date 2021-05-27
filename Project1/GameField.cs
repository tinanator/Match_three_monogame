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

    enum GameState
    {
        click, 
        swap, 
        findMatches, 
        dropItems, 
        findMatchesAfterSwap
    }
    class GameField
    {
        private Dictionary<ItemTypes, Texture2D> atlas = new Dictionary<ItemTypes, Texture2D>();

        private GameState gameState = GameState.click;

        private Dictionary<Position, Item> cells = new Dictionary<Position, Item>();

        private Position clickedItem1;

        private Position clickedItem2;

        private bool clicked = false;

        private int score = 0;

        private SpriteFont font;

        private Item generateItem(int randInt, Position cell) 
        {
            Item item;
            switch (randInt) {
                case 0:
                    return new Item(ItemTypes.square, cell, atlas[ItemTypes.square]);
                case 1:
                    return new Item(ItemTypes.circle, cell, atlas[ItemTypes.circle]);
                case 2:
                    return new Item(ItemTypes.romb, cell, atlas[ItemTypes.romb]);
                case 3:
                    return new Item(ItemTypes.star, cell, atlas[ItemTypes.star]);
                default:
                    return new Item(ItemTypes.triangle, cell, atlas[ItemTypes.triangle]);
            }
        }

        private void generateField() {
            var rand = new Random();
            Position cell = new Position(0, 0);
            for (int row = 0; row < Constants.ROWCOUNT; row++)
            {
                for (int col = 0; col < Constants.ROWCOUNT; col++)
                {
                    cells[cell] = generateItem(rand.Next(5), cell);
                    cell.update(cell.x + 1, cell.y);
                }
                cell.update(0, cell.y + 1);
            }

            while (findMatches() > 0)
            {
                dropItems();
            }

            score = 0;
        }
        public GameField(Dictionary<ItemTypes, Texture2D> atlas, SpriteFont font)
        {
            this.atlas = atlas;
            this.font = font;
            generateField();
        }
        public void placeItems(SpriteBatch spriteBatch)
        {
            foreach (Position cell in cells.Keys)
            {
                cells[cell].draw(spriteBatch);
            }
        }

        private void swap(Position pos1, Position pos2)
        {

            Item tmp = cells[pos1];
            cells[pos1] = cells[pos2];
            cells[pos1].CellPosition = pos1;
            cells[pos2] = tmp;
            cells[pos2].CellPosition = pos2;

            Position tmpPos = clickedItem2;
            clickedItem2 = clickedItem1;
            clickedItem1 = tmpPos;
        }

        public void onClick(int x, int y)
        {
            int _x = x / Constants.CELLSIZE;
            int _y = y / Constants.CELLSIZE;
            Position cell = new Position(_x, _y);
            if (clicked)
            {
                clicked = false;
                cells[clickedItem1].setUnclicked();
                if (clickedItem1 == cell)
                {
                    return;
                }
                if (cell.y == clickedItem1.y && (cell.x - 1 == clickedItem1.x || cell.x + 1 == clickedItem1.x) ||
                    cell.x == clickedItem1.x && (cell.y - 1 == clickedItem1.y || cell.y + 1 == clickedItem1.y))
                {
                    gameState = GameState.swap;
                    clickedItem2 = cell;
                }
            }
            else
            {
                if (cells.ContainsKey(cell))
                {
                    ItemTypes type = cells[cell].ItemType;
                    clickedItem1 = cell;
                    clicked = true;
                    cells[cell].setClicked();
                }

            }
        }


        private bool isMatchThree(Position pos)
        {
            ItemTypes type = cells[pos].ItemType;
            double x = pos.x + 1;
            int count = 1;
            while (x < Constants.ROWCOUNT)
            {
                if (cells[new Position(x, pos.y)].ItemType == type)
                {
                    count++;
                    x++;
                }
                else { 
                    break; 
                }
            }
            x = pos.x - 1;
            while (x >= 0)
            {
                if (cells[new Position(x, pos.y)].ItemType == type)
                {
                    count++;
                    x--;
                }
                else { 
                    break; 
                }
            }

            if (count >= 3)
            {
                cells[pos].State = threeMatchState.horizontal;
                return true;
            }


            double y = pos.y + 1;
            count = 1;
            while (y < Constants.ROWCOUNT)
            {
                if (cells[new Position(pos.x, y)].ItemType == type)
                {
                    count++;
                    y++;
                }
                else { 
                    break; 
                }
            }
            y = pos.y - 1;
            while (y >= 0)
            {
                if (cells[new Position(pos.x, y)].ItemType == type)
                {
                    count++;
                    y--;
                }
                else { 
                    break; 
                }
            }

            if (count >= 3)
            {
                cells[pos].State = threeMatchState.vertical;
                return true;
            }

            return false;
        }

        private int findMatches() {
            int matches = 0;
            foreach (Position cell in cells.Keys)
            {
                if (!cells[cell].IsToRemove && isMatchThree(cell))
                {
                    matches++;
                    cells[cell].IsToRemove = true;
          
                    double x = cell.x + 1;
                    ItemTypes type = cells[cell].ItemType;
                    Position newCell = new Position(x, cell.y);

                    while (x < Constants.ROWCOUNT)
                    {
                        if (cells[newCell].IsToRemove || cells[newCell].ItemType != type)
                        {
                            break;
                        }
                        cells[newCell].IsToRemove = true;
                        x++;
                        newCell.update(x, cell.y);
                    }
                    x = cell.x - 1;
                    newCell.update(x, cell.y);
                    while (x >= 0)
                    {
                        if (cells[newCell].IsToRemove || cells[newCell].ItemType != type)
                        {
                            break;
                        }
                        cells[newCell].IsToRemove = true;
                        x--;
                        newCell.update(x, cell.y);
                    }

                    double y = cell.y + 1;
                    newCell = new Position(cell.x, y);
                    while (y < Constants.ROWCOUNT)
                    {
                        if (cells[newCell].IsToRemove || cells[newCell].ItemType != type)
                        {
                            break;
                        }
                        cells[newCell].IsToRemove = true;
                        y++;
                        newCell.update(cell.x, y);
                    }
                    y = cell.y - 1;
                    newCell.update(cell.x, y);
                    while (y >= 0)
                    {
                        if (cells[newCell].IsToRemove || cells[newCell].ItemType != type)
                        {
                            break;
                        }
                        cells[newCell].IsToRemove = true;
                        y--;
                        newCell.update(cell.x, y);
                    }   
                }
            }
            if (matches == 0)
            {
                gameState = GameState.click;
            }
            return matches;
        }

        private void dropItems() {
            var rand = new Random();
            for (int y = Constants.ROWCOUNT - 1; y >= 0; y--)
            {
                for (int x = 0; x < Constants.ROWCOUNT; x++)
                {
                    Position curcell = new Position(x, y);
                    while (cells[curcell].IsToRemove)
                    {
                        score++;
                        Position tmp = curcell;
                        Position above = new Position(x, tmp.y - 1);
                        while (tmp.y > 0)
                        {
                            cells[tmp] = cells[above];
                            cells[tmp].CellPosition = tmp;
                            tmp = above;
                            above.update(x, tmp.y - 1); ;
                        }
                        cells[tmp] = generateItem(rand.Next(5), tmp);
                    }


                }
            }
        }
        public void Update(GameTime gameTime)
        {
            switch (gameState) {
                case GameState.swap:
                    swap(clickedItem1, clickedItem2);
                    gameState = GameState.findMatchesAfterSwap;
                    break;

                case GameState.findMatchesAfterSwap:
                    if (!isMatchThree(clickedItem1) && !isMatchThree(clickedItem2))
                    {
                        swap(clickedItem1, clickedItem2);
                        gameState = GameState.click;
                        return;
                    }
                    gameState = GameState.findMatches;
                    break;

                case GameState.findMatches:
                    findMatches();
                    gameState = GameState.dropItems;
                    break;

                case GameState.dropItems:
                    dropItems();
                    gameState = GameState.findMatches;
                    break;
            }
        }
        public void drawField(SpriteBatch spriteBatch, Texture2D SimpleTexture)
        {
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(730, 5), Color.Black);

            int[] pixel = { 0xFFFFFF }; 
            SimpleTexture.SetData<int>(pixel, 0, SimpleTexture.Width * SimpleTexture.Height);

            int x1 = 0;
            int y1 = 0;

            for (int i = 0; i < Constants.ROWCOUNT + 1; i++)
            {
                spriteBatch.Draw(SimpleTexture, new Rectangle(x1, y1, Constants.ROWCOUNT * Constants.CELLSIZE, 1), Color.White);
                y1 += Constants.CELLSIZE;
            }

            x1 = 0;
            y1 = 0;

            for (int i = 0; i < Constants.ROWCOUNT + 1; i++)
            {
                spriteBatch.Draw(SimpleTexture, new Rectangle(x1, y1, 1, Constants.ROWCOUNT * Constants.CELLSIZE), Color.White);
                x1 += Constants.CELLSIZE;
            }

            placeItems(spriteBatch);
        }
    }
}
