﻿using System;
using PokemonBejeweled.Pokemon;
using PokemonBejeweled;
using NUnit.Framework;
using Rhino.Mocks;

namespace PokemonBejeweledTest
{
    [TestFixture()]
    public class PokemonBoardTests
    {
        private IBasicPokemonToken[,] _pokemonGrid = new IBasicPokemonToken[PokemonBoard.gridSize, PokemonBoard.gridSize];
        private PokemonBoard _pokemonBoard;
        private MockRepository _mocks;
        private PokemonBoard _mockBoard;

        [SetUp]
        public void setupMocks()
        {
            _mocks = new MockRepository();
            _mockBoard = _mocks.PartialMock<PokemonBoard>();
        }

        [SetUp]
        public void resetPokemonGrid()
        {
            int tokenToAdd = 0;
            _pokemonBoard = new PokemonBoard();
            for (int row = 0; row < PokemonBoard.gridSize; row++) {
                for (int col = 0; col < PokemonBoard.gridSize; col++)
                {
                    _pokemonGrid[row, col] = (PokemonToken)Activator.CreateInstance(PokemonBoard.TokenDict[tokenToAdd++ % 6 + 1]);
                }
            }
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonBoard.NewPokemonGrid = _pokemonGrid;
        }

        [Test]
        public void PokemonGrid_NoError_GridInitializedToIBasicPokemon()
        {
            _pokemonBoard = new PokemonBoard();
            for (int row = 0; row < PokemonBoard.gridSize; row++)
            {
                for (int col = 0; col < PokemonBoard.gridSize; col++)
                {
                    Assert.IsInstanceOf(typeof(IBasicPokemonToken), _pokemonGrid[row, col]);
                }
            }
        }

        [Test]
        public void PokemonGrid_NoError_PokemonAndNewPokemonAreEqual()
        {
            _pokemonBoard = new PokemonBoard();
            Assert.AreEqual(_pokemonBoard.PokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void PiecesAreAdjacent_PiecesAreHorizontallyAdjacent_ReturnTrue()
        {
            Assert.IsTrue(_pokemonBoard.piecesAreAdjacent(1, 1, 1, 2));
        }

        [Test]
        public void PiecesAreAdjacent_PiecesAreVerticallyAdjacent_ReturnTrue()
        {
            Assert.IsTrue(_pokemonBoard.piecesAreAdjacent(1, 2, 1, 1));
        }

        [Test]
        public void PiecesAreAdjacent_PiecesAreNotAdjacent_ReturnFalse()
        {
            Assert.IsFalse(_pokemonBoard.piecesAreAdjacent(1, 1, 2, 2));
        }

        [Test]
        public void StartPlay_CallUpdateSingleRowOnFirstPoint()
        {
            int rowOne = 0;
            int colOne = 0;
            int rowTwo = 1;
            int colTwo = 1;
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.updateSingleRow(rowOne, colOne)).Return(false);
            _mockBoard.Replay();
            _mockBoard.startPlay(rowOne, colOne, rowTwo, colTwo);
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void StartPlay_CallUpdateSingleRowOnSecondPoint()
        {
            int rowOne = 0;
            int colOne = 0;
            int rowTwo = 1;
            int colTwo = 1;
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.updateSingleRow(rowTwo, colTwo)).Return(false);
            _mockBoard.Replay();
            _mockBoard.startPlay(rowOne, colOne, rowTwo, colTwo);
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void StartPlay_CallUpdateSingleColumnOnFirstPoint()
        {
            int rowOne = 0;
            int colOne = 0;
            int rowTwo = 1;
            int colTwo = 1;
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.updateSingleColumn(rowOne, colOne)).Return(false);
            _mockBoard.Replay();
            _mockBoard.startPlay(rowOne, colOne, rowTwo, colTwo);
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void StartPlay_CallUpdateSingleColumnOnSecondPoint()
        {
            int rowOne = 0;
            int colOne = 0;
            int rowTwo = 1;
            int colTwo = 1;
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.updateSingleColumn(rowTwo, colTwo)).Return(false);
            _mockBoard.Replay();
            _mockBoard.startPlay(rowOne, colOne, rowTwo, colTwo);
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void StartPlay_CallSwapDitto()
        {
            int rowOne = 0;
            int colOne = 0;
            int rowTwo = 1;
            int colTwo = 1;
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.swapDitto(rowOne, colOne, rowTwo, colTwo)).Return(false);
            _mockBoard.Replay();
            _mockBoard.startPlay(rowOne, colOne, rowTwo, colTwo);
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void StartPlay_InvalidMove_NewPokemonBoardUnchanged()
        {
            int rowOne = 0;
            int colOne = 0;
            int rowTwo = 1;
            int colTwo = 1;
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.updateSingleRow(rowOne, colOne)).IgnoreArguments().Return(false);
            _mockBoard.Expect(g => g.updateSingleColumn(rowOne, colOne)).IgnoreArguments().Return(false);
            _mockBoard.Expect(g => g.swapDitto(rowOne, colOne, rowTwo, colTwo)).IgnoreArguments().Return(false);
            _mockBoard.Replay();
            _mockBoard.startPlay(rowOne, colOne, rowTwo, colTwo);
            Assert.AreEqual(_pokemonGrid[rowOne, colOne], _mockBoard.NewPokemonGrid[rowOne, colOne]);
            Assert.AreEqual(_pokemonGrid[rowTwo, colTwo], _mockBoard.NewPokemonGrid[rowTwo, colTwo]);
        }

        [Test]
        public void StartPlay_ValidMove_FirstAndSecondPointSwappedOnNewPokemonBoard()
        {
            int rowOne = 0;
            int colOne = 0;
            int rowTwo = 0;
            int colTwo = 1;
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.updateSingleRow(rowOne, rowTwo)).Return(true);
            _mockBoard.Replay();
            _mockBoard.startPlay(rowOne, colOne, rowTwo, colTwo);
            Assert.AreEqual(_pokemonGrid[rowOne, colOne], _mockBoard.NewPokemonGrid[rowTwo, colTwo]);
            Assert.AreEqual(_pokemonGrid[rowTwo, colTwo], _mockBoard.NewPokemonGrid[rowOne, colOne]);
        }

