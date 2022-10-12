using GameLibrary.Classes.Items;
using GameLibrary.Enums;
using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;

namespace GameLibrary.Classes
{
    public class Game
    {
        public Player Player { get; set; }
        public World World { get; set; }
        public bool GameOver { get; set; }
        public bool GameWon { get; set; }

        /// <summary>
        /// Returns praise if the player has won the game and words of encouragment otherwise.
        /// </summary>
        public string GameOverMessage
        {
            get
            {
                return GameWon
                    ? "Congratulations, you beat the game!"
                    : "There's no defeat, in truth, save from within;\nunless you're beaten there, you're bound to win!";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="world"></param>
        public Game(Player player, World world)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player), "Without a player, there is no game...");
            World = world ?? throw new ArgumentNullException(nameof(world), "It would be rather empty around here without a World");
        }

        /// <summary>
        /// Called when player uses an item on another item. Calls UsedWith on necessary objects if applicable.
        /// </summary>
        /// <param name="keywords">List of keywords the user has entered.</param>
        /// <returns>A description of the effect of the user's action.</returns>
        public string Use(List<string> keywords)
        {
            // gebruiker heeft enkel "use" getypt, we vragen wat verduidelijking
            if (keywords.Count == 0)
                return "Use what?";

            // gebruiker heeft geen tweede item gespecifeerd, in dat geval eerste item toevoegen aan de keywords lijst en proberen het item op zichzelf te gebruiken
            if (keywords.Count == 1)
                keywords.Add(keywords[0]);

            // item1 opzoeken in Inventory, item2 in de kamer
            IItem firstItem = FindItemInInventory(keywords[0]);
            IItem secondItem = FindItemInRoom(keywords[1], World.CurrentRoom);

            // eerste item zit niet in de Inventory van de speler, foutboodschap teruggeven
            if (firstItem == null)
                return $"I don't have {keywords[0]} with me.";

            // tweede item is niet gevonden in de kamer
            if (secondItem == null)
            {
                // proberen tweede item terug te vinden in de Inventory, indien niet gevonden, foutboodschap teruggeven
                secondItem = FindItemInInventory(keywords[1]);
                if(secondItem == null)
                    return $"There's no {keywords[1]} here.";
            }

            // eerste item kan enkel gebruikt worden als het van het type IUsable is, anders foutboodschap teruggeven
            if (!(firstItem is IUsable))
                return "I don't think that's a good idea.";

            // volgende stuk code (tussen de switch) is de enige plaats waar we effectief naar het type van de class zelf moeten casten
            switch (firstItem.GetType().Name)
            {
                case "Key":
                    // "use key on [...]", checken of tweede item Combinable is met klasse Key
                    if (secondItem is ICombinableWith<Key>)
                    {
                        // Key can gebruikt worden met ons tweede item, UsedWith aanroepen
                        string message = (secondItem as ICombinableWith<Key>).UsedWith(firstItem as Key);
                        
                        if (secondItem is Door)
                        {
                            // als het tweede item de Door is, even kijken of de speler het heeft open gekregen, m.a.w. einde spel & speler heeft gewonnen.
                            GameOver = GameWon = (secondItem as Door).IsOpened;

                        }
                        return message;
                    }
                    break;
                case "Can":
                    // "use can on [...]", checken of tweede item Combinable is met klasse Can en indien ja, UsedWith uitvoeren en boodschap teruggeven
                    if (secondItem is ICombinableWith<Can>)
                        return (secondItem as ICombinableWith<Can>).UsedWith(firstItem as Can);
                    break;
                case "Beer":
                    // "use beer on [...]", checken of tweede item Combinable is met klasse Beer
                    if (secondItem is ICombinableWith<Beer>)
                    {
                        Beer beer = firstItem as Beer;
                        string message = (secondItem as ICombinableWith<Beer>).UsedWith(beer);

                        // het is mogelijk om "use beer" te typen, wat intern zal geïnterpreteerd worden als "use beer on beer", in het verhaal van het spel betekend dit dat de speler
                        // zal drinken van het beer en potentiëel te veel drinkt (zie ook Beer klasse)
                        if(beer.PlayerDrankTooMuch)
                        {
                            GameOver = true;
                        }
                        return message;
                    }
                    break;
                default:
                    // Item klasse is niet gekend
                    return "I don't want to use that right now.";
            }

            // tweede item is niet combinable met eerste item
            return $"I can't use {keywords[0]} on that.";
        }


