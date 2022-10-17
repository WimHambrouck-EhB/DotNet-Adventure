using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;

namespace GameLibrary.Classes
{
    public class Player
    {
        public string Name { get; set; }
        public List<Item> Inventory { get; set; }

        public Player(string name, List<Item> startInventory)
        {
            Name = name;
            Inventory = startInventory;
        }

        public Player(string name) : this(name, new List<Item>())
        {
            
        }

    }
}
