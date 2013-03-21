using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    abstract class BasicPokemonToken : IPokemonToken
    {
        protected File image;

        public abstract BasicPokemonToken evolve();

        public DittoPokemonToken becomeDitto()
        {
            return null;
        }
    }
}
