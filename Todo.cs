using System.Collections.Generic;
using System;

namespace Rest
{
    public class Todo : Activity
    {
         public int id { get; set; }
    }
    public class Activity
    {
        public string activity { get; set; }
        public string keterangan { get; set; }
        public bool status { get; set; } = false;
    }

    public class RootObject
    {
        public List<Todo> todo { get; set; }
    }
}