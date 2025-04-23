using System;
using System.Collections.Generic;
using TextRPG.CharacterManagemant;

namespace TextRPG.QuestManagement
{
    public class Quest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetMonster { get; set; }
        public int RewardGold { get; set; }
        public int RewardExp { get; set; }

        public Quest(string name, string description, string targetMonster, int rewardGold, int rewardExp)
        {
            Name = name;
            Description = description;
            TargetMonster = targetMonster;
            RewardGold = rewardGold;
            RewardExp = rewardExp;
        }

        public void Complete(Character character)
        {
            Console.WriteLine($"\n[퀘스트 완료] '{Name}' 퀘스트를 완료했습니다!");
            Console.WriteLine($"보상: 골드 +{RewardGold}, 경험치 +{RewardExp}");

            character.Gold += RewardGold;


            CompletedQuestNames.Add(Name);  // 완료 목록에 추가
            ActiveQuest = null;
        }

        public static Quest ActiveQuest = null;

        // 클리어한 퀘스트 이름 목록
        public static HashSet<string> CompletedQuestNames = new HashSet<string>();

        public static void ShowQuestMenu(Character character)
        {
            Console.Clear();
            Console.WriteLine("=== 퀘스트 목록 ===\n");

            List<Quest> questList = new List<Quest>()
            {
                new Quest("송 대리 처치", "송 대리를 처치하자!", "송 대리", 100, 50),
                new Quest("조 과장 처치", "조 과장을 처치하자!", "조 과장", 150, 80),
                new Quest("전 차장 처치", "전 차장을 처치하자!", "전 차장", 200, 120),
                new Quest("이 부장 처치", "이 부장을 처치하자!", "이 부장", 300, 180),
            };

            for (int i = 0; i < questList.Count; i++)
            {
                var q = questList[i];

                string linePrefix = $"{i + 1}. ";

                if (ActiveQuest != null && ActiveQuest.Name == q.Name)
                {
                    Console.Write(linePrefix);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("[E] ");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(linePrefix);
                }

                string status = CompletedQuestNames.Contains(q.Name) ? "(완료됨)" : "";
                Console.WriteLine($"{q.Name} {status}  (보상: {q.RewardGold}G / {q.RewardExp}EXP)");
            }

            Console.WriteLine("0. 돌아가기");
            Console.Write("\n>> ");
            string input = Console.ReadLine();

            if (input == "0") return;

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= questList.Count)
            {
                Quest selected = questList[choice - 1];
                if (CompletedQuestNames.Contains(selected.Name))
                {
                    Console.WriteLine("\n이미 완료한 퀘스트입니다.");
                    Console.WriteLine("0. 계속");
                    Console.ReadLine();
                    return;
                }

                ActiveQuest = selected;
                Console.WriteLine($"\n'{ActiveQuest.Name}' 퀘스트를 수락했습니다!");
                Console.WriteLine("전투 메뉴에서 퀘스트 대상 몬스터를 처치하세요.");
                Console.WriteLine("\n0. 계속");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
            }
        }
    }
}



