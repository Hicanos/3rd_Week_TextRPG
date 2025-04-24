using TextRPG.CharacterManagement;
using TextRPG.OtherMethods;

namespace TextRPG.WeaponManagement
{
    // 장비 목록
    public class Weapons
    {
        public bool IsSelled { get; set; } // 구매한 아이템인지?
        public bool IsEquip { get; set; } // 현재 착용중인지?
        public string Name { get; set; } // 이름
        public string ClassName { get; set; } // 사용가능 부서 이름
        public string WeaponType { get; set; } // 장비 타입
        public string Explain { get; set; } // 장비 설명
        public Dictionary<string, int> Options { get; set; } // ex) {방어력, 10}
        public int Price { get; set; } // 가격

        // (드랍 아이템 전용 프로퍼티)
        public string DropObject { get; set; } // 획득처 
        public int DropChance { get; set; } // 드랍확률
        public int SellingPrice { get; set; } // 드랍아이템은 판매가격이 정해져있음

        //장비들 리스트
        public static List<Weapons> Inventory = new List<Weapons>(); // 상점에서 사는 게 가능한 장비만 모아놓는 리스트
        public static List<Weapons> NotbuyAbleInventory = new List<Weapons>(); // 드랍으로만 얻어야하는 장비만 모아놓는 리스트
        public static List<Weapons> PotionInventory = new List<Weapons>(); // 포션만 모아놓는 리스트
        public static List<Weapons> RewardInventory = new List<Weapons>(); // 전리품만 모아놓는 리스트

        // 상점에서 사는 게 가능한 아이템 생성자
        public Weapons(bool isSelled, bool isEquip, string name, Dictionary<string, int> options, string explain, string className, string weaponType, int price)
        {
            IsSelled = isSelled;
            IsEquip = isEquip;
            Name = name;
            Options = options;
            Explain = explain;
            ClassName = className;
            WeaponType = weaponType;
            Price = price;

            if (weaponType == "포션")
            {
                PotionInventory.Add(this);
            }
            else
            {
                Inventory.Add(this);
            }
        }

        // 드랍으로만 얻을 수 있는 아이템 생성자
        public Weapons(bool isSelled, bool isEquip, string name, Dictionary<string, int> options, string explain, string className, string weaponType, string dropObject, int dropChance, int sellingPrice)
        {
            IsSelled = isSelled;
            IsEquip = isEquip;
            Name = name;
            Options = options;
            Explain = explain;
            ClassName = className;
            WeaponType = weaponType;
            DropObject = dropObject;
            DropChance = dropChance;
            SellingPrice = sellingPrice;

            if (weaponType == "포션")
            {
                PotionInventory.Add(this);
            }
            else
            {
                NotbuyAbleInventory.Add(this);
            }
        }

        // 전리품 전용 생성자
        public Weapons(bool isSelled, string name, string explain, string weaponType, string dropObject, int dropChance, int sellingPrice)
        {
            IsSelled = isSelled;
            Name = name;
            Explain = explain;
            WeaponType = weaponType;
            DropObject = dropObject;
            DropChance = dropChance;
            SellingPrice = sellingPrice;

            RewardInventory.Add(this);
        }


        //인벤토리 보여주기 로직
        public static void ShowInventory(Character character)
        {
            Console.Clear();
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
                    string optionText = string.Join(", ", weapon.Options.Select(m => $"{m.Key} {(m.Value >= 0 ? "+" : "")}{m.Value}")); // 아이템의 효과들을 전부 출력
                    Console.WriteLine($"- {equipmessage}  | {optionText} | {weapon.Explain}");
                }
            }

            Console.WriteLine("-----------------------------");
            Console.WriteLine("\n1. 장착 관리 2. 소모품 사용\n0. 나가기\n");
            Console.Write("원하시는 행동을 입력해주세요.\n>>");

