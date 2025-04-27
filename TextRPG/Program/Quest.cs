using System;
using System.Collections.Generic;
using TextRPG.MonsterManagement;
using TextRPG.CharacterManagement;
using TextRPG.OtherMethods;
using System.Reflection.Emit;

namespace TextRPG.QuestManagement
{
    public class Quest
    {
        // 수정부분: 스테이지 클리어 퀘스트 보상 맵핑
        private static readonly Dictionary<int, (int gold, int exp)> StageClearRewards
            = new Dictionary<int, (int, int)>()
        {
            { 1, (150, 15) },
            { 2, (200, 20) },
            { 3, (300, 30) },
            { 4, (400, 40) },
            { 5, (1000, 50) }
        };

        // ★ 추가: 퀘스트가 속한 스테이지
        public int Stage { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetMonster { get; set; }
        public int RewardGold { get; set; }
        public int RewardExp { get; set; }

        public bool IsCompleted { get; private set; }
        public static bool IsQuestCleared = false;
        public static string ActiveQuestTargetName = null;
        public static Quest ActiveQuest = null;
        public static HashSet<string> CompletedQuestNames = new HashSet<string>();

        public Quest(string name, string description, string targetMonster, int rewardGold, int rewardExp)
        {
            Name = name;
            Description = description;
            TargetMonster = targetMonster;
            RewardGold = rewardGold;
            RewardExp = rewardExp;
            IsCompleted = false;
        }

        // 수정부분: 스테이지 번호로 Quest 인스턴스 생성
        public static Quest GetStageClearQuest(int stage)
        {
            if (!StageClearRewards.TryGetValue(stage, out var reward))
                throw new ArgumentOutOfRangeException(nameof(stage));

            string name = stage < 5
                ? $"{stage}단계 클리어"
                : "보스전 - 석회장 처치";
            string description = stage < 5
                ? $"스테이지 {stage}을(를) 클리어하라."
                : "최종 진급 배틀에서 회장 석회장을 무너뜨려라.";
            string target = stage < 5 ? null : "석회장";

            var quest = new Quest(name, description, target, reward.gold, reward.exp);
            // ★ 추가: 생성된 Quest에 stage 정보 설정
            quest.Stage = stage;
            return quest;
        }

        public void Complete(Character character)
        {
            if (IsCompleted)
            {
                Console.WriteLine($"'{Name}' 퀘스트는 이미 완료되었습니다.");
                return;
            }
            Console.WriteLine($"\n[퀘스트 완료] '{Name}' 퀘스트를 완료했습니다!");
            Console.WriteLine("보상:");
            Console.WriteLine($"  골드: +{RewardGold}");
            if (RewardExp > 0)
                Console.WriteLine($"  경험치: +{RewardExp}");

            character.Gold += RewardGold;
            character.EXP += RewardExp;

            IsCompleted = true;
            CompletedQuestNames.Add(Name);
            ActiveQuest = null;
            ActiveQuestTargetName = null;

            character.LevelUP();
        }

        // 퀘스트 수락 상세 UI
        public static void ShowQuestDetail(Quest quest)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("(승진배틀(주))\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Quest!!\n");
            Console.ResetColor();

            Console.WriteLine(quest.Name + "\n");
            Console.WriteLine(quest.Description + "\n");
            if (!string.IsNullOrEmpty(quest.TargetMonster))
                Console.WriteLine($"- {quest.TargetMonster} 1마리 처치 (0/1)\n");

            Console.WriteLine("- 보상- ");
            Console.WriteLine($"{quest.RewardGold}G");
            if (quest.RewardExp > 0)
                Console.WriteLine($"{quest.RewardExp}EXP");
            Console.WriteLine();

            Console.WriteLine("1. 수락");
            Console.WriteLine("2. 거절");
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");

            string input = Console.ReadLine();
            if (input == "1")
            {
                ActiveQuest = quest;
                ActiveQuestTargetName = quest.TargetMonster;
                Console.WriteLine($"\n'{quest.Name}' 퀘스트를 수락했습니다!");
                Console.WriteLine("전투 메뉴에서 퀘스트 대상 몬스터를 처치하세요.");
                Console.WriteLine("\n0. 계속");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("\n퀘스트를 거절했습니다.");
                Console.WriteLine("\n0. 계속");
                Console.ReadLine();
            }
        }

        // 퀘스트 진행중 UI
        public static void ShowQuestProgressUI(Quest quest)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("(승진배틀(주))\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Quest!! (진행중)\n");
            Console.ResetColor();

            Console.WriteLine(quest.Name + "\n");
            Console.WriteLine(quest.Description + "\n");
            if (!string.IsNullOrEmpty(quest.TargetMonster))
                Console.WriteLine();

