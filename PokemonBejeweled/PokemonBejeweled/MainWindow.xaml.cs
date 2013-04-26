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
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameState gameState = new GameState();
        private System.Windows.Controls.Primitives.UniformGrid gridBoard;
        
        public MainWindow()
        {
            InitializeComponent();
            gridBoard = this.GridBoard;
            setUpGridBoard();
            newGame();
            NewGameButton.Click += delegate { newGame(); };
            QuitGameButton.Click += delegate { this.Close(); };
            this.MouseRightButtonDown += delegate
            {
                DependencyObject scope = FocusManager.GetFocusScope(this);
                FocusManager.SetFocusedElement(scope, this);
            };
        }

        private void newGame()
        {
            gameState.newGame();
            updateGridBoard();
            gameState.Grid.BoardDirtied += new BoardDirtiedEventHandler(delegate { updateGridBoard(); });
        }

        private void setUpGridBoard()
        {
            double buttonHeight = gridBoard.Height / PokemonBoard.gridSize;
            double buttonWidth = gridBoard.Width / PokemonBoard.gridSize;
            for(int row = 0; row < PokemonBoard.gridSize; row++)
            {
                for(int col = 0; col < PokemonBoard.gridSize; col++)
                {
                    GridButton newButton = new GridButton(gameState, row, col);
                    newButton.Height = buttonHeight;
                    newButton.Width = buttonWidth;
                    gridBoard.Children.Add(newButton);
                }
            }
        }

        private void updateGridBoard()
        {
            GridButton currentButton;
            System.Collections.IEnumerator buttonEnumerator = gridBoard.Children.GetEnumerator();
            for (int r = 0; r < PokemonBoard.gridSize; r++)
            {
                for (int c = 0; c < PokemonBoard.gridSize; c++)
                {
                    buttonEnumerator.MoveNext();
                    currentButton = (GridButton) buttonEnumerator.Current;
                    if (null == gameState.Grid.PokemonGrid[r, c])
                    {
                        currentButton.Background = Brushes.Black;
                    }
                    else
                    {
                        currentButton.Background = (gameState.Grid.PokemonGrid[r, c].getPokemonPicture());
                    }
                }
            }
            
            DependencyObject scope = FocusManager.GetFocusScope(this);
            FocusManager.SetFocusedElement(scope, this);
            gridBoard.Dispatcher.Invoke(delegate()
            {
                System.Threading.Thread.Sleep(500);
            }, System.Windows.Threading.DispatcherPriority.Render);
        }       
    }
}
