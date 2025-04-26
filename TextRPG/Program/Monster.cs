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
using TextRPG.CharacterManagement;
using TextRPG.OtherMethods;
using TextRPG.WeaponManagement;
using TextRPG.QuestManagement;
using TextRPG.SkillManagement;
namespace TextRPG.MonsterManagement
{
    // 프로퍼티로 몬스터 상태 저장
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
        public List<Weapons> DropItems { get; set; } = new List<Weapons>();  // 드랍 아이템이 여러개여서 리스트로 추가
        public bool IsDropProcessed { get; set; } = false;

        // 골드 범위 랜덤 설정
        public int GetRandomGold()
        {
            return random.Next(MinGold, MaxGold + 1);
        }
        private static Random random = new Random();

        public static List<Monster> currentBattleMonsters = new List<Monster>(); // 리스트를 만들어서 currentBattleMonsters와 monterTypes를 몬스터 리스트에 저장
        private static List<Monster> monsterTypes = new List<Monster>();
        public static List<Weapons> NotbuyAbleInventory = new List<Weapons>(); // 드랍으로만 얻어야하는 장비만 모아놓는 리스트
        public static List<Weapons> PotionInventory = new List<Weapons>(); // 포션만 모아놓는 리스트
        public static List<Weapons> RewardInventory = new List<Weapons>(); // 전리품만 모아놓는 리스트
        public static int CurrentStage = 1;



