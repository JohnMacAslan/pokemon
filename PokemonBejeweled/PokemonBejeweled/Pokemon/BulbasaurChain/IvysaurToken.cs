﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PokemonBejeweled.Pokemon
{
    public class IvysaurToken : BulbasaurToken, IFirstEvolutionPokemonToken
    {
        public IvysaurToken()
        {
            _pictureLocation = "Pokemon/Pictures/ivysaur.JPG";
        }
    }
}
