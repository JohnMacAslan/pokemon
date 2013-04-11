using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public class DittoToken : PokemonToken
    {
        public DittoToken()
        {
        }

        public override IFirstEvolutionPokemonToken firstEvolvedToken()
        {
            return null;
        }

        public override ISecondEvolutionPokemonToken secondEvolvedToken()
        {
            return null;
        }
    }
}
