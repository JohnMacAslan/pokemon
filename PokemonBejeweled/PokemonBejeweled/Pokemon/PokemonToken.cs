using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBejeweled.Pokemon
{
    public abstract class PokemonToken : IBasicPokemonToken
    {
        protected Type firstEvolution;
        protected Type secondEvolution;
        public enum EvolutionLevel
        {
            BASIC,
            FIRST,
            SECOND
        };

        public virtual IFirstEvolutionPokemonToken firstEvolvedToken()
        {
            return (IFirstEvolutionPokemonToken)Activator.CreateInstance(firstEvolution);
        }

        public virtual ISecondEvolutionPokemonToken secondEvolvedToken()
        {
            return (ISecondEvolutionPokemonToken)Activator.CreateInstance(secondEvolution);
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == GetType();
        }

        public bool isSameSpecies(IBasicPokemonToken pokemonToken)
        {
            return this.Species() == pokemonToken.Species();
        }

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
        
        public bool isEvolutionLevel(EvolutionLevel level)
        {
            switch (level)
            {
                case EvolutionLevel.SECOND:
                    return this.GetType().GetInterfaces().Contains(typeof(ISecondEvolutionPokemonToken));
                case EvolutionLevel.FIRST:
                    return this.GetType().GetInterfaces().Contains(typeof(IFirstEvolutionPokemonToken));
                case EvolutionLevel.BASIC:
                    return this.GetType().GetInterfaces().Contains(typeof(IBasicPokemonToken));
                default:
                    return false;
            }
        }
    }
}