using TextRPG.CharacterManagemant;
using TextRPG.MonsterManagement;

namespace TextRPG.SkillManagement
{
    //스킬 관리 클래스
    //직업별(character.ClassName)로 다른 스킬을 AddSkill() 메서드에 추가 >캐릭터cs에 넣을 예정
    //해당 스킬이 사용 가능일 때, UseSkill 발동-해당 로직을 Battle-스킬사용으로 옮길 것
    // if (!CanUseSkill(character, costMP, cooldown)) return;


    //스킬 클래스
    public abstract class Skill
    {
        //사용직업/스킬 이름/스킬 설명/소모 마나/쿨타임/스킬타입 /효과 유지 턴

        public string className { get; set; } //스킬 사용 직업
        public string skillName { get; set; }
        public string skillDescription { get; set; }
        public int costMP { get; set; }
        public int cooldown { get; set; }
        public int effectDuration { get; set; } //효과 유지 턴
        public bool isEffectActive { get; set; } //효과 활성화 여부
        public int tagetType { get; set; } //타겟 종류 1. 자신 2. 적 단일 3. 적 n체 4. 적 전체

        //액티브/패시브 스킬 구분
        public bool isActive { get; set; } //true: 액티브, false: 패시브

        //스킬 사용
        public abstract void UseSkill(Character character, Monster monster);

        //스킬의 쿨타임 감소
        public void ReduceCooldown()
        {
            if (cooldown > 0)
            {
                cooldown--;
            }
            if (effectDuration > 0)
            {
                effectDuration--;
            }
        }

        //부가효과 활성화 여부
        public void isEffective(Character character)
        {
            Console.WriteLine($"{skillName} 효과가 적용 중입니다.");
            return;
        }

        //효과 종료
        public void EffectRemove()
        {
            Console.WriteLine($"{skillName} 효과가 종료되었습니다.");
            isEffectActive = false; // 효과 비활성화
        }

        //쿨타임 중
        public void coolTime()
        {
            Console.WriteLine($"{skillName}은(는) 쿨타임 중입니다. 남은 쿨타임: {cooldown}턴");
        }

        //기본 스킬사용 로직
        public void SkillUse(Character character, Monster monster)
        {
            Console.WriteLine($"{character.Name}이(가) {monster.Name}에게 {skillName}을(를) 사용!");
            Console.WriteLine($"{character.Name}: {skillDescription}");
            character.MP -= costMP;
        }

        public void BuffUse(Character character)
        {
            Console.WriteLine($"{character.Name}이(가) {character.Name}에게 {skillName}을(를) 사용!");
            Console.WriteLine($"{character.Name}: {skillDescription}");
            character.MP -= costMP;
        }

        //스킬 사용 가능 확인
        public bool CanUseSkill(Character character, int costMP, int cooldown)
        {
            if (cooldown > 0)
            {
                coolTime();
                return false;
            }

            if (character.MP < costMP)
            {
                Console.WriteLine("MP가 부족합니다.");
                return false;
            }
            return true;
        }
    }

    //인사팀 스킬 3개
    public class PersonnelEvaluation : Skill
    {
        public PersonnelEvaluation()
        {
            className = "인사팀";
            skillName = "인사평가";
            skillDescription = "반갑다 내 주먹은 10점 만점에 몇 점이지?";
            costMP = 20;
            cooldown = 2;
            isActive = true;
            effectDuration = 2;
            isEffectActive = false; // 효과가 활성화되지 않은 상태로 초기화
            int tagetType = 4; //타겟 종류 1. 자신 2. 적 단일 3. 적 n체 4. 적 전체
        }

        public override void UseSkill(Character character, Monster monster)
        {
            SkillUse(character, monster); //스킬 사용 선언

            //"적 전체에게 범위 딜(공격력 * 1.3), 2턴 명중률 +30%"
            int skillDmg = (int)(character.Attack * 1.3);//공격력 * 1.3
            monster.Health -= skillDmg;

            Console.WriteLine($"LV.{monster.Level} {monster.Name}에게 {skillDmg}의 피해를 입혔습니다.");
            ApplyEffect(character); //부가효과 적용
            cooldown = 2; // 쿨타임 초기화

        }

