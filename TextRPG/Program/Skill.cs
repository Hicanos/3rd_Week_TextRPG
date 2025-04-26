using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TextRPG.CharacterManagement;
using TextRPG.MonsterManagement;
using TextRPG.SkillManagement;

namespace TextRPG.SkillManagement
{
    
    //직업별(character.ClassName)로 다른 스킬을 캐릭터의 리스트에 추가할 것
    //해당 스킬이 사용 가능일 때, UseSkill 발동-해당 로직을 Battle-스킬사용으로 옮길 것

    //타겟 종류 넘버링
    public enum TargetType
    {
        Self = 1,
        SingleEnemy = 2,
        AllEnemies = 3
    }

    public class SkillData
    {
        public string ClassName { get; set; }
        public string SkillName { get; set; }
        public string SkillMent { get; set; }
        public string SkillDescription { get; set; }
        public int CostMP { get; set; }
        public int CoolDown { get; set; }
        public int CoolTime { get; set; }
        public bool IsActive { get; set; }
        public int EffectDuration { get; set; }
        public int Duration {  get; set; }
        public bool ItWorks { get; set; }
        public int TargetType { get; set; }        

    }


    //스킬 클래스
    public abstract class Skill
    {
        //사용직업/스킬 이름/스킬 설명/소모 마나/쿨타임/스킬타입 /효과 유지 턴
        //사용 직업, 스킬이름, 스킬 설명, 소모마나, 쿨타임, 스킬 타입 등은 각 스킬 내에서 변동이 있는 값은 아니므로 보호해주기

        public string ClassName { get; protected set; }//스킬 사용 직업
        public string SkillName { get; protected set; }
        public string SkillMent { get; protected set; } //스킬 사용 시 대사           
        public string SkillDescription { get; protected set; }//스킬 설명
        public int CostMP { get; protected set; }
        public int CurrentCoolDown { get; set; }   // 현재 쿨다운 상태 ( 0 = 사용가능)
        public int CoolTime { get; protected set; } // 쿨타임
        public TargetType TargetType { get; set; } //타겟 종류 1. 자신 2. 적 단일 3. 적 전체


        public int EffectDuration { get; set; } //현재 효과 종료까지 남은 턴
        public int Duration { get; protected set; } //효과 유지 턴
        public bool ItWorks { get; set; } //효과 활성화 여부
        public bool WorksNow { get; set;} //효과가 등장한 시점 확인 코드. 이 값이 true라면 효과 턴 감소가 되지 않음!
        public bool IsActive { get; set; } //true: 액티브, false: 패시브


        //스킬 사용자=캐릭터로 들고와서 간접적으로 할당 (OnEffectEnd)
        protected Character SkillOwner { get; set; } 
        public void SetSkillOwner(Character character)
        {
            SkillOwner = character;
        }

        //json 파일 받아와서>스킬 데이터 생성
        public virtual void SetData(SkillData data)
        {
            ClassName = data.ClassName;
            SkillName = data.SkillName;
            SkillMent = data.SkillMent;
            SkillDescription = data.SkillDescription;
            CostMP = data.CostMP;
            CurrentCoolDown = data.CoolDown;
            CoolTime = data.CoolTime;
            IsActive = data.IsActive;
            EffectDuration = data.EffectDuration;
            Duration = data.Duration;
            ItWorks = data.ItWorks;
            TargetType = (TargetType)data.TargetType;
        }


        //스킬 사용
        public abstract void UseSkill(Character character, List<Monster> monsters);


        //스킬 타겟팅 로직
        //각 스킬의 targetType를 읽어와서 그에 맞는 타겟을 리스트업.
        //타겟팅에 현재 '생존 중인' 적만 불러오도록 함
        public virtual List<Monster> GetTargets(List<Monster> monsters)
        {
            var aliveMonsters = monsters.Where(m => m.Health > 0).ToList();

            switch (TargetType)
            {
                case TargetType.Self:
                    return new List<Monster>();
                case TargetType.SingleEnemy:
                    return aliveMonsters.Count > 0 ? new List<Monster> { SelectSingleEnemy(aliveMonsters) } : new List<Monster>();
                case TargetType.AllEnemies:
                    return aliveMonsters;
                default:
                    return new List<Monster>();
            }
        }

