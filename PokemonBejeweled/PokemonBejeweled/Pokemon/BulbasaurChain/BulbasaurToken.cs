﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PokemonBejeweled.Pokemon
{
    public class BulbasaurToken : PokemonToken
    {
        public BulbasaurToken()
        {
            _firstEvolution = typeof(IvysaurToken);
            _secondEvolution = typeof(VenusaurToken);
            _pictureLocation = "Pokemon/Pictures/bulbasaur.JPG";
        }
    }
}