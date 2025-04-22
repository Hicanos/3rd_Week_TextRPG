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
        // 캐릭터 스킬 변수 정의
        public class Skill
        {
            public string SkillName { get; set; }
            public string SkillDescription { get; set; }
            public int SkillMPCost { get; set; }
            public int SkillCooldown { get; set; }
            public string SkillType { get; set; } // "Active" or "Passive"
            public string SkillCastMessage { get; set; }
            public Skill(string skillname, string skilldescription, int skillmpCost, int skillcooldown, string skillType, string skillcastMessage)
            {
                SkillName = skillname;
                SkillDescription = skilldescription;
                SkillMPCost = skillmpCost;
                SkillCooldown = skillcooldown;
                SkillType = skillType;
                SkillCastMessage = skillcastMessage;
            }
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


        // 스킬 클래스

        public static Dictionary<string, List<Skill>> DepartmentSkills = new Dictionary<string, List<Skill>>()
    {
        {
            "인사팀", new List<Skill>()
            {
                new Skill("인사평가", "적 전체에게 범위 딜(공격력 * 1.3), 2턴 명중률 +30%", 20, 2, "Active", "반갑다 내 주먹은 10점 만점에 몇 점이지?"),
                new Skill("사내 공지", "자신에게 명중률/치명타확률 +15% (3턴)", 30, 4, "Active", "공지합니다 이제부터 이승 퇴직 권고하겠습니다."),
                new Skill("HR 감시망", "적 회피 -10%, 매 턴 자신 방어력 + 2", 0, 0, "Passive", "")
            }
        },
        {
            "홍보팀", new List<Skill>()
            {
                new Skill("대외 홍보", "적 전체 마나 기반 마법 피해(최대 마나 * 0.3)", 20, 3, "Active", "이 제품으로 설명드릴 것 같으면요 쉽게 볼 수 없는 자연산 구라입니다."),
                new Skill("이미지 메이킹", "자신 회피 +25% (3턴), 체력 회복 + 20%", 35, 3, "Active", "겉 모습이 좋아야 잘 먹히는 법"),
                new Skill("이목 집중", "전투 시 첫 공격 치명타 확률 40% 증가 + 피해 30% 감소", 0, 0, "Passive", "")
            }
        },
        {
            "총무팀", new List<Skill>()
            {
                new Skill("예산 통제", "대상 적 대미지(공격력 *1.6), 공격력 -30% (2턴)", 20, 2, "Active", "이건 예산이 부족하겠는데요?"),
                new Skill("비품 지원", "자신 체력 회복(최대체력*15%) + 방어력 상승 (2턴)", 35, 3, "Active", "회사 지원이라도 좋아야 일할 맛이 나지"),
                new Skill("운영의 달인", "아이템 되팔기 금액 + 20% 증가, 전투 보상 현금 + 20%", 0, 0, "Passive", "")
            }
        },
        {
            "영업팀", new List<Skill>()
            {
                new Skill("실적 압박", "치명타 확률 +50%의 강력 단일 대미지(공격력 * 1.5)", 25, 3, "Active", "본인이 하신 거 맞아요? 왜 이렇게 하셨죠?"),
                new Skill("끈질긴 설득", "적 전체 명중률 감소 - 50%(2턴), 자신 공격력 증가 + 20%(3턴)", 30, 4, "Active", "내가 매번 듣던 말이 있지. 안되면 되게 하라, 센스있게 잘 좀 하자"),
                new Skill("목표는 무조건 달성", "적 처치 시 마나 회복 + 10, 치명타 대미지 + 20%(1턴)", 0, 0, "Passive", "")
            }
        },
        {
            "전산팀", new List<Skill>()
            {
                new Skill("긴급 패치", "자신 방어력 5~10 증가(2턴) + 체력(최대체력*10%) 회복, 적 전체 공격력 – 20%", 30, 4, "Active", "긴급 패치 들어가겠습니다. 협조바랍니다."),
                new Skill("시스템 다운", "적 전체 대미지(공격력 * 1.5) + 명중률 감소 – 20% (2턴)", 35, 3, "Active", "시스템이 튼튼해야 문제가 없지."),
                new Skill("백업 시스템", "체력이 50% 이하일 때 공격력 + 30%, 치명타 확률 + 20%", 0, 0, "Passive", "")
            }
        },
        {
            "기획팀", new List<Skill>()
            {
                new Skill("기획안 폭격", "자신에게 치명타 피해량 + 50%(2턴), 적 전체에 피해(공격력 * 1.2)", 25, 5, "Active", "여기 기획안 확인하셨죠? 이것도 보시고 작업해주세요."),
                new Skill("리스크 분석", "자신에게 회피 + 50%(3턴)", 30, 3, "Active", "이 문제는 회의를 통해서 진행하시죠"),
                new Skill("컨셉 잡았다", "매턴 치명타 확률 5% 증가, 명중률 3% 증가", 0, 0, "Passive", "")
            }
        }
    };

        public List<Skill> Skills { get; set; } = new List<Skill>();

        public void ShowSkills()
        {
            Console.Clear();
            if (DepartmentSkills.ContainsKey(ClassName))
            {
                Console.WriteLine($"\n [{ClassName}] 스킬 목록:");
                foreach (var skill in DepartmentSkills[ClassName])
                {
                    Console.WriteLine($"\n {skill.SkillName} ({skill.SkillType})");
                    Console.WriteLine($"  설명: {skill.SkillDescription}");
                    if (skill.SkillType == "Active")
                    {
                        Console.WriteLine($"  MP 소모: {skill.SkillMPCost}, 쿨타임: {skill.SkillCooldown}");
                        if (!string.IsNullOrWhiteSpace(skill.SkillCastMessage))
                            Console.WriteLine($"  대사: \"{skill.SkillCastMessage}\"");
                    }
                }
            }
            else
            {
                Console.WriteLine("스킬이 없습니다.");
            }

            Console.WriteLine("\n아무키나 입력하여 돌아가기");
            Console.ReadLine();
        }

        public void InitializeSkills()
        {
            Skills.Clear(); // 기존 스킬 초기화
        }




        // 상태 보기
        public void ShowStatus()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("-----------------------------");
                Console.WriteLine($"Lv. {Level} ({Rank})");
                Console.WriteLine($"{Name} ({ClassName})");
                Console.WriteLine($"공격력 : {Attack}");
                Console.WriteLine($"방어력 : {Defense}");
                Console.WriteLine($"체력 : {Health}");
                Console.WriteLine($"마나 : {MP}");
                Console.WriteLine($"소지금: {Gold} 원");
                Console.WriteLine("-----------------------------");

                Console.WriteLine("1. 스킬 보기");
                Console.WriteLine("0. 나가기");
                Console.Write(">> ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ShowSkills();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 엔터를 누르세요.");
                        Console.ReadLine();
                        break;
                }
            }
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

            character.InitializeSkills(); // 스킬 초기화
            
            Console.Clear();


        }





        

        //캐릭터 공격 메서드
        //타겟은 메인 스크립트에서 선택했다고 가정
        public void AttackMethod()
        {
            int DamageMargin = (int)Attack / 10; // 공격력의 10%를 사용하여 공격을 수행하는 메소드.
            //나누기 후 소수점(나머지)가 있을 경우 올림처리
            if (DamageMargin % 10 != 0)
            {
                DamageMargin = DamageMargin / 10 + 1;
            }

            //공격 시 대미지 범위 설정 (11일 경우 10-2부터 10+2까지)
            int damageRange = new Random().Next((int)Attack - DamageMargin, (int)Attack + DamageMargin + 1);

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
                Console.WriteLine($"{Name}의 크리티컬 공격!");
                Console.WriteLine($"Lv. {Level}의 적에게 {damageRange * 1.6}의 피해를 입혔습니다.");

                //타겟 체력 감소- Monster 클래스의 Health를 사용
                Monster.currentBattleMonsters[0].Health -= (int)(damageRange * 1.6); // 몬스터의 체력 감소

            }
            else if (miss <= 10) //크리티컬이 발동하면 miss는 발동하지 않음
            {
                //miss 공격
                Console.WriteLine($"{Name}의 공격!");
                Console.WriteLine($"Lv. {Level}의 적을 공격했지만 아무일도 일어나지 않았습니다.");

            }
            else
            {
                //일반 공격
                Console.WriteLine($"{Name}의 공격!");
                Console.WriteLine($"Lv. {Level}의 적에게 {damageRange}의 피해를 입혔습니다.");
                //타겟 체력 감소- Monster 클래스의 Health를 사용
                Monster.currentBattleMonsters[0].Health -= damageRange; // 몬스터의 체력 감소

            }
        }





    }

}
