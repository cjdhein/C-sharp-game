﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Item //Base Class
    {
        //public variables
        public int ID { get; set; }
        public string Name { get; set; }
        public string NamePlural { get; set; }

        //constructor

    public Item(int id, string name, string namePlural)
        {
            ID = id;
            Name = name;
            NamePlural = namePlural;
        }
    }
}
