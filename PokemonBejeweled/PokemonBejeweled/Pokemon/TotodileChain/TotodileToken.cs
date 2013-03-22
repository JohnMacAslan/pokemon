using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public class TotodileToken : PokemonToken, IBasicPokemonToken
    {
        public TotodileToken()
        {
        
        }

        public override IFirstEvolutionPokemonToken firstEvolvedToken()
        {
            return new CroconawToken();
        }

        public override ISecondEvolutionPokemonToken secondEvolvedToken()
        {
            return new FeraligatorToken();
        }
    }
}
