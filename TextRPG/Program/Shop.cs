using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.CharacterManagemant;
using TextRPG.WeaponManagemant;
using TextRPG.OtherMethods;

namespace TextRPG.ShopManagemant
{
    public class Shop
    {
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
            int match = InputHelper.MatchOrNot(0, 2);
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
            int Choice = InputHelper.MatchOrNot(0, count); // 입력값 검사의 범위는 0~리스트 인수의 갯수까지

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
            int Choice = InputHelper.MatchOrNot(0, count); // 입력값 검사의 범위는 0~리스트 인수의 갯수까지

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

    }
}

