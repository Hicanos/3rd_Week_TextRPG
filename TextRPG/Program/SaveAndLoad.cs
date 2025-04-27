using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json; // 여기 추가
using TextRPG.CharacterManagement;
using TextRPG.GameManager;
using TextRPG.ItemSpawnManagement;
using TextRPG.WeaponManagement;
using TextRPG.MonsterManagement;
using TextRPG.SkillManagement;
using TextRPG.QuestManagement;

namespace TextRPG.GameSaveAndLoad
{
    [Serializable]
    public class GameData
    {
        public Character CharacterData { get; set; }
        public List<Weapons> Inventory { get; set; }
        public List<Weapons> NotBuyAbleInventory { get; set; }
        public List<Weapons> PotionInventory { get; set; }
        public List<Weapons> RewardInventory { get; set; }
        public Quest ActiveQuest { get; set; }
        public bool IsQuestCleared { get; set; }
        public HashSet<string> CompletedQuestNames { get; set; }
    }

    public static class GameSaveLoad
    {
        public static string savePath = "saveData.json";

        public static void SaveGame(Character character, List<Weapons> inventory, List<Weapons> notBuyable, List<Weapons> potions, List<Weapons> rewards, Quest activeQuest, bool isQuestCleared, HashSet<string> completedQuestNames)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto // 타입 정보를 포함
            };

            GameData data = new GameData
            {
                CharacterData = character,
                Inventory = inventory,
                NotBuyAbleInventory = notBuyable,
                PotionInventory = potions,
                RewardInventory = rewards,
                ActiveQuest = activeQuest,
                IsQuestCleared = isQuestCleared,
                CompletedQuestNames = completedQuestNames
            };

            string json = JsonConvert.SerializeObject(data, settings);
            File.WriteAllText(savePath, json);
            Console.WriteLine("게임 저장 완료!");
        }


        public static (Character, List<Weapons>, List<Weapons>, List<Weapons>, List<Weapons>, Quest, bool, HashSet<string>) LoadGame()
        {
            if (!File.Exists(savePath))
            {
                Console.WriteLine("저장 파일이 존재하지 않습니다.");
                return (null, null, null, null, null, null, false, null);
            }

            string json = File.ReadAllText(savePath);
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto // 타입 정보를 처리
            };

            GameData data = JsonConvert.DeserializeObject<GameData>(json, settings);

            return (data.CharacterData, data.Inventory, data.NotBuyAbleInventory, data.PotionInventory, data.RewardInventory, data.ActiveQuest, data.IsQuestCleared, data.CompletedQuestNames);
        }


        public static List<Skill> LoadSkills(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"파일을 찾을 수 없습니다: {filePath}");
                return new List<Skill>();
            }

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto // 타입 정보를 처리
            };

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Skill>>(json, settings);
        }


        public static string SerializeSkills(List<Skill> skills)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto // 타입 정보를 포함
            };

            return JsonConvert.SerializeObject(skills, settings);
        }

        public static void SaveSkills(List<Skill> skills, string filePath)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto, // 타입 정보를 포함
                Formatting = Formatting.Indented // JSON을 보기 좋게 포맷
            };

            var json = JsonConvert.SerializeObject(skills, settings);
            File.WriteAllText(filePath, json);
        }


    }
}