        [Test]
        public void MakePlay_CallArePiecesAdjacent()
        {
            int rowOne = 0;
            int colOne = 0;
            int rowTwo = 1;
            int colTwo = 1;
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.piecesAreAdjacent(rowOne, colOne, rowTwo, colTwo)).Return(false);
            _mockBoard.Replay();
            _mockBoard.makePlay(rowOne, colOne, rowTwo, colTwo);
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void MakePlay_PiecesAreAdjacent_CallStartPlay()
        {
            int rowOne = 0;
            int colOne = 0;
            int rowTwo = 1;
            int colTwo = 1;
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.piecesAreAdjacent(rowOne, colOne, rowTwo, colTwo)).Return(true);
            _mockBoard.Expect(g => g.startPlay(rowOne, colOne, rowTwo, colTwo));
            _mockBoard.Replay();
            _mockBoard.makePlay(rowOne, colOne, rowTwo, colTwo);
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void MakePlay_PiecesAreAdjacent_CallUpdateBoard()
        {
            int rowOne = 0;
            int colOne = 0;
            int rowTwo = 1;
            int colTwo = 1;
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.piecesAreAdjacent(rowOne, colOne, rowTwo, colTwo)).Return(true);
            _mockBoard.Expect(g => g.updateBoard());
            _mockBoard.Replay();
            _mockBoard.makePlay(rowOne, colOne, rowTwo, colTwo);
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void UpdateBoard_NoRowsOrColumnsOfThree_GridUnchanged()
        {
            _pokemonBoard.makePlay(0, 0, 0, 0);
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.PokemonGrid);
        }

