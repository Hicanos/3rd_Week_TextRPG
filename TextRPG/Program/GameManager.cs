using System.Security.Cryptography.X509Certificates;
using System;
using System.IO;
using TextRPG.CharacterManagemant;
using TextRPG.WeaponManagemant;
using TextRPG.ShopManagemant;
using TextRPG.RestManagemant;
using TextRPG.OtherMethods;
using TextRPG.MonsterManagement;

namespace TextRPG.GameManager
{
    internal class Program
    {
        // Main과 클래스 분리
        static void Main(string[] args)
        {
            GameManager game = new GameManager();
            game.Run(); // 게임 실행
        }
    }

    internal class GameManager
    {
        public void Run()
        {
            // 무기 & 캐릭터 객체 생성
            Weapons noobArmor = new Weapons(false, false, "수련자 갑옷", "방어력", 5, "수련에 도움을 주는 갑옷입니다.", 1000);
            Weapons ironArmor = new Weapons(false, false, "무쇠갑옷", "방어력", 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000);
            Weapons rtanArmor = new Weapons(false, false, "스파르타의 갑옷", "방어력", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500);
            Weapons sword = new Weapons(false, false, "낡은 검", "공격력", 2, "쉽게 볼 수 있는 낡은 검입니다.", 600);
            Weapons axe = new Weapons(false, false, "청동 도끼", "공격력", 5, "어디선가 사용됐던 것 같은 도끼입니다.", 1500);
            Weapons rtanSpear = new Weapons(false, false, "스파르타의 창", "공격력", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3000);

            Character character = new Character("Chad", "전사", 1, 100, 100, 10, 5, 10000);

            // 환영합니다 문구는 최초 시작 시 한번만
            bool welcomeText = true;

            while (true)
            {
                if (welcomeText)
                {
                    Console.WriteLine();
                    Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                    Console.WriteLine("이제 전투를 시작할 수 있습니다..\n");
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
                    case 2: // 전투시작 로직 추가 예정
                        Monster.SpawnMonster(character);
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