        public Monster SelectSingleEnemy(List<Monster> monsters)
        {
            Console.WriteLine("대상을 선택하세요:");

            //대상 리스트업
            for (int i = 0; i < monsters.Count; i++)
            {
                Console.WriteLine($"{i + 1}. LV.{monsters[i].Level} {monsters[i].Name} (HP: {monsters[i].Health})");
            }

            //입력-초이스
            int choice;
            while (true)
            {
                Console.Write("번호 입력: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    if (choice == 0)
                    {
                        // 스킬 선택창으로 돌아가도록 할 예정
                        return null;
                    }
                    else if (choice >= 1 && choice <= monsters.Count)
                    {
                        return monsters[choice - 1];
                    }
                }
                Console.WriteLine("올바른 번호를 입력하세요.");
            }
            
        }

        //스킬이 사용 가능한 상태인지 확인. 현재 상태가 false라면 화면에 어둡게 표시할 수 있을까?
        public bool CanUseSkill(Character character)
        {
            if (CurrentCoolDown > 0)
            {
                Console.WriteLine($"{SkillName}은(는) 쿨타임 중입니다. 남은 쿨타임: {CurrentCoolDown}턴");
                return false;
            }

            if (character.MP < CostMP)
            {
                Console.WriteLine("MP가 부족합니다.");
                return false;
            }
            return true;
        }

        //각 스킬에 맞춰 버프받았던 것을 반환하도록 함
        protected abstract void OnEffectEnd();

        //스킬의 쿨타임 감소/효과 제거 관리- 매 턴의 마지막에 확인할 것
        public virtual void UpdateSkillState()
        {
            //WorksNow가 활성화 되어있다면, WorksNow를 종료시키고 쿨타임과 EffectDuration 감소 없음. 
            if (WorksNow == true) { WorksNow = false;}
            else
            {
                if (CurrentCoolDown > 0) CurrentCoolDown--;
                if (EffectDuration > 0) EffectDuration--;
                if (EffectDuration == 0 && ItWorks) //효과 끝! -효과 되돌려야함
                {
                    OnEffectEnd();
                    ItWorks = false;
                    Console.WriteLine($"{SkillOwner.Name}의 {SkillName} 효과가 해제되었습니다.");
                }
            }            
        }

        public virtual void EndSkillState()
        {
            CurrentCoolDown = 0;
            EffectDuration = 0;
            ItWorks = false;
            WorksNow = false;
            OnEffectEnd();
        }

        //스킬 사용 로직 - 버프랑 관계없이 쓰자 통일하자
        protected void SkillUse(Character character)
        {
            Console.WriteLine($"{character.Name}이(가) {SkillName}을(를) 사용!\n");
            if (SkillMent != null)
            {
                Console.WriteLine($"{character.Name}:{SkillMent}\n");
            }            
            character.MP -= CostMP;
        }
       
    }

    //인사팀 스킬 3개
    public class PersonnelEvaluation : Skill
    {
        private int dexBonus = 30;
        private Dictionary<Character, int> DexBonus = new();

        //기본 생성자
        public PersonnelEvaluation(){ }

        //데이터 받아오는 곳
        public PersonnelEvaluation(SkillData data)
        {
            SetData(data);
        }

