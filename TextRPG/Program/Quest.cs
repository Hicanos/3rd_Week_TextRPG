using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.CharacterManagemant;
using TextRPG.WeaponManagemant;
using TextRPG.OtherMethods;

namespace Quest
{
   public class Quest
    {
        public string questName; // 퀘스트 이름
        public string questStroy: // 퀘스트 스토리 
         // public int (보상)    
        public string questPerfoem; // 수행내역
        public qusetstatus ; // 진행 스텟  
        

       public void questName()
        {
            Console.WriteLine("퀘스트 분류\n");
            Console.WriteLine("1.신입 퀘스트");
            Console.WriteLine("2.경력직 퀘스트");
            Console.WriteLine("3.사장 퀘스트");
            Console.WriteLine("4.칭호 퀘스트");
            Console.WriteLine("5.메뉴로 돌아가기");
            Console.ReadLine();


        }
    }
    public class RunQ 
    {
        quwstName();
    }
}
            


