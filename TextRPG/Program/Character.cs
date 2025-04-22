using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        부회장,
        회장
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
        public Character(string name, string className, int level, int maxhealth, int health, int maxMp, int mp, double attack, int defense, int gold)
        {
            Name = name;
            ClassName = className;
            Level = level;
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
            Console.WriteLine($"Lv. {Level}");
            Console.WriteLine($"{Name} ( {ClassName} )");
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
        public void MakeCharacter()
        {
            Console.WriteLine("캐릭터를 생성합니다.");
            Console.Write("이름을 입력하세요 : ");
            Name = Console.ReadLine();
            Console.Write("희망 부서를 입력하세요.\n--부서 리스트--\n1.인사팀\n2.홍보팀\n3.총무팀\n4.영업팀\n5.전산팀\n6.기획팀 ");

            //직업 선택 (번호에 따라 직업 결정. enum Departments 사용) > 이후 직업에 따른 스탯 부여

            ClassName = Enum.GetName(typeof(Departments), Convert.ToInt32(Console.ReadLine()));


            //기본 스탯
            Level = 1;
            MaxHealth = 100; // 최대 체력
            Health = 100;
            MaxMP = 50; // 최대 마나 포인트
            MP = 50;
            Attack = 10;
            Defense = 5;
            Gold = 0;
        }

        //캐릭터 공격 메서드
        //타겟은 메인 스크립트에서 선택했다고 가정
        public void AttackMethod()
        {
            float Damage = (float)Attack * 0.1f; // 공격력의 10%를 사용하여 공격을 수행하는 메소드.
            int damageRange = new Random().Next();
        }

        public int CrIticalAttack() // 캐릭터가 공격을 수행할때 15%확률로 160%의
                                    // 치명타 공격을 하게 해주는 메소드.
        {
            Random rnd = new Random();

            // 0~99까지의 랜덤값을 생성하여 15보다 작으면 true
            bool Critical = rnd.Next(0, 100) < 15;

            // true면 치명타 공격력, false면 일반 공격력
            // ? (true) : (false) 삼항 연산자
            int damage = Critical ? (int)(Attack * 1.6) : (int)Attack;

            if (Critical)
            {
                Console.WriteLine($"{Name}의 치명타 공격!");  // level 옆에 몬스터 이름 추가 해야함! 
                Console.WriteLine($"Lv. {Level}의 적에게 {damage}의 피해를 입혔습니다.");
            }
            else
            {
                Console.WriteLine($"{Name}의 공격!");// level 옆에 몬스터 이름 추가 해야함! 
                Console.WriteLine($"Lv. {Level}의 적에게 {damage}의 피해를 입혔습니다.");
            }
            return damage;
        }

        // 몬스터 객채 설정을 안해서 빨간줄이 뜨는거라고 함. 
        public int MissAttack() // 10% 확률로 공격이 빗나가게 해주는 메소드.
        {
            Random rnd = new Random();
            // 0~99까지의 랜덤값을 생성하여 10보다 작으면 true
            bool Miss = rnd.Next(0, 100) < 10;
            // true면 빗나간 공격력, false면 일반 공격력 
            int miss = Miss ? (int)(Attack * 0) : (int)Attack;

            if (Miss)
            {
                Console.WriteLine($"{Name}의 공격!"); // level 옆에 몬스터 이름 추가 해야함!
                Console.WriteLine($"Lv. {Level}의 적을 공격했지만 아무일도 일어나지 않았습니다.");
            }
            else
            {
                Console.WriteLine($"{Name}의 공격!");// level 옆에 몬스터 이름 추가 해야함! 
                Console.WriteLine($"Lv. {Level}의 적에게 {miss}의 피해를 입혔습니다.");
            }
            return miss;
        }
    }

}
