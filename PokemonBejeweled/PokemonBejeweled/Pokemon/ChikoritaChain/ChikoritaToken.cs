using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public class ChikoritaToken : PokemonToken, IBasicPokemonToken
    {
        public ChikoritaToken()
        {
        
        }

        public override IFirstEvolutionPokemonToken firstEvolvedToken()
        {
            return new BayleefToken();
        }

        public override ISecondEvolutionPokemonToken secondEvolvedToken()
        {
            return new MeganiumToken();
        }

    }
}
