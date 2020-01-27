using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleAdventure
{
    class World
    {
        readonly string name = "World";
        public string Name => name;

        Dictionary<string, Map> maps = new Dictionary<string, Map>();
        Map currentMap;
        public Map CurrentMap => currentMap;

        readonly Random random;

        public World(Random randomSet)
        {
            random = randomSet;
            name = Program.GenerateName("RANDOM");
            currentMap = new Map_Island(random);
            currentMap.GenerateMap(random);
            maps.Add(currentMap.Name, currentMap);

            currentMap = currentMap.SelectedTile.Interior;
            currentMap.GenerateMap(random);
        }

        public void EnterExitInteriorExterior()
        {
            Console.Beep(121, 100);
            Console.Beep(151, 100);
            if (CurrentMap.SelectedTile.Interior != null)
            {
                currentMap = CurrentMap.SelectedTile.Interior;

                if (!CurrentMap.Generated)
                {
                    currentMap.GenerateMap(random);
                }
            }
            else //if (CurrentMap.SelectedTile.Exsterior != null)
            {
                currentMap = CurrentMap.SelectedTile.Exsterior;
            }
        }

        public void Render(Renderer renderer)
        {
            currentMap.Render(renderer);
        }
    }
}
