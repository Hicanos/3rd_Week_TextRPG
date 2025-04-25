using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
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
        public Character CharacterData {  get; set; }
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

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(savePath, json);
            Console.WriteLine("게임 저장 완료!");
        }
        public static GameData LoadGame()
        {
            if (!File.Exists(savePath))
            {
                Console.WriteLine("저장된 파일이 없습니다.");
                return null;
            }

            string json = File.ReadAllText(savePath);
            GameData data = JsonSerializer.Deserialize<GameData>(json);
            Console.WriteLine("게임 불러오기 완료!");
            return data;
        }
    }
}



