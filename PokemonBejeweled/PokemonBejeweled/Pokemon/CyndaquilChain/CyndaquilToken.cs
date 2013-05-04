using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PokemonBejeweled.Pokemon
{
    public class CyndaquilToken : PokemonToken
    {
        public CyndaquilToken()
        {
            _firstEvolution = typeof(QuilavaToken);
            _secondEvolution = typeof(TyphlosionToken);
            _pictureLocation = "Pokemon/Pictures/cyndaquil.JPG";
        }
    }
}