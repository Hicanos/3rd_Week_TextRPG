using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using TextRPG.CharacterManagemant;
using TextRPG.OtherMethods;
namespace TextRPG.MonsterManagement
{
    public class Monster
    {
        public string Name { get; set; } // 몬스터이름
        public int Level { get; set; } // 레벨
        public int Attack { get; set; } // 공격력
        public int Defense { get; set; } // 방어력
        public int Health { get; set; } // 체력
        public int DEX { get; set; } // 명중률
        public int EVA { get; set; } // 회피율
        public int Exp { get; set; } // 경험치
        public int MinGold { get; set; }        // 획득 최소 골드
        public int MaxGold { get; set; }        // 획득 최대 골드
        public List<string> DropItems { get; set; }  // 드랍 아이템이 여러개여서 리스트로 추가
        public List<int> DropChance { get; set; }   // 드랍 아이템(전리품)이 드랍할 확률

        // 골드 범위 랜덤 설정
        public int GetRandomGold()
        {
            return random.Next(MinGold, MaxGold + 1);
        }
        private static Random random = new Random();
        public static List<Monster> currentBattleMonsters = new List<Monster>();
        private static List<Monster> monsterTypes = new List<Monster>();



        public Monster(string name, int level, int attack, int defense, int health, int dex, int eva, int exp = 0,
            int minGold = 0, int maxGold = 0, List<string> dropItems = null, List <int> dropChance = 0)
        {
            Name = name;
            Level = level;
            Attack = attack;
            Defense = defense;
            Health = health;
            DEX = dex;
            EVA = eva;
            Exp = exp;
            MinGold = minGold;
            MaxGold = maxGold;
            DropItems = dropItems ?? new List<string>();
            DropChance = dropChance ?? new List<int>(); // 전리품 드랍 확률
        }

        public static void InitMonsters()
        {

            if (monsterTypes.Count > 0) { return; }
            Monster monster1 = new Monster("빠대리", 1, 10, 4, 3, 60, 15, 1, 40, 80, new List<string> { "대리의 빠때리" }, new List<int>); // 몬스터 종류와 정보
            Monster monster2 = new Monster("신과장", 2, 14, 3, 5, 65, 20, 2, 50, 100, new List<string> { "과장의 사원증" }, new List<int>);
            Monster monster3 = new Monster("임차장", 3, 26, 8, 7, 70, 15, 3, 120, 250, new List<string> { "차장의 가발" }, new List<int>);
            Monster monster4 = new Monster("김부장", 4, 24, 11, 5, 70, 25, 4, 150, 300, new List<string> { "부장의 넥타이" }, new List<int>);
            Monster monster5 = new Monster("오실장", 5, 32, 15, 14, 70, 15, 5, 250, 400, new List<string> { "직업 평가표" }, new List<int>);
            Monster monster6 = new Monster("카이사", 6, 38, 17, 10, 75, 25, 6, 300, 500, new List<string> { "유흥업소 명함" }, new List<int>0);
            Monster monster7 = new Monster("유상무", 7, 43, 25, 18, 75, 30, 7, 400, 700, new List<string> { "한정판 굿즈 명함" }, new List<int>);
            Monster monster8 = new Monster("최전무", 8, 50, 22, 20, 80, 40, 8, 500, 800, new List<string> { "노또 용지" }, new List<int>);
            Monster monster9 = new Monster("석회장", 10, 250, 40, 28, 90, 35, 10, 2000, 3000, new List<string> { "직급 명패" }, new List<int>);
            monsterTypes.AddRange(new List<Monster> { monster1, monster2, monster3, monster4, monster5, monster6, monster7, monster8, monster9 });


        }



