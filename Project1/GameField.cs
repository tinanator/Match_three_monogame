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
        click, swap, findMatches, dropItems, findMatchesAfterSwap
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

        public GameField(Dictionary<ItemTypes, Texture2D> atlas, SpriteFont font)
        {
            this.atlas = atlas;
            this.font = font;
            var rand = new Random();
            Position cell = new Position(0, 0);
            for (int row = 0; row < Constants.ROWCOUNT; row++)
            {
                for (int col = 0; col < Constants.ROWCOUNT; col++)
                {
                    if (rand.Next(5) == 0)
                    {
                        cells[cell] = new Item(ItemTypes.square, cell, atlas[ItemTypes.square]);
                    }
                    else if (rand.Next(5) == 1)
                    {
                        cells[cell] = new Item(ItemTypes.circle, cell, atlas[ItemTypes.circle]);
                    }
                    else if (rand.Next(5) == 2)
                    {
                        cells[cell] = new Item(ItemTypes.romb, cell, atlas[ItemTypes.romb]);
                    }
                    else if (rand.Next(5) == 3)
                    {
                        cells[cell] = new Item(ItemTypes.star, cell, atlas[ItemTypes.star]);
                    }
                    else 
                    {
                        cells[cell] = new Item(ItemTypes.triangle, cell, atlas[ItemTypes.triangle]);
                    }
                    cell.update(cell.x + 1, cell.y);
                }
                cell.update(0, cell.y + 1);
            }

            while (findMatches() > 0) {
                dropItems();
            }
            

            score = 0;
        }
        public void placeItems(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            foreach (Position cell in cells.Keys)
            {
                cells[cell].draw(spriteBatch, graphics);
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
            int _x = Convert.ToInt32(x / Constants.CELLSIZE);
            int _y = Convert.ToInt32(y / Constants.CELLSIZE);
            Position cell = new Position(_x, _y);
            if (clicked)
            {
                clicked = false;
                cells[clickedItem1].setUnclicked();
                if (clickedItem1 == cell)
                {
                    return;
                }
                Debug.WriteLine("second click");
                if (cell.y == clickedItem1.y)
                {
                    if (cell.x - 1 == clickedItem1.x || cell.x + 1 == clickedItem1.x)
                    {
                        gameState = GameState.swap;
                        clickedItem2 = cell;
                    }
                }
                else if (cell.x == clickedItem1.x)
                {
                    if (cell.y - 1 == clickedItem1.y || cell.y + 1 == clickedItem1.y)
                    {
                        gameState = GameState.swap;
                        clickedItem2 = cell;
                    }
                }
            }
            else
            {
                Debug.WriteLine("first click");
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
            Debug.WriteLine("FINDMATCHES");
            foreach (Position cell in cells.Keys)
            {
                if (!cells[cell].isToRemove && isMatchThree(cell))
                {
                    matches++;
                    Debug.WriteLine("match found");
                    cells[cell].isToRemove = true;
                    if (cells[cell].State == threeMatchState.horizontal)
                    {
                        Debug.WriteLine("horizontal");
                        double x = cell.x + 1;
                        ItemTypes type = cells[cell].ItemType;
                        Position newCell = new Position(x, cell.y);
                        while (x < Constants.ROWCOUNT)
                        {
                            if (cells[newCell].isToRemove || cells[newCell].ItemType != type)
                            {
                                break;
                            }
                            cells[newCell].isToRemove = true;
                            x++;
                            newCell.update(x, cell.y);
                        }
                        x = cell.x - 1;
                        newCell.update(x, cell.y);
                        while (x >= 0)
                        {
                            if (cells[newCell].isToRemove || cells[newCell].ItemType != type)
                            {
                                break;
                            }
                            cells[newCell].isToRemove = true;
                            x--;
                            newCell.update(x, cell.y);
                        }
                        cells[cell].State = threeMatchState.none;

                    }
                    else if (cells[cell].State == threeMatchState.vertical)
                    {
                        Debug.WriteLine("vertical");
                        double y = cell.y + 1;
                        ItemTypes type = cells[cell].ItemType;
                        Position newCell = new Position(cell.x, y);
                        while (y < Constants.ROWCOUNT)
                        {
                            if (cells[newCell].isToRemove || cells[newCell].ItemType != type)
                            {
                                break;
                            }
                            cells[newCell].isToRemove = true;
                            y++;
                            newCell.update(cell.x, y);
                        }
                        y = cell.y - 1;
                        newCell.update(cell.x, y);
                        while (y >= 0)
                        {
                            if (cells[newCell].isToRemove || cells[newCell].ItemType != type)
                            {
                                break;
                            }
                            cells[newCell].isToRemove = true;
                            y--;
                            newCell.update(cell.x, y);
                        }
                        cells[cell].State = threeMatchState.none;
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
                    while (cells[curcell].isToRemove)
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
                        if (rand.Next(5) == 0)
                        {
                            cells[tmp] = new Item(ItemTypes.square, tmp, atlas[ItemTypes.square]);
                        }
                        else if (rand.Next(5) == 1)
                        {
                            cells[tmp] = new Item(ItemTypes.circle, tmp, atlas[ItemTypes.circle]);
                        }
                        else if (rand.Next(5) == 2)
                        {
                            cells[tmp] = new Item(ItemTypes.romb, tmp, atlas[ItemTypes.romb]);
                        }
                        else if (rand.Next(5) == 3)
                        {
                            cells[tmp] = new Item(ItemTypes.star, tmp, atlas[ItemTypes.star]);
                        }
                        else
                        {
                            cells[tmp] = new Item(ItemTypes.triangle, tmp, atlas[ItemTypes.triangle]);
                        }
                    }


                }
            }
        }
        public void Update(GameTime gameTime)
        {
            if (gameState == GameState.swap)
            {
                Debug.WriteLine("SWAP");
                swap(clickedItem1, clickedItem2);
                gameState = GameState.findMatchesAfterSwap;
            }
            else if (gameState == GameState.findMatchesAfterSwap) {
                Debug.WriteLine("FINDMATCHESAFTERSWAP");
                if (!isMatchThree(clickedItem1) && !isMatchThree(clickedItem2)) {
                    Debug.WriteLine("no match. swap back");
                    swap(clickedItem1, clickedItem2);
                    gameState = GameState.click;
                    return;
                }
                Debug.WriteLine("matches found");
                gameState = GameState.findMatches;
            }
            
            else if (gameState == GameState.findMatches)
            {
                Debug.WriteLine("FINDMATCHES");
                findMatches();
                gameState = GameState.dropItems;
            }
            else if (gameState == GameState.dropItems)
            {
                Debug.WriteLine("DROPITEMS");
                dropItems();
                gameState = GameState.findMatches;
            }
        }
        public void drawField(SpriteBatch _spriteBatch, Texture2D SimpleTexture, GraphicsDeviceManager graphics)
        {
            _spriteBatch.DrawString(font, "Score: " + score, new Vector2(730, 5), Color.Black);
            int[] pixel = { 0xFFFFFF }; // White. 0xFF is Red, 0xFF0000 is Blue
            SimpleTexture.SetData<int>(pixel, 0, SimpleTexture.Width * SimpleTexture.Height);

            int x1 = 0;
            int y1 = 0;

            for (int i = 0; i < Constants.ROWCOUNT + 1; i++)
            {
                _spriteBatch.Draw(SimpleTexture, new Rectangle(x1, y1, Constants.ROWCOUNT * Constants.CELLSIZE, 1), Color.White);
                y1 += Constants.CELLSIZE;
            }

            x1 = 0;
            y1 = 0;

            for (int i = 0; i < Constants.ROWCOUNT + 1; i++)
            {
                _spriteBatch.Draw(SimpleTexture, new Rectangle(x1, y1, 1, Constants.ROWCOUNT * Constants.CELLSIZE), Color.White);
                x1 += Constants.CELLSIZE;
            }

            placeItems(_spriteBatch, graphics);
        }
    }
}
