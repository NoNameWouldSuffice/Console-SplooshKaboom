using System.Runtime.InteropServices;

namespace Sploosh_Console
{
    class SplooshController()
    {
        public static void Main()
        {
            Console.WriteLine("So glad you can make it to the entry point");

            GameBoard testBoard = new(8,8);

            for (int i = 0; i < 8; i++){
                testBoard.FireShot(7, i);
            }

            // testBoard.printShotMap();

            Ship testShip = new(new Point(5,5), new Point(5,7));

            testShip.printShit();

            testBoard.PlaceShip(testShip);

            testBoard.PrintShipMap();
            // testBoard.printShotMap();z

            testBoard.FireShot(5, 5);
            testBoard.FireShot(5,6);
            testBoard.FireShot(5,7);

            bool shipHealth = testShip.isAlive(testBoard.GetShotMap);

            Console.WriteLine(shipHealth);

            Console.WriteLine(testShip.GetOrientation);

            testShip.flipDirection();

            Console.WriteLine(testShip.GetOrientation);

            int[,] dummyShipLayout = {
                {1,1,0,0,1,0,0,1,},
                {1,1,1,1,0,0,0,1,},
                {1,1,1,1,1,0,1,0,},
                {0,0,0,1,1,1,1,0,},
                {1,1,1,0,0,1,0,0,},
                {1,0,0,0,1,0,0,0,},
                {1,0,1,1,1,0,1,0,},
                {0,0,1,1,0,0,1,0,}
                
                };

            
            testBoard._shipMap = dummyShipLayout;

            // int axis = 0;
            // Dictionary<int, List<int>> yes = SplooshUtil.FindValidShipPlacements(out bool success, dummyShipLayout, 3, axis);
            
            // Console.WriteLine(yes.Count);

            // foreach( KeyValuePair<int, List<int>> kvp in yes )
            // {
            //     Console.WriteLine("Line = {0}, Options = {1}",
            //     kvp.Key, String.Join(", ", kvp.Value));
            // }




        }

        
    }
}