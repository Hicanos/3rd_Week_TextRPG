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
       
    }
    public class Qs
    {
        public void Tmenu()
        { while (true)
            {
                Console.WriteLine("칭호 분류\n");
                Console.WriteLine("1.신입 칭호");
                Console.WriteLine("2.경력자 칭호");
                Console.WriteLine("3.칭호 퀘스트");
                Console.WriteLine("0.나가기");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int num))
                {
                    switch(num)
                    {
                        case 1:
                            Qname.TNube;
                            break
                        case 2:
                            Qname.TPro;
                            break
                        case 3:
                            Qname.TQuest;
                            break
                        case 0:
                            break;
                    }
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
                Console.WriteLine("4.");
            } 
            public void TPro()
            {
                Console.WriteLine("1.능률이 올라갑니다\n"); // 모든 무기 소지 
                Console.WriteLine("2.회장"); // 마지막 스테이지 클리어시 
                Console.WriteLine("3.폭삭 망했수다"); // 10번 죽기 
                Console.WriteLine($"{Character.name}");
            }
            public void TQuest()
            {

            }
        }
}
}
            


