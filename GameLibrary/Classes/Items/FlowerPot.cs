using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Classes.Items
{
    public class FlowerPot : Item, ILookable
    {
        public override List<string> Names => new List<string> { "flower pot", "pot", "ornate pot", "ornate flower pot" };

        public bool LookedAt { get; private set; }

        public string LookMessage(Room currentRoom)
        {
            string message = "A terracotta pot that hasn't been cleaned for some time. It's got a lovely flower in it, though.";

            if (!LookedAt)
            {
                message += Environment.NewLine + "Wait a minute, there's a key hidden behind it!";
                currentRoom.Items.Add(new Key());
            }

            return message;
        }

        public override string ToString()
        {
            return "Flower pot";
        }
    }
}
