using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PokemonBejeweled.Pokemon
{
    public interface IBasicPokemonToken
    {
        Type Species();
        bool isSameSpecies(IBasicPokemonToken pokemonToken);
        IFirstEvolutionPokemonToken firstEvolvedToken();
        ISecondEvolutionPokemonToken secondEvolvedToken();        
    }
}
