using System;
using System.Windows;
using NUnit.Framework;
using PokemonBejeweled;
using Rhino.Mocks;


namespace PokemonBejeweledTest
{
    [TestFixture()]
    public class MainWindowTest
    {
        private MainWindow _mainWindow;

        [SetUp]
        public void resetMainWindow()
        {
            _mainWindow = new MainWindow();
        }

        [Test]
        [STAThread]
        public void ResetTimer_OneMinuteChecked_TimeLeftIsOneMinute()
        {
            _mainWindow.OneMinuteRadio.IsChecked = true;
            _mainWindow.resetTimer();
            Assert.AreEqual(60, _mainWindow.GameState.TimeLeft);
        }

        [Test]
        [STAThread]
        public void ResetTimer_FiveMinuteChecked_TimeLeftIsFiveMinutes()
        {
            _mainWindow.FiveMinuteRadio.IsChecked = true;
            _mainWindow.resetTimer();
            Assert.AreEqual(300, _mainWindow.GameState.TimeLeft);
        }

        [Test]
        [STAThread]
        public void ResetTimer_TenMinuteChecked_TimeLeftIsTenMinutes()
        {
            _mainWindow.TenMinuteRadio.IsChecked = true;
            _mainWindow.resetTimer();
            Assert.AreEqual(600, _mainWindow.GameState.TimeLeft);
        }

        [Test]
        [STAThread]
        public void ResetTimer_UnlimitedChecked_TimeLeftIsUnlimited()
        {
            _mainWindow.UnlimitedRadio.IsChecked = true;
            _mainWindow.resetTimer();
            Assert.AreEqual(GameState.NO_TIME_LIMIT, _mainWindow.GameState.TimeLeft);
        }

        [Test]
        [STAThread]
        public void PauseGame_PausedAlternatesTrueAndFalse()
        {
            Assert.IsFalse(_mainWindow.Paused);
            _mainWindow.pauseGame(_mainWindow, null);
            Assert.True(_mainWindow.Paused);
            _mainWindow.pauseGame(_mainWindow, null);
            Assert.IsFalse(_mainWindow.Paused);
        }

        [Test]
        [STAThread]
        public void Hint_CallsAreMovesLeft()
        {
            MockRepository mocks = new MockRepository();
            GameState gameState = new GameState();
            PokemonBoard mockBoard = mocks.DynamicMock<PokemonBoard>();
            gameState.Board = mockBoard;
            _mainWindow.GameState = gameState;
            int row, col;
            mockBoard.Expect(g => g.areMovesLeft(gameState.CurrentGrid, out row, out col)).Return(false);
            mockBoard.Replay();
            _mainWindow.hint(_mainWindow, null);
            mockBoard.VerifyAllExpectations();
        }
    }
}
