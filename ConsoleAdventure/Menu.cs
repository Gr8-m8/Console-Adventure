using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleAdventure
{
    class Menu
    {
        string title;

        int x;
        int y;
        int width;
        int height;
        int maxHeight;

        int selectIndex;

        List<MenuItem> menuItems = new List<MenuItem>();
        List<MenuButton> menuSelectItems = new List<MenuButton>();

        public Menu(int xSet, int ySet, int widthSet, string titleSet, int maxHeightSet = int.MaxValue)
        {
            title = titleSet;

            x = xSet;
            y = ySet;
            width = widthSet;
            maxHeight = maxHeightSet;

            menuItems = new List<MenuItem>();
            menuSelectItems = new List<MenuButton>();
        }

        public void AddMenuItem(MenuItem mi)
        {
            menuItems.Add(mi);
            if(mi.GetType() == typeof(MenuButton))
            {
                menuSelectItems.Add((MenuButton)mi);
            }

            height = 2 + menuItems.Count;
        }

        public void AddBlankMenuItem()
        {
            AddMenuItem(new MenuItem());
        }

        public void SelectIndex(int amount)
        {
            if (!Program.Mute)
            {
                Console.Beep(151, 100);
            }
            selectIndex += amount;

            if (selectIndex < 0)
            {
                selectIndex = menuSelectItems.Count -1;
            }

            if (selectIndex >= menuSelectItems.Count)
            {
                selectIndex = 0;
            }
        }

        public void EventCheck()
        {
            if (menuItems.Count > 0)
            {
                menuSelectItems[selectIndex].ButtonEvent();
            }
        }

        public void Renderer(Renderer renderer)
        {
            renderer.DrawBox(x, y, width, height, ConsoleColor.DarkCyan);
            renderer.DrawText(x + 1, y, title, ConsoleColor.White, width - 2);

            int index = 0;
            foreach (MenuItem mi in menuItems)
            {
                index++;
                if (y + index <= y + height -2)
                {
                    ConsoleColor miColor = ConsoleColor.Cyan;
                    if (mi != menuSelectItems[selectIndex])
                    {
                        if (index % 2 == 0)
                        {
                            miColor = ConsoleColor.Gray;
                        }
                        else
                        {
                            miColor = ConsoleColor.DarkGray;
                        }
                    } else
                    {
                        miColor = ConsoleColor.DarkYellow;
                    }

                    mi.Render(renderer, x + 1, y + index, width - 2, miColor);
                }
            }
        }
    }

    class MenuItem
    {
        public virtual void Render(Renderer renderer, int x, int y, int width, ConsoleColor color)
        {
            renderer.DrawBox(x, y, width, 1, color);
        }
    }

    class MenuText : MenuItem
    {
        protected string text = "";
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        public MenuText(string textSet)
        {
            text = textSet;
        }

        public override void Render(Renderer renderer, int x, int y, int width, ConsoleColor color)
        {
            base.Render(renderer, x, y, width, color);
            renderer.DrawText(x, y, text, ConsoleColor.White, width);
        }
    }

    class MenuButton : MenuText
    {

        Action buttonEvent = () => 
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("BUTTON_EVENT");
        };
        public Action ButtonEvent
        {
            get
            {
                return buttonEvent;
            }
            set
            {
                buttonEvent = value;
            }
        }

        public MenuButton(string textSet) : base(textSet)
        {

        }

        public override void Render(Renderer renderer, int x, int y, int width, ConsoleColor color)
        {
            base.Render(renderer, x, y, width, color);
        }
    }
}
