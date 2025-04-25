using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json; // 여기 추가
using TextRPG.CharacterManagement;
using TextRPG.GameManager;
using TextRPG.ItemSpawnManagement;
using TextRPG.WeaponManagement;
using TextRPG.MonsterManagement;

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
    }

    public static class GameSaveLoad
    {
        public static string savePath = "saveData.json";

        public static void SaveGame(Character character, List<Weapons> inventory, List<Weapons> notBuyable, List<Weapons> potions, List<Weapons> rewards)
        {
            GameData data = new GameData
            {
                CharacterData = character,
                Inventory = inventory,
                NotBuyAbleInventory = notBuyable,
                PotionInventory = potions,
                RewardInventory = rewards
            };

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(savePath, json);
            Console.WriteLine("게임 저장 완료!");
        }

        public static (Character, List<Weapons>, List<Weapons>, List<Weapons>, List<Weapons>) LoadGame()
        {
            if (!File.Exists(savePath))
            {
                Console.WriteLine("저장 파일이 존재하지 않습니다.");
                return (null, null, null, null, null);
            }

            string json = File.ReadAllText(savePath);
            GameData data = JsonConvert.DeserializeObject<GameData>(json);

            return (data.CharacterData, data.Inventory, data.NotBuyAbleInventory, data.PotionInventory, data.RewardInventory);
        }
    }
}
