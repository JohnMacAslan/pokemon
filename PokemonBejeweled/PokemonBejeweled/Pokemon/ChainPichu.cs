namespace PokemonBejeweled.Pokemon
{
    public class PichuToken : PokemonToken
    {
        public PichuToken()
        {
            _firstEvolution = typeof(PikachuToken);
            _secondEvolution = typeof(RaichuToken);
            _pictureName = "pichu";
        }
    }

    public class PikachuToken : PichuToken, IFirstEvolutionPokemonToken
    {
        public PikachuToken()
        {
            _pictureName = "pikachu";
        }
    }

    public class RaichuToken : PichuToken, ISecondEvolutionPokemonToken
    {
        public RaichuToken()
        {
            _pictureName = "raichu";
        }
    }
}
