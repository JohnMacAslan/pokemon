using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;


namespace PokemonBejeweled
{

    

    public class GridButton : Button
    {
        private int _row;
        public int row
        {
            get
            {
                return _row;
            }
        }
        private int _column;
        public int column
        {
            get
            {
                return _column;
            }
        }

        public GridButton(int row, int column)
        {
            this._row = row;
            this._column = column;

        }

        public void setBackgroundColor(Brush color)
        {
            this.Background = color;
        }

    }
}