        public void ApplyEffect(Character character)
        {
            if (isEffectActive) isEffective(character);
            else
            {
                isEffectActive = true; // 효과 활성화
                effectDuration = 2; //효과 적용 시작
                Console.WriteLine($"{character.Name}의 명중률이 30% 증가했습니다.");
                //2턴 동안 명중률 +30%
                character.DEX += 30;
            }

            if (effectDuration <= 0)
            {
                character.DEX -= 30; // 효과 종료
                EffectRemove();
            }
        }

    }

    public class NoticeToEmployees : Skill
    {
        public NoticeToEmployees()
        {
            className = "인사팀";
            skillName = "사내 공지";
            skillDescription = "공지합니다 이제부터 이승 퇴직 권고하겠습니다.";
            costMP = 30;
            cooldown = 4;
            isActive = true;
            effectDuration = 3;
            int tagetType = 1;
        }
        public override void UseSkill(Character character, Monster monster)
        {
            BuffUse(character);
            ApplyEffect(character);
            cooldown = 4; //쿨타임 초기화
        }
        public void ApplyEffect(Character character)
        {
            if (isEffectActive) isEffective(character);
            else
            {
                isEffectActive = true;
                effectDuration = 3;
                character.DEX += 15;
                character.CRIT += 15;
                Console.WriteLine($"{character.Name}의 명중률과 치명타 확률이 상승합니다!");
            }

            if (effectDuration <= 0)
            {
                character.DEX -= 15;
                character.CRIT -= 15;
                EffectRemove();
            }
        }
    }

    public class HRMonitoringNetwork : Skill
    {
        public HRMonitoringNetwork()
        {
            className = "인사팀";
            skillName = "HR 감시망";
            costMP = 0;
            isActive = false; //패시브                
        }

        //매 턴 시작 시 자동 적용
        public override void UseSkill(Character character, Monster monster)
        {
            Console.WriteLine($"Passive: {skillName}");
            //monster.DEX -= 10; // 적 회피 -10%
            character.Defense += 2; // 매 턴 자신 방어력 + 2
        }

        public void BattleOver(Character character)
        {
            //매 턴 올라간 만큼 초기화
            //character.Defense -= 2*turn; //게임의 매 턴을 계산함
        }

    }

    //홍보팀
    public class PublicRelations : Skill
    {
        public PublicRelations()
        {
            className = "홍보팀";
            skillName = "대외 홍보";
            skillDescription = "30초 광고 들어가겠습니다. 스킵 버튼은 없습니다.";
            costMP = 20;
            cooldown = 3;
            isActive = true;
        }
        public override void UseSkill(Character character, Monster monster)
        {
            SkillUse(character, monster);
            int skillDmg = (int)(character.MaxMP * 0.3);
            Console.WriteLine($"LV.{monster.Level} {monster.Name}에게 {skillDmg}의 피해를 입혔습니다.");
            cooldown = 3;
        }
    }

    public class ImageMaking : Skill
    {
        public ImageMaking()
        {
            className = "홍보팀";
            skillName = "이미지 메이킹";
            skillDescription = "겉 모습이 좋아야 잘 먹히는 법";
            costMP = 35;
            cooldown = 3;
            isActive = true;
            effectDuration = 3;
        }

        public override void UseSkill(Character character, Monster monster)
        {
            BuffUse(character);
            //캐릭터 최대 체력의 20% 회복
            int skillHeal = (int)(character.MaxHealth * 0.2);
            character.Health += skillHeal;
            Console.WriteLine($"{character.Name}의 체력이 {skillHeal} 회복되었습니다.");
            ApplyEffect(character);
        }

