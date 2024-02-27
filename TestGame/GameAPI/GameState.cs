using GameAPI.Data.Characters;
using GameAPI.Data.Events;
using GameAPI.Data.Items.Equipment.Armors;
using GameAPI.Data.Items.Equipment.Weapons;

namespace GameAPI
{
    public class GameState
    {
        public Hero Hero { get; set; }
        public List<Enemy> EnemyList { get; set; } = new List<Enemy>(); // List of potential enemies
        public Location Location { get; set; } // Current location (town or battle)

        public GameState() 
        {
            // Create hero
            Hero = new(1, "Ted");

			// Create some loot
			Weapon sword = new()
            {
                Name = "Sword",
                AttackPower = 2,
                Price = 40
            };
            Armor breastplate = new()
            {
                Name = "Breastplate",
                ArmorValue = 1,
                Price = 35
            };

            // Create enemies
            Enemy enemy1 = new()
            {
                Id = 2,
                Name = "Slime",
                Description = "Japanese first enemy",
                Level = 1,
                AttackPower = 1,
                MaxHP = 5,
                CurrentHP = 5,
                MaxMana = 5,
                ArmorValue = 0,
                XpValue = 5,
				MoneyValue = 10
			};
            enemy1.LootTable.Add(sword);
            enemy1.LootTable.Add(breastplate);

            Enemy enemy2 = new()
            {
                Id = 3,
                Name = "Rat",
                Description = "European first enemy",
                Level = 1,
                AttackPower = 2,
                MaxHP = 3,
                CurrentHP = 3,
                MaxMana = 3,
                ArmorValue = 0,
                XpValue = 5,
				MoneyValue = 10
			};
            enemy2.LootTable.Add(breastplate);

            // Add enemy to enemylist
            EnemyList.Add(enemy1);
            EnemyList.Add(enemy2);

            // Give hero sword
            Hero.EquipmentInBag.Add(sword);

			// Set first location to Town
			Location = new Town("Town");
		}
    }
}
