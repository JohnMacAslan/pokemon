namespace PokemonBejeweled.Pokemon
{
    public class ChikoritaToken : PokemonToken
    {
        public ChikoritaToken()
        {
            _firstEvolution = typeof(BayleefToken);
            _secondEvolution = typeof(MeganiumToken);
            _pictureName = "chikorita";
        }
    }

    public class BayleefToken : ChikoritaToken, IFirstEvolutionPokemonToken
    {
        public BayleefToken()
        {
            _pictureName = "bayleef";
        }
    }

    public class MeganiumToken : ChikoritaToken, ISecondEvolutionPokemonToken
    {
        public MeganiumToken()
        {
            _pictureName = "meganium";
        }
    }
}
