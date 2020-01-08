using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure
{
    class Item
    {
        string name = "ITEM";
        int value = 0;
        int quantity = 1;

        protected ItemType itemtype = ItemType.Generic;

        public enum ItemType
        {
            Generic,
            Consumable,
            Weapon,
            Armor
        }

        public Item(string nameSet, int valueSet)
        {
            name = nameSet;
            value = valueSet;
        }
    }

    class Item_Equipable : Item
    {
        int strengt = 0;
        int endurance = 0;
        int dexsterity = 0;

        int wisdom = 0;
        int intelligence = 0;
        int charisma = 0;

        public Item_Equipable(string nameSet, int valueSet, int strSet, int endSet, int dexSet, int wisSet, int intSet, int chaSet) : base(nameSet, valueSet)
        {
            strengt = strSet;
            endurance = endSet;
            dexsterity = dexSet;

            wisdom = wisSet;
            intelligence = intSet;
            charisma = chaSet;
        }
    }

    class Item_Consumable : Item_Equipable
    {
        int durnation = 1;

        public Item_Consumable(string nameSet, int valueSet, int strSet, int endSet, int dexSet, int wisSet, int intSet, int chaSet) : base(nameSet, valueSet, strSet, endSet, dexSet, wisSet, intSet, chaSet)
        {

        }
    }

    class Item_Weapon : Item_Equipable
    {
        public Item_Weapon(string nameSet, int valueSet, int strSet, int endSet, int dexSet, int wisSet, int intSet, int chaSet) : base(nameSet, valueSet, strSet, endSet, dexSet, wisSet, intSet, chaSet)
        {

        }
    }

    class Item_Armor : Item_Equipable
    {
        public Item_Armor(string nameSet, int valueSet, int strSet, int endSet, int dexSet, int wisSet, int intSet, int chaSet) : base(nameSet, valueSet, strSet, endSet, dexSet, wisSet, intSet, chaSet)
        {

        }
    }
}
