namespace PokemonBejeweled.Pokemon
{
    public class SquirtleToken : PokemonToken
    {
        public SquirtleToken()
        {
            _firstEvolution = typeof(WartortleToken);
            _secondEvolution = typeof(BlastoiseToken);
            _pictureName = "squirtle";
        }
    }

    public class WartortleToken : SquirtleToken, IFirstEvolutionPokemonToken
    {
        public WartortleToken()
        {
            _pictureName = "wartortle";
        }
    }

    public class BlastoiseToken : SquirtleToken, ISecondEvolutionPokemonToken
    {
        public BlastoiseToken()
        {
            _pictureName = "blastoise";
        }
    }
}
