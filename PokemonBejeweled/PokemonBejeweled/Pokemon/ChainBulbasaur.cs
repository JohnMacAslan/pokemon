namespace PokemonBejeweled.Pokemon
{
    public class BulbasaurToken : PokemonToken
    {
        public BulbasaurToken()
        {
            _firstEvolution = typeof(IvysaurToken);
            _secondEvolution = typeof(VenusaurToken);
            _pictureName = "bulbasaur";
        }
    }

    public class IvysaurToken : BulbasaurToken, IFirstEvolutionPokemonToken
    {
        public IvysaurToken()
        {
            _pictureName = "ivysaur";
        }
    }

    public class VenusaurToken : BulbasaurToken, ISecondEvolutionPokemonToken
    {
        public VenusaurToken()
        {
            _pictureName = "venusaur.JPG";
        }
    }
}
