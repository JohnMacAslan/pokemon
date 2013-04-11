using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public class BulbasaurToken : PokemonToken
    {

        public BulbasaurToken()
        {
            //image = null;
        }

        public override IFirstEvolutionPokemonToken firstEvolvedToken()
        {
            return new IvysaurToken();
        }

        public override ISecondEvolutionPokemonToken secondEvolvedToken()
        {
            return new VenusaurToken();
        }




        
    }
}
