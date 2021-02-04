using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestKey
{
    class ActiveInput
    {

        public string Name { get; set; }

        public int KeyCode { get; set; }
    
        public ActiveInput(string name)
        {
            Name = name;
        }
    }
}