        public void ApplyEffect(Character character)
        {
            if (isEffectActive) isEffective(character);
            else
            {
                isEffectActive = true;
                effectDuration = 3;
                character.EVA += 25;
                Console.WriteLine($"{character.Name}의 회피율이 상승합니다!");

            }

            if (effectDuration <= 0)
            {
                character.EVA -= 25;
                EffectRemove();
            }

        }
    }
    public class Attention : Skill
    {
        int bonus;
        public Attention()
        {
            className = "홍보팀";
            skillName = "이목집중";
            costMP = 0;
            isActive = false;
        }

        public override void UseSkill(Character character, Monster monster)
        {

            int turn = 1; //turn은 battle 부분에서 끌고오기
            if (turn == 1)
            {
                bonus = (int)(character.Defense * 0.3);
                Console.WriteLine($"Passive: {skillName}");
                character.CRIT += 40;
                character.Defense += bonus;
                Console.WriteLine($"{character.Name}의 치명타 확률과 방어력이 상승했습니다!");
            }
            else if (turn == 2)
            {
                EffectRemove();
                character.Defense -= bonus;
            }
        }

    }

    //총무팀 스킬 3개
    public class BudgetaryControl : Skill
    {
        public BudgetaryControl()
        {
            className = "총무팀";
            skillName = "예산 통제";
            skillDescription = "이건 예산이 부족하겠는데요?";
            costMP = 20;
            cooldown = 2;
            effectDuration = 2;
            isActive = true;
        }

        public override void UseSkill(Character character, Monster monster)
        {
            SkillUse(character, monster);
            int skillDmg = (int)(character.Attack * 1.6);
            monster.Health -= skillDmg;
            Console.WriteLine($"LV.{monster.Level} {monster.Name}에게 {skillDmg}의 피해를 입혔습니다.");
            ApplyEffect(monster);
        }

        public void ApplyEffect(Monster monster)
        {
            int AttackOrigin = monster.Attack; //기존 공격력 저장
            if (!isEffectActive)
            {
                isEffectActive = true;
                effectDuration = 2;
                monster.Attack = (int)(monster.Attack * 0.7); //타겟 공격력 감소
                Console.WriteLine($"LV.{monster.Level} {monster.Name}의 공격력이 감소합니다!");

            }

            if (effectDuration <= 0)
            {
                monster.Attack = AttackOrigin; //타겟 공격력 복구
                EffectRemove();
            }
        }

    }

    public class SuppliesSupport : Skill
    {
        public SuppliesSupport()
        {
            className = "총무팀";
            skillName = "비품 지원";
            skillDescription = "회사 지원이라도 좋아야 일할 맛이 나지";
            costMP = 35;
            cooldown = 3;
            isActive = true;
            effectDuration = 2;
        }

        public override void UseSkill(Character character, Monster monster)
        {
            BuffUse(character);
            int skillHeal = (int)(character.MaxHealth * 0.15);
            character.Health += skillHeal;
            Console.WriteLine($"{character.Name}의 체력이 {skillHeal} 회복되었습니다.");
        }

        public void ApplyEffect(Character character)
        {
            if (isEffectActive) isEffective(character);
            else
            {
                //방어력에 정확한 수치가 없어서 임의로 5로 정함
                isEffectActive = true;
                effectDuration = 2;
                character.Defense += 5;
                Console.WriteLine($"{character.Name}의 방어력이 상승합니다!");
            }
            if (effectDuration <= 0)
            {
                character.Defense -= 5;
                EffectRemove();
            }
        }
    }

    public class MasterOfOperation : Skill
    {
        public MasterOfOperation()
        {
            className = "총무팀";
            skillName = "운영의 달인";
            isActive = false;
        }

        public override void UseSkill(Character character, Monster monster)
        {
            //상점 판매 매서드와 결과 창에서 직업 확인 후 패시브 적용
        }
    }

