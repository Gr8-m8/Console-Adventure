using System;

namespace ConsoleAdventure
{
    class Tile
    {
        public string Name => tiletype.ToString();

        public enum TileType
        {
            Void,
            Water,
            Mountain,
            Plain,
            Forest,
            Dessert,
            City,
            Road,
            Building
        }

        TileType tiletype = TileType.Void;
        public TileType TilesType => tiletype;

        ConsoleColor color;
        ConsoleColor indicatorColor;
        public ConsoleColor IndicatorColor
        {
            get
            {
                return indicatorColor;
            }
            set
            {
                indicatorColor = value;
            }
        }

        readonly char tileIndicator = 'I';
        ConsoleColor decorationColor = ConsoleColor.Black;
        char decoration = ' ';
        
        char indicator = ' ';
        public char Indicator
        {
            get
            {
                return indicator;
            }
            set
            {
                indicator = value;
            }
        }

        Action trigger = () =>
        {

        };

        public Action Trigger
        {
            get
            {
                return trigger;
            }
            set
            {
                trigger = value;
            }
        }

        Map interior = null;
        public Map Interior
        {
            get
            {
                return interior;
            }
            set
            {
                interior = value;
            }
        }

        Map exterior = null;
        public Map Exsterior
        {
            get
            {
                return exterior;
            }
            set
            {
                exterior = value;
            }
        }

        public Tile(TileType tileTypeSet)
        {
            SetTileType(tileTypeSet);
        }

        protected void SetTileType(TileType tileTypeSet)
        {
            tiletype = tileTypeSet;
            switch (tiletype)
            {
                case TileType.Void:
                    color = ConsoleColor.Black;
                    decoration = ' ';
                    break;

                case TileType.Water:
                    color = ConsoleColor.Blue;
                    decorationColor = ConsoleColor.White;
                    decoration = '~';
                    break;

                case TileType.Mountain:
                    color = ConsoleColor.Gray;
                    decorationColor = ConsoleColor.DarkGray;
                    decoration = 'M';
                    break;

                case TileType.Plain:
                    color = ConsoleColor.Green;
                    decorationColor = ConsoleColor.DarkGreen;
                    decoration = '^';
                    break;

                case TileType.Forest:
                    color = ConsoleColor.DarkGreen;
                    decorationColor = ConsoleColor.Green;
                    decoration = 'A';
                    break;

                case TileType.Dessert:
                    color = ConsoleColor.Yellow;
                    decorationColor = ConsoleColor.DarkYellow;
                    decoration = '~';
                    break;

                case TileType.City:
                    color = ConsoleColor.DarkGray;
                    decorationColor = ConsoleColor.Yellow;
                    decoration = 'H';
                    break;

                case TileType.Road:
                    color = ConsoleColor.Black;
                    decorationColor = ConsoleColor.White;
                    decoration = '-';
                    break;

                case TileType.Building:
                    color = ConsoleColor.DarkYellow;
                    decorationColor = ConsoleColor.Cyan;
                    decoration = 'O';
                    break;
            }
        }

        public void Render(Renderer renderer, int x, int y, int scale)
        {
            renderer.DrawBox(x, y, 2 * scale, 1 * scale, color);

            if (scale > 2)
            {
                renderer.DrawPixel(x + 2 * scale - 1, y, color, tileIndicator, indicatorColor);
                renderer.DrawPixel(x, y + 1 * scale - 1, color, tileIndicator, indicatorColor);
            }

            renderer.DrawPixel(x, y, color, tileIndicator, indicatorColor);
            renderer.DrawPixel(x + 2 * scale - 1, y + 1 * scale - 1, color, tileIndicator, indicatorColor);

            renderer.DrawPixel(x + 2 * scale / 2 - 1, y + 1 * scale / 2, color, decoration, decorationColor);
            renderer.DrawPixel(x + 2 * scale / 2 - 0, y + 1 * scale / 2, color, indicator, indicatorColor);
        }
    }
}
