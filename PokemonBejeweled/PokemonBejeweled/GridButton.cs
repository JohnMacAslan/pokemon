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
            get { return _row; }
        }
        private int _column;
        public int Column
        {
            get { return _column; }
        }
        private GameState _gameState;

        /// <summary>
        /// Constructs a new GridButton with the given GameState, row, and column. 
        /// </summary>
        /// <param name="gameState">The state that this button belongs to. </param>
        /// <param name="row">The row that this button is in. </param>
        /// <param name="column">The column that this button is in. </param>
        public GridButton(GameState gameState, int row, int column)
        {
            this._row = row;
            this._column = column;
            this._gameState = gameState;
        }

        /// <summary>
        /// When a button is clicked, the GameState it belongs to attempts to make a play. 
        /// </summary>
        protected override void OnClick()
        {
            _gameState.makePlay(_row, _column);
        }
    }
}
