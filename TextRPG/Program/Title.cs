using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.CharacterManagemant;
using TextRPG.WeaponManagemant;
using TextRPG.OtherMethods;
using System.Security.Cryptography.X509Certificates;

namespace TextRPG.TitleManagement
{
   public class Title
    {
        public string QName { get; set; }          // 칭호 이름
        public string QExplain { get; set; }   // 설명
        public bool QIsEquipped { get; set; }      // 장착 여부\
        public Title(string name, string description)
        {
            QName = name;
            QExplain = description;
            QIsEquipped = false;
        }
    public class Qs
    {
            private List<Title> titles = new List<Title>();
            public Qs()
            {
                // 초기 칭호 목록 세팅
                titles.Add(new Title("신입", "던전을 한 번 클리어한 모험가"));
                titles.Add(new Title("백수", "죽음을 경험한 전사"));
                titles.Add(new Title("능률이 올라갑니다", "모든 무기를 장착한 자"));
            }

            public void Tmenu()
            {
                while (true)
                {
                    Console.WriteLine("\n[칭호 메뉴]");
                    Console.WriteLine("1. 칭호 목록 보기");
                    Console.WriteLine("2. 칭호 장착하기");
                    Console.WriteLine("0. 나가기");
                    Console.Write("입력: ");
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            ShowTitles();
                            break;
                        case "2":
                            EquipTitle();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("잘못된 입력입니다!");
                            break;
                    }
                }
            }
            public class Qname
        {
            public void TNube()
            {
                Console.WriteLine("1.신입\n"); // 던전 한번 돌기 
                Console.WriteLine("2.백수\n"); // 한번 죽기 
                Console.WriteLine("3.\n"); //  
                Console.WriteLine("4.장착");
                
            } 
            public void TPro()
            {
                Console.WriteLine("1.능률이 올라갑니다\n"); // 모든 무기 소지 
                Console.WriteLine("2.회장"); // 마지막 스테이지 클리어시 
                Console.WriteLine("3.폭삭 망했수다"); // 10번 죽기 
                Console.WriteLine($"");
            }
          
        }
}
}
            


