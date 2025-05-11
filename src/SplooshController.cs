using System.Runtime.InteropServices;

namespace Sploosh_Console
{
    class SplooshController()
    {
        static ConsoleKeyInfo cki;
        static GameBoard board = new(8,8);
        public static void Main()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)){
                Console.OutputEncoding = System.Text.Encoding.Unicode;
            }
            
            SplooshView boardView = new(board);
            bool fire;
            board.PlaceShipsRandomly();

            boardView.homeRow = Console.CursorTop;
            boardView.homeCol = Console.CursorLeft;

            while (!GameWon())
            {
                boardView.Update();
                fire = false;

                while (!fire){
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

            boardView.showShips = true;
            boardView.showCursor = false;
            boardView.Update();
            Console.WriteLine("Game won. 'Grats man.");
            Console.WriteLine("Press any key to quit...");
            Console.ReadKey(true);
        }

    private static bool GameWon(){
        foreach (Ship ship in board.GetShips()){
            if (ship.IsAlive(board.GetShotMap)){
                return false;
            }
        }

        return true;
        }
}
}