            int match = InputHelper.MatchOrNot(0, 1);
            if (match == 0)
            {
                return;
            }
            else if (match == 1)
            {
                Weapons.ManageMentWeapons(character);
            }
        }

        public static void ApplyOptions(Dictionary<string, int> options, bool isEquip, Character character) // 장착 혹은 해제 시 캐릭터의 능력치 변동
        {
            foreach (var option in options)
            {
                int value = isEquip ? option.Value : -option.Value; // 착용이면 더하고, 해제면 빼기
                                                                    // 포션인 경우 무조건 true로 넣으면 됨

                switch (option.Key)
                {
                    case "공격력":
                        character.Attack += value;
                        break;
                    case "방어력":
                        character.Defense += value;
                        break;
                    case "마나":
                        character.MP += value;
                        character.MaxMP += value;
                        break;
                    case "명중률":
                        character.DEX += value;
                        break;
                    case "치명타 배율":
                        character.CRITDMG += value;
                        break;
                    case "치명타 확률":
                        character.CRIT += value;
                        break;
                    case "회피율":
                        character.EVA += value;
                        break;
                    case "HP":
                        character.Health += value;
                        if (character.Health > character.MaxHealth)
                        {
                            character.Health = character.MaxHealth;
                        }
                        break;
                    case "MP":
                        character.MP += value;
                        if (character.MP > character.MaxMP)
                        {
                            character.MP = character.MaxMP;
                        }
                        break;
                    case "MaxHP":
                        character.MaxHealth += value;
                        character.Health += value;
                        break;
                    case "MaxMP":
                        character.MaxMP += value;
                        character.MP += value;
                        break;
                }
            }
        }

        public static void EquipItem(Character character, Weapons selected, List<Weapons> buyweapon) // 아이템 장착 로직
        {
            foreach (Weapons weapon in buyweapon) // 인벤토리 아이템 중
            {
                if (weapon.IsEquip && weapon.WeaponType == selected.WeaponType) // 선택된 아이템 타입의 아이템이 이미 장착되어 있을 시
                {
                    weapon.IsEquip = false; // 해제
                    ApplyOptions(weapon.Options, false, character); // 능력치 감소
                }
            }

            selected.IsEquip = true; // 선택된 아이템 장착
            Console.WriteLine($"{selected.Name}를 장착했습니다.");
            ApplyOptions(selected.Options, true, character); // 능력치 증가
        }


        public static void ManageMentWeapons(Character character)
        {
            Console.Clear();
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
                    string optionText = string.Join(", ", weapon.Options.Select(m => $"{m.Key} {(m.Value >= 0 ? "+" : "")}{m.Value}"));
                    Console.WriteLine($"- {numberOfWeapons} {equipmessage}  | {optionText} | {weapon.Explain}");
                    buyweapon.Add(weapon); // 가지고 있는 무기들 전용 리스트에 추가한다. 이러면 순차적으로 추가된다
                    numberOfWeapons++;
                }
            }

            Console.WriteLine("-----------------------------");
            Console.WriteLine("\n0. 나가기\n");
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
            int count = buyweapon.Count;
            int Choice = InputHelper.MatchOrNot(0, count); // 입력값 검사의 범위는 0~리스트 인수의 갯수까지

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
                    ApplyOptions(selected.Options, selected.IsEquip, character); // 능력치 변경

                    Console.WriteLine($"{selected.Name}의 장착을 해제 했습니다.");
                }
                else // 선택한 장비가 장착되지 않았다면
                {
                    if (character.ClassName == selected.ClassName || selected.ClassName == "전체")
                    {
                        EquipItem(character, selected, buyweapon);
                    }
                    else
                    {
                        Console.WriteLine("선택된 아이템은 다른 부서만 사용 가능합니다..");
                        ManageMentWeapons(character);
                        return;
                    }
                }
                Thread.Sleep(1000);
                ManageMentWeapons(character); // 작업 후 다시 장착 관리 창으로 돌아가기 위해 메소드 재귀호출
            }
        }
    }

}