        public Monster(string name, int level, int attack, int defense, int health, int dex, int eva, int exp = 0,
            int minGold = 0, int maxGold = 0, List<Weapons> dropItems = null)
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
            DropItems = dropItems ?? new List<Weapons>();
        }

      
        public static void InitMonsters()
        {

            if (monsterTypes.Count > 0) return;
            monsterTypes = new List<Monster>()
            {
                 // stage 1
                 new Monster("빠대리", 1, 14, 3, 10, 75, 10, 1, 40, 80, new List<Weapons> { Weapons.CreateDrop("대리의 빠때리") }), // 몬스터 종류와 정보
                 new Monster("신과장", 2, 12, 4, 14, 70, 15, 2, 50, 100, new List<Weapons> { Weapons.CreateDrop("과장의 사원증") }),
                 // stage 2
                 new Monster("임차장", 3, 19, 5, 26, 80, 10, 3, 120, 250, new List<Weapons> { Weapons.CreateDrop("차장의 가발") }),
                 new Monster("김부장", 4, 21, 4, 24, 75, 20, 4, 150, 300, new List<Weapons> { Weapons.CreateDrop("부장의 넥타이")}),
                 // stage 3
                 new Monster("오실장", 5, 24, 10, 36, 80, 15, 5, 250, 400, new List<Weapons> { Weapons.CreateDrop("직업 평가표") }),
                 new Monster("카이사", 6, 26, 8, 32, 80, 25, 6, 300, 500, new List<Weapons> { Weapons.CreateDrop("유흥업소 명함") }),
                 // stage 4
                 new Monster("유상무", 7, 28, 17, 43, 90, 20, 7, 400, 700, new List<Weapons> { Weapons.CreateDrop("한정판 굿즈 명함") }),
                 new Monster("박사장", 8, 31, 14, 50, 95, 30, 8, 500, 800, new List<Weapons> { Weapons.CreateDrop("노또 용지") }),
                 // stage 5 Boss
                 new Monster("석회장", 10, 40, 25, 250, 110, 30, 10, 2000, 3000, new List<Weapons> { Weapons.CreateDrop("직급 명패") })
            };
        }

      
        // 전투 화면 출력
        public static void SpawnMonster(Character character)
        {

            currentBattleMonsters.Clear();
            InitMonsters(); // 모든 몬스터 로드

            List<Monster> availableMonsters;

            // 스테이지 구현
            if (CurrentStage == 5) 
            { 
                // 스테이지 5에서는 보스만 등장
                availableMonsters = monsterTypes.Where(m => m.Name == "석회장").ToList();
            }
            else
            {
                // 스테이지 1~4에서는 일반 몬스터만 등장
                availableMonsters = monsterTypes
                    .Where(m => m.Level <= CurrentStage * 2 && m.Name != "석회장")
                    .ToList();
            }

            // 현재 스테이지에 맞는 몬스터만 필터링
            if (availableMonsters.Count == 0)
            {
                Console.WriteLine("출현 가능한 사원이 없습니다.");
                return;
            }

            Random rand = new Random();
            int numberOfMonster = (CurrentStage == 5) ? 1 : rand.Next(1, 3); // 전투에 나올 몬스터 수

            for (int i = 0; i < numberOfMonster; i++)
            {
              
                var baseMonster = availableMonsters[rand.Next(availableMonsters.Count)]; // base를 사용해서 부모(Monster)에 있는 함수 호출
                var selected = new Monster(
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
                    new List<Weapons>(baseMonster.DropItems)
                );
                currentBattleMonsters.Add(selected);
                Console.WriteLine($"Lv.{selected.Level} {selected.Name}   HP {selected.Health}    공격력 : {selected.Attack}    방어력 : {selected.Defense}");
            }
            // 플레이어 정보 출력
            Console.WriteLine("\n\n[내정보]");
            Console.WriteLine($"Lv.{character.Level} {character.ClassName} {character.Name}    공격력 : {character.Attack}");
            Console.WriteLine($"HP {character.Health}/{character.MaxHealth}");
            Console.WriteLine($"Exp {character.EXP}     소지금 : {character.Gold}원 ");
        }
    }

    // 전투 결과를  출력하는 메소드
    public class BattleResult 
    {
        // 전투가 끝난 뒤 결과(승리,패배)를 계산하고 보상 정보 출력
        public void ShowResult(Character character, List<Monster> monsters)
        {
            //가지고 있는 스킬 전부 효과 제거 및 쿨타임/효과 초기화
            foreach (var skill in character.Skills)
            {
                skill.EndSkillState();
                //남은 쿨타임, 효과지속시간, 효과 적용 여부 등을 0과 false로 변경
                //또한 가지고 있던 모든 효과 제거
            }

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


                if (Monster.CurrentStage < 5)
                {
                    Monster.CurrentStage++;
                    Console.WriteLine($"\n스테이지 클리어! [다음 스테이지 : {Monster.CurrentStage}]");
                }
                else
                {
                    Console.WriteLine("\n최종 스테이지 클리어! '석회장' 처치하였습니다. 승 진 배 틀 에서 최종 승리자가 되었습니다!");
                }
                if (Quest.ActiveQuest != null)
                {
                    Quest.IsQuestCleared = true;
                    // 스테이지마다 퀘스트 클리어 문구(퀘스트 이름)만 출력
                    Console.WriteLine(Quest.ActiveQuest.Name);
                    Console.WriteLine("퀘스트를 성공하셨습니다");
                }
                if (Monster.CurrentStage > 5 && result == "승리")
                {
                    Console.WriteLine("\n\n 축하합니다! 최종보스 '석회장'을 물리치고 게임을 클리어했습니다!");
                    Console.WriteLine("당신은 승진배틀(주)의 회장이 되었습니다!\n");

                    // 게임 종료하거나 메인 메뉴로 복귀
                    Console.WriteLine("\n1. 게임 종료\n2. 메인 메뉴로 돌아가기");
                    int input = InputHelper.MatchOrNot(1, 2);
                    if (input == 1) Environment.Exit(0);
                    else Monster.CurrentStage = 1;
                }
                foreach (var m in monsters.Where(m => m.Health <= 0))
                {
                    totalGold += m.GetRandomGold(); // 골드 획득
                    totalExp += m.Exp;

                    if (m.DropItems != null)
                    {
                        foreach (var item in m.DropItems)
                        {
                            int dropChance = new Random().Next(1, 101);
                            if (dropChance <= 30)
                            {
                                if (!character.NotbuyAbleInventory.Contains(item))
                                {
                                    character.NotbuyAbleInventory.Add(item);
                                }

                                Weapons.PaginateAndDisplayInventory(character, item); // 인벤토리에 보여주기
                                                                                      // 아이템 중복 방지 - 드랍 전에 체크

                                Console.WriteLine($"-> {item.Name} 을(를) 전리품으로 획득했습니다! (확률: {dropChance}%)");
                            }
                        }
                    }
                }

                if(character.ClassName == "총무팀") //운명의 달인 효과로 보상현금 +20%
                {
                    totalGold = (int)(totalGold * 1.2);
                }

                Console.WriteLine("\n[캐릭터 정보]");
                Console.WriteLine($"Lv.{character.Level} {character.Name}");
                Console.WriteLine($"HP {character.Health}");

                Console.WriteLine("\n[획득 아이템]");
                Console.WriteLine($"{totalGold}원");
                Console.WriteLine($"Exp + {totalExp}");

                character.Gold += totalGold;
                character.EXP += totalExp;

                character.LevelUP(); //얻은 경험치에 따라 레벨업 메서드 호출

                // 아이템 카운팅 정리
                // GroupBy를 활용해서 드랍 아이템을 합쳐서 출력
                var itemCounts = dropItems
                    .GroupBy(i => i)
                    .ToDictionary(g => g.Key, g => g.Count());
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
        public static Random rand = new Random();
        public static void StartBattle(Character character)
        {
            Console.Clear();
            Console.WriteLine("=== 승진 전투 개시 ===\n");
            Monster.SpawnMonster(character);
            foreach (var skill in character.Skills)
            {
                skill.SetSkillOwner(character); 
                //Skill함수 내에, Apply효과의 Key값으로 할당하는 SkillOwner를 character=플레이어로 설정
            }
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

                character.AssignSkills();
                character.ShowSkillList();

                Console.WriteLine("[몬스터 목록]");
                for (int i = 0; i < aliveMonsters.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Lv. {aliveMonsters[i].Level} {aliveMonsters[i].Name} HP {aliveMonsters[i].Health}");
                }
                Console.WriteLine();

                //패시브 스킬 자동 적용
                foreach(var skill in character.Skills)
                {
                    if (skill.IsActive == false)
                    {
                        skill.UseSkill(character, Monster.currentBattleMonsters);
                    }
                }

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
                                Console.WriteLine($"{i + 1}. Lv.{Monster.currentBattleMonsters[i].Level} {Monster.currentBattleMonsters[i].Name} HP {Monster.currentBattleMonsters[i].Health} Dead");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine($"{i + 1}. Lv.{Monster.currentBattleMonsters[i].Level} {Monster.currentBattleMonsters[i].Name} HP {Monster.currentBattleMonsters[i].Health}");
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
                        if (target.Health <= 0)
                        {
                            Console.WriteLine($"{target.Name} 은(는) 쓰러졌습니다!");

                            //적 처치시 작동하는 스킬 on
                            if (character.ClassName == "전산팀") //백업 시스템 효과 구현
                            {
                                int realHeal;
                                int realMPheal;

                                if (character.Health+10 > character.MaxHealth) realHeal = character.MaxHealth - character.Health;
                                else realHeal = 10;
                                
                                if (character.MP + 10 > character.MaxMP) realMPheal = character.MaxMP - character.MP;
                                else realMPheal = 10;

                                character.Health += realHeal;
                                character.MP += realMPheal;
                                
                                Console.WriteLine($"{character.Name}의 HP가 {realHeal} 회복되었습니다.");
                                Console.WriteLine($"{character.Name}의 MP가 {realMPheal} 회복되었습니다.");
                            }

                            if(character.ClassName == "기획팀")
                            {
                                foreach(var skill in character.Skills)
                                {
                                    if(skill.IsActive == false)
                                    {
                                        skill.UseSkill(character, Monster.currentBattleMonsters);
                                    }
                                }
                            }


                            if (!target.IsDropProcessed) 
                            { 
                               if (target.DropItems != null && target.DropItems.Count > 0)
                            {
                                    foreach (var item in target.DropItems)
                                    {
                                        int chance = BattleManager.rand.Next(1, 101);
                                        if (chance <= item.DropChance)
                                        {
                                            Weapons.PaginateAndDisplayInventory(character, item);
                                            break;
                                        }
                                    }
                        Console.WriteLine("\n[사원 상태 업데이트]");
                        for (int i = 0; i < Monster.currentBattleMonsters.Count; i++)
                        {
                            var m = Monster.currentBattleMonsters[i];
                            if (m.Health <= 0)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine($"{i + 1}. Lv.{m.Level} {m.Name} [Dead]");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine($"{i + 1}. Lv.{m.Level} {m.Name} HP {m.Health}");
                            }                         
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

                        SelectSkill(character);

                        // 플레이어 차례가 끝나면 몬스터들의 공격 차례 진행
                        EnemyPhase(character);
                        Console.ReadLine();
                        break;

                    case 3:                 
                        Weapons.DrinkingPotion(character);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다");
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

                // 명중 판정
                Random rand = new Random();
                int hitChance = monster.DEX = character.EVA + 10; // 기본 명중률 계산
                if (rand.Next(0, 100) < hitChance)
                {
                    int damage = monster.Attack;
                    character.Health -= damage;
                    if (character.Health < 0) character.Health = 0;
                    Console.WriteLine($"{character.Name} 을(를) 맞췄습니다. [데미지 : {damage}]");
                    Console.WriteLine($"HP {character.Health + damage} -> {character.Health}");
                }
                else
                {
                    Console.WriteLine($"{monster.Name}의 공격이 빗나갔습니다!");
                }
                if (character.Health <= 0) // 에너미 턴 진행도중 플레이어 체력이 0 이하가 될 시 GameOver
                {
                    return;
                }
                Console.WriteLine("Enter를 입력해주세요..");
                Console.ReadLine();
            }

            //가지고 있는 스킬의 쿨다운/효과 감소 진행(액티브)
            foreach(var skill in character.Skills)
            {
                if (skill.IsActive == true)
                {
                    skill.UpdateSkillState();
                }                
            }

            Console.WriteLine("\n상대의 공격이 끝났습니다. [플레이어의 차례]");
            Console.WriteLine("계속하려면 Enter를 누르세요...");
            Console.ReadLine();
        }


        public static void SelectSkill(Character character)
        {
            Console.WriteLine("\n[보유 스킬]");
            if (character.Skills.Count > 0)
            {
                int index = 1;
                foreach (var skill in character.Skills)
                {
                    Console.WriteLine($"{index} {skill.SkillName} (MP 소모: {skill.CostMP}, 쿨타임: {skill.CoolTime}턴)");
                    Console.WriteLine($"  설명: {skill.SkillDescription}");
                    index++;
                }
                Console.WriteLine("사용할 스킬을 선택해주세요.");
                Console.Write(">>");
                int actionChoice = InputHelper.MatchOrNot(0, 3);

                switch (actionChoice)
                {
                    case 0:  return;
                    case 1:
                        character.Skills[actionChoice - 1].UseSkill(character, Monster.currentBattleMonsters);
                        break;
                    case 2:
                        character.Skills[actionChoice - 1].UseSkill(character, Monster.currentBattleMonsters);
                        break;
                    case 3: Console.WriteLine("해당 스킬은 패시브 스킬이므로 사용할 수 없습니다. 다른 스킬을 선택해주세요."); SelectSkill(character); break;
                    default:
                        Console.WriteLine("잘못된 입력입니다");
                        break;

                }
                    
            }
            else
            {
                Console.WriteLine("보유한 스킬이 없습니다.");
            }
        }

    }
}
