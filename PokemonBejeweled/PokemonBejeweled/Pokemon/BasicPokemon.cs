using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    abstract class BasicPokemon
    {
        protected File image;

        public abstract BasicPokemon evolve();

        public Ditto becomeDitto()
        {
            return null;
        }
    }
}
