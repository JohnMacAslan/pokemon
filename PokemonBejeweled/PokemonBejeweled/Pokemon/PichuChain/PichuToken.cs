using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public class PichuToken : PokemonToken, IBasicPokemonToken
    {

        public PichuToken()
        {

        }

        public override IFirstEvolutionPokemonToken firstEvolvedToken()
        {
            return new PikachuToken();
        }

        public override ISecondEvolutionPokemonToken secondEvolvedToken()
        {
            return new RaichuToken();
        }

    }
}
