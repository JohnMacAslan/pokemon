﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PokemonBejeweled.Pokemon{

    public class WartortleToken : SquirtleToken, IFirstEvolutionPokemonToken
    {
        public WartortleToken()
        {
            _pictureLocation = "Pokemon/Pictures/wartortle.JPG";            
        }
    }
}
