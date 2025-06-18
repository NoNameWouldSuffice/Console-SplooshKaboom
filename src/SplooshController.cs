using System.Runtime.InteropServices;

namespace Sploosh_Console
{
    class SplooshController()
    {
        static ConsoleKeyInfo cki;
        static GameBoard board = new(8, 8);
        public static void Main()
        {
            // Explicitly set output encoding to unicode for windows platforms.
            // This stops some characters in the display from rendering incorrectly as '?' on Windows.
            // See issue #16
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.OutputEncoding = System.Text.Encoding.Unicode;
            }

            SplooshView boardView = new(board);
            board.PlaceShipsRandomly();
            Console.Clear();


            boardView.homeRow = Console.CursorTop;
            boardView.homeCol = Console.CursorLeft;

            Console.CursorVisible = false;

            bool fire;
            while (!GameWon())
            {
                boardView.Update();
                fire = false;

                while (!fire)
                {
                    cki = Console.ReadKey();
                    switch (cki.Key)
                    {
                        case ConsoleKey.RightArrow:
                            boardView.boardCursorCol = Math.Min(board.Width - 1, boardView.boardCursorCol + 1);
                            break;

                        case ConsoleKey.LeftArrow:
                            boardView.boardCursorCol = Math.Max(0, boardView.boardCursorCol - 1);
                            break;

                        case ConsoleKey.UpArrow:
                            boardView.boardCursorRow = Math.Max(0, boardView.boardCursorRow - 1);
                            break;

                        case ConsoleKey.DownArrow:
                            boardView.boardCursorRow = Math.Min(board.Height - 1, boardView.boardCursorRow + 1);
                            break;

                        case ConsoleKey.Spacebar:
                            fire = true;
                            break;

                        default:
                            break;
                    }

                    boardView.Update();
                }
                board.FireShot(boardView.boardCursorRow, boardView.boardCursorCol);
            }

            // GameWon() is now true - game has been finished.

            boardView.showShips = true;
            boardView.showCursor = false;
            boardView.Update();
            Console.WriteLine("Game won. 'Grats man.");

            // Wait for the user to press a key to quit
            // This is to stop the window from closing as immediately when the game finishes.
            Console.WriteLine("Press any key to quit...");
            Console.ReadKey(true);
        }

        private static bool GameWon()
        {
            foreach (Ship ship in board.GetShips())
            {
                if (ship.IsAlive(board.GetShotMap))
                {
                    return false;
                }
            }

            return true;
        }
    }
}