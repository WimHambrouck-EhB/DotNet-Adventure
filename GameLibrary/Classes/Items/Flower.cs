﻿using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Classes.Items
{
    public class Flower : Item, ILookable, ICombinableWith<Beer>
    {
        public override List<string> Names => new List<string> { "flower", "white flower", "beautiful flower", "jasmine", "jasmine flower" };
        private bool IsBeered;

        public string LookMessage(Room currentRoom)
        {
            if(IsBeered)
            {
                return "It looks a bit wilted now. And it smells of stale beer.";
            }
            else
            {
                return "A beautiful, pearly white Jasmine is sitting in an ornate pot.";
            }
        }

        public string UsedWith(Beer item)
        {
            if(IsBeered)
            {
                return "I think it's had enough.";
            }
            else
            {
                IsBeered = true;
                return "You empty about half of the beer bottle onto the flower. Watering a plant with beer seems realy unnecessary, not to mention smelly.";
            }
        }

        public override string ToString()
        {
            return "A flower";
        }
    }
}
