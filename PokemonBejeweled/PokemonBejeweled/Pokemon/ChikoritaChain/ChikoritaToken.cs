using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PokemonBejeweled.Pokemon
{
    public class ChikoritaToken : PokemonToken
    {
        public ChikoritaToken()
        {
            _firstEvolution = typeof(BayleefToken);
            _secondEvolution = typeof(MeganiumToken);
            _pictureName = "chikorita";          
        }
    }
}