using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleAdventure
{
    class Program
    {
        readonly static string gameTitle = "Console Adventure!";

        public static bool Mute => false;

        static Random random = new Random();

        static Renderer renderer = new Renderer();
        static Menu menu = new Menu(0, 0, 0,"NULL");

        static World world = new World(random);

        static Player player = new Player(random);


        readonly static int standardMenuWidth = 48;
        readonly static int standardMenuY = renderer.Height / 2 - 10 / 2 - 2;

        static void Main()
        {
            Console.Title = gameTitle;

            renderer = new Renderer();

            world = new World(random);
            player = new Player(random);

            renderer.DrawBorder();
            renderer.DrawClear();

            MainMenu();
            //CharacterCreation();
            Exploring();

            renderer.Render();

            Console.ReadKey();
        }

        static void KeyEvent(bool exploring = false)
        {
            if (Console.KeyAvailable)
            {
                switch (Console.ReadKey().Key)
                {
                    default:
                        break;

                    case ConsoleKey.F5:
                    case ConsoleKey.Escape:
                        if (!Program.Mute)
                        {
                            Console.Beep(100, 100);
                        }
                        Environment.Exit(0);
                        break;

                    case ConsoleKey.NumPad0:
                        renderer.Render(true);
                        break;

                    case ConsoleKey.Enter:
                        menu.EventCheck();
                        break;

                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        if (exploring)
                        {
                            world.CurrentMap.MoveCamera(0, -1);
                        }
                        else
                        {
                            menu.SelectIndex(-1);
                        }
                        break;

                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        if (exploring)
                        {
                            world.CurrentMap.MoveCamera(0, 1);
                        }
                        else
                        {
                            menu.SelectIndex(1);
                        }
                        break;

                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        if (exploring)
                        {
                            world.CurrentMap.MoveCamera(-1, 0);
                        }
                        break;

                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        if (exploring)
                        {
                            world.CurrentMap.MoveCamera(1, 0);
                        }
                        break;

                    case ConsoleKey.Spacebar:
                        world.EnterExitInteriorExterior();
                        break;

                    case ConsoleKey.Tab:
                        menu.SelectIndex(1);
                        break;
                }
            }
        }

        public static void MainMenu()
        {
            Console.Title = gameTitle + " Main Menu";

            bool loopMainMenu = true;

            renderer.DrawClear();

            menu = new Menu(((renderer.Width + 1) / 2 - standardMenuWidth / 2), (standardMenuY), standardMenuWidth, "Main Menu");
            menu.AddMenuItem(new MenuButton("START")
            {
                ButtonEvent = () => 
                {
                    if (!Program.Mute)
                    {
                        Console.Beep(121, 100);
                    }
                    loopMainMenu = false;
                }
            });

            menu.AddBlankMenuItem();
            menu.AddMenuItem(new MenuButton("LOAD SAVE")
            {
                ButtonEvent = () =>
                {
                    if (!Program.Mute)
                    {
                        Console.Beep(121, 100);
                    }
                    Menu startMenu = menu;

                    renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
                    menu = new Menu(((renderer.Width+1) / 2 - standardMenuWidth / 2), (standardMenuY), standardMenuWidth, "Load Save");

                    menu.AddMenuItem(new MenuButton("Back")
                    {
                        ButtonEvent = () =>
                        {
                            if (!Program.Mute)
                            {
                                Console.Beep(121, 100);
                            }
                            renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
                            menu = startMenu;
                        }
                    });
                }
            });

            menu.AddBlankMenuItem();
            menu.AddMenuItem(new MenuButton("OPTIONS")
            {
                ButtonEvent = () => 
                {
                    if (!Program.Mute)
                    {
                        Console.Beep(121, 100);
                    }
                    Menu startMenu = menu;

                    renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
                    menu = new Menu(((renderer.Width +1) / 2 - standardMenuWidth / 2), (standardMenuY), standardMenuWidth, "Options  ");

                    menu.AddMenuItem(new MenuButton("Back")
                    {
                        ButtonEvent = () =>
                        {
                            if (!Program.Mute)
                            {
                                Console.Beep(121, 100);
                            }
                            renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
                            menu = startMenu;
                        }
                    });
                }
            });

            menu.AddBlankMenuItem();
            menu.AddMenuItem(new MenuButton("CREDITS")
            {
                ButtonEvent = () => 
                {
                    if (!Program.Mute)
                    {
                        Console.Beep(121, 100);
                    }
                    Menu startMenu = menu;

                    renderer.DrawClear();
                    menu = new Menu(((renderer.Width + 1) / 2 - standardMenuWidth / 2), (standardMenuY), standardMenuWidth, "Credits  ");

                    menu.AddMenuItem(new MenuText("Game Created by Martin Lindblad"));
                    menu.AddMenuItem(new MenuText("Github: https://github.com/Gr8-m8"));
                    menu.AddBlankMenuItem();
                    menu.AddMenuItem(new MenuButton("Back")
                    {
                        ButtonEvent = () =>
                        {
                            if (!Program.Mute)
                            {
                                Console.Beep(121, 100);
                            }
                            renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
                            menu = startMenu;
                        }
                    });
                }
            });

            menu.AddBlankMenuItem();
            menu.AddMenuItem(new MenuButton("Exit")
            {
                ButtonEvent = () =>
                {
                    if (!Program.Mute)
                    {
                        Console.Beep(100, 100);
                    }
                    Environment.Exit(0);
                }
            });

            if (!Program.Mute)
            {
                Console.Beep(100, 100);
            }

            while (loopMainMenu)
            {
                menu.Renderer(renderer);
                renderer.Render();
                KeyEvent();
            }
        }

        static void CharacterCreation()
        {
            Console.Title = gameTitle + " Character Creation";

            bool loopCharacterCreation = true;

            renderer.DrawBox(0, 0, renderer.Width, renderer.Height, ConsoleColor.Gray);
            player = new Player(random);

            //Character Creation: Name & Race
            #region Character Creation: Name & Race
            loopCharacterCreation = true;

            renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
            menu = new Menu(((renderer.Width+1) / 2 - standardMenuWidth / 2), (standardMenuY), standardMenuWidth, "Character Creation: Name & Race");

            menu.AddMenuItem(new MenuText("NAME:"));
            MenuButton nameInput = new MenuButton("NONAME");
            nameInput.ButtonEvent = () =>
            {
                Console.Beep(131, 50);
                Console.Beep(111, 50);
                bool loopNameInput = true;
                while (loopNameInput)
                {
                    menu.Renderer(renderer);
                    renderer.Render();
                    ConsoleKey ck = Console.ReadKey().Key;
                    switch (ck)
                    {
                        default:
                            nameInput.Text += ck.ToString();
                            break;

                        case ConsoleKey.Backspace:
                            if (nameInput.Text.Length > 0)
                            {
                                nameInput.Text = nameInput.Text.Remove(nameInput.Text.Length - 1);
                            }
                            break;

                        case ConsoleKey.Enter:
                            loopNameInput = false;
                            break;

                        case ConsoleKey.Delete:
                            nameInput.Text = "";
                            break;
                    }
                }
            };
            menu.AddMenuItem(nameInput);
            menu.AddBlankMenuItem();

            int raceIndex = 0;
            Actor_Description.DescriptionRace[] races = new Actor_Description.DescriptionRace[] { Actor_Description.DescriptionRace.Human, Actor_Description.DescriptionRace.Elf, Actor_Description.DescriptionRace.Orc };
            MenuText raceIndexCounter = new MenuText("Race: " + races[raceIndex]);
            menu.AddMenuItem(raceIndexCounter);
            menu.AddMenuItem(new MenuButton("   +")
            {
                ButtonEvent = () =>
                {
                    if (raceIndex < races.Length -1)
                    {
                        Console.Beep(131, 100);
                        raceIndexCounter.Text = "Race: " + races[++raceIndex];
                    }
                }
            });
            menu.AddMenuItem(new MenuButton("   -")
            {
                ButtonEvent = () =>
                {
                    if (raceIndex > 0)
                    {
                        Console.Beep(111, 100);
                        raceIndexCounter.Text = "Race: " + races[--raceIndex];
                    }
                }
            });
            menu.AddBlankMenuItem();

            int genderIndex = 0;
            Actor_Description.DescriptionGender[] genders = new Actor_Description.DescriptionGender[] { Actor_Description.DescriptionGender.None, Actor_Description.DescriptionGender.Male, Actor_Description.DescriptionGender.Female, Actor_Description.DescriptionGender.Male_Female };
            MenuText genderIndexCounter = new MenuText("Gender: " + genders[genderIndex]);
            menu.AddMenuItem(genderIndexCounter);
            menu.AddMenuItem(new MenuButton("   +")
            {
                ButtonEvent = () =>
                {
                    if (genderIndex < genders.Length - 1)
                    {
                        Console.Beep(131, 100);
                        genderIndexCounter.Text = "Gender: " + genders[++genderIndex];
                    }
                }
            });
            menu.AddMenuItem(new MenuButton("   -")
            {
                ButtonEvent = () =>
                {
                    if (genderIndex > 0)
                    {
                        Console.Beep(111, 100);
                        genderIndexCounter.Text = "Gender: " + genders[--genderIndex];
                    }
                }
            });

            menu.AddBlankMenuItem();
            menu.AddMenuItem(new MenuButton("Continue: ")
            {
                ButtonEvent = () => 
                {
                    Console.Beep(121, 100);
                    loopCharacterCreation = false;
                }
            });

            while (loopCharacterCreation)
            {
                menu.Renderer(renderer);
                renderer.Render();
                KeyEvent();
            }

            player.Description.InitilizeNRG(races[raceIndex], genders[genderIndex]);
            #endregion

            //Character Creation: Skillpoints
            #region Character Creation: Skillpoints
            loopCharacterCreation = true;

            renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
            menu = new Menu(((renderer.Width+1) / 2 - standardMenuWidth / 2), (standardMenuY - 6), standardMenuWidth, "Character Creation: Skillpoints");

            //SKILLPOINTS
            int skillPoints = 42;
            MenuText skillPointCounter = new MenuText("Skillpoints: " + skillPoints);
            menu.AddMenuItem(skillPointCounter);

            menu.AddMenuItem(new MenuText("    Physic:"));

            //STRENGHT
            int strenghtPoints = 3;
            MenuText strenghtPointCounter = new MenuText("Strenght: " + strenghtPoints);
            menu.AddMenuItem(strenghtPointCounter);
            menu.AddMenuItem(new MenuButton("       +")
            {
                ButtonEvent = () => 
                {
                    if (skillPoints > 0)
                    {
                        Console.Beep(131, 100);
                        skillPointCounter.Text = "Skillpoints: " + --skillPoints;
                        strenghtPointCounter.Text = "Strenght: " + ++strenghtPoints;
                    }
                }
            });
            menu.AddMenuItem(new MenuButton("       -")
            {
                ButtonEvent = () => 
                {
                    if (strenghtPoints > 0)
                    {
                        Console.Beep(111, 100);
                        skillPointCounter.Text = "Skillpoints: " + ++skillPoints;
                        strenghtPointCounter.Text = "Strenght: " + --strenghtPoints;
                    }
                }
            });

            //ENDURANCE
            int endurancePoints = 3;
            MenuText endurancePointCounter = new MenuText("Endurance: " + endurancePoints);
            menu.AddMenuItem(endurancePointCounter);
            menu.AddMenuItem(new MenuButton("       +")
            {
                ButtonEvent = () => 
                {
                    if (skillPoints > 0)
                    {
                        Console.Beep(131, 100);
                        skillPointCounter.Text = "Skillpoints: " + --skillPoints;
                        endurancePointCounter.Text = "Endurance: " + ++endurancePoints;
                    }
                }
            });
            menu.AddMenuItem(new MenuButton("       -")
            {
                ButtonEvent = () => 
                {
                    if (endurancePoints > 0)
                    {
                        Console.Beep(111, 100);
                        skillPointCounter.Text = "Skillpoints: " + ++skillPoints;
                        endurancePointCounter.Text = "Endurance: " + --endurancePoints;
                    }
                }
            });

            //DEXTERITY
            int dexsterityPoints = 3;
            MenuText dexterityPointCounter = new MenuText("Dexterity: " + dexsterityPoints);
            menu.AddMenuItem(dexterityPointCounter);
            menu.AddMenuItem(new MenuButton("       +")
            {
                ButtonEvent = () => 
                {
                    if (skillPoints > 0)
                    {
                        Console.Beep(131, 100);
                        skillPointCounter.Text = "Skillpoints: " + --skillPoints;
                        dexterityPointCounter.Text = "Dexyesterity: " + ++dexsterityPoints;
                    }
                }
            });
            menu.AddMenuItem(new MenuButton("       -")
            {
                ButtonEvent = () => 
                {
                    if (dexsterityPoints > 0)
                    {
                        Console.Beep(111, 100);
                        skillPointCounter.Text = "Skillpoints: " + ++skillPoints;
                        dexterityPointCounter.Text = "Dexsterity: " + --dexsterityPoints;
                    }
                }
            });

            menu.AddMenuItem(new MenuText("    Psyce:"));

            //WISDOM
            int wisdomPoints = 3;
            MenuText wisdomPointCounter = new MenuText("Wisdom: " + wisdomPoints);
            menu.AddMenuItem(wisdomPointCounter);
            menu.AddMenuItem(new MenuButton("       +")
            {
                ButtonEvent = () => 
                {
                    if (skillPoints > 0)
                    {
                        Console.Beep(131, 100);
                        skillPointCounter.Text = "Skillpoints: " + --skillPoints;
                        wisdomPointCounter.Text = "Wisdom: " + ++wisdomPoints;
                    }
                }
            });
            menu.AddMenuItem(new MenuButton("       -")
            {
                ButtonEvent = () => 
                {
                    if (wisdomPoints > 0)
                    {
                        Console.Beep(111, 100);
                        skillPointCounter.Text = "Skillpoints: " + ++skillPoints;
                        wisdomPointCounter.Text = "Wisdom: " + --wisdomPoints;
                    }
                }
            });

            //INTELLIGENCE
            int intelligencePoints = 3;
            MenuText intelligencePointCounter = new MenuText("Intelligence: " + intelligencePoints);
            menu.AddMenuItem(intelligencePointCounter);
            menu.AddMenuItem(new MenuButton("       +")
            {
                ButtonEvent = () => 
                {
                    if (skillPoints > 0)
                    {
                        Console.Beep(131, 100);
                        skillPointCounter.Text = "Skillpoints: " + --skillPoints;
                        intelligencePointCounter.Text = "Intelligence: " + ++intelligencePoints;
                    }
                }
            });
            menu.AddMenuItem(new MenuButton("       -")
            {
                ButtonEvent = () => 
                {
                    if (intelligencePoints > 0)
                    {
                        Console.Beep(111, 100);
                        skillPointCounter.Text = "Skillpoints: " + ++skillPoints;
                        intelligencePointCounter.Text = "Intelligence: " + --intelligencePoints;
                    }
                }
            });

            //CHARISMA
            int charismaPoints = 3;
            MenuText charismaPointCounter = new MenuText("Charisma: " + charismaPoints);
            menu.AddMenuItem(charismaPointCounter);
            menu.AddMenuItem(new MenuButton("       +")
            {
                ButtonEvent = () => 
                {
                    if (skillPoints > 0)
                    {
                        Console.Beep(131, 100);
                        skillPointCounter.Text = "Skillpoints: " + --skillPoints;
                        charismaPointCounter.Text = "Charisma: " + ++charismaPoints;
                    }
                }
            });
            menu.AddMenuItem(new MenuButton("       -")
            {
                ButtonEvent = () => 
                {
                    if (charismaPoints > 0)
                    {
                        Console.Beep(111, 100);
                        skillPointCounter.Text = "Skillpoints: " + ++skillPoints;
                        charismaPointCounter.Text = "Charisma: " + --charismaPoints;
                    }
                }
            });

            menu.AddBlankMenuItem();
            menu.AddMenuItem(new MenuButton("Continue: ")
            {
                ButtonEvent = () => 
                {
                    Console.Beep(121, 100);
                    loopCharacterCreation = false;
                }
            });

            while (loopCharacterCreation)
            {
                menu.Renderer(renderer);
                renderer.Render();
                KeyEvent();
            }

            player.InitilizeSkillPoints(skillPoints, strenghtPoints, endurancePoints, dexsterityPoints, wisdomPoints, intelligencePoints, charismaPoints);
            #endregion

            //Character Creation: Looks
            #region Character Creation: Looks
            loopCharacterCreation = true;

            renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
            menu = new Menu(((renderer.Width+1) / 2 - standardMenuWidth / 2), (standardMenuY), standardMenuWidth, "Character Creation: Looks");

            //HEAD
            menu.AddMenuItem(new MenuButton("Head")
            {
                ButtonEvent = () =>
                {
                    Console.Beep(121, 100);
                    Menu startMenu = menu;

                    renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
                    menu = new Menu(((renderer.Width+1) / 2 - standardMenuWidth / 2), (standardMenuY), standardMenuWidth, "Head");

                    //HAIR
                    int hairIndex = 0;
                    Actor_Description.DescriptionLenght[] hairs = new Actor_Description.DescriptionLenght[] { Actor_Description.DescriptionLenght.None, Actor_Description.DescriptionLenght.Short, Actor_Description.DescriptionLenght.Medium, Actor_Description.DescriptionLenght.Long };
                    MenuText hairIndexCounter = new MenuText("Hair: " + hairs[hairIndex]);
                    menu.AddMenuItem(hairIndexCounter);
                    menu.AddMenuItem(new MenuButton("   +")
                    {
                        ButtonEvent = () =>
                        {
                            if (hairIndex < hairs.Length - 1)
                            {
                                Console.Beep(131, 100);
                                hairIndexCounter.Text = "Hair: " + hairs[++hairIndex];
                            }
                        }
                    });
                    menu.AddMenuItem(new MenuButton("   -")
                    {
                        ButtonEvent = () =>
                        {
                            if (hairIndex > 0)
                            {
                                Console.Beep(111, 100);
                                hairIndexCounter.Text = "Hair: " + hairs[--hairIndex];
                            }
                        }
                    });
                    menu.AddBlankMenuItem();

                    //EYES
                    int eyeIndex = 0;
                    ConsoleColor[] eyes = new ConsoleColor[] { ConsoleColor.Black, ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.Gray, ConsoleColor.Green, ConsoleColor.Magenta, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Yellow };
                    MenuText eyeIndexCounter = new MenuText("Eyes: " + eyes[eyeIndex]);
                    menu.AddMenuItem(eyeIndexCounter);
                    menu.AddMenuItem(new MenuButton("   +")
                    {
                        ButtonEvent = () =>
                        {
                            if (eyeIndex < eyes.Length - 1)
                            {
                                Console.Beep(131, 100);
                                eyeIndexCounter.Text = "Eyes: " + eyes[++eyeIndex];
                            }
                        }
                    });
                    menu.AddMenuItem(new MenuButton("   -")
                    {
                        ButtonEvent = () =>
                        {
                            if (eyeIndex > 0)
                            {
                                Console.Beep(111, 100);
                                eyeIndexCounter.Text = "Eyes: " + eyes[--eyeIndex];
                            }
                        }
                    });
                    menu.AddBlankMenuItem();

                    //BEARD
                    int beardIndex = 0;
                    Actor_Description.DescriptionLenght[] beard = new Actor_Description.DescriptionLenght[] { Actor_Description.DescriptionLenght.None, Actor_Description.DescriptionLenght.None, Actor_Description.DescriptionLenght.Medium, Actor_Description.DescriptionLenght.Long };
                    MenuText beardIndexCounter = new MenuText("Beard: " + beard[beardIndex]);
                    menu.AddMenuItem(beardIndexCounter);
                    menu.AddMenuItem(new MenuButton("   +")
                    {
                        ButtonEvent = () =>
                        {
                            if (beardIndex < beard.Length - 1)
                            {
                                Console.Beep(131, 100);
                                beardIndexCounter.Text = "Beard: " + beard[++beardIndex];
                            }
                        }
                    });
                    menu.AddMenuItem(new MenuButton("   -")
                    {
                        ButtonEvent = () =>
                        {
                            if (beardIndex > 0)
                            {
                                Console.Beep(111, 100);
                                beardIndexCounter.Text = "Beard: " + beard[--beardIndex];
                            }
                        }
                    });
                    menu.AddBlankMenuItem();

                    menu.AddMenuItem(new MenuButton("Back")
                    {
                        ButtonEvent = () =>
                        {
                            Console.Beep(121, 100);
                            renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
                            menu = startMenu;

                            player.Description.InitilizeDescriptionHead(hairs[hairIndex], eyes[eyeIndex], beard[beardIndex]);
                        }
                    });
                }
            });
            menu.AddBlankMenuItem();

            //TORSO
            menu.AddMenuItem(new MenuButton("Torso")
            {
                ButtonEvent = () =>
                {
                    Console.Beep(121, 100);
                    Menu startMenu = menu;

                    renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
                    menu = new Menu(((renderer.Width+1) / 2 - standardMenuWidth / 2), (standardMenuY), standardMenuWidth, "Torso");
                    
                    //TORSO
                    int sizeIndex = 0;
                    Actor_Description.DescriptionSize[] sizes = new Actor_Description.DescriptionSize[] { Actor_Description.DescriptionSize.Small, Actor_Description.DescriptionSize.Medium, Actor_Description.DescriptionSize.Large };
                    MenuText sizeIndexCounter = new MenuText("Size: " + sizes[sizeIndex]);
                    menu.AddMenuItem(sizeIndexCounter);
                    menu.AddMenuItem(new MenuButton("   +")
                    {
                        ButtonEvent = () =>
                        {
                            if (sizeIndex < sizes.Length - 1)
                            {
                                Console.Beep(131, 100);
                                sizeIndexCounter.Text = "Size: " + sizes[++sizeIndex];
                            }
                        }
                    });
                    menu.AddMenuItem(new MenuButton("   -")
                    {
                        ButtonEvent = () =>
                        {
                            if (sizeIndex > 0)
                            {
                                Console.Beep(111, 100);
                                sizeIndexCounter.Text = "Size: " + sizes[--sizeIndex];
                            }
                        }
                    });
                    menu.AddBlankMenuItem();

                    menu.AddMenuItem(new MenuButton("Back")
                    {
                        ButtonEvent = () =>
                        {
                            Console.Beep(121, 100);
                            renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
                            menu = startMenu;

                            player.Description.InitilizeDescriptionTorso(sizes[sizeIndex]);
                        }
                    });
                }
            });
            menu.AddBlankMenuItem();

            //LEGS
            menu.AddMenuItem(new MenuButton("Legs")
            {
                ButtonEvent = () =>
                {
                    Console.Beep(121, 100);
                    Menu startMenu = menu;

                    renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
                    menu = new Menu(((renderer.Width+1) / 2 - standardMenuWidth / 2), (standardMenuY), standardMenuWidth, "Legs");

                    //LEGS
                    int sizeIndex = 0;
                    Actor_Description.DescriptionSize[] sizes = new Actor_Description.DescriptionSize[] { Actor_Description.DescriptionSize.Small, Actor_Description.DescriptionSize.Medium, Actor_Description.DescriptionSize.Large };
                    MenuText sizeIndexCounter = new MenuText("Size: " + sizes[sizeIndex]);
                    menu.AddMenuItem(sizeIndexCounter);
                    menu.AddMenuItem(new MenuButton("   +")
                    {
                        ButtonEvent = () =>
                        {
                            if (sizeIndex < sizes.Length - 1)
                            {
                                Console.Beep(131, 100);
                                sizeIndexCounter.Text = "Size: " + sizes[++sizeIndex];
                            }
                        }
                    });
                    menu.AddMenuItem(new MenuButton("   -")
                    {
                        ButtonEvent = () =>
                        {
                            if (sizeIndex > 0)
                            {
                                Console.Beep(111, 100);
                                sizeIndexCounter.Text = "Size: " + sizes[--sizeIndex];
                            }
                        }
                    });
                    menu.AddBlankMenuItem();

                    menu.AddMenuItem(new MenuButton("Back")
                    {
                        ButtonEvent = () =>
                        {
                            Console.Beep(121, 100);
                            renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
                            menu = startMenu;

                            player.Description.InitilizeDescriptionLegs(sizes[sizeIndex]);
                        }
                    });
                }
            });
            menu.AddBlankMenuItem();

            //OTHER
            menu.AddMenuItem(new MenuButton("Other")
            {
                ButtonEvent = () =>
                {
                    Console.Beep(121, 100);
                    Menu startMenu = menu;

                    renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
                    menu = new Menu(((renderer.Width+1) / 2 - standardMenuWidth / 2), (standardMenuY), standardMenuWidth, "Other");

                    //HAIRCOLOR
                    int hairIndex = 0;
                    ConsoleColor[] hairs = new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Black, ConsoleColor.Blue, ConsoleColor.DarkBlue, ConsoleColor.Cyan, ConsoleColor.DarkCyan, ConsoleColor.Gray, ConsoleColor.DarkGray, ConsoleColor.Green, ConsoleColor.DarkGreen, ConsoleColor.Magenta, ConsoleColor.DarkMagenta, ConsoleColor.Red, ConsoleColor.DarkRed, ConsoleColor.Yellow, ConsoleColor.DarkYellow };
                    MenuText hairIndexCounter = new MenuText("Hair Color: " + hairs[hairIndex]);
                    menu.AddMenuItem(hairIndexCounter);
                    menu.AddMenuItem(new MenuButton("   +")
                    {
                        ButtonEvent = () =>
                        {
                            if (hairIndex < hairs.Length - 1)
                            {
                                Console.Beep(131, 100);
                                hairIndexCounter.Text = "Hair Color: " + hairs[++hairIndex];
                            }
                        }
                    });
                    menu.AddMenuItem(new MenuButton("   -")
                    {
                        ButtonEvent = () =>
                        {
                            if (hairIndex > 0)
                            {
                                Console.Beep(111, 100);
                                hairIndexCounter.Text = "Hair Color: " + hairs[--hairIndex];
                            }
                        }
                    });
                    menu.AddBlankMenuItem();

                    //SKIN
                    int skinIndex = 0;
                    ConsoleColor[] skin = new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Black, ConsoleColor.Blue, ConsoleColor.DarkBlue, ConsoleColor.Cyan, ConsoleColor.DarkCyan, ConsoleColor.Gray, ConsoleColor.DarkGray, ConsoleColor.Green, ConsoleColor.DarkGreen, ConsoleColor.Magenta, ConsoleColor.DarkMagenta, ConsoleColor.Red, ConsoleColor.DarkRed, ConsoleColor.Yellow, ConsoleColor.DarkYellow };
                    MenuText skinIndexCounter = new MenuText("Skin: " + skin[skinIndex]);
                    menu.AddMenuItem(skinIndexCounter);
                    menu.AddMenuItem(new MenuButton("   +")
                    {
                        ButtonEvent = () =>
                        {
                            if (skinIndex < skin.Length - 1)
                            {
                                Console.Beep(131, 100);
                                skinIndexCounter.Text = "Skin: " + skin[++skinIndex];
                            }
                        }
                    });
                    menu.AddMenuItem(new MenuButton("   -")
                    {
                        ButtonEvent = () =>
                        {
                            if (skinIndex > 0)
                            {
                                Console.Beep(111, 100);
                                skinIndexCounter.Text = "Skin: " + skin[--skinIndex];
                            }
                        }
                    });
                    menu.AddBlankMenuItem();

                    menu.AddMenuItem(new MenuButton("Back")
                    {
                        ButtonEvent = () =>
                        {
                            Console.Beep(121, 100);
                            renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
                            menu = startMenu;

                            player.Description.InitilizeDescriptionOther(hairs[hairIndex], skin[skinIndex]);
                        }
                    });
                }
            });
            menu.AddBlankMenuItem();

            menu.AddMenuItem(new MenuButton("Continue: ")
            {
                ButtonEvent = () => 
                {
                    Console.Beep(121, 100);
                    loopCharacterCreation = false;
                }
            });

            while (loopCharacterCreation)
            {
                menu.Renderer(renderer);
                renderer.Render();
                KeyEvent();
            }
            #endregion

            //RENDER
            renderer.DrawBox(1, 1, renderer.Width - 1, renderer.Height - 1, ConsoleColor.Black);
            player.Description.Render(renderer, (renderer.Width+1) / 2 - 32 / 2, 2);
            renderer.Render();
            Console.ReadKey();
        }

        public static void Exploring()
        {
            if (world == null)
            {
                return;
            }

            Console.Title = gameTitle + " Exploring";

            bool loopExploring = true;

            renderer.DrawClear();

            menu = new Menu(renderer.Width - 24, 1, 24, "Exploring");
            MenuText worldNameDisplay = new MenuText(world.Name);
            menu.AddMenuItem(worldNameDisplay);

            MenuText mapNameDisplay = new MenuText(world.CurrentMap.Name);
            menu.AddMenuItem(mapNameDisplay);

            MenuText tileNameDisplay = new MenuText(world.CurrentMap.SelectedTile.Name);
            menu.AddMenuItem(tileNameDisplay);

            MenuText positionDisplay = new MenuText("");
            menu.AddMenuItem(positionDisplay);
            menu.AddMenuItem(new MenuText(""));
            menu.AddMenuItem(new MenuText(""));
            menu.AddMenuItem(new MenuText(""));

            MenuButton tileEnterExitButton = new MenuButton("Enter Tile")
            {
                ButtonEvent = () =>
                {
                    world.EnterExitInteriorExterior();
                }
            };
            menu.AddMenuItem(tileEnterExitButton);
            

            while (loopExploring)
            {
                worldNameDisplay.Text = "World: " + world.Name;
                mapNameDisplay.Text = "Region: " + world.CurrentMap.Name;
                tileNameDisplay.Text = "Terrain: " + world.CurrentMap.SelectedTile.Name;

                if (world.CurrentMap.SelectedTile.Exsterior != null)
                {
                    tileEnterExitButton.Text = "Explore " + world.CurrentMap.SelectedTile.Exsterior.Name;
                }

                if (world.CurrentMap.SelectedTile.Interior != null)
                {
                    tileEnterExitButton.Text = "Explore " + world.CurrentMap.SelectedTile.Interior.Name;
                }

                positionDisplay.Text = "Position: " + world.CurrentMap.CameraX + ":" + -world.CurrentMap.CameraY;

                world.Render(renderer);
                menu.Renderer(renderer);
                renderer.Render();
                KeyEvent(true);
            }
        }

        public static void Battle(Actor enemy = null)
        {
            Console.Title = gameTitle + " Battle";

            renderer.DrawBorder();
            renderer.DrawClear();

            bool loopBattle = true;

            if (enemy == null)
            {
                enemy = new Actor(random);
            }

            menu = new Menu(((renderer.Width + 1) / 2 - standardMenuWidth / 2), (standardMenuY), standardMenuWidth, "Battle");

            
            MenuText enemyDisplay = new MenuText("Enemy: " + enemy.Name + ": " + enemy.Health);
            menu.AddMenuItem(enemyDisplay);

            menu.AddBlankMenuItem();
            MenuButton attackButton = new MenuButton("Attack")
            {
                ButtonEvent = () =>
                {
                    enemy.HeathManager(-player.Weapon.Attack(player));
                }
            };
            menu.AddMenuItem(attackButton);

            menu.AddBlankMenuItem();
            MenuButton retreat = new MenuButton("Escape")
            {
                ButtonEvent = () =>
                {
                    loopBattle = false;
                }
            };
            menu.AddMenuItem(retreat);
            
            while (loopBattle)
            {
                enemyDisplay.Text = "Enemy: " + enemy.Name + ": " + enemy.Health;
                if (enemy.IsDead)
                {
                    loopBattle = false;
                }

                menu.Renderer(renderer);
                renderer.Render();
                KeyEvent();
            }
        }

        public static void Dialog(Actor actor = null)
        {
            Console.Title = gameTitle + " Dialog";

            renderer.DrawBorder();
            renderer.DrawClear();

            bool loopDialog = true;

            menu = new Menu(((renderer.Width + 1) / 2 - standardMenuWidth / 2), (standardMenuY), standardMenuWidth, "Battle");

            while (loopDialog)
            {
                menu.Renderer(renderer);
                renderer.Render();
                KeyEvent();
            }
        }

        public static string GenerateName(string currentName = "RANDOM")
        {
            if (currentName != "RANDOM")
            {
                return currentName;
            }

            char[] konsonants = new char[] { 'Q', 'W', 'R', 'T', 'P', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' };
            char[] voels = new char[] { 'E', 'Y', 'U', 'I', 'O', 'A' };

            Dictionary<char, char[]> ctrail = new Dictionary<char, char[]>
            {
                { 'Q', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A' } },
                { 'W', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A', 'R' } },
                { 'R', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A' } },
                { 'T', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A', 'R', 'T', 'H' } },
                { 'P', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A', 'R', 'P' } },
                { 'S', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A', 'T', 'P', 'S', 'K', 'L', 'V', 'N', 'M' } },
                { 'D', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A', 'R' } },
                { 'F', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A', 'R', 'L', 'N' } },
                { 'G', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A', 'R', 'J', 'L', 'N' } },
                { 'H', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A', 'R', 'S', 'D', 'G', 'H', 'J', 'L', 'V', 'N', 'M' } },
                { 'J', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A' } },
                { 'K', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A', 'R', 'T', 'S', 'D', 'L' } },
                { 'L', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A', 'L', 'J' } },
                { 'Z', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A' } },
                { 'X', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A' } },
                { 'C', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A', 'K' } },
                { 'V', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A' } },
                { 'B', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A', 'R', 'S', 'H', 'L' } },
                { 'N', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A' } },
                { 'M', new char[] { 'E', 'Y', 'U', 'I', 'O', 'A' } },
                { 'E', new char[] { 'Q', 'W', 'R', 'T', 'P', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' } },
                { 'Y', new char[] { 'Q', 'W', 'R', 'T', 'P', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' } },
                { 'U', new char[] { 'Q', 'W', 'R', 'T', 'P', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' } },
                { 'I', new char[] { 'Q', 'W', 'R', 'T', 'P', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' } },
                { 'O', new char[] { 'Q', 'W', 'R', 'T', 'P', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' } },
                { 'A', new char[] { 'Q', 'W', 'R', 'T', 'P', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' } }
            };

            string nameOut = "";
            int lenght = 0;
            lenght = random.Next(3, 10);

            nameOut += ctrail.ElementAt(random.Next(ctrail.Count)).Key;

            while (nameOut.Length < lenght)
            {
                nameOut += ctrail[nameOut.Last()][random.Next(ctrail[nameOut.Last()].Length)];
            }
            return nameOut;
        }

        public static int SkillCheck(Actor actor)
        {
            return random.Next(1, 21);
        }
    }
}