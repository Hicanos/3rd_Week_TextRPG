using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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

        public Monster(string name, int level, int health, int attack)
        {
            Name = name;
            Level = level;
            Health = health;
            Attack = attack;
        }

        public static List<Monster> monsterTypes = new List<Monster>();

        public static void InitMonsters()
        {
            if (monsterTypes.Count > 0) { return; }

            Monster monster1 = new Monster("송 대리", 2, 15, 5);
            Monster monster2 = new Monster("조 과장", 3, 10, 9);
            Monster monster3 = new Monster("전 차장", 5, 25, 8);
            Monster monster4 = new Monster("이 부장", 7, 30, 10);

            monsterTypes.AddRange(new List<Monster> { monster1, monster2, monster3, monster4 });
        }

        public static List<Monster> currentBattleMonsters = new List<Monster>();
        public static void SpawnMonster(Character character)
        {
            InitMonsters();
            Random rand = new Random();
            int numberOfMonster = rand.Next(1, monsterTypes.Count + 1);

            Console.WriteLine("[상사 등장]\n");

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
        public void ShowResult(Character player, List<Monster> monsters)
        {
            string result;
           

            int killedCount = monsters.Count(m => m.Health <= 0);
            bool allMonstersDead = monsters.All(m => m.Health <= 0);
            bool playerDead = player.Health <= 0;

            if (allMonstersDead)
            {
                result = "승리";
            }
            else if (playerDead)
            {
                result = "패배";
            }
            else result = "오류";

            Console.WriteLine("----");
            Console.WriteLine($"{result}");
            Console.WriteLine("----");

            if (result == "승리")
            {
                Console.WriteLine($"던전에서 상사를 {killedCount}명 쓰러뜨렸습니다.");
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {player.Health}");
            }
            else if (result == "패배")
            {
                Console.WriteLine("당신은 해고당했습니다.");
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {player.Health}");
            }

            Console.WriteLine("\n0. 다음");
            Console.Write(">> ");
        }
    }
    

    public static class BattleManager
    {
        private static List<Monster> currentBattleMonsters = new List<Monster>();

        public static void SpawnMonster(Character player)
        {
            // 간단한 몬스터 스폰 예제 (수정예정)
            currentBattleMonsters.Clear();
            currentBattleMonsters.Add(new Monster("상사 A", player.Level, 20, 5));
            currentBattleMonsters.Add(new Monster("상사 B", player.Level + 1, 25, 6));
        }
       
        public static void StartBattle(Character player)
        {
            Console.WriteLine("=== 승진 전투 개시 ===\n");

            SpawnMonster(player);

            while (true)
            {
                var aliveMonsters = currentBattleMonsters.Where(m => m.Health > 0).ToList();

                if (player.Health <= 0)
                {
                    Console.WriteLine("\n당신은 쓰러졌습니다...해고당했습니다 ㅠㅠ 밀린 신용카드 값을 갚지 못했습니다.");


                    new BattleResult().ShowResult(player, currentBattleMonsters);
                    break;
                }

                if (aliveMonsters.Count == 0)
                {
                    Console.WriteLine("\n승진 성공!! 모든 상사를 제압했습니다!! 회 사 정 복");

                    new BattleResult().ShowResult(player, currentBattleMonsters);
                    break;
                }

                Console.Clear();
                Console.WriteLine("[내 정보]");
                Console.WriteLine($"Lv.{player.Level} {player.Name} ({player.ClassName})");
                Console.WriteLine($"HP {player.Health}/{player.MaxHealth}\n");

                Console.WriteLine("[몬스터 목록]");
                for (int i = 0; i < aliveMonsters.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Lv.{aliveMonsters[i].Level} {aliveMonsters[i].Name} - HP {aliveMonsters[i].Health}");
                }

                Console.WriteLine("\n공격할 대상을 선택해주세요.");
                Console.Write(">> ");
                int choice = InputHelper.MatchOrNot(1, aliveMonsters.Count);
                Monster target = aliveMonsters[choice - 1];

                int damage = (int)player.Attack - (target.Level - 1);
                if (damage < 1) damage = 1;

                target.Health -= damage;
                if (target.Health < 0) target.Health = 0;

                Console.WriteLine($"\n{target.Name} 에게 {damage}의 데미지를 입혔습니다!");
                if (target.Health == 0)
                {
                    Console.WriteLine($"{target.Name} 은(는) 쓰러졌습니다!");
                }

                Console.WriteLine("\nEnter 키를 누르면 적의 차례가 됩니다...");
                Console.ReadLine();

                EnemyPhase(player);
            }

            Console.WriteLine("\n전투 종료. 메인으로 돌아갑니다...");
            Console.ReadLine();
        }
        public static void EnemyPhase(Character player)
        {
            Console.WriteLine("[Enemy Phase] 상대의 턴입니다.\n");

            foreach (var monster in currentBattleMonsters)
            {
                if (monster.Health <= 0) continue;

                Console.WriteLine($"Lv.{monster.Level} {monster.Name} 의 공격!");
                int damage = monster.Attack;
                player.Health -= damage;
                if (player.Health < 0) player.Health = 0;

                Console.WriteLine($"{player.Name} 을(를) 맞췄습니다. [데미지 : {damage}]");
                Console.WriteLine($"HP {player.Health + damage} -> {player.Health}\n");

                Console.WriteLine("0. 다음");
                Console.Write(">> ");
                Console.ReadLine();
            }

            Console.WriteLine("\n상대의 공격이 끝났습니다. [플레이어의 차례]");
            Console.WriteLine("계속하려면 Enter를 누르세요...");
            Console.ReadLine();

        }

    }
}
    

