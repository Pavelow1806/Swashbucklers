using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum LootType
    {
        Unknown = 0,
        Grain = 1,
        Fish = 2,
        Oil = 3,
        Wood = 5,
        Brick = 8,
        Iron = 10,
        Rum = 15,
        Silk = 20,
        Silverware = 30,
        Emerald = 50
    }
    public class Item
    {
        public DateTimeOffset Obtained { get; set; } = default;
        public LootType LootName { get; set; }
        public int Count { get; set; }
        public Item(DateTimeOffset obtained, LootType name, int count)
        {
            Obtained=obtained;
            LootName=name;
            Count=count;
        }
    }
}
