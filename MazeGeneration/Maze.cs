using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeGeneration
{
    public class Maze
    {
        private int[,] grid;
        private Random random;

        private int columnCount;
        private int rowCount;

        private (int Column, int Row) origin;

        public Maze(int numberOfColumns, int numberOfRows, Random randomNumberGenerator)
        {
            columnCount = numberOfColumns;
            rowCount = numberOfRows;
            random = randomNumberGenerator;

            grid = new int[columnCount, rowCount];

            GenerateGraphVerticalSnaking();
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
        private void GenerateGraphSpiralInwards()
        {
            int iMin = 0;
            int iMax = columnCount - 1;

            int jMin = 0;
            int jMax = rowCount - 1;

            int direction = 0b0100;

            origin = (0, 0);

            for (int visited = 0; visited < columnCount * rowCount; visited++)
            {
                if (direction == 0b0100)
                {
                    int j = jMin;
                    for (int i = iMin; i <= iMax; i++)
                    {
                        GenerateCell(i, j);
                    }
                    jMin++;
                    direction = 0b0010;
                }
                else if (direction == 0b0010)
                {
                    int i = iMax;
                    for (int j = jMin; j <= jMax; j++)
                    {
                        GenerateCell(i, j);
                    }
                    iMax--;
                    direction = 0b0001;
                }
                else if (direction == 0b0001)
                {
                    int j = jMax;
                    for (int i = iMax; iMin <= i; i--)
                    {
                        GenerateCell(i, j);
                    }
                    jMax--;
                    direction = 0b1000;
                }
                else if (direction == 0b1000)
                {
                    int i = iMin;
                    for (int j = jMax; jMin <= j; j--)
                    {
                        GenerateCell(i, j);
                    }
                    iMin++;
                    direction = 0b0100;
                }

                jMin = Math.Clamp(jMin, 0, rowCount - 1);
                jMax = Math.Clamp(jMax, 0, rowCount - 1);
                iMin = Math.Clamp(iMin, 0, columnCount - 1);
                iMax = Math.Clamp(iMax, 0, columnCount - 1);
            }
        }
        private void GenerateGraphVerticalSnaking()
        {
            int jStep = 1;
            int j = 0;

            if (random.Next(2) == 0)
            {
                jStep = -1;
                j = rowCount - 1;
            }

            origin = (0, j);

            for (int i = 0; i < columnCount; i++)
            {
                while ( 0 <= j && j < rowCount)
                {
                    GenerateCell(i, j);
                    j += jStep;
                }
                jStep *= -1;
                j += jStep;
            }
        }
        private void GenerateGraphHorizontalSnaking()
        {
            int iStep = 1;
            int i = 0;

            if (random.Next(2) == 0)
            {
                iStep = -1;
                i = columnCount - 1;
            }

            origin = (i, 0);

            for (int j = 0; j < rowCount; j++)
            {
                while (0 <= i && i < columnCount)
                {
                    GenerateCell(i, j);
                    i += iStep;
                }
                iStep *= -1;
                i += iStep;
            }

        }
        private void GenerateGraphDivergentQuadrants()
        {
            origin = (columnCount / 2, rowCount / 2);
            for (int i = columnCount / 2; i >= 0; i --)
            {
                for (int j = rowCount / 2; j >= 0; j --)
                {
                    GenerateCell(i, j);
                }
            }

            for (int i = columnCount / 2 + 1; i < columnCount; i++)
            {
                for (int j = rowCount / 2; j >= 0; j--)
                {
                    GenerateCell(i, j);
                }
            }

            for (int i = columnCount / 2; i >= 0; i--)
            {
                for (int j = rowCount / 2 + 1; j < rowCount; j++)
                {
                    GenerateCell(i, j);
                }
            }

            for (int i = columnCount / 2 + 1; i < columnCount; i++)
            {
                for (int j = rowCount / 2 + 1; j < rowCount; j++)
                {
                    GenerateCell(i, j);
                }
            }
        }
        private void GenerateGraphConvergentQuadrants()
        {
            origin = (0, 0);
            for (int i = 0; i < columnCount / 2; i++)
            {
                for (int j = 0; j < rowCount / 2; j++)
                {
                    GenerateCell(i, j);
                }
            }

            origin = (columnCount - 1, 0);
            for (int i = columnCount - 1; i >= columnCount / 2; i--)
            {
                for (int j = 0; j < rowCount / 2; j++)
                {
                    GenerateCell(i, j);
                }
            }

            origin = (0, rowCount - 1);
            for (int i = 0; i < columnCount / 2; i++)
            {
                for (int j = rowCount - 1; j >= rowCount / 2; j--)
                {
                    GenerateCell(i, j);
                }
            }

            origin = (columnCount - 1, rowCount - 1);
            for (int i = columnCount - 1; i >= columnCount / 2; i--)
            {
                for (int j = rowCount - 1; j >= rowCount / 2; j--)
                {
                    GenerateCell(i, j);
                }
            }
        }
        private void GenerateGraphDivergeFromOrigin()
        {
            for (int i = 0; i < columnCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    GenerateCell(i, j);
                }
            }
        }
        private void GenerateCell(int column, int row)
        {
            grid[column, row] = GenerateCell((column, row), GetCell(column, row - 1),
                                                            GetCell(column + 1, row),
                                                            GetCell(column, row + 1),
                                                            GetCell(column - 1, row));
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
            if (north == -1 && (location == origin || cellValue != 0b0000))
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
            if (east == -1 && (location == origin || cellValue != 0b0000))
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
            if (south == -1 && (location == origin || cellValue != 0b0000))
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
            if (west == -1 && (location == origin || cellValue != 0b0000))
            {
                return (0b0001, 1);
            }
            return (0, 0);
        }
    }
}
