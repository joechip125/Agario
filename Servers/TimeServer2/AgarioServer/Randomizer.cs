using System;
using System.Security.Cryptography;

namespace AgarioServer
{
    public class Randomizer : Random
    {
        private static Randomizer rand;
        public static Randomizer Instance
        {
            get
            {
                rand ??= new Randomizer();
                return rand;
            }
        }
        
        
        
        
        
    }
}