        /// <summary>
        /// Finds item in current room and adds it to Player inventory if it is ITakeable
        /// </summary>
        /// <param name="itemName">The item to find.</param>
        /// <returns>The TakeMessage of the object or an error message if it can't be picked up or it is not in the current room.</returns>
        public string Take(string itemName)
        {
            // zoek naar item in huidige kamer
            IItem item = FindItemInRoom(itemName, World.CurrentRoom);

            // indien item gevonden
            if (item != null)
            {
                // indien item van het type ITakeable is, mogen we het ook effectief meenemen
                if (item is ITakeable)
                {
                    // item uit huidige kamer halen, toevoegen aan Inventory van de speler en TakeMessage weergeven
                    World.CurrentRoom.Items.Remove(item);
                    Player.Inventory.Add(item);
                    return (item as ITakeable).TakeMessage();
                }
                else
                {
                    // item is niet van het type ITakeable en mag dus niet worden opgepakt
                    return "I don't want that.";
                }
            }
            else
            {
                // item niet aanwezig in huidige kamer
                return $"There's no {itemName} here.";
            }
        }

        /// <summary>
        /// Finds item in current room or inventory and returns look message when found.
        /// </summary>
        /// <param name="itemName">The item to find.</param>
        /// <returns>If <paramref name="itemName"/> is "inventory", displays a list of inventory items. If not, returns the LookMessage of the object or a generic message if object was not found or is not <see cref="ILookable"/>.</returns>
        public string Look(string itemName)
        {
            // "look at inventory"
            if (itemName.Contains("inventory"))
                return string.Format("You are currently carrying: {0}", string.Join(", ", Player.Inventory));

            IItem roomItem;
            // nakijken of het item in de huidige kamer, dan wel in de Inventory van de speler zit. Als beide functies null teruggeven, is het item niet gevonden, indien wel, opslagen in roomItem
            if((roomItem = FindItemInRoom(itemName, World.CurrentRoom)) == null && (roomItem = FindItemInInventory(itemName)) == null)
            {
                // item niet gevonden in kamer of inventory
                return "I don't see that anywhere.";
            }
            else
            {
                // als het item ILookable is, casten naar ILookable en LookMessage teruggeven, anders generische boodschap teruggeven
                return roomItem is ILookable ? ((ILookable)roomItem).LookMessage() : $"That's the second biggest {itemName} I've ever seen!";
            }
        }

        /// <summary>
        /// Checks if there's an exit in the given <paramref name="direction"/> and moves player to the adjacent room. Returns true if successfull.
        /// </summary>
        /// <param name="direction">A <see cref="Direction"/> value, specifying the direction to go.</param>
        /// <returns>True if move was successfull, false if no such Exit exists in current Room.</returns>
        public bool Move(Direction direction)
        {
            if(World.CurrentRoom.Exits.TryGetValue(direction, out Room room))
            {
                World.CurrentRoom = room;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Looks for an item with a certain name in a specific Room
        /// </summary>
        /// <param name="itemName">Name of the item to find.</param>
        /// <param name="room"><see cref="Room"/> in which to find the item.</param>
        /// <returns>The <see cref="IItem"/> with corresponding name, null if not found.</returns>
        private static IItem FindItemInRoom(string itemName, Room room)
        {
            //// alternatieve oplossing met LINQ-query:
            // return room.Items.Where(item => item.Names.Contains(itemName)).FirstOrDefault();

            foreach (IItem roomItem in room.Items)
            {
                if (roomItem.Names.Contains(itemName))
                {
                    return roomItem;
                }
            }

            return null;
        }

        /// <summary>
        /// Looks for an item in the player's inventory
        /// </summary>
        /// <param name="itemName">Name of the item to find.</param>
        /// <returns>An <see cref="IItem"/> with corresponding name, null if not found.</returns>
        private IItem FindItemInInventory(string itemName)
        {
            foreach (IItem item in Player.Inventory)
            {
                if(item.Names.Contains(itemName))
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Converts a string with a direction to a <see cref="Direction"/> enum value.
        /// </summary>
        /// <param name="directionName">Name of the direction.</param>
        /// <returns>A <see cref="Direction"/> enum value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="directionName"/> does not correspond with a valid <see cref="Direction"/>.</exception>
        public static Direction StringToDirection(string directionName)
        {
            switch (directionName)
            {
                case "up":
                case "north":
                    return Direction.North;
                case "right":
                case "east":
                    return Direction.East;
                case "down":
                case "south":
                    return Direction.South;
                case "left":
                case "west":
                    return Direction.West;
                default:
                    throw new ArgumentOutOfRangeException(nameof(directionName));
            }
        }
    }
}
