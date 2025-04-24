using TextRPG.CharacterManagement;
using TextRPG.OtherMethods;
using TextRPG.WeaponManagement;

namespace TextRPG.ShopManagement
{
    public class Shop
    {
        public static void PaginateAndDisplayItems(List<Weapons> weapons, int page, int itemsPerPage, Character character, string mode)
        { // 아이템 목록 페이지로 나눠 보여주기
            int totalPages = (int)Math.Ceiling((double)weapons.Count / itemsPerPage); // 페이지 수 계산 -> Ceiling으로 반올림
                                                                                      // 무기 수에서 한 페이지에 보여줄 무기수를 나눈 다음, 반올림
            page = Math.Max(1, Math.Min(page, totalPages));

            Console.Clear();
            Console.WriteLine("-----------------------------");
            Console.WriteLine($"상점 - {(mode == "buy" ? "아이템 구매" : mode == "sell" ? "아이템 판매" : "아이템 보기")}"); // buy, sell, view로 나눔 텍스트 출력에 씀
            Console.WriteLine($"[보유 골드] {character.Gold} G\n");
            Console.WriteLine($"[아이템 목록 - {page}/{totalPages} 페이지]\n");

            int startIndex = (page - 1) * itemsPerPage; // 페이지당 보여줄 첫 아이템의 인덱스, 처음엔 0으로 시작해야하므로 page - 1
            int endIndex = Math.Min(startIndex + itemsPerPage, weapons.Count); // 페이지당 보여줄 마지막 아이템 인덱스, 첫 인덱스 + 페이지당 보여줄 아이템 갯수로 계산
                                                                               // 무기 개수를 초과하여 계산되지 않도록 Min( , weapons.Count)했음

            for (int i = startIndex; i < endIndex; i++) // 무기 출력 for문
            {
                Weapons weapon = weapons[i];
                string alreadyBuy; // 가격 부분에 쓸 텍스트 지정
                if (mode == "sell")
                {
                    if (weapon.Price > 0) // 드랍된 아이템이 아닐 시
                    {
                        alreadyBuy = $"판매가 : {(int)weapon.Price * 0.5} G";
                    }
                    else // 드랍된 아이템일시
                    {
                        alreadyBuy = $"판매가 : {weapon.SellingPrice} G";
                    }
                }
                else // 모드가 구매, 보기라면
                {
                    if (weapon.IsSelled)
                    {
                        alreadyBuy = "구매완료";
                    }
                    else
                    {
                        alreadyBuy = $"{weapon.Price} G";
                    }
                }
                string classNameOnly = weapon.ClassName == null ? "" : weapon.ClassName == "전체" ? " ( 전체 )" : $" ({weapon.ClassName}) 전용";
                string optionText = weapon.Options == null ? "없음" : string.Join(", ", weapon.Options.Select(m => $"{m.Key} {(m.Value >= 0 ? "+" : "")}{m.Value}"));

                Console.WriteLine(new string('-', 80));
                Console.WriteLine($"{i + 1}. {weapon.Name}{classNameOnly} - {alreadyBuy}");
                Console.WriteLine($"   옵션: {optionText}");
                Console.WriteLine($"   설명: {weapon.Explain}");
            }

            string selectText = $"0. 나가기 | {(mode == "view" ? "1. 물건 구매 | 2. 물건 판매 |" : mode == "buy" ? $"{startIndex + 1} ~ {endIndex}. 아이템 구매 |" : mode == "sell" ? $"{startIndex + 1} ~ {endIndex}. 아이템 판매 |" : "")} p. 이전 페이지 | n. 다음 페이지";
            Console.WriteLine(new string('-', 80));
            Console.WriteLine(selectText);
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");

        }

