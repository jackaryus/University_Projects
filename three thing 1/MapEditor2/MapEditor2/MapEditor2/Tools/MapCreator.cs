using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MapEditor2.Tools
{
    class MapCreator
    {
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        private Point[] NSEW = new Point[4] { new Point(-1, 0), new Point(0, -1), new Point(1, 0), new Point(0, 1)},
        NSEW2 = new Point[8] { new Point(-1, 0), new Point(0, -1), new Point(1, 0), new Point(0, 1),
                                new Point(-1, -1), new Point(1, -1), new Point(1, 1), new Point(-1, 1)};
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public int[,] CreateMaze(int width, int height, int id)
        {
            int[,] maze = new int[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    maze[x, y] = id;
                }
            }

            Random random = new Random();
            List<Point> Stack = new List<Point>();

            int StartX;
            int StartY;
            while ((StartX = random.Next(width - 1)) % 2 != 1) ;
            while ((StartY = random.Next(height - 1)) % 2 != 1) ;

            Point CurrentPoint = new Point(StartX, StartY);
            Stack.Add(CurrentPoint);

            while (Stack.Count > 0)
            {

                List<Point> neighbours = new List<Point>();
                maze[CurrentPoint.X, CurrentPoint.Y] = 0;

                //getting neighbourghs here
                foreach (var offset in this.NSEW)
                {
                    Point ToCheck = new Point(CurrentPoint.X + offset.X, CurrentPoint.Y + offset.Y);

                    if (ToCheck.X % 2 == 1 || ToCheck.Y % 2 == 1)
                    {
                        if (maze[ToCheck.X, ToCheck.Y] == 1 && HasThreeWalls(ToCheck, maze))
                        {
                            neighbours.Add(ToCheck);
                        }
                    }
                }

                if (neighbours.Count > 0)
                {
                    CurrentPoint = neighbours[random.Next(neighbours.Count)];
                    Stack.Add(CurrentPoint);
                }
                else
                {
                    Stack.RemoveAt(Stack.Count - 1);
                    if (Stack.Count > 0)
                    {
                        CurrentPoint = (Point)Stack[Stack.Count - 1];
                    }
                }

            }

            return maze;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public int[,] CreateCave(int width, int height, int id)
        {
            int[,] cave = new int[width, height];
            Random random = new Random();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int chance = random.Next(100);
                    if (chance < 40)
                    {
                        cave[x, y] = id;
                    }
                    else
                    {
                        cave[x, y] = 0;
                    }
                    
                }
            }

            for (int i = 0; i < 3; i++)
            {
                cave = CellularAutomata(cave, width, height, id); 
            }

            return cave;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        private int[,] CellularAutomata(int[,] cave, int width, int height, int id)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Point currentPoint = new Point(x, y);

                    int wallcount = 0;

                    foreach (var offset in NSEW2)
                    {
                        Point target = new Point(x - offset.X, y - offset.Y);

                        if (IsInside(target, width, height))
                        {
                            if (cave[target.X, target.Y] == id)
                            {
                                wallcount++;
                            }
                        }
                        else
                        {
                            wallcount++;
                        }

                    }

                    if (cave[x, y] == id)
                    {
                        wallcount++;
                    }

                    if (wallcount >= 5)
                    {
                        cave[x, y] = id;
                    }
                    else
                    {
                        cave[x, y] = 0;
                    }
                }
            }

            return cave;
        }

        //-------------------------------------------------------------------------------------------------
        // ** 
        //-------------------------------------------------------------------------------------------------
        private Boolean HasThreeWalls(Point p, int[,] maze)
        {
            int wallCount = 0;

            foreach (var offset in this.NSEW)
            {
                Point ToCheck = new Point(p.X + offset.X, p.Y + offset.Y);
                if (IsInside(ToCheck, maze.GetLength(0), maze.GetLength(1)) && maze[ToCheck.X, ToCheck.Y] == 1)
                {
                    wallCount++;
                }
            }
            return (wallCount == 3);
        }
        //-------------------------------------------------------------------------------------------------
        // ** 
        //-------------------------------------------------------------------------------------------------
        private Boolean IsInside(Point p, int width, int height)
        {
            return (p.X >= 0 && p.X < width && p.Y >= 0 && p.Y < height);
        }
    }
}
