using TextRPG.CharacterManagement;
using TextRPG.OtherMethods;

namespace TextRPG.RestManagement
{
    public class Rest
    {
        public static void ShowRestMenu(Character character)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-----------------------------");
            Console.WriteLine("휴식하기");
            Console.WriteLine($"500 G를 내면 체력을 회복할 수 있습니다. (보유 골드 : {character.Gold} G)");
            Console.ResetColor();
            Console.WriteLine($"\n1. 휴식하기\n0. 나가기");
            Console.Write("\n원하시는 행동을 입력해주세요.\n>>");

            int match = InputHelper.MatchOrNot(0, 1);
            if (match == 0)
            {
                Console.WriteLine();
                return;
            }
            else
            {
                RestWithMoney(character);
            }
        }

        public static void RestWithMoney(Character character)
        {
            if (character.Gold >= 500)
            {
                if (character.Health == 100)
                {
                    Console.WriteLine("체력이 최대입니다.");
                }
                else
                {
                    character.Health = 100;
                    character.Gold -= 500;
                    Console.WriteLine("휴식을 완료했습니다.");
                }
            }
            else
            {
                Console.WriteLine("저런, 돈이 부족하네.");
            }
            Thread.Sleep(2000);
        }
    }
}
