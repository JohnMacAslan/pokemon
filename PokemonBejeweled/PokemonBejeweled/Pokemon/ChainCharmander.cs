namespace PokemonBejeweled.Pokemon
{
    public class CharmanderToken : PokemonToken
    {
        public CharmanderToken()
        {
            _firstEvolution = typeof(CharmeleonToken);
            _secondEvolution = typeof(CharizardToken);
            _pictureName = "charmander";
        }
    }

    public class CharmeleonToken : CharmanderToken, IFirstEvolutionPokemonToken
    {
        public CharmeleonToken()
        {
            _pictureName = "charmeleon";
        }
    }

    public class CharizardToken : CharmanderToken, ISecondEvolutionPokemonToken
    {
        public CharizardToken()
        {
            _pictureName = "charizard";
        }
    }
}
