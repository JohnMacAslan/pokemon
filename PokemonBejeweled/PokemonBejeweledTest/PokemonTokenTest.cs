using NUnit.Framework;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweledTest
{
    [TestFixture()]
    public class PokemonTokenTest
    {
        private BulbasaurToken bulbasaur = new BulbasaurToken();
        private IvysaurToken ivysaur = new IvysaurToken();
        private VenusaurToken venusaur = new VenusaurToken();
        private CharmanderToken charmander = new CharmanderToken();
        private CharmeleonToken charmeleon = new CharmeleonToken();
        private CharizardToken charizard = new CharizardToken();
        private SquirtleToken squirtle = new SquirtleToken();
        private WartortleToken wartortle = new WartortleToken();
        private BlastoiseToken blastoise = new BlastoiseToken();
        private PichuToken pichu = new PichuToken();
        private PikachuToken pikachu = new PikachuToken();
        private RaichuToken raichu = new RaichuToken();
        private ChikoritaToken chikorita = new ChikoritaToken();
        private BayleefToken bayleef = new BayleefToken();
        private MeganiumToken meganium = new MeganiumToken();
        private CyndaquilToken cyndaquil = new CyndaquilToken();
        private QuilavaToken quilava = new QuilavaToken();
        private TyphlosionToken typhlosion = new TyphlosionToken();
        private TotodileToken totodile = new TotodileToken();
        private CroconawToken croconaw = new CroconawToken();
        private FeraligatorToken feraligator = new FeraligatorToken();

        [Test]
        public void FirstEvolvedToken_BulbasaurChain_ReturnsNewIvysaur()
        {
            Assert.IsInstanceOf<IvysaurToken>(bulbasaur.firstEvolvedToken());
            Assert.IsInstanceOf<IvysaurToken>(ivysaur.firstEvolvedToken());
            Assert.IsInstanceOf<IvysaurToken>(venusaur.firstEvolvedToken());
        }

        [Test]
        public void SecondEvolvedToken_BulbasaurChain_ReturnsNewVenusaur()
        {
            Assert.IsInstanceOf<VenusaurToken>(bulbasaur.secondEvolvedToken());
            Assert.IsInstanceOf<VenusaurToken>(ivysaur.secondEvolvedToken());
            Assert.IsInstanceOf<VenusaurToken>(venusaur.secondEvolvedToken());
        }

        [Test]
        public void FirstEvolvedToken_CharmanderChain_ReturnsNewCharmelon()
        {
            Assert.IsInstanceOf<CharmeleonToken>(charmander.firstEvolvedToken());
            Assert.IsInstanceOf<CharmeleonToken>(charmeleon.firstEvolvedToken());
            Assert.IsInstanceOf<CharmeleonToken>(charizard.firstEvolvedToken());
        }

        [Test]
        public void SecondEvolvedToken_CharmanderChain_ReturnsNewCharizard()
        {
            Assert.IsInstanceOf<CharizardToken>(charmander.secondEvolvedToken());
            Assert.IsInstanceOf<CharizardToken>(charmeleon.secondEvolvedToken());
            Assert.IsInstanceOf<CharizardToken>(charizard.secondEvolvedToken());
        }

        [Test]
        public void FirstEvolvedToken_ChikoritaChain_ReturnsNewBayleef()
        {
            Assert.IsInstanceOf<BayleefToken>(chikorita.firstEvolvedToken());
            Assert.IsInstanceOf<BayleefToken>(bayleef.firstEvolvedToken());
            Assert.IsInstanceOf<BayleefToken>(meganium.firstEvolvedToken());
        }

        [Test]
        public void SecondEvolvedToken_ChikoritaChain_ReturnsNewMeganium()
        {
            Assert.IsInstanceOf<MeganiumToken>(chikorita.secondEvolvedToken());
            Assert.IsInstanceOf<MeganiumToken>(bayleef.secondEvolvedToken());
            Assert.IsInstanceOf<MeganiumToken>(meganium.secondEvolvedToken());
        }

        [Test]
        public void FirstEvolvedToken_CyndaquilChain_ReturnsNewQuilava()
        {
            Assert.IsInstanceOf<QuilavaToken>(cyndaquil.firstEvolvedToken());
            Assert.IsInstanceOf<QuilavaToken>(quilava.firstEvolvedToken());
            Assert.IsInstanceOf<QuilavaToken>(typhlosion.firstEvolvedToken());
        }

        [Test]
        public void SecondEvolvedToken_CyndaquilChain_ReturnsNewTyphlosion()
        {
            Assert.IsInstanceOf<TyphlosionToken>(cyndaquil.secondEvolvedToken());
            Assert.IsInstanceOf<TyphlosionToken>(quilava.secondEvolvedToken());
            Assert.IsInstanceOf<TyphlosionToken>(typhlosion.secondEvolvedToken());
        }

        [Test]
        public void FirstEvolvedToken_PichuChain_ReturnsNewPikachu()
        {
            Assert.IsInstanceOf<PikachuToken>(pichu.firstEvolvedToken());
            Assert.IsInstanceOf<PikachuToken>(pikachu.firstEvolvedToken());
            Assert.IsInstanceOf<PikachuToken>(raichu.firstEvolvedToken());
        }

        [Test]
        public void SecondEvolvedToken_PichuChain_ReturnsNewRaichu()
        {
            Assert.IsInstanceOf<RaichuToken>(pichu.secondEvolvedToken());
            Assert.IsInstanceOf<RaichuToken>(pikachu.secondEvolvedToken());
            Assert.IsInstanceOf<RaichuToken>(raichu.secondEvolvedToken());
        }

        [Test]
        public void FirstEvolvedToken_SquirtleChain_ReturnsNewWartortle()
        {
            Assert.IsInstanceOf<WartortleToken>(squirtle.firstEvolvedToken());
            Assert.IsInstanceOf<WartortleToken>(wartortle.firstEvolvedToken());
            Assert.IsInstanceOf<WartortleToken>(blastoise.firstEvolvedToken());
        }

        [Test]
        public void SecondEvolvedToken_SquirtleChain_ReturnsNewBlastoise()
        {
            Assert.IsInstanceOf<BlastoiseToken>(squirtle.secondEvolvedToken());
            Assert.IsInstanceOf<BlastoiseToken>(wartortle.secondEvolvedToken());
            Assert.IsInstanceOf<BlastoiseToken>(blastoise.secondEvolvedToken());
        }

        [Test]
        public void FirstEvolvedToken_TotodileChain_ReturnsNewCroconaw()
        {
            Assert.IsInstanceOf<CroconawToken>(totodile.firstEvolvedToken());
            Assert.IsInstanceOf<CroconawToken>(croconaw.firstEvolvedToken());
            Assert.IsInstanceOf<CroconawToken>(feraligator.firstEvolvedToken());
        }

        [Test]
        public void SecondEvolvedToken_TotodileChain_ReturnsNewFeraligator()
        {
            Assert.IsInstanceOf<FeraligatorToken>(totodile.secondEvolvedToken());
            Assert.IsInstanceOf<FeraligatorToken>(croconaw.secondEvolvedToken());
            Assert.IsInstanceOf<FeraligatorToken>(feraligator.secondEvolvedToken());
        }
    }
}
