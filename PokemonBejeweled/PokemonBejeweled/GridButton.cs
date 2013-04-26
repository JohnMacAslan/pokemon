using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;


namespace PokemonBejeweled
{

    

    public class GridButton : Button
    {
        private int _row;
        public int Row
        {
            get
            {
                return _row;
            }
        }
        private int _column;
        public int Column
        {
            get
            {
                return _column;
            }
        }
        private GameState _gameState;

        public GridButton(GameState gameState, int row, int column)
        {
            this._row = row;
            this._column = column;
            this._gameState = gameState;
        }

        protected override void OnClick()
        {
            _gameState.makePlay(_row, _column);
        }
    }
}
