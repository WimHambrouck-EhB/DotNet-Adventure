using System;
using GameLibrary.Classes;
using GameLibrary.Classes.Items;
using System.Collections.Generic;
using GameLibrary.Interfaces;
using WE03.Properties;
using System.Globalization;
using static AdventureLib.Parser;
using GameLibrary.Enums;

namespace WE03
{
    static class Program
    {
        private static Game game;

        private const ConsoleColor higlightColor = ConsoleColor.White;
        private const ConsoleColor badColor = ConsoleColor.Red;
        private const ConsoleColor goodColor = ConsoleColor.Green;
        private const ConsoleColor neutralColor = ConsoleColor.Blue;

        static void Main()
        {
            TextInfo textInfo = new CultureInfo("nl-BE", true).TextInfo;
            game = InitGame(ReadStringFromConsole("Before you embark on your grand adventure; what is your name? "));
            int headerHeight;
            int prevLeft, prevTop;

            Console.Clear();

            // game loop: zolang spel niet beëindigd is, huidige kamer weergeven en invoer vragen aan de speler
            while (!game.GameOver)
            {
                prevLeft = Console.CursorLeft;
                prevTop = Console.CursorTop;

                // kamerinfo weergeven bovenaan scherm
                string lijntje = new string('=', game.World.CurrentRoom.Description.Length);
                headerHeight = 5 + game.World.CurrentRoom.ToString().Length / Console.WindowWidth;
                ClearLine(0, headerHeight);
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(lijntje);
                Console.WriteLine(game.World.CurrentRoom);
                Console.WriteLine(lijntje);

                // als er items in de kamer liggen, een lijstje aan de gebruiker laten zien
                if (game.World.CurrentRoom.Items?.Count > 0)
                {
                    string items = $"You can see: {string.Join(", ", game.World.CurrentRoom.Items)}";
                    ClearLine(headerHeight, 1 + items.Length / Console.WindowWidth);
                    Console.ForegroundColor = neutralColor;
                    Console.WriteLine(items);
                    Console.ResetColor();
                }
                
                Console.WriteLine();

                if (prevTop != 0)
                    Console.SetCursorPosition(prevLeft, prevTop + 1);

                // invoer vragen aan de gebruiker en dit doorgeven aan de parser
                string command = ReadStringFromConsole($"Ok, {textInfo.ToTitleCase(game.Player.Name)}, what next? ");
                Console.ForegroundColor = higlightColor;
                CommandType commandType = ParseCommand(command, out List<string> keywords);
                switch (commandType)
                {
                    case CommandType.Undefined:
                        // parser heeft het commando niet begrepen
                        if (keywords.Contains("help"))
                        {
                            // voor het geval dat de speler hulp vraagt, lijst van beschikbare commando's teruggeven
                            Console.WriteLine("Available commands: use, take, look, move, exit");
                        }
                        else
                        {
                            Console.WriteLine($"I don't know what you mean by that.");
                        }
                        break;
                    case CommandType.Use:
                        Console.WriteLine(game.Use(keywords));
                        break;
                    case CommandType.Take:
                        // als er geen keywords zijn, heeft de gebruiker gewoon "take" getypt. In dat geval verduidelijking vragen, anders Take methode aanroepen in Game.
                        Console.WriteLine((keywords.Count == 0) ? "Take what?" : game.Take(keywords[0]));
                        break;
                    case CommandType.Look:
                        // als er geen keywords zijn, heeft de gebruiker gewoon "look" getypt. In dat geval verduidelijking vragen, anders Look methode aanroepen in Game.
                        Console.WriteLine((keywords.Count == 0) ? "Look at what?" : game.Look(keywords[0]));
                        break;
                    case CommandType.Move:
                        if (keywords.Count == 0)
                        {
                            // gebruiker heeft enkel "move" getypt, even verduidelijking vragen.
                            Console.WriteLine("Move to where? (North, East, South or West)");
                        }
                        else
                        {
                            try
                            {
                                if (!game.Move(Game.StringToDirection(keywords[0])))
                                {
                                    // huidige kamer heeft geen Exit in de gevraagde Direction
                                    Console.WriteLine("There's nothing there...");
                                }
                                else
                                {
                                    // game.Move() update automatisch de huidige kamer, dus scherm leegmaken en op naar de volgende iteratie van de while-lus
                                    Console.Clear();
                                }
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                // als StringToDirection een exception gooit, is dat omdat de gebruiker een ongeldige richting heeft ingevoerd
                                Console.WriteLine("That's not a valid direction!");
                            }
                        }
                        break;
                    case CommandType.Exit:
                        // gebruiker wilt het spel verlaten
                        game.GameOver = true;
                        break;
                    default:
                        // zou niet mogen voorvallen, maar voor het geval dat er een CommandType wordt teruggegeven dat we niet kennen, wat debug info weergeven.
                        Console.WriteLine("ERROR: Unknown {0}: {1}. Keywords: {2}", commandType.GetType().Name, commandType, string.Join(", ", keywords));
                        break;
                }
                Console.ResetColor();
            }

            Console.ForegroundColor = (game.GameWon) ? goodColor : badColor;
            Console.WriteLine(game.GameOverMessage);
            Console.ResetColor();
        }

