using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure
{
    class Renderer
    {
        int width = 120;
        int height = 30;
        public int Width => width;
        public int Height => height;

        Dictionary<string, Pixel> display = new Dictionary<string, Pixel>();
        Dictionary<string, Pixel> displayUpdate = new Dictionary<string, Pixel>();

        public Renderer()
        {
            Console.CursorVisible = false;
            //Console.SetWindowSize(120, 30);
            Resize();
        }

        string DisplayKey(int x, int y)
        {
            return x + ":" + y;
        }

        public void Resize()
        {
            width = Console.WindowWidth -1;
            height = Console.WindowHeight -1;
        }

        public void Render(bool rerender = false)
        {
            Resize();
            foreach(KeyValuePair<string, Pixel> p in displayUpdate)
            {
                if (!display.ContainsKey(p.Key))
                {
                    display.Add(p.Key, new Pixel(p.Value.X, p.Value.Y, ConsoleColor.Black));
                }

                if (p.Value.Compare != display[p.Key].Compare || rerender)
                {
                    if (p.Value.X > -1 && 
                        p.Value.X < width+1 &&
                        p.Value.Y > -1 &&
                        p.Value.Y < height+1)
                    {
                        display[p.Key] = p.Value;
                        Console.CursorLeft = p.Value.X;
                        Console.CursorTop = p.Value.Y;
                        Console.BackgroundColor = p.Value.Color;
                        Console.ForegroundColor = p.Value.IconColor;
                        Console.Write(p.Value.Icon);
                    }
                }
            }

            
            Console.CursorLeft = 2-1;
            Console.CursorTop = 0;
            Console.CursorTop = height-2;
            if (display.ContainsKey(DisplayKey(Console.CursorLeft, Console.CursorTop)))
            {
                Console.BackgroundColor = display[DisplayKey(Console.CursorLeft, Console.CursorTop)].Color;
                Console.ForegroundColor = display[DisplayKey(Console.CursorLeft, Console.CursorTop)].IconColor;
            } else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public void DrawBorder(ConsoleColor color = ConsoleColor.Gray)
        {
            this.DrawBox(0, 0, this.Width + 1, this.Height + 1, color);
        }

        public void DrawClear(ConsoleColor color = ConsoleColor.Black)
        {
            this.DrawBox(1, 1, this.Width - 1, this.Height - 1, color);
        }

        public void DrawPixel(int x, int y, ConsoleColor color, char icon = ' ', ConsoleColor iconColor = ConsoleColor.Black)
        {
            Pixel t = new Pixel(x, y, color, icon, iconColor);
            if (displayUpdate.ContainsKey(DisplayKey(x, y)))
            {
                if (t.Compare != displayUpdate[DisplayKey(x, y)].Compare)
                {
                    displayUpdate.Remove(DisplayKey(x, y));
                    displayUpdate.Add(DisplayKey(x, y), t);
                    return;
                }
            } else
            {
                displayUpdate.Add(DisplayKey(x, y), t);
            }
            
        }

        public void DrawBox(int xpos, int ypos, int width, int height, ConsoleColor color)
        {
            for (int y = ypos; y < ypos + height; y++)
            {
                for (int x = xpos; x < xpos + width; x++)
                {
                    DrawPixel(x, y, color);
                }
            }
        }

        public void DrawText(int x, int y, string text, ConsoleColor textColor, int maxLenght = int.MaxValue)
        {
            if (text.Length > maxLenght)
            {
                text = text.Remove(maxLenght - 3);
                text += "...";
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (displayUpdate.ContainsKey(DisplayKey(x + i, y)))
                {
                    displayUpdate[DisplayKey(x+i, y)].IconColor = textColor;
                    displayUpdate[DisplayKey(x+i, y)].Icon = text[i];
                }
                else
                {
                    displayUpdate.Add(DisplayKey(x + i, y), new Pixel(x+i, y, ConsoleColor.Black, text[i], textColor));
                }
            }
        }
    }

    class Pixel
    {
        int x;
        int y;
        public int X => x;
        public int Y => y;

        ConsoleColor color;
        public ConsoleColor Color => color;


        char icon = ' ';
        public char Icon
        {
            get
            {
                return icon;
            }
            set
            {
                icon = value;
            }
        }
        ConsoleColor iconColor = ConsoleColor.Black;
        public ConsoleColor IconColor
        {
            get
            {
                return iconColor;
            }
            set
            {
                iconColor = value;
            }
        }

        public Pixel(int eyeset, int ySet, ConsoleColor colorSet, char iconSet = ' ', ConsoleColor iconColorSet = ConsoleColor.Black)
        {
            x = eyeset;
            y = ySet;

            color = colorSet;

            icon = iconSet;
            iconColor = iconColorSet;
        }

        public string Compare
        {
            get
            {
                return (color + "|" + icon + "|" + iconColor).ToString();
            }
        }
    }
}
