using TextRPG.CharacterManagemant;
using TextRPG.WeaponManagemant;

namespace TextRPG.ShopManagemant
{
    public class Shop
    {
        public static void PaginateAndDisplayItems(List<Weapons> weapons, int page, int itemsPerPage, Character character, string mode)
        { // 아이템 목록 페이지로 나눠 보여주기
            int totalPages = (int)Math.Ceiling((double)weapons.Count / itemsPerPage); // 페이지 수 계산 -> Ceiling으로 반올림
                                                                                      // 무기 수에서 한 페이지에 보여줄 무기수를 나눈 다음, 반올림
            page = Math.Max(1, Math.Min(page, totalPages)); // 현 페이지 -> 1 이상 전체 페이지 수 이하로 코딩
                                                            // Max(1, )이므로 1 이하로 내려갈 수 없으며, Min(page, totalPages)이므로 전체 페이지 수 초과 불가능

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
                string alreadyBuy = weapon.IsSelled ? "구매완료" : weapon.Price.ToString();
                string classNameOnly = weapon.ClassName == "전체" ? "전체" : $"{weapon.ClassName} 전용";
                string optionText = string.Join(", ", weapon.Options.Select(m => $"{m.Key} {(m.Value >= 0 ? "+" : "")}{m.Value}"));

                Console.WriteLine(new string('-', 80));
                Console.WriteLine($"{i + 1}. {weapon.Name} ({classNameOnly}) - {alreadyBuy} G");
                Console.WriteLine($"   옵션: {optionText}");
                Console.WriteLine($"   설명: {weapon.Explain}");
            }

            string selectText = $"0. 나가기 | {(mode == "view" ? "1. 물건 구매 | 2. 물건 판매 |" : mode == "buy" ? $"{startIndex + 1} ~ {endIndex}. 아이템 구매 |" : mode == "sell" ? $"{startIndex + 1} ~ {endIndex}. 아이템 판매 |" : "")} p. 이전 페이지 | n. 다음 페이지";
            Console.WriteLine(new string('-', 80));
            Console.WriteLine(selectText);
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");

        }

        public static void ShowShop(Character character)
        {

            int currentPage = 1;
            int itemsPerPage = 4; // 5부터는 상단부가 Clear 안됨

            while (true)
            {
                int totalPages = (int)Math.Ceiling((double)Weapons.Inventory.Count / itemsPerPage);
                currentPage = Math.Max(1, Math.Min(currentPage, totalPages));

                PaginateAndDisplayItems(Weapons.Inventory, currentPage, itemsPerPage, character, "view");

                string input = Console.ReadLine();
                if (input == "0") return;
                else if (input == "1")
                {
                    BuyItems(character);
                    return;
                }
                else if (input == "2")
                {
                    SellingItems(character);
                    return;
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


        public static void BuyItems(Character character)
        {
            //foreach문 대신 람다
            List<Weapons> availableWeapons = Weapons.Inventory
                .Where(w => (w.ClassName == character.ClassName || w.ClassName == "전체"))
                .ToList();

            int currentPage = 1;
            int itemsPerPage = 4;

            while (true)
            {
                int totalPages = (int)Math.Ceiling((double)availableWeapons.Count / itemsPerPage);
                currentPage = Math.Max(1, Math.Min(currentPage, totalPages));


                PaginateAndDisplayItems(availableWeapons, currentPage, itemsPerPage, character, "buy");

                string input = Console.ReadLine();

                if (input == "0") { ShowShop(character); return; }

                else if (input.ToLower() == "p") currentPage--;
                else if (input.ToLower() == "n") currentPage++;
                else if (int.TryParse(input, out int index))
                {
                    int startIndex = (currentPage - 1) * itemsPerPage;
                    int endIndex = Math.Min(startIndex + itemsPerPage, availableWeapons.Count);

                    if (index >= startIndex + 1 && index <= endIndex)
                    {
                        // 구매 처리
                        Weapons selected = availableWeapons[index - 1];
                        if (!selected.IsSelled && character.Gold >= selected.Price) // 구매하지 않은 물건이며, 충분한 돈을 가지고있다면
                        {
                            Console.WriteLine($"{selected.Name}의 구매를 완료했습니다.");
                            selected.IsSelled = true;
                            character.Gold -= selected.Price;
                            Thread.Sleep(1000);
                        }
                        else if (selected.IsSelled) // 이미 샀다면
                        {
                            Console.WriteLine("이미 구매한 아이템입니다.");
                            Thread.Sleep(1000);
                        }
                        else // 돈이 없다면
                        {
                            Console.WriteLine("저런, 돈이 부족하네.");
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

        public static void SellingItems(Character character)
        {
            List<Weapons> buyweapon = Weapons.Inventory.Where(w => w.IsSelled).ToList();

            int currentPage = 1;
            int itemsPerPage = 4;

            while (true)
            {
                int totalPages = (int)Math.Ceiling((double)buyweapon.Count / itemsPerPage);
                currentPage = Math.Max(1, Math.Min(currentPage, totalPages));

                PaginateAndDisplayItems(buyweapon, currentPage, itemsPerPage, character, "sell");

                string input = Console.ReadLine();

                if (input == "0") { ShowShop(character); return; }
                else if (input.ToLower() == "p") currentPage--;
                else if (input.ToLower() == "n") currentPage++;
                else if (int.TryParse(input, out int index))
                {
                    int startIndex = (currentPage - 1) * itemsPerPage;
                    int endIndex = Math.Min(startIndex + itemsPerPage, buyweapon.Count);

                    if (index >= startIndex + 1 && index <= endIndex)
                    {
                        Weapons selected = buyweapon[index - 1];
                        selected.IsSelled = false;
                        selected.IsEquip = false;
                        character.Gold += (int)(selected.Price * 0.5);
                        Console.WriteLine($"{selected.Name}의 판매를 완료했습니다.");

                        Thread.Sleep(1000);
                        SellingItems(character);
                        break;
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

    }
}

