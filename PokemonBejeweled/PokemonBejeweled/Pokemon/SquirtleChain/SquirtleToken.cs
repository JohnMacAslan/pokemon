using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public class SquirtleToken : PokemonToken, IBasicPokemonToken
    {

        public SquirtleToken()
        {

        }

        public override IFirstEvolutionPokemonToken firstEvolvedToken()
        {
            return new WartortleToken();
        }

        public override ISecondEvolutionPokemonToken secondEvolvedToken()
        {
            return new BlastoiseToken();
        }
    }
}
