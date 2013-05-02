using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PokemonBejeweled;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;


namespace PokemonBejeweledTest
{
    [TestFixture()]
    public class GameBoardGUITest
    {
        
        [Test]
        public void TestButtonLocation()
        {
            GridButton testButton = new GridButton(0, 0);
            int getColumn = testButton.column;
            int getRow = testButton.row;
            Assert.AreEqual(0, getColumn);
            Assert.AreEqual(0, getRow);
        }

    }
}
