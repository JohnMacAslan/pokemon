using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public class CyndaquilToken : PokemonToken
    {
        public CyndaquilToken()
        {

        }

        public override IFirstEvolutionPokemonToken firstEvolvedToken()
        {
            return new QuilavaToken();
        }

        public override ISecondEvolutionPokemonToken secondEvolvedToken()
        {
            return new TyphlosionToken();
        }

    }
}
