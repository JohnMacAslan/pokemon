﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PokemonBejeweled.Pokemon
{
    public class SquirtleToken : PokemonToken
    {
        public SquirtleToken()
        {
            firstEvolution = typeof(WartortleToken);
            secondEvolution = typeof(BlastoiseToken);

            pictureLocation = "Pokemon/Pictures/squirtle.jpg";            
        
        }
    }
}