        //스킬 사용처
        public override void UseSkill(Character character, List<Monster>monsters)
        {
            if(!CanUseSkill(character)) return;
            var targets = GetTargets(monsters);
            SkillUse(character); //스킬 사용 선언


            //"적 전체에게 범위 딜(공격력 * 1.3), 2턴 명중률 +30%"
            int skillDmg = (int)(character.Attack * 1.3);//공격력 * 1.3

            foreach (var target in targets.Where(m => m != null))
            {
                target.Health -= skillDmg;
                Console.WriteLine($"LV.{target.Level} {target.Name}에게 {skillDmg}의 피해를 입혔습니다.");
                if (target.Health <= 0) target.Health = 0;
            }
            
            ApplyEffect(); //부가효과 적용
            WorksNow = true;
            CurrentCoolDown = CoolTime; //쿨다운을 쿨타임이랑 맞춤
        }

        //부가효과
        public void ApplyEffect()
        {
            if (!ItWorks)
            {
                ItWorks = true;
                EffectDuration = Duration;

                DexBonus[SkillOwner] = dexBonus;
                SkillOwner.DEX += dexBonus;
                Console.WriteLine($"{SkillOwner.Name}의 명중률이 {dexBonus}% 증가했습니다.");
            }
        }

        //받은 효과 되돌리기
        protected override void OnEffectEnd()
        {
            if (DexBonus.ContainsKey(SkillOwner))
            {
                SkillOwner.DEX -= DexBonus[SkillOwner];
                DexBonus.Remove(SkillOwner);
            }
        }

    }

    public class NoticeToEmployees : Skill
    {
        private int DEXBonus = 15;
        private int critBonus = 15;
        private Dictionary<Character, (int dex, int crit)> buffs = new();

        public NoticeToEmployees(){ }
        public NoticeToEmployees(SkillData data)
        {
            SetData(data);
        }

        public override void UseSkill(Character character, List<Monster> monsters)
        {
            if (!CanUseSkill(character)) return;
            SkillUse(character);
            ApplyEffect();
            WorksNow = true;
            CurrentCoolDown = CoolTime;
        }
        public void ApplyEffect()
        {
            if (!ItWorks)
            {
                ItWorks = true;                
                EffectDuration = Duration;

                //딕셔너리에 각각의 버프값 저장
                buffs[SkillOwner] = (DEXBonus, critBonus);
                SkillOwner.DEX += DEXBonus;
                SkillOwner.CRIT += critBonus;
                Console.WriteLine($"{SkillOwner.Name}의 명중률이 {DEXBonus}%, 치명타 확률이 {critBonus}% 증가했습니다");
            }
        }
        protected override void OnEffectEnd()
        {
            if (buffs.ContainsKey(SkillOwner))
            {
                var buff = buffs[SkillOwner];

                SkillOwner.DEX -= buff.dex;
                SkillOwner.CRIT -= buff.crit;
                buffs.Remove(SkillOwner);
            }
        }
    }

    public class HRMonitoringNetwork : Skill
    {
        //턴마다 들어가는 버프
        private int defenseBonusPerTurn = 2;
        private int dexBonusPerTurn = 3;
        private int evaBonusPerTurn = 3;

        // 방어력, 명중률, 회피율 증가 딕셔너리
        private Dictionary<Character, (int defenseSum, int dexSum, int evaSum)> buffs = new();
        public HRMonitoringNetwork() { }
        public HRMonitoringNetwork(SkillData data)
        {
            SetData(data);
        }

        //매 턴 시작 시 자동 적용-매턴 사용 가능하므로 별도의 판정 필요 없음
        //매턴 방어력 +2 명중률 +3 회피율 +3
        public override void UseSkill(Character character, List<Monster> monsters)
        {   
            ApplyEffect();            
        }
        //전투 중에는 해제되지 않음. 계속해서 쌓임.
        public void ApplyEffect()
        {
            //buffs 딕셔너리에 SkillOwner라는 key값이 존재하지 않는 경우
            if (!buffs.ContainsKey(SkillOwner))
            {
                buffs[SkillOwner] = (0, 0, 0);
            }

            //현재 버프값 저장
            var current = buffs[SkillOwner];
            
            //추가되는 버프값 저장 (기존 값+추가값)
            var newBuff = (current.defenseSum + defenseBonusPerTurn,
                           current.dexSum + dexBonusPerTurn,
                           current.evaSum + evaBonusPerTurn);

            //최신값 갱신
            buffs[SkillOwner] = newBuff;
            SkillOwner.Defense += defenseBonusPerTurn;
            SkillOwner.DEX += dexBonusPerTurn;
            SkillOwner.EVA += evaBonusPerTurn;
            Console.WriteLine($"{SkillOwner.Name}의 방어력이 {defenseBonusPerTurn}, 명중률이 {dexBonusPerTurn}%, 회피율이 {evaBonusPerTurn}% 증가했습니다.");
        }

