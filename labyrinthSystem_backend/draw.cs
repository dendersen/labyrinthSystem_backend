using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace labyrinthSystem_backend
{
    internal class Color
    {
        private byte r, g, b;
        public Color(int rgb)
        {
            this.r = (byte)((rgb >>  0) & 0xff);
            this.g = (byte)((rgb >>  8) & 0xff);
            this.b = (byte)((rgb >> 16) & 0xff);

        }
        public Color (byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        public void WriteAnsiColor()
        {
            Console.Write("\e[38;2;{0};{1};{2}m", r, g, b);
        }
        public string GetAnsiColor()
        {
            return $"\e[38;2;{r};{g};{b}m";
        }
    }
    internal class Draw
    {
        private int width;
        private int height;
        private bool supportsColor;
        private bool supportsMove;
        private Color[,] buffer;
        public Draw(int width, int height, bool supportsColor, bool supportsMove)
        {
            this.width = width;
            this.height = height;
            this.supportsColor = supportsColor;
            this.supportsMove = supportsMove;
            this.buffer = new Color[width,height];
        }
        public void UpdateSize(int width, int height)
        {
            Color[,] buffer = new Color[width, height];
            for (int x = 0; x < this.width && x < width; x++)
            {
                for (int y = 0; y < this.height && y < height; y++)
                {
                    buffer[x, y] = this.buffer[x, y];
                }
            }
            this.buffer = buffer;
            this.width  = width;
            this.height = height;
        }
        public void DrawBuff()
        {
            Console.SetCursorPosition(0, 0);
            if (this.buffer == null)
            {
                throw new Exception("missing draw buffer");
            }
            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    if (this.buffer[x, y] != null)
                    {
                        this.buffer[x, y].WriteAnsiColor();
                        Console.Write("█");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write("\n");
            }
        }
        /**
         * update a single pixel in the draw buffer
         * null color sets transparent
         * printing null will result in background color
         */
        public void UpdateBuff(int x, int y, Color? col,bool print)
        {
            this.buffer[(int)x, (int)y] = col;
            if (print && col != null)
            {
                Console.SetCursorPosition(x, y);
                this.buffer[x, y].WriteAnsiColor();
                Console.Write("█");
            }
            else if (print) 
            {
                Console.SetCursorPosition(x, y);
                Console.Write(' ');
            }
        }
        /**
         * draws all provided Draw classes
         * threating null color as transparent
         * all sizes must be equal
         */
        public static void DrawAll(Draw[] screens)
        {
            Draw collective = new(screens[0].width, screens[0].height, screens[0].supportsColor, screens[0].supportsMove);
            for (int i = 0; i < screens.Length; i++)
            {
                if (screens[i].supportsColor && !collective.supportsColor)
                {
                    collective.supportsColor = true;
                }
                if (screens[i].supportsMove && !collective.supportsMove)
                {
                    collective.supportsMove = true;
                }
                if (screens[i].height != collective.height)
                {
                    throw new Exception("not all screen heights match");
                }
                if (screens[i].width != collective.width)
                {
                    throw new Exception("not all screen widths match");
                }
                for (int x = 0; x < screens[i].width; x++)
                {
                    for (int y = 0; y < screens[i].height; y++)
                    {
                        if (screens[i].buffer[x,y] != null)
                        {
                            collective.buffer[x,y] = screens[i].buffer[x,y];
                        }
                    }
                }
            }
            collective.DrawBuff();
        }
    }
}
