using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Classes.Items
{
    public class Door : IItem, ILookable, ICombinableWith<Key>, ICombinableWith<Can>
    {
        public List<string> Names => new List<string> { "door", "oak door", "massive door", "massive oak door" };
        public bool IsOpened { get; set; }
        
        private bool isLubricated;

        public string LookMessage()
        {
            return "A massive oak door in the middle of a forest clearing. It's got a large keyhole covered with cobwebs.";
        }

        public string UsedWith(Key item)
        {
            if(isLubricated)
            {
                IsOpened = true;                
                return "The key fits perfectly. With great force, you slowly turn the key. A low, rumbling creak emits from the old door as the hinges sigh under it's enormous weight. It slowly, but steadely, opens...";
            }
            else
            {
                return "The key fits perfectly, but the hinges are al rusted up. I couldn't possibly open it.";
            }
        }

        public string UsedWith(Can can)
        {
            if(can.IsEmpty)
            {
                return "You press the sprayhead, but the can seems to be empty.";
            }
            else
            {
                isLubricated = true;
                can.IsEmpty = true;
                return "You spray a generous amount of lubricant on the doors hinges. This seems to remove some of the rust.";
            }            
        }

        //public string UsedWith(IUsable item)
        //{
        //    return "That does not have any effect on the door.";
        //}

        public override string ToString()
        {
            return "Oak door";
        }


    }
}
