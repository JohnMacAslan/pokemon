using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PokemonBejeweled.Pokemon
{
    public class CharmanderToken : PokemonToken
    {
        public CharmanderToken()
        {
            _firstEvolution = typeof(CharmeleonToken);
            _secondEvolution = typeof(CharizardToken);
            _pictureName = "charmander";
        }
    }
}