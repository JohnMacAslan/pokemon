namespace PokemonBejeweled.Pokemon
{
    public class TotodileToken : PokemonToken
    {
        public TotodileToken()
        {
            _firstEvolution = typeof(CroconawToken);
            _secondEvolution = typeof(FeraligatorToken);
            _pictureName = "totodile";
        }
    }

    public class CroconawToken : TotodileToken, IFirstEvolutionPokemonToken
    {
        public CroconawToken()
        {
            _pictureName = "croconaw";
        }
    }

    public class FeraligatorToken : TotodileToken, ISecondEvolutionPokemonToken
    {
        public FeraligatorToken()
        {
            _pictureName = "feraligator";
        }
    }
}
