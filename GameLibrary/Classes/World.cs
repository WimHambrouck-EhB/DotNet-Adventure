using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Classes
{
    public class World
    {
        public Room CurrentRoom { get; set; }

        /// <summary>
        /// Sets the <see cref="CurrentRoom"/> (i.e.: the room the player will be in at the start of the game)
        /// </summary>
        /// <param name="startingRoom">The <see cref="Room"/> to set.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="startingRoom"/> is null.</exception>
        public World(Room startingRoom)
        {
            CurrentRoom = startingRoom ?? throw new ArgumentNullException(nameof(startingRoom));
        }
    }
}
