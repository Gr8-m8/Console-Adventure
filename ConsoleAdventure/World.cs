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
        string name = "World";
        public string Name => name;

        Dictionary<string, Map> maps = new Dictionary<string, Map>();
        Map currentMap;
        public Map CurrentMap => currentMap;

        public World(Random random)
        {
            name = GenerateName(random);
            currentMap = new Map_Island(random);
            maps.Add(currentMap.Name, currentMap);
            currentMap = currentMap.SelectedTile.Interior;
        }

        string GenerateName(Random random, string currentName = "RANDOM")
        {
            if (currentName != "RANDOM")
            {
                return currentName;
            }

            char[] konsonants = new char[] { 'Q', 'W', 'R', 'T', 'P', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' };
            char[] voels = new char[] { 'E', 'Y', 'U', 'I', 'O', 'A' };
            string nameOut = "";
            int lenght = 0;
            lenght = random.Next(3, 10);

            for (int i = 0; i < 2; i++)
            {
                if (random.Next(100) < 50 + 30 * i)
                {
                    nameOut += konsonants[random.Next(konsonants.Length)];
                }
                else
                {
                    nameOut += voels[random.Next(voels.Length)];
                }
            }

            while (nameOut.Length < lenght)
            {
                if (konsonants.Contains(nameOut[nameOut.Length - 1]))
                {
                    if (konsonants.Contains(nameOut[nameOut.Length - 2]))
                    {
                        nameOut += voels[random.Next(voels.Length)];
                    }
                    else
                    {
                        if (random.Next(100) < 50)
                        {
                            nameOut += konsonants[random.Next(konsonants.Length)];
                        }
                        else
                        {
                            nameOut += voels[random.Next(voels.Length)];
                        }
                    }
                }
                else if (voels.Contains(nameOut[nameOut.Length - 1]))
                {
                    if (voels.Contains(nameOut[nameOut.Length - 2]))
                    {
                        nameOut += konsonants[random.Next(konsonants.Length)];
                    }
                    else
                    {
                        if (random.Next(100) < 80)
                        {
                            nameOut += konsonants[random.Next(konsonants.Length)];
                        }
                        else
                        {
                            nameOut += voels[random.Next(voels.Length)];
                        }
                    }
                }
                else
                {
                    if (random.Next(100) < 50)
                    {
                        nameOut += konsonants[random.Next(konsonants.Length)];
                    }
                    else
                    {
                        nameOut += voels[random.Next(voels.Length)];
                    }
                }
            }

            return nameOut;
        }

        public void EnterExitInteriorExterior()
        {
            Console.Beep(121, 100);
            Console.Beep(151, 100);
            if (CurrentMap.SelectedTile.Interior != null)
            {
                currentMap = CurrentMap.SelectedTile.Interior;

                if (CurrentMap.GetType() == typeof(Map_Trigger))
                {
                    Map_Trigger trigger = (Map_Trigger)currentMap;
                    trigger.TriggerActivate();
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
