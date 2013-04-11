using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public class CharmanderToken : PokemonToken
    {
        public CharmanderToken()
        {
            firstEvolution = typeof(CharmeleonToken);
            secondEvolution = typeof(CharizardToken);
        }
    }
}