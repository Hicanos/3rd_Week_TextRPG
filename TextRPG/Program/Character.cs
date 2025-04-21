using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.CharacterManagemant
{
    // 캐릭터 상태 저장
    public class Character
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public string ClassName { get; set; }
        
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public double Attack { get; set; }
        public int Defense { get; set; }
        public int Gold { get; set; }
        public bool IEquipedDefense { get; set; }
        public bool IEquipedAttack { get; set; }
        public int ClearedCount { get; set; }

        //캐릭터 생성자
        public Character(string name, string className, int level, int maxhealth, int health, double attack, int defense, int gold)
        {
            Name = name;
            ClassName = className;
            Level = level;
            MaxHealth = maxhealth;
            Health = health;
            Attack = attack;
            Defense = defense;
            Gold = gold;
        }

        // 상태 보기
        public void ShowStatus()
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine($"Lv. {Level}");
            Console.WriteLine($"{Name} ( {ClassName} )");
            Console.WriteLine($"공격력 : {Attack}");
            Console.WriteLine($"방어력 : {Defense}");
            Console.WriteLine($"체력 : {Health}");
            Console.WriteLine($"Gold: {Gold} G");
            Console.WriteLine("-----------------------------");

            Console.WriteLine("\n0. 나가기\n");
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
        }
    }
}