        // 전투 화면 출력
        public static void SpawnMonster(Character character)
        {

            currentBattleMonsters.Clear();
            Monster.InitMonsters(); // 몬스터를 InitMonsters에 정의해둔 리스트에서 불러옴
            Random rand = new Random();
            int numberOfMonster = rand.Next(1, monsterTypes.Count + 1); // 전투에 나올 몬스터 수를 랜덤으로 정함
            for (int i = 0; i < numberOfMonster; i++)
            {
                // 랜덤하게 몬스터를 골라 새 인스턴스를 생성하고 전투 몬스터 리스트에 추가
                // 생성된 몬스터를 화면에 출력
                int index = rand.Next(0, monsterTypes.Count);
                Monster baseMonster = monsterTypes[index]; // base를 사용해서 부모(Monster)에 있는 함수 호출
                Monster selected = new Monster(
                    baseMonster.Name,
                    baseMonster.Level,
                    baseMonster.Attack,
                    baseMonster.Defense,
                    baseMonster.Health,
                    baseMonster.DEX,
                    baseMonster.EVA,
                    baseMonster.Exp,
                    baseMonster.MinGold,
                    baseMonster.MaxGold,
                    baseMonster.DropItems
                );
                currentBattleMonsters.Add(selected);
                Console.WriteLine($"Lv.{selected.Level} {selected.Name}   HP {selected.Health}    공격력 : {selected.Attack}    방어력 : {selected.Defense}");
            }
            // 플레이어 정보 출력
            Console.WriteLine("\n\n[내정보]");
            Console.WriteLine($"Lv.{character.Level} {character.ClassName} {character.Name}    공격력 : {character.Attack}");
            Console.WriteLine($"HP {character.Health}/{character.MaxHealth}");
            Console.WriteLine($"Exp {character.EXP}     소지금 : {character.Gold}원 ");
            Console.WriteLine("\n1. 공격\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");
            int choice = InputHelper.MatchOrNot(1, 1);
        }
    }
    public class BattleResult // 전투 결과를  출력하는 메소드
    {
        // 전투가 끝난 뒤 결과(승리,패배)를 계산하고 보상 정보 출력
        public void ShowResult(Character character, List<Monster> monsters)
        {
            // 전투 결과 계산
            string result;
            int killedCount = monsters.Count(m => m.Health <= 0);
            bool allMonstersDead = monsters.All(m => m.Health <= 0);
            bool characterDead = character.Health <= 0;
            if (allMonstersDead)
            {
                result = "승리";
            }
            else if (characterDead)
            {
                result = "패배";
            }
            else result = "오류";

            Console.WriteLine("----");
            Console.WriteLine($"{result}");
            Console.WriteLine("----");

            if (result == "승리")
            {
                Console.WriteLine($"사원을 {killedCount}명 쓰러뜨렸습니다.\n");

                // 죽은 몬스터에게 골드와 아이템 수집
                int totalGold = 0;
                int totalExp = 0;
                List<string> dropItems = new List<string>();

                foreach (var m in monsters.Where(m => m.Health <= 0)) // 전투에서 쓰러진 체력0이하인 몬스터들만 골라서 반복문을 돔
                {                                                     // monsters는 전투에 참여한 모든 몬스터 리스트이고 m.Health <= 조건에 해당하는 몬스터만 순회
                    totalGold += m.GetRandomGold(); // 지정된 골드 범위 내에서 랜덤 보상
                    totalExp += m.Exp;

                    foreach (var item in m.DropItems) // 몬스터가 드롭한 아이템 리스트를 하나씩 꺼내기 위한 반복문
                    {
                        if (!string.IsNullOrWhiteSpace(item)) // 아이템이 null , 빈 문자열 , 공백만 있는 문자열이 아닌 경우에만 처리
                        {
                            dropItems.Add(item); // 드롭 아이템 리스트dropItems에 아이템을 추가
                        }
                    }
                }
                Console.WriteLine("\n[캐릭터 정보]");
                Console.WriteLine($"Lv.{character.Level} {character.Name}");
                Console.WriteLine($"HP {character.Health}");

                Console.WriteLine("\n[획득 아이템]");
                Console.WriteLine($"{totalGold}원");
                Console.WriteLine($"Exp + {totalExp}");

                // 아이템 카운팅 정리
                // GroupBy를 활용해서 드랍 아이템을 합쳐서 출력
                var itemCounts = dropItems.GroupBy(i => i).ToDictionary(g => g.Key, g => g.Count()); // itemCounts는 Dictionary의 자료형

                foreach (var item in itemCounts)  // itemCounts 딕셔너리 안에 있는 각 Key-Value 쌍을 하나씩 가져와서 item에 넣음
                {
                    Console.WriteLine($"{item.Key} - {item.Value}");
                }
            }

            else if (result == "패배")
            {
                Console.WriteLine("당신은 해고당했습니다.");
                Console.WriteLine($"Lv.{character.Level} {character.Name}");
                Console.WriteLine($"HP {character.Health}");
            }

            Console.WriteLine("\n0. 다음");
            Console.Write(">> ");
            Console.ReadLine();
        }
    }

