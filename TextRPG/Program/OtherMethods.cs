namespace TextRPG.OtherMethods
{
    public class InputHelper
    {
        //사용자 입력값 정상여부 판단
        public static int MatchOrNot(int min, int max)
        {
            string input = Console.ReadLine();
            bool wrong = int.TryParse(input, out int choice);

            while (!wrong || choice < min || choice > max)
            {
                Console.WriteLine("잘못된 입력입니다.\n");
                Console.Write(">> ");
                input = Console.ReadLine();
                wrong = int.TryParse(input, out choice);
            }

            return choice;
        }

        //사용자입력값 정상여부판단 (오로지 0만 가능)
        public static void WaitForZeroInput()
        {
            string input = Console.ReadLine();
            bool wrong = int.TryParse(input, out int choice);

            while (!wrong || choice != 0)
            {
                Console.WriteLine("잘못된 입력입니다.\n");
                Console.Write(">> ");
                input = Console.ReadLine();
                wrong = int.TryParse(input, out choice);
            }

            Console.WriteLine();
        }
    }
}