        [Test]
        public void MarkNullRow_NumberOfSameTokensLessThan3_PokemonGridUnchanged()
        {
            _pokemonBoard.markNullRow(0, 0, 0);
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void MarkNullRow_NumberOfSameTokensGreaterThan3_RowMarkNull()
        {
            _pokemonGrid[0, 0] = null;
            _pokemonGrid[0, 1] = null;
            _pokemonGrid[0, 2] = null;
            _pokemonGrid[0, 3] = null;
            _pokemonBoard.markNullRow(0, 0, 4);
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkNullRow_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            _pokemonBoard.markNullRow(-1, 0, 4);
        }

        [Test]
        public void MarkSurroundingTokensNull_IndicesWithinRange_SurroundingTokensMarkedNull()
        {
            int row = 4;
            int col = 4;
            _pokemonGrid[row - 1, col] = null;
            _pokemonGrid[row - 1, col - 1] = null;
            _pokemonGrid[row - 1, col + 1] = null;
            _pokemonGrid[row, col - 1] = null;
            _pokemonGrid[row, col + 1] = null;
            _pokemonGrid[row + 1, col] = null;
            _pokemonGrid[row + 1, col - 1] = null;
            _pokemonGrid[row + 1, col + 1] = null;
            _pokemonBoard.markSurroundingTokensNull(row, col);
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void MarkSurroundingTokensNull_IndexOnEdge_SurroundingTokensMarkedNull()
        {
            int row = 4;
            int col = 0;
            _pokemonGrid[row - 1, col] = null;
            _pokemonGrid[row - 1, col + 1] = null;
            _pokemonGrid[row, col + 1] = null;
            _pokemonGrid[row + 1, col] = null;
            _pokemonGrid[row + 1, col + 1] = null;
            _pokemonBoard.markSurroundingTokensNull(row, col);
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkSurroundingTokensNull_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            _pokemonBoard.markSurroundingTokensNull(-12, -12);
        }

        [Test]
        public void MarkFullColumnAndRowAsNull_IndicesWithinRange_RowAndColumnMarkedAsNull()
        {
            int row = 4;
            int col = 4;
            for (int currentRow = 0; currentRow < PokemonBoard.gridSize; currentRow++)
            {
                _pokemonGrid[currentRow, col] = null;
            }
            for (int currentCol = 0; currentCol < PokemonBoard.gridSize; currentCol++)
            {
                _pokemonGrid[row, currentCol] = null;
            }
            _pokemonBoard.markFullRowAndColumnAsNull(row, col);
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);            
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkFullColumnAndRowAsNull_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            _pokemonBoard.markFullRowAndColumnAsNull(-12, -12);
        }

        [Test]
        public void MarkAllTokensOfTheSameTypeAsNull_ValidTokenType_TokensMarkedAsNull()
        {
            IBasicPokemonToken bulba = new BulbasaurToken();
            for (int row = 0; row < PokemonBoard.gridSize; row++)
            {
                for (int col = 0; col < PokemonBoard.gridSize; col++)
                {
                    if (_pokemonGrid[row, col].isSameSpecies(bulba))
                    {
                        _pokemonGrid[row, col] = null;
                    }
                }
            }
            _pokemonBoard.markAllTokensOfSameTypeAsNull(bulba);
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void MarkAllTokensOfTheSameTypeAsNull_NoTokensOfGivenType_BoardUnchanged()
        {
            Type type = typeof(TotodileToken);
            _pokemonBoard.markAllTokensOfSameTypeAsNull(new TotodileToken());
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void EvolveToken_RowOfFour_TokenReplacedWithFirstEvolution()
        {
            _pokemonGrid[0, 0] = new TotodileToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[0, 0] = new CroconawToken();
            _pokemonBoard.evolveToken(0, 0, 4);
            Assert.AreEqual(_pokemonGrid[0, 0], _pokemonBoard.NewPokemonGrid[0, 0]);
        }

        [Test]
        public void EvolveToken_RowOfFive_TokenReplacedWithDitto()
        {
            _pokemonGrid[0, 0] = new DittoToken();
            _pokemonBoard.evolveToken(0, 0, 5);
            Assert.AreEqual(_pokemonGrid[0, 0], _pokemonBoard.NewPokemonGrid[0, 0]);
        }

        [Test]
        public void EvolveToken_RowOfSix_TokenReplacedWithSecondEvolution()
        {
            _pokemonGrid[0, 0] = new TotodileToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[0, 0] = new FeraligatorToken();
            _pokemonBoard.evolveToken(0, 0, 6);
            Assert.AreEqual(_pokemonGrid[0, 0], _pokemonBoard.NewPokemonGrid[0, 0]);
        }

        [Test]
        public void SwapDitto_DittoIsFirstToken_MarkAllTokensOfSameTypeAsNullCalled()
        {
            _pokemonGrid[0, 0] = new DittoToken();
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.markAllTokensOfSameTypeAsNull(_pokemonGrid[0, 1]));
            _mockBoard.Replay();
            _mockBoard.swapDitto(0, 0, 0, 1);
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void SwapDitto_DittoIsSecondToken_MarkAllTokensOfSameTypeAsNullCalled()
        {
            _pokemonGrid[0, 1] = new DittoToken();
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.markAllTokensOfSameTypeAsNull(_pokemonGrid[0, 0]));
            _mockBoard.Replay();
            _mockBoard.swapDitto(0, 0, 0, 1);
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void UpdateAllColumns_ColumnOfThreeOnTopEdge_ColumnMarkedAsNull()
        {
            _pokemonGrid[0, 0] = new TotodileToken();
            _pokemonGrid[1, 0] = new TotodileToken();
            _pokemonGrid[2, 0] = new TotodileToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[0, 0] = null;
            _pokemonGrid[1, 0] = null;
            _pokemonGrid[2, 0] = null;
            _pokemonBoard.updateAllColumns();
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllColumns_ColumnOfThreeOnBottomEdge_ColumnMarkedAsNull()
        {
            _pokemonGrid[5, 0] = new TotodileToken();
            _pokemonGrid[6, 0] = new TotodileToken();
            _pokemonGrid[7, 0] = new TotodileToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[5, 0] = null;
            _pokemonGrid[6, 0] = null;
            _pokemonGrid[7, 0] = null;
            _pokemonBoard.updateAllColumns();
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllColumns_ColumnOfThreeInMiddle_ColumnMarkedAsNull()
        {
            _pokemonGrid[3, 0] = new TotodileToken();
            _pokemonGrid[4, 0] = new TotodileToken();
            _pokemonGrid[5, 0] = new TotodileToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[3, 0] = null;
            _pokemonGrid[4, 0] = null;
            _pokemonGrid[5, 0] = null;
            _pokemonBoard.updateAllColumns();
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllRows_RowOfThreeOnLeftEdge_RowMarkedAsNull()
        {
            _pokemonGrid[0, 0] = new TotodileToken();
            _pokemonGrid[0, 1] = new TotodileToken();
            _pokemonGrid[0, 2] = new TotodileToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[0, 0] = null;
            _pokemonGrid[0, 1] = null;
            _pokemonGrid[0, 2] = null;
            _pokemonBoard.updateAllRows();
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllRows_RowOfThreeOnRightEdge_RowMarkedAsNull()
        {
            _pokemonGrid[0, 5] = new TotodileToken();
            _pokemonGrid[0, 6] = new TotodileToken();
            _pokemonGrid[0, 7] = new TotodileToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[0, 5] = null;
            _pokemonGrid[0, 6] = null;
            _pokemonGrid[0, 7] = null;
            _pokemonBoard.updateAllRows();
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllRows_RowOfThreeInMiddle_RowMarkedAsNull()
        {
            _pokemonGrid[0, 3] = new TotodileToken();
            _pokemonGrid[0, 4] = new TotodileToken();
            _pokemonGrid[0, 5] = new TotodileToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[0, 3] = null;
            _pokemonGrid[0, 4] = null;
            _pokemonGrid[0, 5] = null;
            _pokemonBoard.updateAllRows();
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllRows_RowOfFour_MarkSpecialsAndRow()
        {
            _pokemonGrid[0, 0] = new TotodileToken();
            _pokemonGrid[0, 1] = new TotodileToken();
            _pokemonGrid[0, 2] = new TotodileToken();
            _pokemonGrid[0, 3] = new TotodileToken();
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.markNullRow(0, 0, 4));
            _mockBoard.Expect(g => g.evolveToken(0, 0, 4));
            _mockBoard.Replay();
            _mockBoard.updateAllRows();
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void UpdateAllRows_RowOfFive_MarkSpecialsAndRow()
        {
            _pokemonGrid[0, 3] = new TotodileToken();
            _pokemonGrid[0, 4] = new TotodileToken();
            _pokemonGrid[0, 5] = new TotodileToken();
            _pokemonGrid[0, 6] = new TotodileToken();
            _pokemonGrid[0, 7] = new TotodileToken();
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.markNullRow(0, 3, 5));
            _mockBoard.Expect(g => g.evolveToken(0, 3, 5));
            _mockBoard.Replay();
            _mockBoard.updateAllRows();
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void UpdateAllRows_RowOfSix_MarkSpecialsAndRow()
        {
            _pokemonGrid[0, 2] = new TotodileToken();
            _pokemonGrid[0, 3] = new TotodileToken();
            _pokemonGrid[0, 4] = new TotodileToken();
            _pokemonGrid[0, 5] = new TotodileToken();
            _pokemonGrid[0, 6] = new TotodileToken();
            _pokemonGrid[0, 7] = new TotodileToken();
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.markNullRow(0, 2, 6));
            _mockBoard.Expect(g => g.evolveToken(0, 2, 6));
            _mockBoard.Replay();
            _mockBoard.updateAllRows();
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void UpdateAllColumns_NoError_CallMarkRow()
        {
            _mockBoard.Expect(g => g.updateAllRows());
            _mockBoard.Replay();
            _mockBoard.updateAllColumns();
            _mockBoard.VerifyAllExpectations();
        }
        
        [Test]
        public void UpdateSingleRow_RowOfThreeStartFromLeft_NullifyRowAndEvolveToken()
        {
            _pokemonGrid[0, 0] = new TotodileToken();
            _pokemonGrid[0, 1] = new TotodileToken();
            _pokemonGrid[0, 2] = new TotodileToken();
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.markNullRow(0, 0, 3));
            _mockBoard.Expect(g => g.evolveToken(0, 0, 3));
            _mockBoard.Replay();
            _mockBoard.updateSingleRow(0, 0);
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void UpdateSingleRow_RowOfThreeStartFromMiddle_NullifyRowAndEvolveToken()
        {
            _pokemonGrid[0, 0] = new TotodileToken();
            _pokemonGrid[0, 1] = new TotodileToken();
            _pokemonGrid[0, 2] = new TotodileToken();
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.markNullRow(0, 0, 3));
            _mockBoard.Expect(g => g.evolveToken(0, 1, 3));
            _mockBoard.Replay();
            _mockBoard.updateSingleRow(0, 1);
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void UpdateSingleRow_RowOfThreeStartFromRight_NullifyRowAndEvolveToken()
        {
            _pokemonGrid[0, 0] = new TotodileToken();
            _pokemonGrid[0, 1] = new TotodileToken();
            _pokemonGrid[0, 2] = new TotodileToken();
            int pointsAdded = 0;
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.markNullRow(0, 0, 3));
            _mockBoard.Expect(g => g.evolveToken(0, 2, 3));
            _mockBoard.Replay();
            _mockBoard.updateSingleRow(0, 2);
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void UpdateSingleColumn_ValidIndices_InvertGridsUpdateRowInvertBack()
        {
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.updateSingleRow(0, 0)).Return(false);
            _mockBoard.Replay();
            _mockBoard.updateSingleColumn(0, 0);
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void UpdateSingleRow_NoRowOfThree_BoardUnchanged()
        {
            _pokemonBoard.updateSingleRow(0, 0);
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void PullDownTokens_NoError_NoNullsLeftInGrid()
        {
            _pokemonGrid[0, 0] = null;
            _pokemonGrid[2, 3] = new TotodileToken();
            _pokemonGrid[3, 3] = null;
            _pokemonGrid[7, 7] = null;
            _pokemonGrid[6, 7] = new TotodileToken();
            _pokemonBoard.NewPokemonGrid = _pokemonGrid;
            _pokemonBoard.pullDownTokens();
            Assert.IsNotNull(_pokemonBoard.PokemonGrid[0, 0]);
            Assert.AreEqual(_pokemonGrid[2, 3], _pokemonBoard.PokemonGrid[3,3]);
            Assert.AreEqual(_pokemonGrid[6, 7], _pokemonBoard.PokemonGrid[7,7]);
        }

        [Test]
        public void AreMovesLeft_NoMovesLeft_ReturnFalse()
        {
            int row;
            int col;
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            Assert.IsFalse(_pokemonBoard.areMovesLeft(out row, out col));
        }

        [Test]
        public void AreMovesLeft_MovesLeft_ReturnTrue()
        {
            int row;
            int col;
            _pokemonGrid[0,0] = new PichuToken();
            _pokemonGrid[0, 1] = new TotodileToken();
            _pokemonGrid[0, 2] = new TotodileToken();
            _pokemonGrid[0, 1] = new TotodileToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            Assert.IsTrue(_pokemonBoard.areMovesLeft(out row, out col));
            Assert.AreEqual(0, row);
            Assert.AreEqual(0, col);
        }

        [Test]
        public void UpdateToken_FirstLevelEvolution_CallMarkSurroundingTokensNull()
        {
            int row = 0;
            int col = 0;
            _pokemonGrid[row, col] = new PikachuToken();
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.markSurroundingTokensNull(row, col));
            _mockBoard.Replay();
            _mockBoard.updateToken(row, col);
            _mockBoard.VerifyAllExpectations();
        }

        [Test]
        public void UpdateToken_SecondLevelEvolution_CallMarkRowAndColumnNull()
        {
            int row = 0;
            int col = 0;
            _pokemonGrid[row, col] = new RaichuToken();
            _mockBoard.PokemonGrid = _pokemonGrid;
            _mockBoard.Expect(g => g.markFullRowAndColumnAsNull(row, col));
            _mockBoard.Replay();
            _mockBoard.updateToken(row, col);
            _mockBoard.VerifyAllExpectations();
        }
    }
}
