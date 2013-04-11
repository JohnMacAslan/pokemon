using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public class ChikoritaToken : PokemonToken
    {
        public ChikoritaToken()
        {
            firstEvolution = typeof(BayleefToken);
            secondEvolution = typeof(MeganiumToken);
        }
    }
}