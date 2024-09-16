using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MazeGeneration
{
    public class Maze
    {
        private int[,] grid;
        private Random random;

        private int columnCount;
        private int rowCount;

        public Maze(int numberOfColumns, int numberOfRows, Random randomNumberGenerator)
        {
            columnCount = numberOfColumns;
            rowCount = numberOfRows;
            random = randomNumberGenerator;

            grid = new int[columnCount, rowCount];

            GenerateGraph();
        }
        public void Draw(MazeSprite sprites, SpriteBatch spriteBatch)
        {
            int x = sprites.tileProportions.X;
            int y = sprites.tileProportions.Y;
            int width = sprites.tileProportions.Width;
            int height = sprites.tileProportions.Height;

            for (int i = 0; i < columnCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    sprites.Draw(spriteBatch, grid[i, j], x, y);
                    y += height;
                }
                y = 0;
                x += width;
            }
        }
        private void GenerateGraph()
        {
            for (int i = 0; i < columnCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    grid[i, j] = GenerateCell((i, j), GetCell(i, j - 1),
                                                      GetCell(i + 1, j),
                                                      GetCell(i, j + 1),
                                                      GetCell(i - 1, j));
                }
            }
        }
        private int GenerateCell((int Row, int Col) location, int north, int east, int south, int west)
        {
            int cell = 0b0000;
            int connections = 0;
            (int Cell, int Connections) result;

            if (north != -1)
                cell |= (north & 0b0010) << 2;

            if (east != -1)
                cell |= (east & 0b0001) << 2;

            if (south != -1)
                cell |= (south & 0b1000) >> 2;

            if (west != -1)
                cell |= (west & 0b0100) >> 2;
            
            (Func<(int, int), int, int, (int, int)> Function, int Direction)[] toProcess =
            {
            (ProcessNorth, north),
            (ProcessEast, east),
            (ProcessSouth, south),
            (ProcessWest, west)
            };

            int selection;
            for (int i = 3; i >= 0 && connections < 1; i--)
            {
                selection = random.Next(i + 1);

                var temp = toProcess[i];
                toProcess[i] = toProcess[selection];
                toProcess[selection] = temp;

                result = toProcess[i].Function(location, toProcess[i].Direction, cell);
                cell |= result.Cell;
                connections += result.Connections;
            }
            
            return cell;
        }
        private int GetCell(int column, int row)
        {
            if (column < 0 || columnCount - 1 < column ||
                row < 0 || rowCount - 1 < row)
            {
                return 0b0000;
            }
            if (grid[column, row] == 0b0000)
            {
                return -1;
            }
            return grid[column, row];
        }

        private (int Cell, int Connections) ProcessNorth((int Col, int Row) location, int north, int cellValue)
        {
            if (cellValue == 0b0000 && north > 0)
            {
                grid[location.Col, location.Row - 1] |= 0b0010;
                return (0b1000, 1);
            }
            if (north == -1 && (location == (0, 0) || cellValue != 0b0000))
            {
                return (0b1000, 1);
            }
            return (0, 0);
        }
        private (int Cell, int Connections) ProcessEast((int Col, int Row) location, int east, int cellValue)
        {
            if (cellValue == 0b0000 && east > 0)
            {
                grid[location.Col + 1, location.Row] |= 0b0001;
                return (0b0100, 1);
            }
            if (east == -1 && (location == (0, 0) || cellValue != 0b0000))
            {
                return (0b0100, 1);
            }
            return (0, 0);
        }
        private (int Cell, int Connections) ProcessSouth((int Col, int Row) location, int south, int cellValue)
        {
            if (cellValue == 0b0000 && south > 0)
            {
                grid[location.Col, location.Row + 1] |= 0b1000;
                return (0b0010, 1);
            }
            if (south == -1 && (location == (0, 0) || cellValue != 0b0000))
            {
                return (0b0010, 1);
            }
            return (0, 0);
        }
        private (int Cell, int Connections) ProcessWest((int Col, int Row) location, int west, int cellValue)
        {
            if (cellValue == 0b0000 && west > 0)
            {
                grid[location.Col - 1, location.Row] |= 0b0100;
                return (0b0001, 1);
            }
            if (west == -1 && (location == (0, 0) || cellValue != 0b0000))
            {
                return (0b0001, 1);
            }
            return (0, 0);
        }
    }
}