        //누적 값 다 털기
        protected override void OnEffectEnd()
        {
            if (buffs.ContainsKey(SkillOwner))
            {
                var buff = buffs[SkillOwner];

                SkillOwner.Defense -= buff.defenseSum;
                SkillOwner.DEX -= buff.dexSum;
                SkillOwner.EVA -= buff.evaSum;

                buffs.Remove(SkillOwner);
            }
        }

    }

    //홍보팀 스킬 3개
    public class PublicRelations : Skill
    {
        private int skillDmg;
        public PublicRelations() { }
        public PublicRelations(SkillData data)
        {
            SetData(data);
        }

        //적 전체에게 최대 마나*0.3 범위딜
        public override void UseSkill(Character character, List<Monster> monsters)
        {
            if (!CanUseSkill(character)) return; //여기서 
            var targets = GetTargets(monsters);
            SkillUse(character); //스킬 사용 선언

            skillDmg = (int)(character.MaxMP * 0.3);

            //범위딜 on
            foreach (var target in targets) 
            {
                target.Health -= skillDmg;
                Console.WriteLine($"LV.{target.Level} {target.Name}에게 {skillDmg}의 피해를 입혔습니다.");
                if (target.Health <= 0) target.Health = 0;
            }
            WorksNow = true;
            //쿨타임 on
            CurrentCoolDown = CoolTime;
        }
        //해당 효과 없음
        protected override void OnEffectEnd()
        { }
    }

    public class ImageMaking : Skill
    {
        private int heal;
        private int evaBonus = 25;
        private Dictionary<Character, int> EVAbonus = new();

        public ImageMaking() { }
        public ImageMaking(SkillData data)
        {
            SetData(data);
        }

        //체력 회복(최대체력의 20%), 3턴간 자신의 회피 +25%
        public override void UseSkill(Character character, List<Monster> monsters)
        {
            if (!CanUseSkill(character)) return;
            SkillUse(character); //스킬 사용 선언

            //회복량=최대체력의 20%
            heal = (int)(character.MaxHealth * 0.2);

            if (character.Health+heal > character.MaxHealth) //캐릭터 체력+힐량이 최대체력보다 크면, 최대체력으로 맞춤
            {
                int realheal = character.MaxHealth - character.Health;
                character.Health = character.MaxHealth;
                Console.WriteLine($"{character.Name}의 HP가 {realheal} 회복되었습니다.");
            }
            else
            {
                character.Health += heal;
                Console.WriteLine($"{character.Name}의 HP가 {heal} 회복되었습니다.");
            }

            ApplyEffect();
            WorksNow = true;
            CurrentCoolDown = CoolTime;

        }
        public void ApplyEffect()
        {
            if (!ItWorks)
            {
                ItWorks = true;
                EffectDuration = Duration; //효과 턴 수 넣기
                EVAbonus[SkillOwner] = evaBonus;
                SkillOwner.EVA += evaBonus;
                Console.WriteLine($"{SkillOwner.Name}의 회피율이 {evaBonus}증가했습니다.");
            }

        }
        protected override void OnEffectEnd()
        {
            if (EVAbonus.ContainsKey(SkillOwner))
            {
                SkillOwner.EVA -= EVAbonus[SkillOwner];
                EVAbonus.Remove(SkillOwner);
            }
        }
    }
    public class Attention : Skill
    {
        private int critBonus = 20;
        private Dictionary<Character, int> CRITBonus = new();