        /// <summary>
        /// Initiates the Items, Rooms, World and Player objects needed during gameplay.
        /// </summary>
        /// <param name="playerName">Name of the player.</param>
        /// <returns>A populated <see cref="Game"/>, ready for use.</returns>
        private static Game InitGame(string playerName)
        {
            Beer beer = new Beer();
            Player player = new Player(playerName, new List<IItem> { beer });

            Room townCentre = new Room { Name = "Town Centre", Description = "The centre of town. To the left is the local bar and up ahead is the forest." };

            Room forest = new Room { Name = "Forest", Description = "A dark dense forest, lined with Sycamore trees. A clearing in the middle reveals a strange wooden door..." };
            forest.Items = new List<IItem> { new Door() };

            Room bar = new Room { Name = "Bar", Description = "The sleaziest, grubbiest bar you've ever seen." };
            bar.Items = new List<IItem> { new Flower(), new FlowerPot(), new Key() };

            Room seaside = new Room { Name = "Seaside", Description = "A beautiful beach and a sky-blue sea." };
            seaside.Items = new List<IItem> { new Can() };

            townCentre.AddRoom(Direction.North, forest);
            townCentre.AddRoom(Direction.West, bar);
            townCentre.AddRoom(Direction.South, seaside);

            World world = new World(townCentre);
            return new Game(player, world);
        }

        /// <summary>
        /// Outputs <paramref name="message"/> to the console and asks for input as long as the user's input is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="message">The message to display in the console</param>
        /// <returns>A string containing the user's input.</returns>
        private static string ReadStringFromConsole(string message)
        {
            string input;
            bool inputIsInvalid;
            do
            {
                Console.Write(message);
                Console.ForegroundColor = higlightColor;
                input = Console.ReadLine();
                inputIsInvalid = string.IsNullOrWhiteSpace(input);
                if (inputIsInvalid)
                {
                    Console.ForegroundColor = badColor;
                    Console.WriteLine("That is not a valid input.\n");
                }
                Console.ResetColor();
            } while (inputIsInvalid);

            return input;
        }

        /// <summary>
        /// Clears (i.e.: writes empty string of length <see cref="Console.BufferWidth"/>) the content of <paramref name="amount"/> of lines in the console starting from given position.
        /// </summary>
        /// <param name="top">The row position to start.</param>
        /// <param name="amount">The amount of lines to clear.</param>
        private static void ClearLine(int top, int amount)
        {
            int origLeft = Console.CursorLeft;
            int origTop = Console.CursorTop;

            Console.SetCursorPosition(0, top);

            for (int i = 0; i < amount; i++)
            {
                Console.Write(new string(' ', Console.BufferWidth));
            }

            Console.SetCursorPosition(origLeft, origTop);
        }
    }
}
