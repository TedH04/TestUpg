using GameAPI.Data.Items.Equipment;

namespace GameAPI.Data.Characters
{
    public class Enemy : Character, ICloneable
    {
        public Enemy()
        {
            // does not work, fix later
            CurrentHP = MaxHP;
            CurrentMana = MaxMana;
        }
        public int NoDropChance { get; set; } = 80; // int between 0 and 100 indication % chance to not drop items on death (80 = 80% chance of no loot)
        public int XpValue { get; set; }
        public int MoneyValue { get; set; }
        public List<Equipment> LootTable { get; set; } = new List<Equipment>();

        /// <summary>
        /// Creates a clone of the enemy object
        /// </summary>
        /// <returns>Clone of the object</returns>
		public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Checks for Equipment in enemy loot table, picks one at random, and returns it (or null for empty loot tables)
        /// </summary>
        /// <returns>Equipment or null</returns>
		public Equipment? DropEquipment()
        {
            // Check for empty loot table
            if (NoDropChance < 0 || NoDropChance > 100) { throw new ArgumentOutOfRangeException(nameof(NoDropChance), "NoDropChance cannot be negative or surpass 100"); }
            if (LootTable == null || LootTable.Count == 0) { return null; }

            Random rng = new Random();

            // Check for no drop chance
            int randomNumber = rng.Next(0, 100);
            if (randomNumber < NoDropChance) { return null; }

            // Choose item to drop
            randomNumber = rng.Next(0, LootTable.Count);
            Equipment loot = LootTable[randomNumber];

            return loot;
        }
    }
}
