using Newtonsoft.Json;
using TextRPG.MonsterManagement;
using TextRPG.SkillManagement;
using TextRPG.TitleManagement;
using TextRPG.WeaponManagement;

namespace TextRPG.CharacterManagement
{

    enum Departments
    {
        인사팀 = 1,
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
        실장,
        이사,
        상무,
        전무,
        사장,
        부회장,
        회장,
    }

    public struct Stats
    {
        public double Attack;
        public int Defense;
        public int MaxHealth;
        public int Health;
        public int MaxMP;
        public int MP;
        public int DEX;
        public int CRIT;
        public float CRITDMG;
        public int EVA;
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

        public Title EquippedTitle { get; set; }
        public int DEX { get; set; } // 민첩=명중률
        public int CRIT { get; set; } // 치명타 확률
        public float CRITDMG { get; set; } // 치명타 대미지 배수
        public int EVA { get; set; } // 회피율

        public int EXP { get; set; } // 경험치
        public double MaxEXP { get; set; } // 필요 경험치
        public List<Weapons> NotbuyAbleInventory { get; set; } = new List<Weapons>();
        public HashSet<string> PurchasedOnceItems { get; set; } = new HashSet<string>(); // 아이템 구매 기록 추가
       
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto)]
        public List<Skill> Skills { get; set; } = new List<Skill>(); //스킬 추가용 프로퍼티


        public bool YouClearedGame { get; set; }



        //역직렬용 생성자
        public Character() { }

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
            EXP = 0;
            MaxEXP = 15 * Level; //기존 필요 경험치 공식 (2.5*level^2 + 17.5*level - 10)
        }

        public void AssignSkills()
        {
            // 모든 스킬 로드
            var allSkills = SkillLoader.LoadSkillObjects();

            // ClassName과 일치하는 스킬 필터링
            Skills = allSkills.Where(skill => skill.ClassName == ClassName).ToList();
        }

        public void ShowSkillList() 
        {
            // 스킬 리스트 출력
            Console.WriteLine("\n[보유 스킬]");
            if (Skills.Count > 0)
            {
                foreach (var skill in Skills)
                {
                    Console.WriteLine($"- {skill.SkillName} (MP 소모: {skill.CostMP}, 쿨타임: {skill.CoolTime}턴)");
                    Console.WriteLine($"  설명: {skill.SkillDescription}");
                }
            }
            else
            {
                Console.WriteLine("보유한 스킬이 없습니다.");
            }
        }

        // 상태 보기
        public void ShowStatus()
        {
            Console.Clear();
            Console.WriteLine("-----------------------------");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Lv. {Level} ({Rank})");
            Console.WriteLine($"{Name} ({ClassName})");
            Console.WriteLine($"공격력 : {Attack}");
            Console.WriteLine($"방어력 : {Defense}");
            Console.WriteLine($"체력 : {Health}");
            Console.WriteLine($"마나 : {MP}");
            Console.WriteLine($"명중률 : {DEX}");
            Console.WriteLine($"회피율 : {EVA}");
            Console.WriteLine($"소지금: {Gold} 원");
            Console.WriteLine($"칭호 : {EquippedTitle?.Name ?? "없음"}");
            Console.ResetColor();

            AssignSkills();
            ShowSkillList();
            Console.WriteLine("\n-----------------------------");
            Console.ResetColor();
            Console.WriteLine("\n0. 나가기\n");
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
        }


        Dictionary<Departments, Stats> classStats = new Dictionary<Departments, Stats>
        {
            { Departments.인사팀, new Stats { Attack = 8, Defense = 8, MaxHealth = 100, Health = 100, MaxMP = 70, MP = 70, DEX = 80, CRIT = 24, CRITDMG = 1.4f, EVA = 12 }},
            { Departments.홍보팀, new Stats { Attack = 10, Defense = 4, MaxHealth = 90, Health = 90, MaxMP = 80, MP = 80, DEX = 85, CRIT = 18, CRITDMG = 1.6f, EVA = 16 }},
            { Departments.총무팀, new Stats { Attack = 8, Defense = 10, MaxHealth = 100, Health = 100, MaxMP = 70, MP = 70, DEX = 75, CRIT = 22, CRITDMG = 1.6f, EVA = 8 }},
            { Departments.영업팀, new Stats { Attack = 14, Defense = 4, MaxHealth = 90, Health = 90, MaxMP = 70, MP = 70, DEX = 80, CRIT = 22, CRITDMG = 1.6f, EVA = 8 }},
            { Departments.전산팀, new Stats { Attack = 12, Defense = 6, MaxHealth = 80, Health = 80, MaxMP = 90, MP = 90, DEX = 80, CRIT = 22, CRITDMG = 1.5f, EVA = 8 }},
            { Departments.기획팀, new Stats { Attack = 10, Defense = 8, MaxHealth = 80, Health = 80, MaxMP = 80, MP = 80, DEX = 70, CRIT = 22, CRITDMG = 1.7f, EVA = 12 }}
        };

        //캐릭터 생성 메서드
        public static void MakeCharacter(Character character)
        {
            Console.WriteLine("캐릭터를 생성합니다.");
            Console.Write("이름을 입력하세요 : ");
            //이름 입력-공백 포함 불가

            while (true)
            {
                character.Name = Console.ReadLine();

                if (character.Name.Contains(' '))
                {
                    Console.WriteLine("이름에 공백이 포함될 수 없습니다.");
                    Console.WriteLine("이름을 입력하세요 : ");

                    continue;
                }

                // 이름이 유효하면 반복문 종료
                break;
            }

            Console.Write("\n희망 부서를 입력하세요.\n--부서 리스트--\n1.인사팀: 정곡을 찌르는 HR 전략으로 치명타 확률이 높고, 사람을 상대하는 만큼 안정감(방어력, 체력)이 있음.\n2.홍보팀: 말발로 명중률이 높고, 이미지를 꾸미는 데 능숙해 회피율과 치명타도 강함.\n3.총무팀: 회사의 실질적 방패. 강한 방어력과 치명타 운영 능력을 지님.\n4.영업팀: 공격적인 자세로 거래를 성사시키는 딜러형. 치명타 위주의 세팅.\n5.전산팀: 기술 기반 마법 직군. 높은 마나와 명중률로 스킬 캐스터 역할.\n6.기획팀: 전략과 기획으로 정밀한 타격을 주는 포지션. 치명타 특화.\n>>");

            //직업 선택 (번호에 따라 직업 결정. enum Departments 사용) > 이후 직업에 따른 스탯 부여

            int departmentChoice;
            while (!int.TryParse(Console.ReadLine(), out departmentChoice) || !Enum.IsDefined(typeof(Departments), departmentChoice))
            {
                Console.WriteLine("유효한 번호를 입력하세요.");
                Console.Write("선택: ");
            }

            //캐릭터 직업에 따른 스탯 부여
            Departments selectedDepartment = (Departments)departmentChoice;
            character.ClassName = selectedDepartment.ToString();

            if (character.classStats.TryGetValue(selectedDepartment, out Stats selectedStats))
            {
                character.Attack = selectedStats.Attack;
                character.Defense = selectedStats.Defense;
                character.MaxHealth = selectedStats.MaxHealth;
                character.Health = selectedStats.Health;
                character.MaxMP = selectedStats.MaxMP;
                character.MP = selectedStats.MP;
                character.DEX = selectedStats.DEX;
                character.CRIT = selectedStats.CRIT;
                character.CRITDMG = selectedStats.CRITDMG;
                character.EVA = selectedStats.EVA;
            }

            //캐릭터 직업에 따른 스킬 부여
            character.AssignSkills();

            character.Gold = 1500;



            Console.Clear();
        }


        //캐릭터 레벨업 메서드
        public void LevelUP()
        {
            if (EXP >= MaxEXP)
            {
                while (EXP >= MaxEXP)
                {
                    EXP -= (int)MaxEXP; // 남은 경험치
                    Level++;
                    ApplyBenefits(); // 레벨업 시 스탯 증가
                    MaxEXP = 15 * Level; //MaxEXP 갱신
                    //레벨업 시 클래스이름-랭크 증가(직급 상승)
                    if (!YouClearedGame)
                    {
                        Rank = Enum.GetName(typeof(Ranks), Level);
                        Console.WriteLine($"{Name}이(가) {Rank}(으)로 승진했습니다! 현재 레벨: {Level}");
                    }
                }
            }
        }

        //레벨업 시 스탯 증가
        private void ApplyBenefits()
        {
            MaxHealth += 10;    // 체력 증가
            Attack += 0.5;      // 공격력 증가
            Defense += 1;       // 방어력 증가
            MaxMP += 5;         // 마나 증가
        }

        //캐릭터 공격 메서드
        //타겟은 메인 스크립트에서 선택했다고 가정
        public static void AttackMethod(Character character, Monster monster)
        {
            //공격력이 11일때 나머지가 있으므로 오차범위 +1됨.
            int DamageMargin;
            //나누기 후 소수점(나머지)가 있을 경우 올림처리
            if ((int)character.Attack % 10 != 0)
            {
                DamageMargin = (int)character.Attack / 10 + 1;
            }
            else //없으면 그대로 오차범위 확정
            {
                DamageMargin = (int)character.Attack / 10;
            }

            //공격 시 대미지 범위 설정 (11일 경우 10-2부터 10+2까지) 중 랜덤으로 들어감
            int damage = new Random().Next((int)character.Attack - DamageMargin, (int)character.Attack + DamageMargin + 1);

            //공격 시 일정 확률로 크리티컬 혹은 miss 발생
            //크리티컬 공격
            Random probability = new Random();
            int Accuracy = probability.Next(1, 101); // 명중률에 따라 일반공격 적용
            int critical = probability.Next(1, 101); // 15% 확률로 크리티컬 공격 발생

            //최종 명중률 = 공격자 명중률 - 상대방 회피율
            //monster Attack을 이후 EVA 데이터가 만들어지면 교체할 것.           
            if (Accuracy <= character.DEX - monster.Attack)
            {
                if (critical <= character.CRIT)
                {
                    //크리티컬 공격
                    //크리티컬 공격력 = 최종대미지*치명타 대미지 배수
                    int critDamage = (int)(damage * character.CRITDMG);
                    Console.WriteLine($"{character.Name}의 크리티컬 공격!");
                    Console.WriteLine($"Lv.{monster.Level} {monster.Name}에게 {critDamage}의 피해를 입혔습니다.");

                    //타겟 체력 감소- Monster 클래스의 Health를 사용
                    monster.Health -= critDamage; // 몬스터의 체력 감소
                }
                else
                {
                    //일반 공격
                    Console.WriteLine($"{character.Name}의 공격!");
                    Console.WriteLine($"Lv.{monster.Level} {monster.Name}에게 {damage}의 피해를 입혔습니다.");
                    //타겟 체력 감소- Monster 클래스의 Health를 사용
                    monster.Health -= damage; // 몬스터의 체력 감소
                }
            }
            else
            {
                //miss
                Console.WriteLine($"{character.Name}의 공격!");
                Console.WriteLine($"Lv.{monster.Level} {monster.Name}을(를) 공격했지만 아무일도 일어나지 않았습니다.");
            }

            if (monster.Health <= 0)
            {
                monster.Health = 0;                
                BattleManager.CheckDead(character);
            } //몬스터 체력이 0 이하로 떨어지지 않도록 처리

        }


    }

}
