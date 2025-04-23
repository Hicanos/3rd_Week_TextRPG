using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.CharacterManagemant;
using TextRPG.OtherMethods;

namespace TextRPG.WeaponManagemant
{
    // 장비 목록
    public class Weapons
    {
        public int Level { get; set; }
        public bool IsSelled { get; set; } // false 못산거  
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
                    Console.WriteLine($"- {equipmessage}  | {weapon.Option} + {weapon.OptionStatus} | {weapon.Explain}");
                }
            }
            Console.WriteLine("-----------------------------");
            Console.WriteLine("\n1. 장착 관리\n0. 나가기\n");
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
                    Console.WriteLine($"- {numberOfWeapons} {equipmessage}  | {weapon.Option} + {weapon.OptionStatus} | {weapon.Explain}");
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
                Thread.Sleep(1000);
                ManageMentWeapons(character); // 작업 후 다시 장착 관리 창으로 돌아가기 위해 메소드 재귀호출
            }
        }
    }

}