        public Attention() { }
        public Attention(SkillData data)
        {
            SetData(data);
        }

        //전투 지속 치명타 확률 +20 :패시브
        public override void UseSkill(Character character, List<Monster> monsters)
        {
            ApplyEffect();
        }
        public void ApplyEffect()
        {
            if (!ItWorks) //중복 적용 방지
            {
                ItWorks = true;

                CRITBonus[SkillOwner] = critBonus;
                SkillOwner.CRIT += critBonus;
                Console.WriteLine($"{SkillOwner.Name}의 치명타 확률이 {critBonus}% 증가했습니다.");
            }
        }

        //전투 종료시 한 번에 처리
        protected override void OnEffectEnd()
        {
            if (CRITBonus.ContainsKey(SkillOwner))
            {
                SkillOwner.CRIT -= CRITBonus[SkillOwner];
                CRITBonus.Remove(SkillOwner);
            }
        }

    }

    //총무팀 스킬 3개
    public class BudgetaryControl : Skill
    {
        int skillDmg;
        public BudgetaryControl() { }
        public BudgetaryControl(SkillData data)
        {
            SetData(data);
        }

        //단일 대상 적 대미지 (공격력+방어력*2)
        public override void UseSkill(Character character, List<Monster> monsters)
        {
            if (!CanUseSkill(character)) return;
            var targets = GetTargets(monsters); //선택된 개체 하나만 List에 들어감
            var target = targets[0]; //0번 호출 = 선택된 대상 호출

            SkillUse(character); //스킬 사용 선언
            skillDmg = (int)(character.Attack + character.Defense * 2);

            target.Health-= skillDmg;
            Console.WriteLine($"LV.{target.Level} {target.Name}에게 {skillDmg}의 피해를 입혔습니다.");
            if (target.Health <= 0) target.Health = 0;

            WorksNow = true;
            CurrentCoolDown = CoolTime;
        }
        protected override void OnEffectEnd()
        {
            //해당 요소 없음
        }

    }

    public class SuppliesSupport : Skill
    {
        int heal;
        int deffenseBonus;
        private Dictionary<Character, int> DEFBonus = new();
        public SuppliesSupport() { }
        public SuppliesSupport(SkillData data)
        {
            SetData(data);
        }
        
        //체력 회복 (최대체력 +15%), 방어력 40% 상승
        public override void UseSkill(Character character, List<Monster> monsters)
        {
            if (!CanUseSkill(character)) return;
            SkillUse(character); //스킬 사용 선언

            heal = (int)(character.MaxHealth * 0.15);
            if (character.Health + heal > character.MaxHealth) //캐릭터 체력+힐량이 최대체력보다 크면, 최대체력으로 맞춤
            {
                int realheal = character.MaxHealth - character.Health;
                character.Health = character.MaxHealth;
                Console.WriteLine($"{character.Name}의 HP가 {realheal} 회복되었습니다.");
            }
            else
            {
                character.Health += heal;
                Console.WriteLine($"{character.Name}의 HP가 {heal} 회복되었습니다.");
            }
            ApplyEffect();
            WorksNow = true;
            CurrentCoolDown = CoolTime; //쿨타임 시작
        }
        public void ApplyEffect()
        {
            if (!ItWorks)
            {
                ItWorks = true;

                deffenseBonus = (int)(SkillOwner.Defense * 0.4);
                DEFBonus[SkillOwner] = deffenseBonus;
                SkillOwner.Defense += deffenseBonus;
                Console.WriteLine($"{SkillOwner.Name}의 방어력이 {deffenseBonus} 증가했습니다.");
                EffectDuration = Duration; //효과 on
            }
        }
        protected override void OnEffectEnd()
        {
            if (DEFBonus.ContainsKey(SkillOwner))
            {
                SkillOwner.Defense -= DEFBonus[SkillOwner];
                DEFBonus.Remove(SkillOwner);
            }
        }
    }

