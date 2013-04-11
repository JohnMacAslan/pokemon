using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PokemonBejeweled
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private GameState gameState;
        private System.Windows.Controls.Primitives.UniformGrid gridBoard;
        
        public MainWindow()
        {
            InitializeComponent();
            gameState = new GameState();
            gridBoard = this.GridBoard;
            setUpGridBoard();
            
        }

        

        public void setUpGridBoard()
        {
            
            for(int r = 0; r < PokemonGrid.gridSize; r++)
            {
                for(int c = 0; c < PokemonGrid.gridSize; c++)
                {
                    GridButton newButton = new GridButton(r, c);
                    //for now this is just filling the board with something
                    Brush color;
                    if((c+(r%2))%2 == 0)
                    {
                       color = Brushes.Yellow;
                    }
                    else
                    {
                       color = Brushes.SkyBlue;
                    }
                    newButton.setBackgroundColor(color);
                    newButton.Height = gridBoard.Height / PokemonGrid.gridSize;
                    newButton.Width = gridBoard.Width / PokemonGrid.gridSize;

                   gridBoard.Children.Add(newButton);
                }
            }
            
        }

        
    }
}
