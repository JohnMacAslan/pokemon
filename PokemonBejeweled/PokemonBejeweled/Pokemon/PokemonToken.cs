using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public abstract class PokemonToken
    {
        protected File image;

        public abstract IFirstEvolutionPokemonToken firstEvolvedToken();

        public abstract ISecondEvolutionPokemonToken secondEvolvedToken();

        public DittoPokemonToken becomeDitto()
        {
            return null;
        }

        public File getTokenImage()
        {
            return image;
        }
    }
}
