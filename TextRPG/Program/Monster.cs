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
// 수정부분
using TextRPG.QuestManagement;  // Quest 연동을 위해 추가

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
        public List<string> DropItems { get; set; }  // 드랍 아이템 리스트

        private static Random random = new Random();

        public Monster(string name, int level, int attack, int defense, int health, int dex, int eva, int exp = 0,
            int minGold = 0, int maxGold = 0, List<string> dropItems = null)
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
        }

        // 골드 범위 랜덤 반환
        public int GetRandomGold()
        {
            return random.Next(MinGold, MaxGold + 1);
        }

        public static List<Monster> currentBattleMonsters = new List<Monster>();
        private static List<Monster> monsterTypes = new List<Monster>();
        public static int CurrentStage = 1;

        public static void InitMonsters()
        {
            if (monsterTypes.Count > 0) return;
            monsterTypes = new List<Monster>()
            {
                // stage1
                new Monster("빠대리", 1, 4, 3, 10, 60, 15, 1, 40, 80, new List<string>{"대리의 빠때리"}),
                new Monster("신과장", 2, 3, 5, 14, 65, 20, 2, 50, 100, new List<string>{"과장의 사원증"}),
                // stage2
                new Monster("임차장", 3, 8, 7, 26, 65, 15, 3, 120, 250, new List<string>{"차장의 가발"}),
                new Monster("김부장", 4, 11, 5, 24, 70, 25, 4, 150, 300, new List<string>{"부장의 넥타이","방석&등받이 쿠션"}),
                // stage3
                new Monster("오실장", 5, 15, 14, 32, 70, 20, 5, 250, 400, new List<string>{"직업 평가표","브랜드 구두"}),
                new Monster("카이사", 6, 17, 10, 38, 75, 30, 6, 300, 500, new List<string>{"유흥업소 명함","브랜드 정장 하의"}),
                // stage4
                new Monster("유상무", 7, 25, 18, 43, 75, 30, 7, 400, 700, new List<string>{"한정판 굿즈 명함","든든한 국밥","브랜드 정장 상의"}),
                new Monster("박사장", 8, 22, 20, 50, 80, 40, 8, 500, 800, new List<string>{"노또 용지","든든한 국밥","최신 스마트폰"}),
                // stage5 Boss
                new Monster("석회장", 10, 40, 28, 250, 90, 35, 10, 2000, 3000, new List<string>{"직급 명패","든든한 국밥"})
            };
        }

        public static void SpawnMonster(Character character)
        {
            currentBattleMonsters.Clear();
            InitMonsters();

            List<Monster> availableMonsters;
            if (CurrentStage == 5)
                availableMonsters = monsterTypes.Where(m => m.Name == "석회장").ToList();
            else
                availableMonsters = monsterTypes
                    .Where(m => m.Level <= CurrentStage * 2 && m.Name != "석회장")
                    .ToList();

            if (availableMonsters.Count == 0)
            {
                Console.WriteLine("출현 가능한 사원이 없습니다.");
                return;
            }

            Random rand = new Random();
            int numberOfMonster = (CurrentStage == 5) ? 1 : rand.Next(1, 3);

            for (int i = 0; i < numberOfMonster; i++)
            {
                var baseMonster = availableMonsters[rand.Next(availableMonsters.Count)];
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
                    baseMonster.DropItems
                );
                currentBattleMonsters.Add(selected);
                Console.WriteLine($"Lv.{selected.Level} {selected.Name}   HP {selected.Health}    공격력 : {selected.Attack}    방어력 : {selected.Defense}");
            }

            Console.WriteLine("\n\n[내정보]");
            Console.WriteLine($"Lv.{character.Level} {character.ClassName} {character.Name}    공격력 : {character.Attack}");
            Console.WriteLine($"HP {character.Health}/{character.MaxHealth}");
            Console.WriteLine($"Exp {character.EXP}     소지금 : {character.Gold}원 ");
        }
    }

    public class BattleResult
    {
        public void ShowResult(Character character, List<Monster> monsters)
        {
            // 전투 결과 계산
            string result;
            int killedCount = monsters.Count(m => m.Health <= 0);
            bool allMonstersDead = monsters.All(m => m.Health <= 0);
            bool characterDead = character.Health <= 0;

            if (allMonstersDead) result = "승리";
            else if (characterDead) result = "패배";
            else result = "오류";

            Console.WriteLine("----");
            Console.WriteLine($"{result}");
            Console.WriteLine("----");

            if (result == "승리")
            {
                Console.WriteLine($"사원을 {killedCount}명 쓰러뜨렸습니다.\n");

                // 스테이지 갱신 전 저장
                int clearedStage = Monster.CurrentStage;
                if (Monster.CurrentStage < 5)
                {
                    Monster.CurrentStage++;
                    Console.WriteLine($"\n스테이지 {clearedStage} 클리어! [다음 스테이지 : {Monster.CurrentStage}]");
                }
                else
                {
                    Console.WriteLine("\n최종 스테이지 클리어! 보스를 처치하였습니다. 승진배틀(주)에서 최종 승리자가 되었습니다!");
                }

                // 수정부분
                var stageQuest = Quest.GetStageClearQuest(clearedStage);
                Quest.ActiveQuest = stageQuest;
                Quest.ActiveQuestTargetName = null;
                Quest.IsQuestCleared = true;
                Console.WriteLine($"퀘스트 '{stageQuest.Name}'가 수락되었습니다. 퀘스트 목록에서 보상을 받으세요.");

                // 전리품 집계
                int totalGold = 0;
                int totalExp = 0;
                List<string> dropItems = new List<string>();
                foreach (var m in monsters.Where(m => m.Health <= 0))
                {
                    totalGold += m.GetRandomGold();
                    totalExp += m.Exp;
                    foreach (var item in m.DropItems)
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                            dropItems.Add(item);
                    }
                }

                Console.WriteLine("\n[캐릭터 정보]");
                Console.WriteLine($"Lv.{character.Level} {character.Name}");
                Console.WriteLine($"HP {character.Health}");

                Console.WriteLine("\n[획득 아이템]");
                Console.WriteLine($"{totalGold}원");
                Console.WriteLine($"Exp + {totalExp}");
                var itemCounts = dropItems.GroupBy(i => i).ToDictionary(g => g.Key, g => g.Count());
                foreach (var item in itemCounts)
                    Console.WriteLine($"{item.Key} - {item.Value}");
            }
            else // 패배
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

    public static class BattleManager
    {
        public static void StartBattle(Character character)
        {
            Console.Clear();
            Console.WriteLine("=== 승진 전투 개시 ===\n");
            Monster.SpawnMonster(character);

            while (true)
            {
                var aliveMonsters = Monster.currentBattleMonsters.Where(m => m.Health > 0).ToList();
                if (character.Health <= 0 || aliveMonsters.Count == 0)
                {
                    new BattleResult().ShowResult(character, Monster.currentBattleMonsters);
                    break;
                }

                Console.Clear();
                Console.WriteLine("[내 정보]");
                Console.WriteLine($"Lv.{character.Level} {character.Name} ({character.ClassName})");
                Console.WriteLine($"HP {character.Health}/{character.MaxHealth}\n");

                Console.WriteLine("[몬스터 목록]");
                for (int i = 0; i < aliveMonsters.Count; i++)
                    Console.WriteLine($"{i + 1}. Lv.{aliveMonsters[i].Level} {aliveMonsters[i].Name} HP {aliveMonsters[i].Health}");
                Console.WriteLine();

                Console.WriteLine("[행동 선택]");
                Console.WriteLine("1. 일반 공격");
                Console.WriteLine("2. 스킬");
                Console.WriteLine("3. 소모품 사용");
                Console.Write(">> ");

                int actionChoice = InputHelper.MatchOrNot(1, 3);
                switch (actionChoice)
                {
                    case 1:
                        // 일반 공격
                        Console.WriteLine("[몬스터 목록]");
                        for (int i = 0; i < Monster.currentBattleMonsters.Count; i++)
                        {
                            var m = Monster.currentBattleMonsters[i];
                            if (m.Health <= 0)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine($"{i + 1}. Lv.{m.Level} {m.Name} HP {m.Health} Dead");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine($"{i + 1}. Lv.{m.Level} {m.Name} HP {m.Health}");
                            }
                        }

                        Console.WriteLine("\n공격할 대상이나 행동을 선택해주세요.");
                        Console.Write(">> ");
                        Monster target = null;
                        while (true)
                        {
                            int choice = InputHelper.MatchOrNot(1, Monster.currentBattleMonsters.Count);
                            target = Monster.currentBattleMonsters[choice - 1];
                            if (target.Health <= 0)
                            {
                                Console.WriteLine("이미 쓰러진 상대입니다.. 다시 선택하세요.");
                            }
                            else break;
                        }

                        Character.AttackMethod(character, target);
                        if (target.Health <= 0)
                            Console.WriteLine($"{target.Name} 은(는) 쓰러졌습니다!");

                        Console.WriteLine("\n[사원 상태 업데이트]");
                        foreach (var m in Monster.currentBattleMonsters)
                        {
                            if (m.Health <= 0)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine($"Lv.{m.Level} {m.Name} [Dead]");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine($"Lv.{m.Level} {m.Name} HP {m.Health}");
                            }
                            // 드랍 확률
                            if (target.DropItems != null && target.DropItems.Count > 0)
                            {
                                Random rand = new Random();
                                foreach (var item in target.DropItems)
                                {
                                    int chance = rand.Next(1, 101);
                                    if (chance <= 30)
                                    {
                                        Console.WriteLine($"-> {item} 획득! (확률:{chance}%)");
                                        break;
                                    }
                                }
                            }
                            Console.WriteLine();
                        }
                        EnemyPhase(character);
                        break;

                    case 2:
                        Console.WriteLine("스킬");
                        Console.ReadLine();
                        break;
                    case 3:
                        Console.WriteLine("소모품");
                        Console.ReadLine();
                        break;
                }
            }
        }

        public static void EnemyPhase(Character character)
        {
            var aliveMonsters = Monster.currentBattleMonsters.Where(m => m.Health > 0).ToList();
            if (aliveMonsters.Count == 0) return;

            Console.WriteLine("\nEnter 키를 누르면 적의 차례입니다...");
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
                }
                else
                {
                    Console.WriteLine($"Lv.{monster.Level} {monster.Name} 의 공격!");
                    Random rand = new Random();
                    int hitChance = monster.DEX - character.EVA + 50;
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
                }
                Console.WriteLine();
                Console.WriteLine("Enter를 눌러 계속...");
                Console.ReadLine();

                if (character.Health <= 0) return;
            }

            Console.WriteLine("\n상대의 공격이 끝났습니다. [플레이어의 차례]");
            Console.WriteLine("계속하려면 Enter를 누르세요...");
            Console.ReadLine();
        }
    }
}