            Console.WriteLine("1. 포기하기");
            Console.WriteLine("2. 돌아가기");
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");

            string input = Console.ReadLine();
            if (input == "1")
            {
                Console.WriteLine($"\n'{quest.Name}' 퀘스트를 포기했습니다.");
                ActiveQuest = null;
                ActiveQuestTargetName = null;
                Console.WriteLine("\n0. 계속");
                Console.ReadLine();
            }
        }

        // 퀘스트 보상받기 UI
        public static void ShowQuestRewardUI(Quest quest, Character character)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("(승진배틀(주))\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Quest!!\n");
            Console.ResetColor();

            Console.WriteLine(quest.Name + "\n");
            Console.WriteLine(quest.Description + "\n");
            Console.WriteLine("- 보상- ");
            Console.WriteLine($"{quest.RewardGold}G\n");
            if (quest.RewardExp > 0)
                Console.WriteLine($"{quest.RewardExp}EXP\n");

            Console.WriteLine("1. 보상 받기");
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");

            int input = InputHelper.MatchOrNot(1, 1);
            if (input == 1)
            {
                if (quest.TargetMonster == "석회장")
                {
                    quest.Complete(character);
                    while (character.Level < 11)
                    {
                        character.EXP = (int)character.MaxEXP + 10;
                        character.LevelUP();
                    }
                    Console.WriteLine("\n보상을 수령했습니다. Enter를 누르면 퀘스트 목록으로 돌아갑니다.");
                    Console.ReadLine();
                }
                else
                {
                    quest.Complete(character);
                    Console.WriteLine("\n보상을 수령했습니다. Enter를 누르면 퀘스트 목록으로 돌아갑니다.");
                    Console.ReadLine();
                }
            }
        }

        // 퀘스트 메뉴 (5개 퀘스트)
        public static void ShowQuestMenu(Character character)
        {
            Console.Clear();

            if (ActiveQuest != null && IsQuestCleared)
            {
                ShowQuestRewardUI(ActiveQuest, character);
                IsQuestCleared = false;
                return;
            }

            Console.Clear();
            Console.WriteLine("=== 퀘스트 목록 ===\n");

            var questList = new List<Quest>
            {
                new Quest(
                    "stage 1: 신과장과 빠대리  격파   ",
                    $"평범한 무역회사에 일하는 20년차 대리 {character.Name}, 빠대리와 신과장을 무력으로 제압하고 1단계를 클리어하라.",
                    null,
                    150,
                    15
                ),
                new Quest(
                    "stage 2: 임차장·김부장 토벌      ",
                    $"사장님 눈밖에 난 {character.Name}, 임차장과 김부장을 쓰러뜨려 2단계를 넘어서라.",
                    null,
                    200,
                    20
                ),
                new Quest(
                    "stage 3: 오실장·카이사 장벽 돌파 ",
                    $"진급 배틀 3단계 돌파! 오실장과 카이사의 장벽을 부숴라.",
                    null,
                    300,
                    30
                ),
                new Quest(
                    "stage 4: 유상무·박사장 섬멸     ",
                    $"권력의 끝판왕 유상무와 박사장을 쓰러뜨리고 마지막 보스를 준비하라.",
                    null,
                    400,
                    40
                ),
                new Quest(
                    "stage 5 Boss: 회장 석회장 격파   ",
                    $"이 회사는 내가 먹는다! 최종 진급 배틀에서 회장 석회장을 무너뜨려라.",
                    "석회장",
                    1000,
                    50
                ),
            };

            for (int i = 0; i < questList.Count; i++)
            {
                var q = questList[i];
                string prefix = $"{i + 1}. ";
                if (ActiveQuest != null && ActiveQuest.Name == q.Name)
                {
                    Console.Write(prefix);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("[진행중] ");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(prefix);
                }

                string status = CompletedQuestNames.Contains(q.Name) ? "(완료됨)" : string.Empty;
                Console.WriteLine($"{q.Name} {status}  (보상: {q.RewardGold}G/{q.RewardExp}EXP)");
            }

            Console.WriteLine("0. 돌아가기");
            Console.Write("\n>> ");
            string input = Console.ReadLine();

            if (input == "0") return;
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= questList.Count)
            {
                var selected = questList[choice - 1];
                if (CompletedQuestNames.Contains(selected.Name))
                {
                    Console.WriteLine("\n이미 완료한 퀘스트입니다.");
                    Console.WriteLine("0. 계속");
                    Console.ReadLine();
                    return;
                }

                if (ActiveQuest != null && selected.Name == ActiveQuest.Name)
                    ShowQuestProgressUI(selected);
                else
                    ShowQuestDetail(selected);
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
            }
        }
    }
}