    public class MasterOfOperation : Skill
    {
        public MasterOfOperation() { }
        public MasterOfOperation(SkillData data)
        {
            SetData(data);
        }

        //해당 스킬은 전투 중에 사용되지 않음
        public override void UseSkill(Character character, List<Monster> monsters) { }
        protected override void OnEffectEnd(){ }
    }

    //영업팀 스킬 3개
    public class PerformancePressure : Skill
    {
        int skillDmg;

        public PerformancePressure() { }
        public PerformancePressure(SkillData data)
        {
            SetData(data);
        }
        public override void UseSkill(Character character, List<Monster> monsters)
        {
            if (!CanUseSkill(character)) return;
            var targets = GetTargets(monsters);
            var target = targets[0];
            SkillUse(character); //스킬 사용 선언
            skillDmg = (int)(character.Attack * 2);

            target.Health -= skillDmg;

            Console.WriteLine($"LV.{target.Level} {target.Name}에게 {skillDmg}의 피해를 입혔습니다.");

            if(target.Health <= 0) target.Health = 0;

            WorksNow = true;
            CurrentCoolDown = CoolTime;
        }
        //해당 없음
        protected override void OnEffectEnd() {   }
    }

    public class PersistentPersuasion : Skill
    {
        int EVAbonus=40;
        int attackbonus;
        Dictionary<Character, (int eva, int attack)> buffs = new();

        public PersistentPersuasion() { }
        public PersistentPersuasion(SkillData data)
        {
            SetData(data);
        }
        public override void UseSkill(Character character, List<Monster> monsters)
        {
            if (!CanUseSkill(character)) return;
            SkillUse(character); //스킬 사용 선언
            //즉발 효과 없음
            ApplyEffect();
            WorksNow = true;
            CurrentCoolDown = CoolTime;
        }
        public void ApplyEffect()
        {
            if (!ItWorks)
            {
                ItWorks = true;

                attackbonus = (int)(SkillOwner.Attack * 0.2);
                buffs[SkillOwner] = (EVAbonus, attackbonus);
                SkillOwner.EVA += EVAbonus;
                SkillOwner.Attack += attackbonus;
                Console.WriteLine($"{SkillOwner.Name}의 회피율이 {EVAbonus}%, 공격력이 {attackbonus} 증가했습니다.");
                EffectDuration = Duration;
            }

        }
        protected override void OnEffectEnd()
        {
            if (buffs.ContainsKey(SkillOwner))
            {
                var buff = buffs[SkillOwner];

                SkillOwner.EVA -= buff.eva;
                SkillOwner.Attack -= buff.attack;

                buffs.Remove(SkillOwner);
            }
        }
    }

    public class GoalMustBeAchieved : Skill
    {
        float critdmgBonus= 0.3f;
        int critBonus = 20;

        private Dictionary<Character, (float critdmg, int crit)> buffs = new();

        public GoalMustBeAchieved() { }
        public GoalMustBeAchieved(SkillData data)
        {
            SetData(data);
        }

        //어차피 HP가 80 밑으로 깎임=에너미 페이즈 다음이므로 시작 타이밍에 사용
        public override void UseSkill(Character character, List<Monster> monsters)
        {
            if (character.Health <= 80)
            {
                ApplyEffect();
            }
        }
        public void ApplyEffect()
        {
            if(!ItWorks) //효과 중복 적용 방지
            {
                ItWorks = true;
                buffs[SkillOwner]=(critdmgBonus, critBonus);
                SkillOwner.CRIT += critBonus;
                SkillOwner.CRITDMG += critdmgBonus;
                Console.WriteLine($"{SkillOwner.Name}의 치명타 확률이 {critBonus}%, 치명타 대미지 배율이 +{critdmgBonus*100}% 증가했습니다.");
            }
        }
        protected override void OnEffectEnd()
        {
            if (buffs.ContainsKey(SkillOwner))
            {
                var buff = buffs[SkillOwner];
                SkillOwner.CRIT -= buff.crit;
                SkillOwner.CRITDMG -= buff.critdmg;
                buffs.Remove(SkillOwner);
            }
        }
    }

