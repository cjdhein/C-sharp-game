using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class LivingCreature //base class
    {
        //public variables
        public int MaxHitPoints { get; set; }
        public int CurHitPoints { get; set; }

        //constructor
        public LivingCreature(int maxHitpoints, int curHitpoints)
        {
            MaxHitPoints = maxHitpoints;
            CurHitPoints = curHitpoints;
        }
    }
}
