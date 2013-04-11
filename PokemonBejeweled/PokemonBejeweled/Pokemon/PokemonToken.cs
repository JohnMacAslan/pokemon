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
    }
}