    //전산팀 스킬 3개
    public class EmergencyPatch : Skill
    {
        int skillDmg;

        public EmergencyPatch() { }
        public EmergencyPatch(SkillData data)
        {
            SetData(data);
        }

        public override void UseSkill(Character character, List<Monster> monsters)
        {
            if (!CanUseSkill(character)) return;
            var targets = GetTargets(monsters);
            var target= targets[0];
            SkillUse(character); //스킬 사용 선언

            skillDmg = (int)(character.Attack * 1.2 + character.MaxMP * 0.15);
            target.Health -= skillDmg;
            if (target.Health < 0)
            {
                BattleManager.CheckDead(character);
                target.Health = 0;
            }

            Console.WriteLine($"LV.{target.Level} {target.Name}에게 {skillDmg}의 피해를 입혔습니다.");
            CurrentCoolDown = CoolTime;
        }
        protected override void OnEffectEnd() {  } //해당 없음
    }

    public class SystemDown : Skill
    {
        int skillDmg;
        public SystemDown() { }
        public SystemDown(SkillData data)
        {
            SetData(data);
        }

        public override void UseSkill(Character character, List<Monster> monsters)
        {
            if (!CanUseSkill(character)) return;
            var targets = GetTargets(monsters);
            SkillUse(character); //스킬 사용 선언
            skillDmg = (int)(character.Attack * 1.2 + character.MaxMP * 0.2);

            foreach(var target in targets)
            {
                target.Health -= skillDmg;
                Console.WriteLine($"LV.{target.Level} {target.Name}에게 {skillDmg}의 피해를 입혔습니다.");
                if (target.Health <= 0)
                { 
                    target.Health = 0;
                    BattleManager.CheckDead(character);
                }
            }

            CurrentCoolDown = CoolTime;
        }

        protected override void OnEffectEnd() {   }
    }

    public class BackupSystem : Skill
    {
        public BackupSystem() { }
        public BackupSystem(SkillData data)
        {
            SetData(data);
        }
        public override void UseSkill(Character character, List<Monster> monsters)
        {
            //전투 로직 직접 수정
        }

        protected override void OnEffectEnd() {   } //해당 없음
    }

    //기획팀 스킬 3개
    public class PlanningBombing : Skill
    {
        int skillDmg;
        public PlanningBombing() { }
        public PlanningBombing(SkillData data)
        {
            SetData(data);
        }

        public override void UseSkill(Character character, List<Monster> monsters)
        {
            if (!CanUseSkill(character)) return;
            var targets = GetTargets(monsters);
            SkillUse(character); //스킬 사용 선언
            skillDmg = (int)(character.Attack * 1.6);

            foreach (var target in targets)
            {
                target.Health -= skillDmg;
                if (target.Health <= 0) target.Health = 0;
                Console.WriteLine($"LV.{target.Level} {target.Name}에게 {skillDmg}의 피해를 입혔습니다.");
                BattleManager.CheckDead(character);
            }
            CurrentCoolDown = CoolTime;
        }
        protected override void OnEffectEnd() {  } //해당 없음
    }

    public class RiskAnalysis : Skill
    {
        int evaBonus = 50;
        Dictionary<Character, int> EvaBonus = new();
        public RiskAnalysis() { }
        public RiskAnalysis(SkillData data)
        {
            SetData(data);
        }

