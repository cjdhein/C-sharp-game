using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Weapon : Item
    {
        //public variables
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }

        //constructor
        public Weapon(int id, string name, string namePlural,
            int minDamage, int maxDamage) : base(id, name, namePlural)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }
    }
}
