using TextRPG.CharacterManagemant;
using TextRPG.MonsterManagement;
<<<<<<< HEAD
=======
using TextRPG.OtherMethods;
using TextRPG.QuestManagement;
using TextRPG.RestManagemant;
using TextRPG.ShopManagemant;
using TextRPG.WeaponManagemant;
>>>>>>> main


namespace TextRPG.GameManager
{
    internal class Program
    {
        // Main과 클래스 분리
        static void Main(string[] args)
        {
            Weapons.BuyAbleWeaponSpawn();
            GameManager game = new GameManager();
            game.Run(); // 게임 실행
        }
    }

    internal class GameManager
    {
        public object Battle { get; private set; }

        public void Run()
        {
            //(string name, string className, int level, string rank, int maxhealth, int health, int maxMp, int mp, double attack, int defense, int gold)
            Character character = new Character("Chad", "전사", 1, "대리", 100, 100, 50, 50, 10, 5, 10000);
            Character.MakeCharacter(character);

            // 환영합니다 문구는 최초 시작 시 한번만
            bool welcomeText = true;

            while (true)
            {
                Console.Clear();
                if (welcomeText)
                {
                    Console.WriteLine();
                    Console.WriteLine($"평범한 무역회사에 일하는 20년차 대리, 그 이름은 {character.Name}.");
                    Console.WriteLine("업무 능력도, 출근 태도도 나무랄 데 없지만");
                    Console.WriteLine("하필 사장에게 찍혀 진급은 커녕 자리 지키기도 벅찼다!");
                    Console.WriteLine("그러던 어느 날, 회장님이 선언했다.");
                    Console.WriteLine("오늘 하루, 무력으로 진급하라!");
                    Console.WriteLine("단 하루뿐인 진급 배틀의 날!");
                    Console.WriteLine($"평소 쌓인 게 많았던 {character.Name}. 결국 결심한다.\r\n");
                    Console.WriteLine(" '이 회사는 내가 먹는다.' \n\n");

                    welcomeText = false;
                }

                // 메인 메뉴
                Console.WriteLine("-----------------------------");
                Console.WriteLine("1. 상태창\n2. 전투 시작\n3. 인벤토리\n4. 상점\n5. 휴식하기\n\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                // 1~6중 선택 후 switch문 발동
                int choice = InputHelper.MatchOrNot(1, 5);

                switch (choice)
                {
                    case 1:
                        character.ShowStatus();
                        InputHelper.WaitForZeroInput();
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("[스테이지 선택]");
                        Console.WriteLine("1. 스테이지 1");
                        Console.WriteLine("2. 스테이지 2");
                        Console.WriteLine("3. 스테이지 3");
                        Console.WriteLine("4. 스테이지 4");
                        Console.WriteLine("5. 스테이지 5 (Boss)");
                        Console.Write("입장할 스테이지를 선택하세요 >> ");
                        int stageChoice = InputHelper.MatchOrNot(1, 5);

                        BattleManager.StartBattle(character, stageChoice); 
                        break;
                    case 3:
                        Weapons.ShowInventory(character);
                        break;
                    case 4:
                        Shop.ShowShop(character);
                        break;
                    case 5:
                        Rest.ShowRestMenu(character);
                        break;
                }
            }
        }



    }
}
