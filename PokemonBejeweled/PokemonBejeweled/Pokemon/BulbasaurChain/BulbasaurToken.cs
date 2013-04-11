﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public class BulbasaurToken : PokemonToken
    {
        public BulbasaurToken()
        {
            firstEvolution = typeof(IvysaurToken);
            secondEvolution = typeof(VenusaurToken);
        }        
    }
}