using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public class CharmanderToken : PokemonToken
    {
         public CharmanderToken()
        {
            //image = null;
        }

        public override IFirstEvolutionPokemonToken firstEvolvedToken()
        {
            return new CharmeleonToken();
        }

        public override ISecondEvolutionPokemonToken secondEvolvedToken()
        {
            return new CharizardToken();
        }
    }
}
