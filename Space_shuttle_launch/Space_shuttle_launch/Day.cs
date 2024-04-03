using System;
using System.Collections.Generic;
using System.Text;

namespace Space_shuttle_launch
{
    class Day
    {
        public int number;
        public int temperature;
        public int wind;
        public int humidity;
        public int precipitation;
        public bool lightning;

        //for the clouds criteria: lowering the letters to cover all possible ways of typing
        private string _cloud;
        public string clouds
        {
            get
            {
                return _cloud;
            }
            set
            {
                _cloud=value.ToLower();
            }
        }
    }
}
