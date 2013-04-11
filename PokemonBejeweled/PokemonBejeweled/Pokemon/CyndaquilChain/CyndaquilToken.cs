using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public class CyndaquilToken : PokemonToken
    {
        public CyndaquilToken()
        {
            firstEvolution = typeof(QuilavaToken);
            secondEvolution = typeof(TyphlosionToken);
        }
    }
}