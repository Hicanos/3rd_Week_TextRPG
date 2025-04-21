using System.Security.Cryptography.X509Certificates;
using static ConsoleApp2.TextRPG;
using System;
using System.IO;

namespace ConsoleApp2
{
    internal class Program
    {
        // Main과 클래스 분리
        static void Main(string[] args)
        {
            TextRPG game = new TextRPG();
            game.Run(); // 게임 실행
        }
    }

    internal class TextRPG
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

            Character character = new Character("Chad", "전사", 1, 100, 10, 5, 10000);

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
                int choice = MatchOrNot(1, 5);

                switch (choice)
                {
                    case 1:
                        character.ShowStatus();
                        WaitForZeroInput();
                        break;
                    case 2: // 전투시작 로직 추가 예정
                        break;
                    case 3:
                        Weapons.ShowInventory(character);
                        break;
                    case 4:
                        ShowShop(character);
                        break;
                    case 5:
                        ShowRestMenu(character);
                        break;
                }
            }
        }

        // 캐릭터 상태 저장
        public class Character
        {
            public string Name { get; set; }
            public int Level { get; set; }
            public string ClassName { get; set; }
            public int Health { get; set; }
            public double Attack { get; set; }
            public int Defense { get; set; }
            public int Gold { get; set; }
            public bool IEquipedDefense { get; set; }
            public bool IEquipedAttack { get; set; }

            public int ClearedCount { get; set; }

            //캐릭터 생성자
            public Character(string name, string className, int level, int health, double attack, int defense, int gold)
            {
                Name = name;
                ClassName = className;
                Level = level;
                Health = health;
                Attack = attack;
                Defense = defense;
                Gold = gold;
            }

            // 상태 보기
            public void ShowStatus()
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine($"Lv. {Level}");
                Console.WriteLine($"{Name} ( {ClassName} )");
                Console.WriteLine($"공격력 : {Attack}");
                Console.WriteLine($"방어력 : {Defense}");
                Console.WriteLine($"체력 : {Health}");
                Console.WriteLine($"Gold: {Gold} G");
                Console.WriteLine("-----------------------------");

                Console.WriteLine("\n0. 나가기\n");
                Console.Write("원하시는 행동을 입력해주세요.\n>>");
            }
        }

        // 장비 목록
        public class Weapons
        {
            public int Level { get; set; }
            public bool IsSelled { get; set; }
            public bool IsEquip { get; set; }
            public string Name { get; set; }
            public string Option { get; set; }
            public int OptionStatus { get; set; }
            public string Explain { get; set; }
            public int Price { get; set; }

            //장비들 리스트
            public static List<Weapons> Inventory = new List<Weapons>();

            // 장비 생성자
            public Weapons(bool isSelled, bool isEquip, string name, string option, int optionStatus, string explain, int price)
            {
                IsSelled = isSelled;
                IsEquip = isEquip;
                Name = name;
                Option = option;
                OptionStatus = optionStatus;
                Explain = explain;
                Price = price;

                Inventory.Add(this);
            }

            //인벤토리 보여주기 로직
            public static void ShowInventory(Character character)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine("\n인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n\n");
                Console.WriteLine("[아이템 목록]");
                foreach (Weapons weapon in Inventory)
                {
                    String equipmessage;
                    if (weapon.IsEquip == false)
                    {
                        equipmessage = weapon.Name;
                    }
                    else
                    {
                        equipmessage = $"[E]{weapon.Name}";
                    }

                    if (weapon.IsSelled)
                    {
                        Console.WriteLine($"- {equipmessage}  | {weapon.Option} + {weapon.OptionStatus} | {weapon.Explain}");
                    }
                }
                Console.WriteLine("-----------------------------");
                Console.WriteLine("\n1. 장착 관리\n0. 나가기\n");
                Console.Write("원하시는 행동을 입력해주세요.\n>>");

                int match = MatchOrNot(0, 1);
                if (match == 0)
                {
                    return;
                }
                else if (match == 1)
                {
                    Weapons.ManageMentWeapons(character);
                }
            }

            public static void ManageMentWeapons(Character character)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine("\n인벤토리 - 장착 관리\n보유 중인 아이템을 관리할 수 있습니다.\n\n");
                Console.WriteLine("[아이템 목록]");
                int numberOfWeapons = 1;
                List<Weapons> buyweapon = new List<Weapons>();

                //착용되어 있을 시 [E]표시 넣기
                foreach (Weapons weapon in Inventory)
                {
                    String equipmessage;
                    if (weapon.IsEquip == false)
                    {
                        equipmessage = weapon.Name;
                    }
                    else
                    {
                        equipmessage = $"[E]{weapon.Name}";
                    }

                    if (weapon.IsSelled) // 구매 했다면.
                    {
                        Console.WriteLine($"- {numberOfWeapons} {equipmessage}  | {weapon.Option} + {weapon.OptionStatus} | {weapon.Explain}");
                        buyweapon.Add(weapon); // 가지고 있는 무기들 전용 리스트에 추가한다. 이러면 순차적으로 추가된다
                        numberOfWeapons++;
                    }
                }

                Console.WriteLine("-----------------------------");
                Console.WriteLine("\n0. 나가기\n");
                Console.Write("원하시는 행동을 입력해주세요.\n>>");
                int count = buyweapon.Count;
                int Choice = MatchOrNot(0, count); // 입력값 검사의 범위는 0~리스트 인수의 갯수까지

                if (Choice == 0)
                {
                    Console.WriteLine();
                    return; // return 실행 시 ManageMentWeapon()을 탈출함 -> 바로 아래에 있던 break; 작동 -> 메인메뉴를 다시 비춤
                }
                else
                {
                    Weapons selected = buyweapon[Choice - 1]; // Choice는 1부터 시작이므로 -1을 해야 리스트값 참조가 정상적으로 가능
                    if (selected.IsEquip) //장착되어 있다면
                    {
                        selected.IsEquip = false; // 해제
                        Console.WriteLine($"{selected.Name}의 장착을 해제 했습니다.");
                        if (selected.Option == "공격력")
                        {
                            character.Attack -= selected.OptionStatus;
                            character.IEquipedAttack = false;
                        }
                        else if (selected.Option == "방어력")
                        {
                            character.Defense -= selected.OptionStatus;
                            character.IEquipedDefense = false;
                        }
                    }
                    else // 선택한 장비가 장착되지 않았다면
                    {
                        if (selected.Option == "방어력")
                        { // 선택한 장비가 방어구라면, 
                            if (character.IEquipedDefense) // 만약 장착한 방어구가 있을 시
                            {
                                foreach (Weapons weapon in buyweapon) // 보유 아이템 내에서
                                {
                                    if (weapon.IsEquip && weapon.Option == "방어력")  // 장착 중이며 옵션이 방어구인 것의
                                    {
                                        weapon.IsEquip = false; // 장착을 해제
                                        character.Defense -= weapon.OptionStatus; // 해제한 장비의 수치만큼 방어력 하락
                                    }
                                }
                                selected.IsEquip = true; //장착
                                Console.WriteLine($"{selected.Name}를 장착했습니다.");
                                character.Defense += selected.OptionStatus;
                            }
                            else
                            {
                                selected.IsEquip = true; //장착
                                character.IEquipedDefense = true; // 캐릭터가 방어구를 장착햇음으로 상태 변경
                                Console.WriteLine($"{selected.Name}를 장착했습니다.");
                                character.Defense += selected.OptionStatus;
                            }
                        }
                        else
                        {
                            if (character.IEquipedAttack) // 만약 장착한 공격무기가 있을 시
                            {
                                foreach (Weapons weapon in buyweapon) // 보유 아이템 내에서
                                {
                                    if (weapon.IsEquip && weapon.Option == "공격력")  // 장착 중이며 옵션이 공격력인 것의
                                    {
                                        weapon.IsEquip = false; // 장착을 해제
                                        character.Attack -= weapon.OptionStatus; // 해제한 장비의 수치만큼 공격력 하락
                                    }
                                }
                                selected.IsEquip = true; //장착
                                Console.WriteLine($"{selected.Name}를 장착했습니다.");
                                character.Attack += selected.OptionStatus;
                            }
                            else
                            {
                                selected.IsEquip = true; //장착
                                character.IEquipedAttack = true; // 캐릭터가 방어구를 장착햇음으로 상태 변경
                                Console.WriteLine($"{selected.Name}를 장착했습니다.");
                                character.Attack += selected.OptionStatus;
                            }
                        }
                    }

                    ManageMentWeapons(character); // 작업 후 다시 장착 관리 창으로 돌아가기 위해 메소드 재귀호출
                }
            }
        }

        public static void ShowShop(Character character)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다.\n\n");
            Console.WriteLine($"[보유 골드] {character.Gold} G\n\n");
            Console.WriteLine("[아이템 목록]\n\n");

            foreach (Weapons weapon in Weapons.Inventory)
            {
                string alreadyBuy;
                if (weapon.IsSelled == false)
                {
                    alreadyBuy = weapon.Price.ToString();
                }
                else
                {
                    alreadyBuy = "구매완료";
                }

                Console.WriteLine($"- {weapon.Name}  | {weapon.Option} + {weapon.OptionStatus} | {weapon.Explain} | {alreadyBuy}");
            }
            Console.WriteLine("-----------------------------");
            Console.WriteLine("\n1. 아이템 구매\n2. 아이템 판매\n0. 나가기\n");
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
            int match = MatchOrNot(0, 2);
            if (match == 0)
            {
                Console.WriteLine();
                return;
            }
            else if (match == 1)
            {
                BuyItems(character);
            }
            else
            {
                SellingItems(character);
            }
        }

        public static void BuyItems(Character character)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("상점 - 아이템 구매\n필요한 아이템을 얻을 수 있는 상점입니다.\n\n");
            Console.WriteLine($"[보유 골드] {character.Gold} G\n\n");
            Console.WriteLine("[아이템 목록]\n\n");

            //무기들 출력
            int numberOfWeapons = 1;
            foreach (Weapons weapon in Weapons.Inventory)
            {
                string alreadyBuy;
                if (weapon.IsSelled == false)
                {
                    alreadyBuy = weapon.Price.ToString();
                }
                else
                {
                    alreadyBuy = "구매완료";
                }

                Console.WriteLine($"- {numberOfWeapons} {weapon.Name}  | {weapon.Option} + {weapon.OptionStatus} | {weapon.Explain} | {alreadyBuy}");
                numberOfWeapons++;
            }
            Console.WriteLine("0. 나가기");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");
            int count = Weapons.Inventory.Count;
            int Choice = MatchOrNot(0, count); // 입력값 검사의 범위는 0~리스트 인수의 갯수까지

            if (Choice == 0)
            {
                Console.WriteLine();
                return;
            }
            else
            {
                Weapons selected = Weapons.Inventory[Choice - 1]; // Choice는 1부터 시작이므로 -1을 해야 리스트값 참조가 정상적으로 가능
                if (!selected.IsSelled && character.Gold >= selected.Price) // 구매가 가능한 아이템이고, 돈이 충분하다면
                {
                    Console.WriteLine($"{selected.Name}의 구매를 완료했습니다.");
                    selected.IsSelled = true;
                    character.Gold -= selected.Price;
                }
                else if (selected.IsSelled)//이미 구매되어 있다면
                {
                    Console.WriteLine("이미 구매한 아이템입니다.");
                }
                else // 돈이 부족하다면
                {
                    Console.WriteLine("저런, 돈이 부족하네.");
                }

                BuyItems(character); // 작업 후 다시 아이템 구매 창으로 돌아가기 위해 메소드 재귀호출
            }
        }


        public static void SellingItems(Character character)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("상점 - 아이템 판매\n아이템을 판매할 수 있습니다.\n\n");
            Console.WriteLine($"[보유 골드] {character.Gold} G\n\n");
            Console.WriteLine("[아이템 목록]\n\n");

            //무기들 출력
            int numberOfWeapons = 1;
            List<Weapons> buyweapon = new List<Weapons>();

            //착용되어 있을 시 [E]표시 넣기
            foreach (Weapons weapon in Weapons.Inventory)
            {
                if (weapon.IsSelled) // 구매 했다면.
                {
                    buyweapon.Add(weapon); // 가지고 있는 무기들 전용 리스트에 추가한다. 이러면 순차적으로 추가된다
                }
            }

            foreach (Weapons weapon in buyweapon)
            {
                String equipmessage;
                if (weapon.IsEquip == false)
                {
                    equipmessage = weapon.Name;
                }
                else
                {
                    equipmessage = $"[E]{weapon.Name}";
                }

                Console.WriteLine($"- {numberOfWeapons} {equipmessage}  | {weapon.Option} + {weapon.OptionStatus} | {weapon.Explain} | 판매가 : {(int)(weapon.Price * (85.0 / 100))}");
                numberOfWeapons++;
            }

            Console.WriteLine("0. 나가기");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");
            int count = buyweapon.Count;
            int Choice = MatchOrNot(0, count); // 입력값 검사의 범위는 0~리스트 인수의 갯수까지

            if (Choice == 0)
            {
                Console.WriteLine();
                return;
            }
            else
            {
                Weapons selected = buyweapon[Choice - 1]; // Choice는 1부터 시작이므로 -1을 해야 리스트값 참조가 정상적으로 가능
                selected.IsSelled = false;
                selected.IsEquip = false;
                character.Gold += (int)(selected.Price * (85.0 / 100));

                Console.WriteLine($"{selected.Name}의 판매를 완료했습니다.");
                SellingItems(character); // 작업 후 다시 아이템 판매 창으로 돌아가기 위해 메소드 재귀호출
            }
        }


        public static void ShowRestMenu(Character character)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("휴식하기");
            Console.WriteLine($"500 G를 내면 체력을 회복할 수 있습니다. (보유 골드 : {character.Gold} G)");
            Console.WriteLine($"\n1. 휴식하기\n0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.\n>>");

            int match = MatchOrNot(0, 1);
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
        }


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
        public void WaitForZeroInput()
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
