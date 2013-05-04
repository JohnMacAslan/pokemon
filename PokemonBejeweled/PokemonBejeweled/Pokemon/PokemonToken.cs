using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace PokemonBejeweled.Pokemon
{
    public abstract class PokemonToken : IBasicPokemonToken
    {
        protected Type _firstEvolution;
        protected Type _secondEvolution;
        protected String _pictureLocation;

        public IFirstEvolutionPokemonToken firstEvolvedToken()
        {
            return (IFirstEvolutionPokemonToken)Activator.CreateInstance(_firstEvolution);
        }

        public ISecondEvolutionPokemonToken secondEvolvedToken()
        {
            return (ISecondEvolutionPokemonToken)Activator.CreateInstance(_secondEvolution);
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

        public ImageBrush getPokemonPicture()
        {
            Uri image = null;
            Uri shader = null;
            try
            {
                image = new Uri(System.Reflection.Assembly.GetEntryAssembly().Location);
                shader = new Uri(image, _pictureLocation);
            }
            catch (ArgumentNullException e)
            {
                System.Console.WriteLine(e.Message);
            }
            catch (UriFormatException f)
            {
                System.Console.WriteLine(f.Message);
            }
            catch
            {
                System.Console.WriteLine("whoah somethin different");
            }


            BitmapImage bitImage = null;
            try
            {
                bitImage = new BitmapImage(shader);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            ImageBrush background = new ImageBrush(bitImage);
            return background;
        }

       

    }
}