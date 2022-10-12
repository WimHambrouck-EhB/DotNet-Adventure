using GameLibrary.Enums;
using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Classes
{
    public class Room
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<IItem> Items { get; set; }
        public Dictionary<Direction, Room> Exits { get; set; }

        /// <summary>
        /// Default ctor, initializes Exits Dictionary and Items List with empty collections.
        /// </summary>
        public Room()
        {
            Exits = new Dictionary<Direction, Room>();
            Items = new List<IItem>();
        }

        /// <summary>
        /// Adds an exit to a <paramref name="room"/> associated with a <paramref name="direction"/>. Also updates the inverse direction in the other room.
        /// (e.g: when updating room X's <see cref="Direction.North"/> with room Y, room Y's <see cref="Direction.South"/> will automatically be updated to point to room X)
        /// </summary>
        /// <param name="direction">A <see cref="Direction"/> that specifies the location of the exit.</param>
        /// <param name="room">The <see cref="Room"/> the Player will go to when using the exit.</param>
        public void AddRoom(Direction direction, Room room)
        {
            Exits.Add(direction, room);
            room.Exits.Add(GetInverseDirection(direction), this);
        }

        /// <summary>
        /// Inverts the given direction.
        /// </summary>
        /// <param name="direction">The <see cref="Direction"/> to be inverted.</param>
        /// <returns>The opposite of the given direction.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="direction"/> is not a valid <see cref="Direction"/></exception>
        private static Direction GetInverseDirection(Direction direction)
        {
            // als de cases van een switch niet veel code bevatten, kan je vanaf C# 8 de switch expression gebruiken en het resultaat meteen returnen
            // cf.: https://www.c-sharpcorner.com/article/c-sharp-8-0-new-feature-swtich-expression/
            // en https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#switch-expressions
            return direction switch
            {
                Direction.North => Direction.South,
                Direction.East => Direction.West,
                Direction.South => Direction.North,
                Direction.West => Direction.East,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Enum value not recognized"),
            };

            /* Ter referentie: dezelfde switch, maar op de traditionele manier */
            //switch (direction)
            //{
            //    case Direction.North:
            //        return Direction.South;
            //    case Direction.East:
            //        return Direction.West;
            //    case Direction.South:
            //        return Direction.North;
            //    case Direction.West:
            //        return Direction.East;
            //    default:
            //        throw new ArgumentOutOfRangeException(nameof(direction), direction, "Enum value not recognized");
            //}
        }

        /// <summary>
        /// Returns the name and the description of the room in the following format:
        /// 
        /// ROOM NAME
        /// Description of the room.
        /// </summary>
        /// <returns>The name and the description of the room.</returns>
        public override string ToString()
        {
            return $"{Name.ToUpperInvariant()}\n{Description}";
        }
    }
}
