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

        public static void SpawnMonster(Character character)
        {
            InitMonsters();
            Random rand = new Random();
            int numberOfMonster = rand.Next(1, monsterTypes.Count+1);

            for (int i = 1; i <= numberOfMonster; i++)
            {
                int indexOfMonster = rand.Next(0, monsterTypes.Count);
                Monster monster = monsterTypes[i];
                Console.WriteLine($"Lv.{monster.Level} {monster.Name}   HP {monster.Health}");                
            }

            Console.WriteLine("\n\n[내정보]");
            Console.WriteLine($"Lv.{character.Level}  {character.Name} ({character.ClassName})");
            Console.WriteLine($"HP {character.Health}/{character.MaxHealth}");
            Console.WriteLine("\n1. 공격\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");

            int choice = InputHelper.MatchOrNot(1, 1);
            
            // 여기에 공격 메소드 입력

        }

    }
  
}
