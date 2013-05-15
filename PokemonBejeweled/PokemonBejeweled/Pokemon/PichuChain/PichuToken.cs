using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PokemonBejeweled.Pokemon
{
    public class PichuToken : PokemonToken
    {
        public PichuToken()
        {
            _firstEvolution = typeof(PikachuToken);
            _secondEvolution = typeof(RaichuToken);
            _pictureName = "pichu"; 
        }
    }
}