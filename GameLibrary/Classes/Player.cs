using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;

namespace GameLibrary.Classes
{
    public class Player
    {
        public string Name { get; set; }
        public List<IItem> Inventory { get; set; }

        public Player(string name, List<IItem> startInventory)
        {
            Name = name;
            Inventory = startInventory;
        }

        public Player(string name) : this(name, new List<IItem>())
        {
            
        }

    }
}
