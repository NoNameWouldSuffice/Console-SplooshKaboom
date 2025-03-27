namespace SplooshUtil
{
    record struct Point(int R, int C)
    {
        public override string ToString() => $"({R}, {C})";
    };

    enum Orientation
    {
        UNSET,
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    class SplooshRandom(){
        
        private static readonly Random random = new();

        public static Random GetRandom => random;

        public static bool RollChanceBool(double probability){
            if (probability < 0 || probability > 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(probability), "Probability must be between 0 and 1.");
                }
            
            return random.NextDouble() < probability;
        }
        
        public static T GetRandomElement<T>(List<T> list){
        if (list == null || list.Count == 0)
            throw new ArgumentException("List cannot be null or empty.");

        int index = random.Next(list.Count);
        return list[index];
        }   
    }

    class SplooshArray(){
        public static void Print2DArray<T>(T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }
    }


}