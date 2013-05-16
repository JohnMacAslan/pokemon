namespace PokemonBejeweled.Pokemon
{
    public class CyndaquilToken : PokemonToken
    {
        public CyndaquilToken()
        {
            _firstEvolution = typeof(QuilavaToken);
            _secondEvolution = typeof(TyphlosionToken);
            _pictureName = "cyndaquil";
        }
    }

    public class QuilavaToken : CyndaquilToken, IFirstEvolutionPokemonToken
    {
        public QuilavaToken()
        {
            _pictureName = "quilava";
        }
    }

    public class TyphlosionToken : CyndaquilToken, ISecondEvolutionPokemonToken
    {
        public TyphlosionToken()
        {
            _pictureName = "typhlosion";
        }
    }
}
