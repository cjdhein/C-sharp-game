using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Location
    {
        //required variables
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //additional variables
        public Item ItemRequiredToEnter { get; set; }
        public Quest QuestAvailableHere { get; set; }
        public Monster MonsterHere { get; set; }
        public Location LocToNorth { get; set; }
        public Location LocToEast { get; set; }
        public Location LocToSouth { get; set; }
        public Location LocToWest { get; set; }

        //constructor
        public Location(int id, string name, string description,
            Item itemRequiredToEnter = null,
                Quest questAvailableHere = null,
                    Monster monsterHere = null)
        {
            ID = id;
            Name = name;
            Description = description;

            ItemRequiredToEnter = itemRequiredToEnter;
            QuestAvailableHere = questAvailableHere;
            MonsterHere = monsterHere;
        }
    }
}