    //영업팀
    public class PerformancePressure : Skill
    {
        public PerformancePressure()
        {
            className = "영업팀";
            skillName = "실적 압박";
            skillDescription = "본인이 하신 거 맞아요? 왜 이렇게 하셨죠?";
            isActive = true;
            costMP = 25;
            cooldown = 3;

        }
        public override void UseSkill(Character character, Monster monster)
        {
            SkillUse(character, monster);
            character.CRIT += 50;
            double attackBonus = character.Attack * 1.5;
            character.Attack += attackBonus;
            Character.AttackMethod(character, monster);
            //대미지 계산 메서드는 AttackMethod에 있으므로 따로 출력하지 않음.

            //공격 진행 후 데이터 리셋
            character.CRIT -= 50;
            character.Attack -= attackBonus;
            cooldown = 3;
        }
    }

    public class PersistentPersuasion : Skill
    {
        int bonus;
        public PersistentPersuasion()
        {
            className = "영업팀";
            skillName = "끈질긴 설득";
            skillDescription = "내가 매번 듣던 말이 있지. 안되면 되게 하라, 센스있게 잘 좀 하자";
            isActive = true;
            costMP = 30;
            cooldown = 4;
            effectDuration = 3;


        }
        public override void UseSkill(Character character, Monster monster)
        {
            SkillUse(character, monster);
            ApplyEffect(character, monster);
            cooldown = 4;
        }

        public void ApplyEffect(Character character, Monster monster)
        {
            if (isEffectActive) isEffective(character);
            else
            {
                isEffectActive = true;
                effectDuration = 3;
                monster.DEX -= 50;
                bonus = (int)(character.Attack * 0.2);
                character.Attack += bonus;

            }

            if (effectDuration <= 0)
            {
                EffectRemove();
                monster.DEX += 50;
                character.Attack -= bonus;
            }
        }
    }

    public class GoalMustBeAchieved : Skill
    {
        public GoalMustBeAchieved()
        {
            className = "영업팀";
            skillName = "목표는 무조건 달성";
            skillDescription = "적 처치 시 마나 회복 + 10";
            isActive = false;
            costMP = 0;
            cooldown = 0;
        }

        public override void UseSkill(Character character, Monster monster)
        {
            // 해당 패시브 스킬은 자동으로 발동되므로 별도의 구현이 필요 없음
        }
    }

    //전산팀 스킬 3개
    public class EmergencyPatch : Skill
    {
        int defenseIncrease;
        public EmergencyPatch()
        {
            className = "전산팀";
            skillName = "긴급 패치";
            skillDescription = "자신 방어력 5~10 증가(2턴) + 체력(최대체력*10%) 회복, 적 전체 공격력 – 20%";
            isActive = true;
            costMP = 30;
            cooldown = 4;
        }

        public override void UseSkill(Character character, Monster monster)
        {
            // 스킬 사용 로직
            SkillUse(character, monster);

            // 체력 회복 (최대체력의 10%)
            int healthRecovery = (int)(character.MaxHealth * 0.1);
            character.Health += healthRecovery;
            if (character.Health > character.MaxHealth)
            {
                character.Health = character.MaxHealth;
                Console.WriteLine($"{character.Name}의 HP가 전부 회복되었습니다!");
            }
            else
            {
                Console.WriteLine($"{character.Name}의 HP가 {healthRecovery} 회복되었습니다!");
            }

            // 적 전체 공격력 감소 20%
            foreach (var enemy in Monster.currentBattleMonsters)
            {
                enemy.Attack -= (int)(enemy.Attack * 0.2);
            }
            cooldown = 4;
        }

        public void ApplyEffect(Character character, Monster monster)
        {
            if (isEffectActive) isEffective(character);
            else
            {
                isEffectActive = true;
                effectDuration = 2;
                defenseIncrease = new Random().Next(5, 11);
                character.Defense += defenseIncrease;
            }

            if (effectDuration < 0)
            {
                EffectRemove();
                character.Defense -= defenseIncrease;
            }
            
        }
    }



}



/* 
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
        }*/
