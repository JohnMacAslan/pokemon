using System;

namespace PokemonBejeweled.Pokemon
{
    public abstract class PokemonToken : IBasicPokemonToken
    {
        protected Type _firstEvolution;
        protected Type _secondEvolution;
        protected static String _pictureName;
        
        /// <summary>
        /// Returns an instance of the first evolutionary form of a species of pokemon
        /// </summary>
        public IFirstEvolutionPokemonToken firstEvolvedToken()
        {
            return (IFirstEvolutionPokemonToken)Activator.CreateInstance(_firstEvolution);
        }

        /// <summary>
        /// Returns an instance of the second evolutionary form of a species of pokemon
        /// </summary>
        public ISecondEvolutionPokemonToken secondEvolvedToken()
        {
            return (ISecondEvolutionPokemonToken)Activator.CreateInstance(_secondEvolution);
        }

        /// <summary>
        /// Checks if two PokemonTokens are equal by comparing their type. 
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj.GetType() == GetType();
        }

        /// <summary>
        /// Checks to see if a PokemonToken is in the same evolution chain as another PokemonToken
        /// </summary>
        public bool isSameSpecies(IBasicPokemonToken pokemonToken)
        {
            return this.Species() == pokemonToken.Species();
        }

        /// <summary>
        /// Returns the type of the zeroeth evolutionary form of a PokemonToken. 
        /// </summary>
        public Type Species()
        {
            if (this.GetType().BaseType == typeof(PokemonToken))
            {
                return this.GetType();
            }
            else
            {
                return this.GetType().BaseType;
            }
        }
    }
}