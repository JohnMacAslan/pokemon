using System;

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
