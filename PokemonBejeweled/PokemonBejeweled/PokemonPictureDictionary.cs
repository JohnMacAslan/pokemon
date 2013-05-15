using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    abstract class PokemonPictureDictionary
    {
        private static ResourceManager _pictureManager = new ResourceManager("PokemonBejeweled.PokemonPictures", Assembly.GetExecutingAssembly());
        private static Dictionary<Type, ImageBrush> _pictureDictionary = pokemonPictureDictionary();
        
        private static Dictionary<Type, ImageBrush> pokemonPictureDictionary()
        {
            Dictionary<Type, ImageBrush> dict = new Dictionary<Type, ImageBrush>();
            dict.Add(typeof(BulbasaurToken), getBrushFromString("bulbasaur"));
            dict.Add(typeof(IvysaurToken), getBrushFromString("ivysaur"));
            dict.Add(typeof(VenusaurToken), getBrushFromString("venusaur"));
            dict.Add(typeof(CharmanderToken), getBrushFromString("charmander"));
            dict.Add(typeof(CharmeleonToken), getBrushFromString("charmeleon"));
            dict.Add(typeof(CharizardToken), getBrushFromString("charizard"));
            dict.Add(typeof(SquirtleToken), getBrushFromString("squirtle"));
            dict.Add(typeof(WartortleToken), getBrushFromString("wartortle"));
            dict.Add(typeof(BlastoiseToken), getBrushFromString("blastoise"));
            dict.Add(typeof(PichuToken), getBrushFromString("pichu"));
            dict.Add(typeof(PikachuToken), getBrushFromString("pikachu"));
            dict.Add(typeof(RaichuToken), getBrushFromString("raichu"));
            dict.Add(typeof(CyndaquilToken), getBrushFromString("cyndaquil"));
            dict.Add(typeof(QuilavaToken), getBrushFromString("quilava"));
            dict.Add(typeof(TyphlosionToken), getBrushFromString("typhlosion"));
            dict.Add(typeof(ChikoritaToken), getBrushFromString("chikorita"));
            dict.Add(typeof(BayleefToken), getBrushFromString("bayleef"));
            dict.Add(typeof(MeganiumToken), getBrushFromString("meganium"));
            dict.Add(typeof(TotodileToken), getBrushFromString("totodile"));
            dict.Add(typeof(CroconawToken), getBrushFromString("croconaw"));
            dict.Add(typeof(FeraligatorToken), getBrushFromString("feraligator"));
            dict.Add(typeof(DittoToken), getBrushFromString("ditto"));
            dict.Add(typeof(PokeballToken), getBrushFromString("pokeball"));
            return dict;
        }

        private static ImageBrush getBrushFromString(string pokemon)
        {
            try
            {
                Bitmap bitmap = (Bitmap)_pictureManager.GetObject(pokemon);
                BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                return new ImageBrush(bitmapSource);
            }
            catch (Exception)
            {
                return new ImageBrush();
            }
        }

        public static ImageBrush getImageBrush(IBasicPokemonToken pokemon)
        {
            return _pictureDictionary[pokemon.GetType()];
        }
    }
}
