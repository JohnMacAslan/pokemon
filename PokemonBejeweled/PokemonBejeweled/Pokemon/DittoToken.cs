using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PokemonBejeweled.Pokemon
{
    public class DittoToken : PokemonToken
    {
        public DittoToken()
        {
            pictureLocation = "Pokemon/Pictures/ditto.JPG";  
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
