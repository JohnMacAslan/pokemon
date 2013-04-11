﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public interface IBasicPokemonToken
    {
        IFirstEvolutionPokemonToken firstEvolvedToken();
        ISecondEvolutionPokemonToken secondEvolvedToken();
    }
}