        public override void UseSkill(Character character, List<Monster> monsters)
        {
            if (!CanUseSkill(character)) return;
            SkillUse(character); //스킬 사용 선언
            ApplyEffect();
            WorksNow = true;
            CurrentCoolDown = CoolTime;
        }
        public void ApplyEffect()
        {
            if (!ItWorks)
            {
                ItWorks = true;

                EvaBonus[SkillOwner] = evaBonus;
                SkillOwner.EVA += evaBonus;
                Console.WriteLine($"{SkillOwner.Name}의 회피율이 {evaBonus}% 증가했습니다.");

                EffectDuration = Duration;
            }
            

        }
        protected override void OnEffectEnd()
        {
            if (EvaBonus.ContainsKey(SkillOwner))
            {
                SkillOwner.EVA -= EvaBonus[SkillOwner];
                EvaBonus.Remove(SkillOwner);
            }
        }
    }

    public class ConceptEstablished : Skill
    {
        float critdmgBonus = 0.12f;
        int critBonus = 10;

        private Dictionary<Character, (float critdmgSum,int critSum)> buffs =new();

        public ConceptEstablished() { }
        public ConceptEstablished(SkillData data)
        {
            SetData(data);
        }

        public override void UseSkill(Character character, List<Monster> monsters)
        {
                ApplyEffect();
        }
        public void ApplyEffect()
        {
            if (!buffs.ContainsKey(SkillOwner))
            {
                buffs[SkillOwner] = (0, 0);
            }

            var current = buffs[SkillOwner];

            //추가되는 버프값 저장 (기본+추가)
            var newBuff = (current.critdmgSum + critdmgBonus,
                           current.critSum + critBonus);

            //최신값 갱신
            buffs[SkillOwner] = newBuff;
            SkillOwner.CRITDMG += critdmgBonus;
            SkillOwner.CRIT += critBonus;

            Console.WriteLine($"{SkillOwner.Name}의 치명타 대미지가 {critdmgBonus * 100}%, 치명타 확률이 {critBonus}% 증가했습니다.");

        }
        protected override void OnEffectEnd()
        {
            if (buffs.ContainsKey(SkillOwner))
            {
                var buff = buffs[SkillOwner];

                SkillOwner.CRITDMG -= buff.critdmgSum;
                SkillOwner.CRIT -= buff.critSum;

                buffs.Remove(SkillOwner);
            }
        }
    }


    public static class SkillLoader
    {
        public static List<Skill> LoadSkillObjects()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = Path.Combine(baseDir, "Data", "skills.json");

            if (!File.Exists(jsonFilePath))
            {
                Console.WriteLine($"스킬 JSON 파일을 찾을 수 없습니다: {jsonFilePath}");
                return new List<Skill>();
            }

            var jsonString = File.ReadAllText(jsonFilePath);
            var skills = new List<Skill>();

            try
            {
                // JSON 데이터를 리스트로 역직렬화
                var rawSkillData = JsonConvert.DeserializeObject<List<dynamic>>(jsonString);

                foreach (var rawSkill in rawSkillData)
                {
                    // Type 정보를 읽어와 동적으로 객체 생성
                    string typeName = rawSkill.Type;
                    var skillData = rawSkill.Data;

                    // 어셈블리 이름을 명시적으로 추가
                    if (!typeName.Contains(","))
                    {
                        typeName += ", Program";
                    }

                    Type skillType = Type.GetType(typeName);
                    if (skillType == null)
                    {
                        Console.WriteLine($"알 수 없는 스킬 타입: {typeName}");
                        continue;
                    }

                    // Skill 객체 생성 및 데이터 설정
                    var skill = (Skill)Activator.CreateInstance(skillType);
                    var skillDataJson = JsonConvert.SerializeObject(skillData);
                    var skillDataObject = JsonConvert.DeserializeObject<SkillData>(skillDataJson);

                    skill.SetData(skillDataObject);
                    skills.Add(skill);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"스킬 데이터를 로드하는 중 오류가 발생했습니다: {ex.Message}");
            }

            return skills;
        }

    }


}