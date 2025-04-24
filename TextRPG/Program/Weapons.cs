using TextRPG.CharacterManagemant;
using TextRPG.OtherMethods;

namespace TextRPG.WeaponManagemant
{
    // 장비 목록
    public class Weapons
    {
        public bool IsSelled { get; set; } // false 못산거  
        public bool IsEquip { get; set; } // false 착용안한거
        public string Name { get; set; } // 이름
        public string ClassName { get; set; } // 부서 이름
        public string WeaponType { get; set; } // 장비 타입
        public string Explain { get; set; } // 장비 설명
        public Dictionary<string, int> Options { get; set; } // ex) {방어력, 10}
        public int Price { get; set; } // 가격
        public bool Buyable { get; set; } // 상점에서 구매 가능 여부


        public bool IsGettedByCharacter { get; set; } // 전리품이 획득되었는지 판단하는 값 (드랍 Only 아이템만 적용)
        public int DropChance { get; set; } // 드랍률 (드랍 Only 아이템만 적용)                                                                            
        public string GettingFromWhere { get; set; } // 획득처 (드랍 Only 아이템만 적용)
        public int SellingPrice { get; set; } // 판매가격 (기존 판매가격 로직을 따라가지 않는 가격, 드랍 Only 아이템만 적용)

        //장비들 리스트
        public static List<Weapons> Inventory = new List<Weapons>(); // 상점에서 사는 게 가능한 장비만 모아놓는 리스트
        public static List<Weapons> NotBuyableInventory = new List<Weapons>(); // 상점에서 사는게 불가능하고, 드랍으로만 얻을 수 있는 장비만 모아놓는 리스트
        public static List<Weapons> PotionInventory = new List<Weapons>(); // 상점에서 사는 게 가능한 포션만 모아놓는 리스트
        public static List<Weapons> NotBuyablePotionInventory = new List<Weapons> { }; // 상점에서 사는 게 불가능하고, 드랍으로만 얻을 수 있는 포션만 모아놓는 리스트
        public static List<Weapons> RewardInventory = new List<Weapons>(); // 전리품 모음 (사용, 구매 불가)

        // 상점에서 사는 게 가능한 장비 생성자
        public Weapons(bool isSelled, bool isEquip, string name, Dictionary<string, int> options, string explain, string className, string weaponType, int price, bool buyable)
        {
            IsSelled = isSelled;
            IsEquip = isEquip;
            Name = name;
            Options = options;
            Explain = explain;
            ClassName = className;
            WeaponType = weaponType;
            Price = price;
            Buyable = buyable;

            Inventory.Add(this);
        }

        // 상점에서 구매 불가, 드랍으로만 얻을 수 있는 장비 생성자
        public Weapons(bool isGettedByCharacter, string name, string weaponType, string gettingFromWhere, int dropchance, Dictionary<string, int> options, string explain, int sellingPrice, bool buyable) 
        {
            IsGettedByCharacter = isGettedByCharacter;
            Name = name;
            WeaponType = weaponType;
            GettingFromWhere = gettingFromWhere;
            DropChance = dropchance;
            Options = options;
            Explain = explain;
            SellingPrice = sellingPrice;
            Buyable = buyable;

            NotBuyableInventory.Add(this);
        }

        // 상점에서 사는 게 가능한 포션 생성자
        public Weapons(string name, Dictionary<string, int>options, string explain, string weaponType, int price, bool buyable) 
        {
            Name = name;
            Options = options;
            Explain = explain;
            WeaponType = weaponType;
            Price= price;
            Buyable = buyable;
        }

        // 온전히 판매만 가능한 전리품 생성자
        public Weapons(bool isGettedByCharacter, string name, string weaponType, string gettingFromWhere, int dropchance, string explain, int sellingPrice, bool buyable) 
        {
            isGettedByCharacter = isGettedByCharacter;
            Name = name;
            WeaponType = weaponType;
            GettingFromWhere = gettingFromWhere;
            DropChance = dropchance;
            Explain = explain;
            SellingPrice = sellingPrice;
            Buyable = buyable;

            RewardInventory.Add(this);
        }