        public static void ShowShop(Character character) // 상점 출력 메소드
        {

            int currentPage = 1;
            int itemsPerPage = 4; // 5부터는 상단부가 Clear 안됨

            List<Weapons> totalShopOptions = Weapons.Inventory;
            totalShopOptions.AddRange(Weapons.PotionInventory.Where(x => x.Price > 0));

            while (true)
            {
                currentPage = PageCheck(currentPage, totalShopOptions.Count, itemsPerPage);
                PaginateAndDisplayItems(totalShopOptions, currentPage, itemsPerPage, character, "view");

                string input = Console.ReadLine();
                if (input == "0") return;
                else if (input == "1")
                {
                    Console.Clear();
                    Console.WriteLine("어떤 아이템을 구매하시겠습니까?");
                    Console.WriteLine("1. 장비");
                    Console.WriteLine("2. 소모품");
                    Console.Write(">> ");
                    int choice = InputHelper.MatchOrNot(1, 2);
                    List<Weapons> availableWeapons = Weapons.Inventory.Where(w => (w.ClassName == character.ClassName || w.ClassName == "전체")).ToList(); // 구매 가능한 무기 리스트
                    List<Weapons> potions = Weapons.PotionInventory.Where(w => w.WeaponType == "포션" && w.Price > 0).ToList(); // 구매 가능한 포션 리스트

                    if (choice == 1) BuyItems(character, availableWeapons);  // 장비 구매
                    else if (choice == 2) BuyItems(character, potions);  // 소모품 구매
                }
                else if (input == "2")
                {
                    Console.Clear();
                    Console.WriteLine("어떤 아이템을 판매하시겠습니까?");
                    Console.WriteLine("1. 장비");
                    Console.WriteLine("2. 소모품");
                    Console.WriteLine("3. 전리품");
                    Console.Write(">> ");
                    int choice = InputHelper.MatchOrNot(1, 3);

                    List<Weapons> buyweapon = Weapons.Inventory.Where(w => w.IsSelled).ToList(); // 판매가능한 장비 리스트
                    buyweapon.AddRange(Weapons.NotbuyAbleInventory.Where(w => w.IsSelled)); // 드랍하여 얻은 장비도 포함

                    List<Weapons> potions = Weapons.PotionInventory.Where(w => w.IsSelled).ToList(); // 판매가능한 포션 리스트

                    List<Weapons> rewards = Weapons.RewardInventory.Where(w => w.IsSelled).ToList(); // 판매가능한 전리품 리스트

                    if (choice == 1) SellItems(character, buyweapon);
                    else if (choice == 2) SellItems(character, potions);
                    else if (choice == 3) SellItems(character, rewards);
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

        public static int PageCheck(int currentPage, int totalItems, int itemsPerPage) // 현재페이지가 유효한지 검사하는 메소드 (PaginateAndDisplayItems에서도 쓰는 로직)
        {                                                                              // PaginateAndDisplayItems에서는 totalPages를 계산에 사용해서 충돌이 날까봐 쓰지않음
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            return Math.Max(1, Math.Min(currentPage, totalPages));
        }

        public static (int start, int end) GetPageRange(int currentPage, int itemsPerPage, int itemCount) // 한 페이지 안에서 시작 아이템번호와 끝 아이템번호를 구하는 메소드
        {
            int startIndex = (currentPage - 1) * itemsPerPage; // 1페이지라면 0부터, 2페이지라면 4부터
            int endIndex = Math.Min(startIndex + itemsPerPage, itemCount); // 끝나는 인덱스는 아이템 전체 수 이하여야함
            return (startIndex, endIndex);
        }

        private static void BuyOrSellItems(Character character, List<Weapons> items, string mode)
        {
            int currentPage = 1;
            int itemsPerPage = 4;

            while (true)
            {
                currentPage = PageCheck(currentPage, items.Count, itemsPerPage);
                PaginateAndDisplayItems(items, currentPage, itemsPerPage, character, mode);

                string input = Console.ReadLine();
                if (input == "0") {  return; }
                else if (input.ToLower() == "p") currentPage--;
                else if (input.ToLower() == "n") currentPage++;
                else if (int.TryParse(input, out int index))
                {
                    (int startIndex, int endIndex) = GetPageRange(currentPage, itemsPerPage, items.Count);
                    index -= 1; // 유저가 보는 번호는 1부터 시작 → 내부 인덱스는 0부터 시작
                    if (index >= startIndex && index < endIndex)
                    {
                        Weapons selected = items[index];

                        if (mode == "buy")
                        {
                            if (!selected.IsSelled && character.Gold >= selected.Price)
                            {
                                Console.WriteLine($"{selected.Name}의 구매를 완료했습니다.");
                                selected.IsSelled = true;
                                character.Gold -= selected.Price;
                            }
                            else
                            {
                                Console.WriteLine(selected.IsSelled ? "이미 구매한 아이템입니다." : "저런, 돈이 부족하네.");
                            }
                        }

                        else if (mode == "sell")
                        {
                            selected.IsSelled = false;
                            selected.IsEquip = false;
                            int sellPrice = selected.Price > 0 ? (int)(selected.Price * 0.5) : selected.SellingPrice;
                            character.Gold += sellPrice;
                            Console.WriteLine($"{selected.Name}의 판매를 완료했습니다.");
                        }

                        Thread.Sleep(1000);
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

        public static void BuyItems(Character character, List<Weapons> items)
        {
            BuyOrSellItems(character, items, "buy");
        }

        public static void SellItems(Character character, List<Weapons> items)
        {
            BuyOrSellItems(character, items, "sell");
        }
    }
}





