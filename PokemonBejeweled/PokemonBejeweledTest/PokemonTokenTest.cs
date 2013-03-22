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

        [Test()]
        public void TestThatBulbasaurTokenCreatesFirstEvolvedToken()
        {
            Assert.AreSame(new IvysaurToken(), bulba.firstEvolvedToken());
           
        }

        [Test()]
        public void TestThatBulbasaurTokenCreatesSecondEvolvedToken()
        {
            Assert.AreSame(new IvysaurToken(), bulba.secondEvolvedToken());

        }

        
    }
}
