using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TextRPG.MonsterManagement;

namespace TextRPG.CharacterManagemant
{
    enum Departments
    {
        인사팀=1,
        홍보팀,
        총무팀,
        영업팀,
        전산팀,
        기획팀
    }

    enum Ranks
    {
        대리 = 1,
        과장,
        차장,
        부장,
        전무,
        상무,
        이사,        
        사장,
        부회장
    }

    // 캐릭터 상태 저장
    public class Character
    {
        public string Name { get; set; }
        public int Level { get; set; }

        public string Rank { get; set; } // 직급
        public string ClassName { get; set; }
        
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int MaxMP { get; set; } // 최대 마나 포인트
        public int MP { get; set; } // 마나 포인트
        public double Attack { get; set; }
        public int Defense { get; set; }
        public int Gold { get; set; }
        public bool IEquipedDefense { get; set; }
        public bool IEquipedAttack { get; set; }
        public int ClearedCount { get; set; }


        //역직렬용 생성자
        public Character(){  }

        //캐릭터 생성자
        public Character(string name, string className, int level, string rank, int maxhealth, int health, int maxMp, int mp, double attack, int defense, int gold)
        {
            Name = name;
            ClassName = className;
            Level = level;
            Rank = rank;
            MaxHealth = maxhealth;
            Health = health;
            MaxMP = maxMp;
            MP = mp;
            Attack = attack;
            Defense = defense;
            Gold = gold;
        }

        // 상태 보기
        public void ShowStatus()
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine($"Lv. {Level} ({Rank})");
            Console.WriteLine($"{Name} ({ClassName})");
            Console.WriteLine($"공격력 : {Attack}");
            Console.WriteLine($"방어력 : {Defense}");
            Console.WriteLine($"체력 : {Health}");
            Console.WriteLine($"마나 : {MP}");
            Console.WriteLine($"소지금: {Gold} 원");
            Console.WriteLine("-----------------------------");

            Console.WriteLine("\n0. 나가기\n");
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
        }

        //캐릭터 생성 메서드
        public static void MakeCharacter(Character character)
        {
            Console.WriteLine("캐릭터를 생성합니다.");
            Console.Write("이름을 입력하세요 : ");
            character.Name = Console.ReadLine();
            Console.Write("희망 부서를 입력하세요.\n--부서 리스트--\n1.인사팀\n2.홍보팀\n3.총무팀\n4.영업팀\n5.전산팀\n6.기획팀\n>>");

            //직업 선택 (번호에 따라 직업 결정. enum Departments 사용) > 이후 직업에 따른 스탯 부여

           character.ClassName = Enum.GetName(typeof(Departments), Convert.ToInt32(Console.ReadLine()));


            //기본 스탯
            character.Level = 1;
            character.Rank = Enum.GetName(typeof(Ranks), 1); // 대리
            character.MaxHealth = 100; // 최대 체력
            character.Health = 100;
            character.MaxMP = 50; // 최대 마나 포인트
            character.MP = 50;
            character.Attack = 10;
            character.Defense = 5;
            character.Gold = 0;

            Console.Clear();
        }

        //캐릭터 공격 메서드
        //타겟은 메인 스크립트에서 선택했다고 가정
        public static void AttackMethod(Character character, Monster monster)
        {
            int DamageMargin = (int)character.Attack / 10; // 공격력의 10%를 사용하여 공격을 수행하는 메소드.
            //나누기 후 소수점(나머지)가 있을 경우 올림처리
            if (DamageMargin % 10 != 0)
            {
                DamageMargin = DamageMargin / 10 + 1;
            }

            //공격 시 대미지 범위 설정 (11일 경우 10-2부터 10+2까지)
            int damageRange = new Random().Next((int)character.Attack - DamageMargin, (int)character.Attack + DamageMargin + 1);

            //공격 시 일정 확률로 크리티컬 혹은 miss 발생
            //크리티컬 공격
            Random probability = new Random();
            int critical = probability.Next(1, 101); // 15% 확률로 크리티컬 공격 발생
            int miss = probability.Next(1, 101); // 10% 확률로 miss 발생

            // level 옆에 몬스터 이름 추가 해야함
            if (critical <= 15)
            {
                //크리티컬 공격
                //크리티컬 공격력 = 공격력 * 1.6
                Console.WriteLine($"{character.Name}의 크리티컬 공격!");
                Console.WriteLine($"Lv.{monster.Level} {monster.Name}에게 {damageRange * 1.6}의 피해를 입혔습니다.");

                //타겟 체력 감소- Monster 클래스의 Health를 사용
                monster.Health -= (int)(damageRange * 1.6); // 몬스터의 체력 감소

            }
            else if (miss <= 10) //크리티컬이 발동하면 miss는 발동하지 않음
            {
                //miss 공격
                Console.WriteLine($"{character.Name}의 공격!");
                Console.WriteLine($"Lv.{monster.Level} {monster.Name}을(를) 공격했지만 아무일도 일어나지 않았습니다.");

            }
            else
            {
                //일반 공격
                Console.WriteLine($"{character.Name}의 공격!");
                Console.WriteLine($"Lv.{monster.Level} {monster.Name}에게 {damageRange}의 피해를 입혔습니다.");
                //타겟 체력 감소- Monster 클래스의 Health를 사용
                monster.Health -= damageRange; // 몬스터의 체력 감소

            }
            //몬스터가 죽었을 경우

            if (monster.Health <= 0)
            {
                monster.Health = 0; // 몬스터의 체력을 0으로 설정
            }

        }


    }

}