        public static void BuyAbleWeaponSpawn() // 상점에서 구매 가능한 아이템들
        {
            if (Inventory.Count > 0) return; // 중복 생성 방지

            // 무기
            new Weapons(false, false, "최신 노트북", new Dictionary<string, int>
            {
                { "공격력", 25 }, { "치명타 배율", 80 }, { "치명타 확률", 30 }
            }, "최신형 노트북. 강력한 성능!", "전체", "무기", 10000, true);

            new Weapons(false, false, "구형 스마트폰", new Dictionary<string, int>
            {
                { "공격력", 15 }, { "치명타 배율", 20 }, { "치명타 확률", 10 }
            }, "낡았지만 아직 쓸 만함.", "전체", "무기", 3500, true);

            new Weapons(false, false, "무선 마우스", new Dictionary<string, int>
            {
                { "공격력", 15 }, { "마나", 30 }, { "치명타 배율", 25 }
            }, "무선이라 자유롭다.", "전체", "무기", 4400, true);

            new Weapons(false, false, "사무용 키보드", new Dictionary<string, int>
            {
                { "공격력", 20 }, { "명중률", 10 }, { "치명타 확률", 20 }
            }, "타자에 최적화된 키보드.", "전체", "무기", 5200, true);

            new Weapons(false, false, "그래픽카드5090", new Dictionary<string, int>
            {
                { "공격력", 14 }, { "치명타 배율", 50 }, { "마나", 90 }
            }, "고성능 그래픽카드.", "전체", "무기", 5090, true);

            new Weapons(false, false, "Moo나미볼펜", new Dictionary<string, int>
            {
                { "공격력", 2 }, { "치명타 확률", 3 }
            }, "무난한 볼펜이다.", "전체", "무기", 500, true);

            new Weapons(false, false, "형광펜", new Dictionary<string, int>
            {
                { "공격력", 5 }, { "치명타 배율", 15 }
            }, "형광이 빛난다.", "전체", "무기", 1100, true);

            new Weapons(false, false, "빗자루&쓰레받이", new Dictionary<string, int>
            {
                { "공격력", 7 }, { "마나", 20 }, { "치명타 배율", 20 }
            }, "청소용 무기지만 꽤 유용하다.", "전체", "무기", 1800, true);

            new Weapons(false, false, "결제 화일", new Dictionary<string, int>
            {
                { "공격력", 9 }, { "방어력", -2 }, { "치명타 확률", -3 }
            }, "결제를 자주 하는 사람의 필수품.", "인사팀", "무기", 2000, true);

            new Weapons(false, false, "명함", new Dictionary<string, int>
            {
                { "공격력", 5 }, { "마나", 40 }, { "치명타 확률", -5 }
            }, "자기 소개용 무기.", "홍보팀", "무기", 2000, true);

            new Weapons(false, false, "계산기", new Dictionary<string, int>
            {
                { "공격력", 9 }, { "마나", -15 }, { "회피율", -10 }
            }, "계산은 빠르지만 체력 소모가 크다.", "총무팀", "무기", 2000, true);

            new Weapons(false, false, "커터칼", new Dictionary<string, int>
            {
                { "공격력", 8 }, { "방어력", 2 }, { "명중률", -10 }
            }, "날카로운 도구.", "영업팀", "무기", 2000, true);

            new Weapons(false, false, "만년필", new Dictionary<string, int>
            {
                { "공격력", 8 }, { "마나", -20 }, { "명중률", -5 }
            }, "고급스러운 만년필.", "전산팀", "무기", 2000, true);

            new Weapons(false, false, "수정 테이프", new Dictionary<string, int>
            {
                { "공격력", 7 }, { "명중률", -10 }, { "치명타 확률", 10 }
            }, "실수를 고치고 공격한다.", "기획팀", "무기", 2000, true);

            // 방어구
            new Weapons(false, false, "싸구려 정장 상의", new Dictionary<string, int>
            {
                { "방어력", 4 }, { "회피율", 4 }
            }, "저렴한 상의.", "전체", "상의", 1500, true);

            new Weapons(false, false, "싸구려 정장 하의", new Dictionary<string, int>
            {
                { "방어력", 3 }, { "마나", 15 }
            }, "저렴한 하의.", "전체", "하의", 1200, true);

            new Weapons(false, false, "통풍셔츠", new Dictionary<string, int>
            {
                { "방어력", 5 }, { "마나", 25 }, { "치명타 확률", 10 }
            }, "시원한 셔츠.", "전체", "상의", 3000, true);

            new Weapons(false, false, "반바지", new Dictionary<string, int>
            {
                { "방어력", 4 }, { "명중률", 10 }, { "회피율", 5 }
            }, "편안한 하의.", "전체", "하의", 2800, true);

            new Weapons(false, false, "냄새나는 구두", new Dictionary<string, int>
            {
                { "방어력", 2 }, { "치명타 배율", 14 }
            }, "냄새가 나지만 성능은 좋다.", "전체", "하의", 1600, true);

            new Weapons(false, false, "발가락 양말", new Dictionary<string, int>
            {
                { "방어력", 2 }, { "회피율", 5 }
            }, "발가락 하나하나 따뜻하다.", "전체", "신발", 1000, true);

            new Weapons(false, false, "크록스", new Dictionary<string, int>
            {
                { "방어력", 3 }, { "치명타 배율", 20 }, { "마나", 15 }
            }, "편안한 크록스.", "전체", "신발", 2400, true);

            // 장신구
            new Weapons(false, false, "알없는 안경", new Dictionary<string, int>
            {
                { "공격력", 3 }, { "방어력", 2 }
            }, "그저 멋.", "총무팀", "장신구", 2000, true);

            new Weapons(false, false, "서류철", new Dictionary<string, int>
            {
                { "공격력", 2 }, { "방어력", 2 }, { "명중률", 5 }
            }, "서류 정리에도 좋고, 때릴 때도 좋음.", "인사팀", "장신구", 1500, true);

            new Weapons(false, false, "테이프", new Dictionary<string, int>
            {
                { "공격력", 2 }, { "방어력", 2 }, { "회피율", 3 }
            }, "붙이는 데도, 싸우는 데도!", "홍보팀", "장신구", 1500, true);

            new Weapons(false, false, "C타입 충전기", new Dictionary<string, int>
            {
                { "공격력", 3 }, { "방어력", 3 }, { "치명타 확률", -5 }
            }, "C타입이 제일 편해.", "총무팀", "장신구", 1500, true);

            new Weapons(false, false, "USB", new Dictionary<string, int>
            {
                { "공격력", 2 }, { "방어력", 3 }
            }, "작지만 강력함.", "영업팀", "장신구", 1500, true);

            new Weapons(false, false, "보조배터리", new Dictionary<string, int>
            {
                { "공격력", 2 }, { "방어력", 2 }, { "마나", 15 }
            }, "배터리가 생명이다.", "전산팀", "장신구", 1500, true);

            new Weapons(false, false, "건전지", new Dictionary<string, int>
            {
                { "공격력", 2 }, { "방어력", 2 }, { "치명타 확률", 3 }
            }, "지속력이 좋아진다.", "기획팀", "장신구", 1500, true);

            new Weapons(false, false, "A4용지", new Dictionary<string, int>
            {
                { "마나", 15 }, { "치명타 확률", 5 }
            }, "가볍고 활용도 높다.", "전체", "장신구", 900, true);

            new Weapons(false, false, "에어팟", new Dictionary<string, int>
            {
                { "마나", 50 }, { "방어력", -3 }, { "회피율", 25 }
            }, "감성템.", "전체", "장신구", 2500, true);

            new Weapons(false, false, "노트북백팩", new Dictionary<string, int>
            {
                { "공격력", 3 }, { "방어력", 3 }, { "치명타 확률", 15 }
            }, "휴대성과 전투력을 동시에!", "전체", "장신구", 3300, true);

            new Weapons(false, false, "명품 정장 상의", new Dictionary<string, int>
            {
                { "공격력", 3 }, { "방어력", 6 }, { "치명타 확률", 10 }, { "체력", 15 }
            }, "명품의 위엄.", "전체", "상의", 10000, true);

            new Weapons(false, false, "명품 정장 하의", new Dictionary<string, int>
            {
                { "공격력", 3 }, { "방어력", 5 }, { "치명타 배율", 20 }, { "체력", 10 }
            }, "바지도 명품이다.", "전체", "하의", 10000, true);

            new Weapons(false, false, "명품 구두", new Dictionary<string, int>
            {
                { "공격력", 4 }, { "방어력", 3 }, { "명중률", 10 }, { "회피율", 10 }
            }, "명품은 발끝에서부터.", "전체", "신발", 10000, true);

            new Weapons(false, false, "명품 시계", new Dictionary<string, int>
            {
                { "공격력", 2 }, { "방어력", 2 }, { "마나", 50 }, { "치명타 배율", 30 }
            }, "정확한 시간은 생존과 직결된다.", "전체", "장신구", 10000, true);
        }

