using SplooshUtil;

namespace Sploosh_Console
{
    class SplooshController()
    {
        static GameBoard board = new(8,8);
        public static void Main()
        {
            SplooshView boardView = new(board);
            board.PlaceShipsRandomly();

            while (!GameWon())
            {
                boardView.Update();
                Point target = GetTargetFromUser();
                board.FireShot(target.R, target.C);
            }

            boardView.showShips = true;
            boardView.Update();
            Console.WriteLine("Game won. 'Grats man.");
            
            // Console.WriteLine("So glad you can make it to the entry point");

            // GameBoard testBoard = new(8,8);
            
            // testBoard.PlaceShipsRandomly();

            // testBoard.PrintShipMap();

            // SplooshView testView = new(testBoard);

            // for (int r = 0; r < testBoard.Height; r++){
            //     for (int c = 0; c < testBoard.Width; c++){
            //         if (r % 2 == 0){
            //             testBoard.FireShot(r, c);
            //         }
            //     }
            // }

            // testView.CompileLayers();
            // testView.Update();


        }

    private static bool GameWon(){
        foreach (Ship ship in board.GetShips()){
            if (ship.IsAlive(board.GetShotMap)){
                return false;
            }
        }

        return true;
        }
    
    private static Point GetTargetFromUser(){
        while(true){

            Console.Write("Input target: letter+number pair: ");
            
            string userInput = Console.ReadLine() ?? string.Empty;

            // Ways that the input from the user can be invalid:
            // Is less than 2 characters
            // Does not start with a letter
            // Contains more than one letter
            // Has a letter that is outside the range of the rows in the current game board
            // The remaining numbers is outside the range of columns in the current game board

            char letterInput = userInput[0];

            int colNum;

            bool intParseSuccess = int.TryParse(userInput[1..], out colNum);

            string alphabetString = "abcdefghijklmnopqrstuvwxyz";

            int rowNum = alphabetString.IndexOf(char.ToLower(letterInput));

            if (intParseSuccess && rowNum >= 0){
                return new Point(rowNum, colNum - 1);
            }
            
            Console.WriteLine("Invalid input. Try again.");





    }


        
    }
}
}