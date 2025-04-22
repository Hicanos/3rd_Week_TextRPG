using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TextRPG.CharacterManagemant;
using TextRPG.OtherMethods;
namespace TextRPG.MonsterManagement
{
    public class Monster
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Exp { get; set; }
        public int Gold { get; set; }
        public string DropItem { get; set; }

        public Monster(string name, int level, int health, int attack, int exp = 0, int gold = 0, string dropItem = "")
        {
            Name = name;
            Level = level;
            Health = health;
            Attack = attack;
            Exp = exp;
            Gold = gold;
            DropItem = dropItem;

        }
        public static List<Monster> monsterTypes = new List<Monster>();
        public static void InitMonsters()
        {
            if (monsterTypes.Count > 0) { return; }
            Monster monster1 = new Monster("송 대리", 2, 15, 5 , 10 , 200 , "포션"); // 경험치,골드,드랍아이템은 기획서를 보고 수정
            Monster monster2 = new Monster("조 과장", 3, 10, 9 , 15 , 300 , "가방");
            Monster monster3 = new Monster("전 차장", 5, 25, 8 , 30 , 500 , "만년필");
            Monster monster4 = new Monster("이 부장", 7, 30, 10 , 50 , 1000 , "좋은아이템");
            Monster monster5 = new Monster("석 회장", 11, 40, 14, 70, 1500, "좋은아이템");
            Monster monster6 = new Monster("노 부회장", 9, 35, 12, 60, 1200, "좋은아이템");
            monsterTypes.AddRange(new List<Monster> { monster1, monster2, monster3, monster4 , monster5  , monster6  });
        }
        public static List<Monster> currentBattleMonsters = new List<Monster>();
        public static void SpawnMonster(Character character)
        {
            
            currentBattleMonsters.Clear();
            Monster.InitMonsters(); // 몬스터 타입 초기화
            Random rand = new Random();
            int numberOfMonster = rand.Next(1, monsterTypes.Count + 1);
            for (int i = 0; i < numberOfMonster; i++)
            {
                int index = rand.Next(0, monsterTypes.Count);
                Monster selected = new Monster(
                    monsterTypes[index].Name,
                    monsterTypes[index].Level,
                    monsterTypes[index].Health,
                    monsterTypes[index].Attack
                );
                currentBattleMonsters.Add(selected);
                Console.WriteLine($"Lv.{selected.Level} {selected.Name}   HP {selected.Health}");
            }
            Console.WriteLine("\n\n[내정보]");
            Console.WriteLine($"Lv.{character.Level}  {character.Name} ({character.ClassName})");
            Console.WriteLine($"HP {character.Health}/{character.MaxHealth}");
            Console.WriteLine("\n1. 공격\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");
            int choice = InputHelper.MatchOrNot(1, 1);
        }
    }
    public class BattleResult // 전투 결과를  출력하는 메소드
    {
        public void ShowResult(Character character, List<Monster> monsters)
        {
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
                Console.WriteLine($"사원을 {killedCount}명 쓰러뜨렸습니다.");
                Console.WriteLine($"Lv.{character.Level} {character.Name}");
                Console.WriteLine($"HP {character.Health}");
            }
            else if (result == "패배")
            {
                Console.WriteLine("당신은 해고당했습니다.");
                Console.WriteLine($"Lv.{character.Level} {character.Name}");
                Console.WriteLine($"HP {character.Health}");
            }
            Console.Write(">> ");
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
                if (character.Health <= 0)
                {
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
                Console.WriteLine("[몬스터 목록]");
                for (int i = 0; i < Monster.currentBattleMonsters.Count; i++)
                {
                    if (Monster.currentBattleMonsters[i].Health <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"{i + 1}. Lv.{Monster.currentBattleMonsters[i].Level} {Monster.currentBattleMonsters[i].Name} - HP {Monster.currentBattleMonsters[i].Health} Dead");
                        Console.ResetColor();
                        continue;
                    }
                    Console.WriteLine($"{i + 1}. Lv.{Monster.currentBattleMonsters[i].Level} {Monster.currentBattleMonsters[i].Name} - HP {Monster.currentBattleMonsters[i].Health}");
                }
                Console.WriteLine("\n공격할 대상을 선택해주세요.");
                Console.Write(">> ");

                Monster target = null;

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
                    else
                    {
                        break;
                    }
                }
                Character.AttackMethod(character, target);
                if (target.Health == 0)
                {
                    Console.WriteLine($"{target.Name} 은(는) 쓰러졌습니다!");
                    Console.WriteLine();
                }
                EnemyPhase(character);
            }
            Console.WriteLine("\n전투 종료. 메인으로 돌아갑니다...");
            Console.ReadLine();
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

        // 보상 지급 메서드
     
    }
}