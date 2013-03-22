using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PokemonBejeweled;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweledTest
{
    [TestFixture()]
    public class PokemonTokenTest
    {

        private readonly BulbasaurToken bulba = new BulbasaurToken();
        private readonly CharmanderToken charm = new CharmanderToken();
        private readonly SquirtleToken squirt = new SquirtleToken();
        private readonly PichuToken pichu = new PichuToken();
        private readonly ChikoritaToken chik = new ChikoritaToken();
        private readonly CyndaquilToken cynda = new CyndaquilToken();
        private readonly TotodileToken toto = new TotodileToken();

        [Test()]
        public void TestThatBulbasaurTokenCreatesFirstEvolvedToken()
        {
            Assert.AreEqual((new IvysaurToken()).GetType(), bulba.firstEvolvedToken().GetType());
           
        }

        [Test()]
        public void TestThatBulbasaurTokenCreatesSecondEvolvedToken()
        {
            Assert.AreEqual((new VenusaurToken()).GetType(), bulba.secondEvolvedToken().GetType());

        }

        [Test()]
        public void TestThatCharmanderTokenCreatesFirstEvolvedToken()
        {
            Assert.AreEqual((new CharmeleonToken()).GetType(), charm.firstEvolvedToken().GetType());

        }

        [Test()]
        public void TestThatCharmanderTokenCreatesSecondEvolvedToken()
        {
            Assert.AreEqual((new CharizardToken()).GetType(), charm.secondEvolvedToken().GetType());

        }

        [Test()]
        public void TestThatSquirtleTokenCreatesFirstEvolvedToken()
        {
            Assert.AreEqual((new WartortleToken()).GetType(), squirt.firstEvolvedToken().GetType());

        }

        [Test()]
        public void TestThatSquirtleTokenCreatesSecondEvolvedToken()
        {
            Assert.AreEqual((new BlastoiseToken()).GetType(), squirt.secondEvolvedToken().GetType());

        }

        [Test()]
        public void TestThatPichuTokenCreatesFirstEvolvedToken()
        {
            Assert.AreEqual((new PikachuToken()).GetType(), pichu.firstEvolvedToken().GetType());

        }

        [Test()]
        public void TestThatPichuTokenCreatesSecondEvolvedToken()
        {
            Assert.AreEqual((new RaichuToken()).GetType(), pichu.secondEvolvedToken().GetType());

        }


        
    }
}
