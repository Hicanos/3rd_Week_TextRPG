using TextRPG.CharacterManagement;
using TextRPG.OtherMethods;
using TextRPG.ShopManagement;

namespace TextRPG.WeaponManagement
{
    // 장비 목록
    public class Weapons
    {
        public bool IsSelled { get; set; } // 구매한 아이템인지?
        public bool? IsEquip { get; set; } // 현재 착용중인지?
        public string Name { get; set; } // 이름
        public string ClassName { get; set; } // 사용가능 부서 이름
        public string WeaponType { get; set; } // 장비 타입
        public string Explain { get; set; } // 장비 설명
        public Dictionary<string, int> Options { get; set; } // ex) {방어력, 10}
        public int Price { get; set; } // 가격
        public bool IsOneTimePurchase { get; set; } = false; // 1회 제한 구매 아이템 참거짓값

        // (드랍 아이템 전용 프로퍼티)
        public string DropObject { get; set; } // 획득처 
        public int DropChance { get; set; } // 드랍확률
        public int SellingPrice { get; set; } // 드랍아이템은 판매가격이 정해져있음
        public object AttackPower { get; private set; }

        //장비들 리스트
        public static List<Weapons> Inventory = new List<Weapons>(); // 상점에서 사는 게 가능한 장비만 모아놓는 리스트
        public static List<Weapons> NotbuyAbleInventory = new List<Weapons>(); // 드랍으로만 얻어야하는 장비만 모아놓는 리스트
        public static List<Weapons> PotionInventory = new List<Weapons>(); // 포션만 모아놓는 리스트
        public static List<Weapons> RewardInventory = new List<Weapons>(); // 전리품만 모아놓는 리스트
        
        // 1회 제한 아이템
        private List<string> purchasedItemNames = new List<string>(); // 이미 구매한 아이템을 추척

        public override string ToString()
        {
            return Name;
        }

        public Weapons() { }

