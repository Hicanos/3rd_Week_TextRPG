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
    public class EveryData() 
    {
        public Character CharacterData {  get; set; }
        public List<Weapons> Inventory { get; set; }
        public List<Weapons> NotBuyAbleInventory { get; set; }
        public List<Weapons> PotionInventory { get; set; }
        public List<Weapons> RewardInventory { get; set; }



    }
}
