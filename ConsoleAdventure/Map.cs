using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleAdventure
{
    class Map
    {
        protected string name = "Map";
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        protected Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
        protected Tile selectedTile;
        public Tile SelectedTile => selectedTile;

        public bool Generated => tiles.Count > 0;

        protected int cameraX = 0;
        public int CameraX => cameraX;
        protected int cameraY = 0;
        public int CameraY => cameraY;

        protected int offsetX;
        protected int offsetY;

        protected int scale = 3;

        protected string TileKey(int x, int y)
        {
            return x + ":" + y;
        }

        protected int TileKey_X(string tileKey)
        {
            return int.Parse(tileKey.Split(':')[0]);
        }

        protected int TileKey_Y(string tileKey)
        {
            return int.Parse(tileKey.Split(':')[1]);
        }

        public Map(Random random)
        {
            name = Program.GenerateName("RANDOM");
            selectedTile = new Tile(Tile.TileType.Void);            
        }

        public void MoveCamera(int x, int y)
        {
            if (!Program.Mute)
            {
                Console.Beep(121, 100);
            }

            if (tiles.ContainsKey(TileKey(cameraX + x, cameraY + y)))
            {
                selectedTile.IndicatorColor = ConsoleColor.Black;
                selectedTile.Indicator = ' ';

                cameraX += x;
                cameraY += y;

                if (selectedTile != tiles[TileKey(cameraX, cameraY)])
                {
                    selectedTile = tiles[TileKey(cameraX, cameraY)];

                    selectedTile.IndicatorColor = ConsoleColor.Red;
                    selectedTile.Indicator = 'X';
                }

                selectedTile.Trigger();
                //Program.Exploring();
            }
        }

        public virtual void GenerateMap(Random random)
        {
            tiles.Add(TileKey(0, 0), new Tile(Tile.TileType.Void));
            MoveCamera(0, 0);
        }

        protected virtual void RenderBackground(Renderer renderer, ConsoleColor color, char icon, ConsoleColor iconColor)
        {
            renderer.DrawBox(3, 1, renderer.Width - 5, renderer.Height - 2, color);

            for (int y = 1 + 1; y < renderer.Height - 2; y += scale)
            {
                for (int x = 2 + 2; x < renderer.Width - 3; x += scale)
                {
                    renderer.DrawPixel(x, y, color, icon, iconColor);
                }
            }
        }

        public void Render(Renderer renderer)
        {
            offsetX = renderer.Width / 2 - scale;
            offsetY = renderer.Height / 2 - scale;

            RenderBackground(renderer, ConsoleColor.Black, ' ', ConsoleColor.White);

            for (int y = -offsetY; y * scale < renderer.Height; y++)
            {
                for (int x = -offsetX; x * scale < renderer.Width; x++)
                {
                    if ((x - cameraX) * scale * 2 + offsetX > 1 && 
                        (y - cameraY) * scale + offsetY > 1 &&
                            (x - cameraX) * scale * 2 + offsetX + scale < renderer.Width && 
                            (y - cameraY) * scale * 1 + offsetY < renderer.Height)
                    {
                        if (tiles.ContainsKey(TileKey(x, y)))
                        {
                            tiles[TileKey(x, y)].Render(renderer, 
                                (x - cameraX) * scale * 2 + offsetX + 1, 
                                (y - cameraY) * scale * 1 + offsetY -1, scale);
                        }
                    }
                }
            }  
        }
    }

    class Map_Island : Map
    {
        public Map_Island(Random random) : base(random)
        {
            //GenerateMap(random);
        }

        public override void GenerateMap(Random random)
        {
            int tilesNumber = random.Next(40, 100);
            Tile.TileType[] tileTypes = new Tile.TileType[] { Tile.TileType.Mountain, Tile.TileType.Plain, Tile.TileType.Forest, Tile.TileType.Dessert };

            int getX = 0;
            int getY = 1;
            int[] pos = new int[] { 0, 0 };
            int[] dirs = new int[] { -1, 1 };

            tiles.Add(TileKey(getX, getY), new Tile(tileTypes[random.Next(tileTypes.Length)]));

            bool tilePlaced = false;

            while (tiles.Count < tilesNumber)
            {
                tilePlaced = false;
                while (!tilePlaced)
                {
                    string randomTileKey = tiles.ElementAt(random.Next(tiles.Count)).Key;
                    pos[getX] = TileKey_X(randomTileKey);
                    pos[getY] = TileKey_Y(randomTileKey);

                    int index = random.Next(pos.Length);
                    int dir = dirs[random.Next(dirs.Length)];
                    pos[index] += dir;

                    if (!tiles.ContainsKey(TileKey(pos[getX], pos[getY])))
                    {
                        tilePlaced = true;
                        if (random.Next(100) < 70)
                        {
                            tiles.Add(TileKey(pos[getX], pos[getY]), new Tile(tiles[randomTileKey].TilesType));
                        }
                        else
                        {
                            tiles.Add(TileKey(pos[getX], pos[getY]), new Tile(tileTypes[random.Next(tileTypes.Length)]));
                        }
                    }
                }
            }

            foreach (KeyValuePair<string, Tile> t in tiles)
            {
                t.Value.Interior = new Map_Tile(random, t.Value.TilesType, this);
            }

            MoveCamera(TileKey_X(tiles.ElementAt(0).Key), TileKey_Y(tiles.ElementAt(0).Key));
        }

        protected override void RenderBackground(Renderer renderer, ConsoleColor color = ConsoleColor.Blue, char icon = '~', ConsoleColor iconColor = ConsoleColor.White)
        {
            color = ConsoleColor.Blue;
            icon = '~';
            iconColor = ConsoleColor.White;
            base.RenderBackground(renderer, color, icon, iconColor);
        }
    }

    class Map_Tile : Map
    {
        readonly Tile.TileType tileType;
        readonly Map exterior;


        public Map_Tile(Random random, Tile.TileType tileTypeSet, Map exteriorSet) : base(random)
        {
            tileType = tileTypeSet;
            exterior = exteriorSet;
            //GenerateMap(random);
        }

        public override void GenerateMap(Random random)
        {
            Tile.TileType[] tileTypes = new Tile.TileType[] { Tile.TileType.Mountain, Tile.TileType.Plain, Tile.TileType.Forest, Tile.TileType.Dessert, Tile.TileType.Water };
            int clutter = random.Next(5, 20);
            int explorationSites = 1;

            int tileSize = 16;
            int tileOffset = -tileSize / 2;

            bool tilePlaced = false;

            while(explorationSites-- > 0)
            {
                tilePlaced = false;
                while (!tilePlaced)
                {
                    int x = random.Next(tileOffset, tileSize + tileOffset);
                    int y = random.Next(tileOffset, tileSize + tileOffset);

                    if (!tiles.ContainsKey(TileKey(x, y)))
                    {
                        tilePlaced = true;
                        tiles.Add(TileKey(x, y), new Tile(Tile.TileType.City));
                        tiles[TileKey(x, y)].Interior = new Map_City(random, this);
                    }
                }
            }

            while (clutter-- > 0)
            {
                tilePlaced = false;
                while (!tilePlaced)
                {
                    int x = random.Next(tileOffset, tileSize + tileOffset);
                    int y = random.Next(tileOffset, tileSize + tileOffset);

                    if (!tiles.ContainsKey(TileKey(x, y)))
                    {
                        tilePlaced = true;
                        tiles.Add(TileKey(x, y), new Tile(tileTypes[random.Next(tileTypes.Length)]));
                    }
                }
            }

            for (int y = 0 - 8; y < 16 - 8; y++)
            {
                for (int x = 0 - 8; x < 16 - 8; x++)
                {
                    if (!tiles.ContainsKey(TileKey(x, y)))
                    {
                        tiles.Add(TileKey(x, y), new Tile(tileType));
                    }
                }
            }

            foreach (KeyValuePair<string, Tile> t in tiles)
            {
                t.Value.Exsterior = exterior;
            }

            MoveCamera(0, 0);
        }

        protected override void RenderBackground(Renderer renderer, ConsoleColor color = ConsoleColor.Black, char icon = '~', ConsoleColor iconColor = ConsoleColor.White)
        {
            base.RenderBackground(renderer, color, icon, iconColor);
        }
    }

    class Map_City : Map
    {
        Map exterior;

        List<Actor> citizens = new List<Actor>();

        public Map_City(Random random, Map exteriorSet) : base(random)
        {
            exterior = exteriorSet;
            name = exterior.Name + " CITY";
            //GenerateMap(random);
        }

        public override void GenerateMap(Random random)
        {
            int offset = 8;

            int roads = random.Next(10, 20);
            int buildings = random.Next(9, roads);

            int getX = 0;
            int getY = 1;
            int[] pos = new int[] { 0, 0 };
            int[] dirs = new int[] { -1, 1 };
            int index = 1; //random.Next(pos.Length);
            int dir = 1; //dirs[random.Next(dirs.Length)];

            tiles.Add(TileKey(6, -7), new Tile(Tile.TileType.Road));

            bool tilePlaced = false;

            while (roads-- > 0)
            {
                string randomTileKey = tiles.ElementAt(random.Next(tiles.Count)).Key;
                pos[getX] = TileKey_X(randomTileKey);
                pos[getY] = TileKey_Y(randomTileKey);
                if (++index >= pos.Length)
                {
                    index = 0;
                }

                if (++dir >= dirs.Length)
                {
                    dir = 0;
                }

                int lng = 0;

                bool roadEnd = false;
                while (!roadEnd)
                {
                    if (!tiles.ContainsKey(TileKey(pos[getX], pos[getY])))
                    {
                        tiles.Add(TileKey(pos[getX], pos[getY]), new Tile(Tile.TileType.Road));
                    }

                    pos[index] += dirs[dir];

                    if (++lng > 3)
                    {
                        if (random.Next(100) < 30)
                        {
                            roadEnd = true;
                        }
                    }

                    if (
                        pos[index] >= 16 - offset ||
                        pos[index] < 0 - offset)
                    {
                        roadEnd = true;
                    }
                }
            }


            string[] road = tiles.Keys.ToArray();
            while (buildings-- > 0)
            {
                tilePlaced = false;
                while (!tilePlaced)
                {
                    string tileKey = road[random.Next(road.Length)];
                    pos[getX] = TileKey_X(tileKey);
                    pos[getY] = TileKey_Y(tileKey);

                    dir = dirs[random.Next(dirs.Length)];
                    pos[index] += dir;
                    if (!tiles.ContainsKey(TileKey(pos[getX], pos[getY])))
                    {
                        tilePlaced = true;
                        tiles.Add(TileKey(pos[getX], pos[getY]), new Tile(Tile.TileType.Building));
                    }
                }
            }

            for (int y = 0 - offset; y < 16 - offset; y++)
            {
                for (int x = 0 - offset; x < 16 - offset; x++)
                {
                    if (!tiles.ContainsKey(TileKey(x, y)))
                    {
                        tiles.Add(TileKey(x, y), new Tile(Tile.TileType.Plain));
                    }
                }
            }

            foreach (KeyValuePair<string, Tile> t in tiles)
            {
                if (t.Value.TilesType == Tile.TileType.Building)
                {
                    Actor owner = new Actor(random);
                    citizens.Add(owner);
                    t.Value.Trigger = () => 
                    {
                        Program.Battle(owner);
                        if (owner.IsDead)
                        {
                            t.Value.Trigger = () =>
                            {

                            };
                        }
                        Program.Exploring();
                    };
                    t.Value.Interior = new Map_Dungeon(random, this)
                    {
                        Name = owner.Name + "'S HOUSE"
                    };
                } else
                {
                    t.Value.Exsterior = exterior;
                }
            }
            

            MoveCamera(6, -7);
        }

        protected override void RenderBackground(Renderer renderer, ConsoleColor color = ConsoleColor.Black, char icon = '~', ConsoleColor iconColor = ConsoleColor.White)
        {
            base.RenderBackground(renderer, ConsoleColor.DarkGray, '=', ConsoleColor.Gray);
        }
    }

    class Map_Dungeon : Map
    {
        Map exterior;

        public Map_Dungeon(Random random, Map exteriorSet) : base(random)
        {
            exterior = exteriorSet;
            //GenerateMap(random);
        }

        public override void GenerateMap(Random random)
        {
            Tile t = new Tile(Tile.TileType.Void)
            {
                Exsterior = exterior
            };
            tiles.Add(TileKey(0, 0), t);
            MoveCamera(0, 0);
        }

        protected override void RenderBackground(Renderer renderer, ConsoleColor color = ConsoleColor.Black, char icon = ' ', ConsoleColor iconColor = ConsoleColor.White)
        {
            base.RenderBackground(renderer, ConsoleColor.Black, '-', ConsoleColor.DarkGray);
        }
    }
}