        public static void NotBuyAbleWeaponSpawn() // 상점에서 구매가 불가능한 아이템들
        {
            new Weapons(false, false, "최신 스마트폰", new Dictionary<string, int>
            {
                { "공격력", 18 }, { "치명타 확률", 10 }
            }, "최신 기술이 담긴 스마트폰.", "전체", "무기", 2400, false);

                    new Weapons(false, false, "브랜드 정장 상의", new Dictionary<string, int>
            {
                { "방어력", 6 }, { "명중률", 15 }, { "치명타 확률", 20 }
            }, "고급 브랜드 정장 상의.", "전체", "상의", 2200, false);

                    new Weapons(false, false, "브랜드 정장 하의", new Dictionary<string, int>
            {
                { "방어력", 5 }, { "회피율", 15 }, { "치명타 확률", 10 }
            }, "고급 브랜드 정장 하의.", "전체", "하의", 2100, false);

                    new Weapons(false, false, "브랜드 구두", new Dictionary<string, int>
            {
                { "공격력", 2 }, { "방어력", 2 }, { "마나", 15 }
            }, "세련된 디자인의 명품 구두.", "전체", "신발", 2000, false);

                    new Weapons(false, false, "방석&등받이 쿠션", new Dictionary<string, int>
            {
                { "방어력", 2 }, { "마나", 20 }, { "회피율", 10 }
            }, "장시간 앉아도 편한 쿠션 세트.", "전체", "장신구", 2000, false);
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

        public static void ApplyOptions(Dictionary<string, int> options, bool isEquip, Character character) // 장착 혹은 해제 시 캐릭터의 능력치 변동
        {
            foreach (var option in options)
            {
                int value = isEquip ? option.Value : -option.Value; // 착용이면 더하고, 해제면 빼기

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
