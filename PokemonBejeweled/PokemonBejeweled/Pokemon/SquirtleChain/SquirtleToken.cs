using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public class SquirtleToken : PokemonToken
    {
        public SquirtleToken()
        {
            firstEvolution = typeof(WartortleToken);
            secondEvolution = typeof(BlastoiseToken);
        }
    }
}