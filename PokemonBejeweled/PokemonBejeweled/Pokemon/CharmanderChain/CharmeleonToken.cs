using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PokemonBejeweled.Pokemon
{
    public class CharmeleonToken : CharmanderToken, IFirstEvolutionPokemonToken
    {
        public CharmeleonToken()
        {
            _pictureLocation = "Pokemon/Pictures/charmeleon.JPG";
        }
    }
}
