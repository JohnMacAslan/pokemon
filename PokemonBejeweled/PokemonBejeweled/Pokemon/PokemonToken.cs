using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    abstract class PokemonToken
    {
        protected File image;

        public abstract PokemonToken evolve();

        public DittoPokemonToken becomeDitto()
        {
            return null;
        }
    }
}
