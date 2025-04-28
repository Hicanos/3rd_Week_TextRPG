using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.CharacterManagement;
using TextRPG.WeaponManagement;
using TextRPG.OtherMethods;
using System.Security.Cryptography.X509Certificates;
using System.Numerics;

namespace TextRPG.TitleManagement
{
   public class Title
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEquipped { get; set; } // 장착 여부 
        public bool IsUnlocked { get; set; } //해금 여부 
       
        
        // 해금 조건 함수
        public Func<Character, bool> UnlockCondition { get; set; }
        // 타이틀 해금 조건을 확인하는 함수 (Character 객체를 인자로 받음)
        // 예를 들어 특정 조건을 만족하면 타이틀을 해금하는 함수

        // 생성자: 타이틀의 이름, 설명, 해금 조건을 받아서 객체 초기화
        
        public Title(string name, string desc, Func<Character, bool> condition) 
        {
            Name = name;
            Description = desc;
            UnlockCondition = condition;
            IsEquipped = false;
            IsUnlocked = false;
        }
        public class TitleManager
        {
            public List<Title> titles = new List<Title>(); 
            public Character character;
            public Title EquippedTitle { get; set; } // 장착한 칭호
            public TitleManager(Character character)
            {
                this.character = character;
                //이름 , 설명 , 해금 조건 , 나중에 바꿔야함 임의로 설정 해놈 
                titles.Add(new Title("이제 한 걸음", "lv2", c => c.Level >= 2)); // 레벨 1
                titles.Add(new Title("고인물", "lv10", c => c.Level >= 10));
                titles.Add(new Title("텟카이", "방어력 25 달성", c => c.Defense >= 25));
                titles.Add(new Title("원펀맨", "공격력 40 달성", c => c.Attack >= 40));
                titles.Add(new Title("백발백중", "명중률 100 달성", c => c.DEX >= 100));
                titles.Add(new Title("무한", "마나 200 달성", c => c.MP >= 200));
                titles.Add(new Title("만수르", "1만 골드 보유", c => c.Gold >= 10000));
                titles.Add(new Title("폭싹 망했수다", "???", c => c.Gold <= 0));          
            }

            public void Tmenu(Character character)
            {
                while (true)
                {
                    Console.Clear();
                    CheckUnlocks();
                    Console.WriteLine("\n[칭호 메뉴]\n");
                    Console.WriteLine("1. 칭호 목록 보기");
                    Console.WriteLine("2. 칭호 장착하기");
                    Console.WriteLine("0. 나가기\n");
                    Console.Write("입력: ");
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            ShowTitles();// 칭호 목록
                            break;
                        case "2": 
                            EquipTitle();// 장착 
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("잘못된 입력입니다!");
                            break;
                    }
                }
            }
            public void CheckUnlocks()
            {   // titles 목록에 있는 모든 타이틀을 반복하면서 해금 조건 체크
                foreach (var title in titles)
                {
                           // 타이틀이 아직 해금되지 않았고 그리고 
                           // , 해금 조건이 만족되면
                        if (!title.IsUnlocked && title.UnlockCondition(character))
                    {   // 타이틀 해금 상태 설정 
                        title.IsUnlocked = true;

                        Console.WriteLine($" '{title.Name}' 칭호를 해금했습니다!");
                    }
                }
            }

            public void ShowTitles()// 해금 여부에 따라 칭호를 다른 색상으로 출력하는 함수
            {
                Console.Clear();    
                Console.WriteLine("\n[칭호 목록]");
               
                int count = 0;
                // title list 안에있는거 하나씩 다 돈다 
                for (int i = 0; i < titles.Count; i++)
                { //  t 변수에 저장 
                    var t = titles[i];

                    if (t.IsUnlocked)
                    {
                        count++;
                        Console.ForegroundColor = ConsoleColor.White;
                    }// 해금된 칭호: 흰색
                    else
                        Console.ForegroundColor = ConsoleColor.DarkGray;  // 잠긴 칭호: 회색

                    // 현재 칭호가 장착되어 있다면 "(장착됨)" 표시
                    string equipped = t.IsEquipped ? " (장착됨)" : "";

                    // 해금되지 않았다면 "[잠김]" 표시 추가
                    string tlock = t.IsUnlocked ? "" : " [잠김]";

                    // 최종 출력: 번호. 이름 - 설명 + 상태
                    Console.WriteLine($"{i + 1}. {t.Name} - {t.Description}{tlock}{equipped}\n");
                    Console.ResetColor(); // 색상 원상복구!          
                }
                Console.WriteLine("0.나가기\n");

                string input = Console.ReadLine();
              
                switch (input)
                {
                    case "0":
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다!");
                        break;
                }
            }
            // 장착 기능
            public void EquipTitle()
             // 장착 기능 메서드: 해금된 칭호 중에서 하나 골라서 장착할 수 있게 해줌!
            {
                List<Title> unlocked = titles.FindAll(t => t.IsUnlocked);
                // 해금된 칭호들만 골라서 리스트로 저장 
                if (unlocked.Count == 0) 
                {    // 하나도 없으면 밑처럼 띄우고 메뉴로 리턴 
                    Console.WriteLine("아직 장착 가능한 칭호가 없습니다!");
                    Thread.Sleep(1000);
                    return;
                }
                Console.Clear();
                Console.WriteLine("\n장착할 칭호를 선택하세요:");

                for (int i = 0; i < unlocked.Count; i++)
                {   // 해금된 칭호들을 쭉 보여줌 (번호랑 이름, 설명까지!)
                    var t = unlocked[i];// i번째 해금된 칭호 꺼냄
                    string equipped = t.IsEquipped ? "(장착됨)" : ""; // 이미 장착 중인 건 표시해줌
                    Console.WriteLine($"{i + 1}. {t.Name} {equipped}");
                }

                Console.Write("번호 입력: ");
                string input = Console.ReadLine();
               // 번호 입력 받기(몇 번째 꺼 장착할지)
                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= unlocked.Count)
                { // 입력한 게 숫자고, 범위 안에 있으면 장착 진행

                    foreach (var t in unlocked) t.IsEquipped = false;

                    // 모든 칭호 장착 해제하고
                    unlocked[choice - 1].IsEquipped = true;
                    EquippedTitle = unlocked[choice - 1];
                    character.EquippedTitle = unlocked[choice - 1];// 캐릭터 cs에 적용

                    // 선택한 칭호만 장착 
                    Console.WriteLine($"'{unlocked[choice - 1].Name}' 칭호를 장착했습니다!");
                }
                else
                {
                    // 잘못된 입력이면 에러 메시지!
                    Console.WriteLine("잘못된 번호입니다.");
                }
            }
        }
}
}
            


