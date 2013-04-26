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
        private int _score = 0;
        private GameState gameState;
        private System.Windows.Controls.Primitives.UniformGrid gridBoard;
        private Dictionary<Type, Brush> _tokenColors = tokenColors();
        private Boolean inMove;
        private int previousRow;
        private int previousColumn;
        
        public MainWindow()
        {
            InitializeComponent();
            gameState = new GameState();
            gridBoard = this.GridBoard;
            gameState.Grid.BoardDirtied += new BoardDirtiedEventHandler(delegate { updateGridBoard(); });
            inMove = false;
            _score = 0;
            previousColumn = 0;
            previousRow = 0;
            setUpGridBoard();
            NewGameButton.Click += delegate
            {
                _score = 0;
                inMove = false;
                gameState.newGame();
                updateGridBoard();
                gameState.Grid.BoardDirtied += new BoardDirtiedEventHandler(delegate { updateGridBoard(); });
            };
            QuitGameButton.Click += delegate { this.Close(); };
        }

        public void setUpGridBoard()
        {
            for(int r = 0; r < PokemonBoard.gridSize; r++)
            {
                for(int c = 0; c < PokemonBoard.gridSize; c++)
                {
                    GridButton newButton = new GridButton(r, c);
                    newButton.Height = gridBoard.Height / PokemonBoard.gridSize;
                    newButton.Width = gridBoard.Width / PokemonBoard.gridSize;
                    newButton.Click += delegate
                    {
                        if (!inMove)
                        {
                            inMove = true;
                            previousRow = newButton.row;
                            previousColumn = newButton.column;
                        }
                        else
                        {
                            inMove = false;
                            _score += gameState.Grid.makePlay(newButton.row, newButton.column, previousRow, previousColumn);
                        }
                    };
                    gridBoard.Children.Add(newButton);
                }
            }
            updateGridBoard();
        }

        public void updateGridBoard()
        {
            GridButton currentButton;
            System.Collections.IEnumerator buttonEnumerator = gridBoard.Children.GetEnumerator();
            for (int r = 0; r < PokemonBoard.gridSize; r++)
            {
                for (int c = 0; c < PokemonBoard.gridSize; c++)
                {
                    buttonEnumerator.MoveNext();
                    currentButton = (GridButton)buttonEnumerator.Current;
                    if (null == gameState.Grid.PokemonGrid[r, c])
                    {
                        currentButton.setBackgroundColor(Brushes.Black);
                    }
                    else
                    {
                        currentButton.Background = (gameState.Grid.PokemonGrid[r, c].getPokemonPicture());
                        //currentButton.setBackgroundColor(tokenColors[gameState.Grid.Pokemon[r, c].GetType()]);
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

        private static Dictionary<Type, Brush> tokenColors() {
            Dictionary<Type, Brush> tokenColors = new Dictionary<Type, Brush>();
            tokenColors.Add(typeof(BulbasaurToken), Brushes.MediumSeaGreen);
            tokenColors.Add(typeof(IvysaurToken), Brushes.MediumSeaGreen);
            tokenColors.Add(typeof(VenusaurToken), Brushes.MediumSeaGreen);
            tokenColors.Add(typeof(CharmanderToken), Brushes.Orange);
            tokenColors.Add(typeof(CharmeleonToken), Brushes.Orange);
            tokenColors.Add(typeof(CharizardToken), Brushes.Orange);
            tokenColors.Add(typeof(SquirtleToken), Brushes.SkyBlue);
            tokenColors.Add(typeof(WartortleToken), Brushes.SkyBlue);
            tokenColors.Add(typeof(BlastoiseToken), Brushes.SkyBlue);
            tokenColors.Add(typeof(PichuToken), Brushes.Yellow);
            tokenColors.Add(typeof(PikachuToken), Brushes.Yellow);
            tokenColors.Add(typeof(RaichuToken), Brushes.Yellow);
            tokenColors.Add(typeof(TotodileToken), Brushes.CadetBlue);
            tokenColors.Add(typeof(CroconawToken), Brushes.CadetBlue);
            tokenColors.Add(typeof(FeraligatorToken), Brushes.CadetBlue);
            tokenColors.Add(typeof(ChikoritaToken), Brushes.LightGreen);
            tokenColors.Add(typeof(BayleefToken), Brushes.LightGreen);
            tokenColors.Add(typeof(MeganiumToken), Brushes.LightGreen);
            tokenColors.Add(typeof(CyndaquilToken), Brushes.OrangeRed);
            tokenColors.Add(typeof(QuilavaToken), Brushes.OrangeRed);
            tokenColors.Add(typeof(TyphlosionToken), Brushes.OrangeRed);
            tokenColors.Add(typeof(DittoToken), Brushes.Pink);
            return tokenColors;
        }
       
    }
}