        // 상점에서 사는 게 가능한 아이템 생성자
        public Weapons(bool isSelled, bool isEquip, string name, Dictionary<string, int> options, string explain, string className, string weaponType, int price, bool isOneTimePurchase = false)
        {
            IsSelled = isSelled;
            IsEquip = isEquip;
            Name = name;
            Options = options;
            Explain = explain;
            ClassName = className;
            WeaponType = weaponType;
            Price = price;
            IsOneTimePurchase = isOneTimePurchase;


            if (weaponType == "포션" && !PotionInventory.Contains(this))
            {
                PotionInventory.Add(this);
            }
            else
            {
                NotbuyAbleInventory.Add(this); ;
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

            if (weaponType == "포션" && !PotionInventory.Contains(this))
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

        public static Weapons CreateDrop(string name)
        {
            return new Weapons
            {
                Name = name,
                IsSelled = false,
                IsEquip = false,
                DropObject = "몬스터",
                DropChance = 30,
                SellingPrice = 0,
                Options = new Dictionary<string, int>(),
                Explain = "몬스터가 떨어뜨린 장비입니다.",
                ClassName = "전체",
                WeaponType = "기타"
            };
        }


        
        public static void PaginateAndDisplayInventory(List<Weapons> weapons, int page, int itemsPerPage, Character character, string mode)
        { // 아이템 목록 페이지로 나눠 보여주기
            int totalPages = (int)Math.Ceiling((double)weapons.Count / itemsPerPage); // 페이지 수 계산 -> Ceiling으로 반올림
                                                                                      // 무기 수에서 한 페이지에 보여줄 무기수를 나눈 다음, 반올림
            page = Math.Max(1, Math.Min(page, totalPages));

            Console.Clear();
            Console.WriteLine("-----------------------------");
            if (mode == "view")
            {
                Console.WriteLine("\n인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n\n");
            }
            else if (mode == "drink")
            {
                Console.WriteLine("\n인벤토리\n소모품을 섭취할 수 있습니다.\n\n");
            }
            else if (mode == "equip")
            {
                Console.WriteLine("\n인벤토리 - 장착 관리\n보유중인 아이템을 장착할 수 있습니다.\n\n");
            }
            Console.WriteLine($"[아이템 목록 - {page}/{totalPages} 페이지]\n");

            int startIndex = (page - 1) * itemsPerPage; // 페이지당 보여줄 첫 아이템의 인덱스, 처음엔 0으로 시작해야하므로 page - 1
            int endIndex = Math.Min(startIndex + itemsPerPage, weapons.Count); // 페이지당 보여줄 마지막 아이템 인덱스, 첫 인덱스 + 페이지당 보여줄 아이템 갯수로 계산
                                                                               // 무기 개수를 초과하여 계산되지 않도록 Min( , weapons.Count)했음

            for (int i = startIndex; i < endIndex; i++) // 무기 출력 for문
            {
                Weapons weapon = weapons[i];
                string equipmessage; // 착용 여부 판단 텍스트
                if (weapon.IsEquip == false || weapon.IsEquip == null)
                {
                    equipmessage = weapon.Name;
                }
                else
                {
                    equipmessage = $"[E]{weapon.Name}";
                }

                string classNameOnly = weapon.ClassName == null ? "" : weapon.ClassName == "전체" ? " ( 전체 )" : $" ({weapon.ClassName} 전용)";
                string optionText = weapon.Options != null && weapon.Options.Count > 0 ? 
                string.Join(", ", weapon.Options.Select(m => $"{m.Key} {(m.Value >= 0 ? "+" : "")}{m.Value}")) : "없음";

                Console.WriteLine(new string('-', 80));
                Console.WriteLine($"{i + 1}. {equipmessage} {classNameOnly} - {weapon.WeaponType}");
                Console.WriteLine($"   옵션: {optionText}");
                Console.WriteLine($"   설명: {weapon.Explain}");
            }

            string selectText = $"0. 나가기 | " +
                    (mode == "view" ? "1. 장착 관리 | 2. 소모품 사용 | " :
                    mode == "drink" ? $"{startIndex} ~ {endIndex}. 소모품 사용 | " :
                    mode == "equip" ? $"{startIndex} ~ {endIndex}. 아이템 장착 | " : "") +
                    "p. 이전 페이지 | n. 다음 페이지";
            Console.WriteLine(new string('-', 80));
            Console.WriteLine(selectText);
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");

        }

        //인벤토리 보여주기 로직
        public static void ShowInventory(Character character)
        {
            List<Weapons> isSelledItems = new List<Weapons>();
            isSelledItems.AddRange(Inventory.Where(w => w.IsSelled == true));
            isSelledItems.AddRange(NotbuyAbleInventory.Where(w => w.IsSelled == true));
            isSelledItems.AddRange(PotionInventory.Where(w => w.IsSelled == true));
            isSelledItems.AddRange(RewardInventory.Where(w => w.IsSelled == true));

            int currentPage = 1;
            int itemsPerPage = 4;

            while (true)
            {
                currentPage = Shop.PageCheck(currentPage, isSelledItems.Count, itemsPerPage);
                PaginateAndDisplayInventory(isSelledItems, currentPage, itemsPerPage, character, "view");

                string input = Console.ReadLine();
                if (input == "0") return;
                else if (input.ToLower() == "p") currentPage--;
                else if (input.ToLower() == "n") currentPage++;
                else if (input == "1")
                {
                    ManageMentWeapons(character);
                    break;
                }
                else if (input == "2")
                {
                    DrinkingPotion(character);
                    break;
                }
                else if (input == "p") currentPage--;
                else if (input == "n") currentPage++;
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                }
                Console.Clear();
            }
        }

        public static void DrinkingPotion(Character character)
        {
            int currentPage = 1;
            int itemsPerPage = 4;

            while (true)
            {
                List<Weapons> items = PotionInventory.Where(w => w.IsSelled).ToList();
                currentPage = Shop.PageCheck(currentPage, items.Count, itemsPerPage);
                PaginateAndDisplayInventory(items, currentPage, itemsPerPage, character, "drink");

                string input = Console.ReadLine();
                if (input == "0") { return; }
                else if (input.ToLower() == "p") currentPage--;
                else if (input.ToLower() == "n") currentPage++;
                else if (int.TryParse(input, out int index))
                {
                    (int startIndex, int endIndex) = Shop.GetPageRange(currentPage, itemsPerPage, items.Count);
                    index -= 1;
                    if (index >= startIndex && index < endIndex)
                    {
                        Weapons selected = items[index];
                        EquipItem(character, selected, items);
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                }
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
                if (weapon.IsEquip == true && weapon.WeaponType == selected.WeaponType) // 선택된 아이템 타입의 아이템이 이미 장착되어 있을 시
                {
                    weapon.IsEquip = false; // 해제
                    ApplyOptions(weapon.Options, false, character); // 능력치 감소
                }
            }
            if (selected.WeaponType == "포션")
            {
                if (selected.Options.ContainsKey("HP"))
                {
                    if (character.Health >= character.MaxHealth)
                    {
                        Console.WriteLine("체력이 이미 최대입니다.");
                        Thread.Sleep(1000);
                        return;
                    }
                }
                else if (selected.Options.ContainsKey("MP"))
                {
                    if (character.MP >= character.MaxMP)
                    {
                        Console.WriteLine("마나가 이미 최대입니다.");
                        Thread.Sleep(1000);
                        return;
                    }
                }

                Console.WriteLine("소모품을 사용했습니다.");
                List<string> recovered = new List<string>();
                foreach (var option in selected.Options)
                {
                    if (option.Key == "HP")
                    {
                        int before = character.Health;
                        character.Health = Math.Min(character.Health + option.Value, character.MaxHealth);
                        recovered.Add($"HP +{character.Health - before}");
                    }
                    else if (option.Key == "MP")
                    {
                        int before = character.MP;
                        character.MP = Math.Min(character.MP + option.Value, character.MaxMP);
                        recovered.Add($"MP +{character.MP - before}");
                    }
                    else
                    {
                        ApplyOptions(new Dictionary<string, int> { { option.Key, option.Value } }, true, character);
                        recovered.Add($"{option.Key} +{option.Value}");
                    }
                }

                Console.WriteLine($"[사용 효과] {string.Join(", ", recovered)}");
                // 사용 후 인벤토리에서 제거
                selected.IsSelled = false;
                PotionInventory.Remove(selected);
                Thread.Sleep(1000);
                return;
            }

            selected.IsEquip = true;
            ApplyOptions(selected.Options, true, character); // 장착 아이템 능력치 반영
            Console.WriteLine($"{selected.Name} 장착 완료!");
            Thread.Sleep(1000);
        }
        


        public static void ManageMentWeapons(Character character)
        {
            List<Weapons> myweapon = new List<Weapons>();
            myweapon.AddRange(Weapons.Inventory.Where(w => w.IsSelled && (w.ClassName == "전체" || w.ClassName == character.ClassName)));
            myweapon.AddRange(Weapons.NotbuyAbleInventory.Where(w => w.IsSelled && (w.ClassName == "전체" || w.ClassName == character.ClassName)));

            int currentPage = 1;
            int itemsPerPage = 4;

            while (true)
            {
                currentPage = Shop.PageCheck(currentPage, myweapon.Count, itemsPerPage);
                PaginateAndDisplayInventory(myweapon, currentPage, itemsPerPage, character, "equip");

                string input = Console.ReadLine();
                if (input == "0") return;
                else if (input.ToLower() == "p") currentPage--;
                else if (input.ToLower() == "n") currentPage++;
                else if (int.TryParse(input, out int index))
                {
                    (int startIndex, int endIndex) = Shop.GetPageRange(currentPage, itemsPerPage, myweapon.Count);
                    index -= 1;
                    if (index >= startIndex && index < endIndex)
                    {
                        Weapons selected = myweapon[index];
                        if (selected.IsEquip == true)
                        {
                            selected.IsEquip = false; // 해제
                            ApplyOptions(selected.Options, false, character); // 능력치 감소
                            Console.WriteLine($"{selected.Name}의 장착이 해제되었습니다.");
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            EquipItem(character, selected, myweapon);
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        Console.WriteLine("현재 페이지에 있는 아이템 번호만 입력해주세요.");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                }
            }
        }

        internal static void PaginateAndDisplayInventory(Character character, Weapons item)
        {
            // 캐릭터 인벤토리에 추가
            if (!character.NotbuyAbleInventory.Contains(item))
                character.NotbuyAbleInventory.Add(item);

            if (!RewardInventory.Contains(item))
                RewardInventory.Add(item);

            item.IsSelled = true;
        }   
        }
    }

