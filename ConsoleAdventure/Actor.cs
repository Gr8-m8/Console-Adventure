using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleAdventure
{
    class Actor
    {
        private string name;
        public string Name => name;

        int skillPoints;
        int strenght;
        int endurance;
        int dexsterity;
        int wisdom;
        int intelligence;
        int charisma;

        Actor_Description description;
        public Actor_Description Description => description;

        Dictionary<string, Item> inventory = new Dictionary<string, Item>();

        public Actor(Random random)
        {
            name = GenerateName(random, "RANDOM");
            description = new Actor_Description(name);

            this.InitilizeSkillPoints(0, 10, 10, 10, 10, 10, 10);
        }

        public void InitilizeSkillPoints(int skp, int str, int end, int dex, int wis, int ilg, int cha)
        {
            skillPoints = skp;
            strenght = str;
            endurance = end;
            dexsterity = dex;
            wisdom = wis;
            intelligence = ilg;
            charisma = cha;
        }

        string GenerateName(Random random, string currentName)
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
                if (random.Next(100) < 50 + 30*i)
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

        
    }

    class Actor_Description
    {
        string name = "";

        DescriptionRace race;
        public enum DescriptionRace
        {
            None,
            Human,
            Elf,
            Orc
        }
        DescriptionGender gender;
        public enum DescriptionGender
        {
            None,
            Male,
            Female,
            Male_Female
        }

        public enum DescriptionSize
        {
            None,
            Small,
            Medium,
            Large
        }

        public enum DescriptionLenght
        {
            None,
            Short,
            Medium,
            Long
        }

        DescriptionLenght description_hair;
        ConsoleColor description_eyes;
        DescriptionLenght description_beard;
        DescriptionSize description_torso;
        DescriptionSize description_legs;
        ConsoleColor desctription_hairColor;
        ConsoleColor description_skin;

        public Actor_Description(string nameSet)
        {
            name = nameSet;
            this.InitilizeNRG(DescriptionRace.Human, DescriptionGender.None);

            this.InitilizeDescriptionHead(DescriptionLenght.None, ConsoleColor.Green, DescriptionLenght.None);
            this.InitilizeDescriptionTorso(DescriptionSize.Medium);
            this.InitilizeDescriptionLegs(DescriptionSize.Medium);
            this.InitilizeDescriptionOther(ConsoleColor.DarkGray, ConsoleColor.White);
        }

        public void InitilizeNRG(DescriptionRace raceSet, DescriptionGender genderSet)
        {

            race = raceSet;
            gender = genderSet;
        }

        public void InitilizeDescriptionHead(DescriptionLenght hair, ConsoleColor eyes, DescriptionLenght beard)
        {
            description_hair = hair;
            description_eyes = eyes;
            description_beard = beard;
        }

        public void InitilizeDescriptionTorso(DescriptionSize torso)
        {
            description_torso = torso;
        }

        public void InitilizeDescriptionLegs(DescriptionSize legs)
        {
            description_legs = legs;
        }

        public void InitilizeDescriptionOther(ConsoleColor hairColor, ConsoleColor skin)
        {
            desctription_hairColor = hairColor;
            description_skin = skin;
        }

        public virtual void Render(Renderer renderer, int x, int y)
        {

            //BG
            int width = 32;
            renderer.DrawBox(x, y, width, 26, ConsoleColor.Gray);
            renderer.DrawText(x, y - 1, name, ConsoleColor.White, width);

            //HAIR
            if (description_hair > 0)
            {
                renderer.DrawBox(x + (width / 2 - 8), y + 0, (2 * 8 + 1), 1, desctription_hairColor);
                int lenght = 0;
                if (description_hair == DescriptionLenght.Short)
                {
                    lenght = 2;
                }
                if (description_hair == DescriptionLenght.Medium)
                {
                    lenght = 5;
                }
                if (description_hair == DescriptionLenght.Long)
                {
                    lenght = 7;
                    renderer.DrawBox(x + (width / 2 - 10 + 1), y + lenght + 1, (2 * 10 - 2 * 1 + 1), 1, desctription_hairColor);
                    renderer.DrawBox(x + (width / 2 - 10 + 2), y + lenght + 2, (2 * 10 - 2 * 2 + 1), 1, desctription_hairColor);
                }
                renderer.DrawBox(x + (width / 2 - 10), y + 1, (2 * 10 + 1), lenght, desctription_hairColor);
            }
            else
            {
                //(HEAD)
                renderer.DrawBox(x + (width / 2 - 8), y + 2 - 0, (2 * 8 + 1), 1, description_skin);
                renderer.DrawBox(x + (width / 2 - 6), y + 2 - 1, (2 * 6 + 1), 1, description_skin);
            }

            //HEAD
            renderer.DrawBox(x + (width / 2 - 6), y + 2, (2 * 6 + 1), 1, description_skin);
            renderer.DrawBox(x + (width / 2 - 8), y + 3, (2 * 8 + 1), 4, description_skin);
            renderer.DrawBox(x + (width / 2 - 6), y + 7, (2 * 6 + 1), 1, description_skin);
            renderer.DrawBox(x + (width / 2 - 2), y + 8, (2 * 2 + 1), 1, description_skin);

            //EYES
            renderer.DrawBox(x + (width / 2 - 2 - 3), y + 4, 3, 1, description_eyes);
            renderer.DrawPixel(x + (width / 2 - 1 - 3), y + 4, description_eyes, '0', ConsoleColor.White);

            renderer.DrawBox(x + (width / 2 + 0 + 3), y + 4, 3, 1, description_eyes);
            renderer.DrawPixel(x + (width / 2 + 1 + 3), y + 4, description_eyes, '0', ConsoleColor.White);

            //BEARD
            if (description_beard > 0)
            {
                if (description_beard == DescriptionLenght.Short)
                {
                    renderer.DrawBox(x + (width / 2 - 1), y + 7, 3, 1, desctription_hairColor);
                }

                if (description_beard == DescriptionLenght.Medium)
                {
                    renderer.DrawBox(x + (width / 2 - 3), y + 6, 7, 2, desctription_hairColor);
                }

                if (description_beard == DescriptionLenght.Long)
                {
                    renderer.DrawBox(x + (width / 2 - 3 - 3), y + 6, 7 + 6, 2, desctription_hairColor);
                    renderer.DrawBox(x + (width / 2 - 3 - 2), y + 8, 7 + 4, 1, desctription_hairColor);
                }
            }

            //MOUTH
            for (int i = 0; i < 5; i++)
            {
                char line = '—';
                if (i < 1 || i > 3)
                {
                    line = ' ';
                }
                renderer.DrawPixel(x + (width / 2 - 2) + i, y + 6, ConsoleColor.Red, line, ConsoleColor.DarkRed);
            }


            //TORSO
            int torsoSize = 0;
            if (description_torso == DescriptionSize.Small)
            {
                torsoSize = 0;
            }
            if (description_torso == DescriptionSize.Medium)
            {
                torsoSize = 1;
            }
            if (description_torso == DescriptionSize.Large)
            {
                torsoSize = 2;
            }

            renderer.DrawBox(x + (width / 2 - 5 - torsoSize), y + 9, (2 * 5 + 2 * torsoSize + 1), 1, description_skin);
            renderer.DrawBox(x + (width / 2 - 8 - torsoSize), y + 10, (2 * 8 + 2 * torsoSize + 1), 2, description_skin);
            renderer.DrawBox(x + (width / 2 - 6 - torsoSize), y + 12, (2 * 6 + 2 * torsoSize + 1), 5, description_skin);
            renderer.DrawBox(x + (width / 2 - 5 - torsoSize), y + 17, (2 * 5 + 2 * torsoSize + 1), 1, description_skin);

            //ARMS
            renderer.DrawBox(x + (width / 2 - 9 - 2 * torsoSize), y + 11, (2 + torsoSize), 7, description_skin);

            renderer.DrawBox(x + (width / 2 + 8 + torsoSize), y + 11, (2 + torsoSize), 7, description_skin);


            //LEGS
            int legSize = 0;
            if (description_legs == DescriptionSize.Small)
            {
                legSize = 0;
            }
            if (description_legs == DescriptionSize.Medium)
            {
                legSize = 1;
            }
            if (description_legs == DescriptionSize.Large)
            {
                legSize = 2;
            }
            renderer.DrawBox(x + (width / 2 - 2 - 3 - torsoSize), y + 18, 3 + legSize, 7, description_skin);
            renderer.DrawBox(x + (width / 2 - 2 + 0 - torsoSize + legSize), y + 18, 1, 1, description_skin);

            renderer.DrawBox(x + (width / 2 + 2 + 1 + torsoSize - legSize), y + 18, 3 + legSize, 7, description_skin);
            renderer.DrawBox(x + (width / 2 + 2 - 0 + torsoSize - legSize), y + 18, 1, 1, description_skin);
        }
    }
}