    // 전투를 관리하는 BattleManager 클래스 생성
    public static class BattleManager
    {
        public static void StartBattle(Character character)
        {
            Console.Clear();
            Console.WriteLine("=== 승진 전투 개시 ===\n");
            Monster.SpawnMonster(character);
            // 전투가 끝날 때까지 무한 반복
            while (true)
            {
                // aliveMonster 리스트를 통해 살아있는 몬스터만 판단
                var aliveMonsters = Monster.currentBattleMonsters.Where(m => m.Health > 0).ToList();
                if (character.Health <= 0)
                {
                    // 캐릭터 체력이 0이되거나 몬스터 체력이 0이되면 ShowResult를 통해 전투 결과 출력 후 탈출
                    new BattleResult().ShowResult(character, Monster.currentBattleMonsters);
                    break;
                }
                if (aliveMonsters.Count == 0)
                {
                    new BattleResult().ShowResult(character, Monster.currentBattleMonsters);
                    break;
                }
                Console.Clear();
                Console.WriteLine("[내 정보]");
                Console.WriteLine($"Lv.{character.Level} {character.Name} ({character.ClassName})");
                Console.WriteLine($"HP {character.Health}/{character.MaxHealth}\n");

                Console.WriteLine("[행동 선택]");
                Console.WriteLine("1. 일반 공격");
                Console.WriteLine("2. 스킬");
                Console.WriteLine("3. 소모품 사용");
                Console.Write(">> ");
                int actionChoice = InputHelper.MatchOrNot(1, 3); // InputHelper.MatchOrNot함수로 사용자가 최소~최대 범위 내에서 숫자를 입력할 때까지 반복

                switch (actionChoice)
                {
                    case 1:
                        Console.WriteLine("[몬스터 목록]");
                        for (int i = 0; i < Monster.currentBattleMonsters.Count; i++)
                        {
                            if (Monster.currentBattleMonsters[i].Health <= 0)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine($"{i + 1}. Lv.{Monster.currentBattleMonsters[i].Level} {Monster.currentBattleMonsters[i].Name} - HP {Monster.currentBattleMonsters[i].Health} Dead");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine($"{i + 1}. Lv.{Monster.currentBattleMonsters[i].Level} {Monster.currentBattleMonsters[i].Name} - HP {Monster.currentBattleMonsters[i].Health}");
                            }
                        }

                        Console.WriteLine("\n공격할 대상이나 행동을 선택해주세요.");
                        Console.Write(">> ");

                        Monster target = null;

                        // 유저가 공격할 몬스터를 번호로 선택
                        // 이미 죽은 몬스터는 다시 선택하게 만듦
                        while (true)
                        {
                            int choice = InputHelper.MatchOrNot(1, Monster.currentBattleMonsters.Count);
                            target = Monster.currentBattleMonsters[choice - 1];
                            if (target.Health <= 0)
                            {
                                Console.WriteLine("이미 쓰러진 상대입니다.. 그렇게 미웠나요?");
                                Console.WriteLine("\n공격할 대상을 다시 선택해주세요.");
                                Console.WriteLine(">> ");
                            }
                            else break;
                        }


                        // Character.AttackMethod 호출
                        // 체력이 0이 되면 쓰러졌습니다 출력
                        Character.AttackMethod(character, target);
                        if (target.Health == 0)
                        {
                            Console.WriteLine($"{target.Name} 은(는) 쓰러졌습니다!");

                            // 아이템 드랍 확률 계산
                            if (target.DropItems != null && target.DropItems.Count > 0)
                            {
                                Random rand = new Random();
                                foreach (var item in target.DropItems)
                                {
                                    int chance = Drop.Next(1, 101);
                                    if (chance == 30)
                                    {
                                        Console.WriteLine($"-> {item}을(를) 획득했습니다! (획득 확률 : {chance}%)");
                                        // 인벤토리에 추가하는 싶으면 여기에 코드 작성
                                        // character.Inventory.Add(item);
                                        break;
                                    }
                                }
                            }

                            Console.WriteLine();
                        }
                        // 플레이어 차례가 끝나면 몬스터들의 공격 차례 진행
                        EnemyPhase(character);
                        break;

                    case 2:
                        Console.WriteLine("스킬");
                        Console.WriteLine("Enter를 눌러 계속...");
                        Console.ReadLine();
                        break;

                    case 3:
                        Console.WriteLine("소모품");
                        Console.WriteLine("Enter를 눌러 계속...");
                        Console.ReadLine();
                        break;
                }
            }
        }
            // 적과 대치하는 EnemyPhase 메서드 생성
            public static void EnemyPhase(Character character)
            {
                var aliveMonsters = Monster.currentBattleMonsters.Where(m => m.Health > 0).ToList();
                if (aliveMonsters.Count == 0) //코드 위치 이동, 텍스트 출력 전에 몬스터가 다 죽었다 판단되면 GameOver
                {
                    return;
                }
                Console.WriteLine("\nEnter 키를 누르면 적의 차례가 됩니다...");
                Console.ReadLine();
                Console.Clear();

                Console.WriteLine("[Enemy Phase] 상대의 턴입니다.\n");
                foreach (var monster in Monster.currentBattleMonsters)
                {
                    if (monster.Health <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"Lv.{monster.Level} {monster.Name} 은(는) 쓰러진 상태입니다.");
                        Console.ResetColor();
                        Console.WriteLine();
                        continue;
                    }
                    // 살아있는 적이 공격
                    Console.WriteLine($"Lv.{monster.Level} {monster.Name} 의 공격!");
                    int damage = monster.Attack;
                    character.Health -= damage;
                    if (character.Health < 0) character.Health = 0;
                    Console.WriteLine($"{character.Name} 을(를) 맞췄습니다. [데미지 : {damage}]");
                    Console.WriteLine($"HP {character.Health + damage} -> {character.Health}\n");
                    if (character.Health <= 0) // 에너미 턴 진행도중 플레이어 체력이 0 이하가 될 시 GameOver
                    {
                        return;
                    }
                    Console.WriteLine("Enter를 입력해주세요..");
                    Console.ReadLine();
                }
                Console.WriteLine("\n상대의 공격이 끝났습니다. [플레이어의 차례]");
                Console.WriteLine("계속하려면 Enter를 누르세요...");
                Console.ReadLine();
            }

        }
    }
