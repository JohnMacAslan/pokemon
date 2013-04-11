using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public class PichuToken : PokemonToken
    {
        public PichuToken()
        {
            firstEvolution = typeof(PikachuToken);
            secondEvolution = typeof(RaichuToken);
        }
    }
}