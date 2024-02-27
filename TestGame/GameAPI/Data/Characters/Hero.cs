using GameAPI.Data.Items.Equipment;
using GameAPI.Data.Items.Equipment.Armors;
using GameAPI.Data.Items.Equipment.Weapons;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GameAPI.Tests")]

namespace GameAPI.Data.Characters
{
    public class Hero : Character
    {
        public Hero(int id, string name)
        {
            Id = id;
            Name = name;

            SetStats();
        }

        public int Xp { get; set; } = 10;
        public Weapon? EquippedWeapon { get; set; } = null;
        public Armor? EquippedArmor { get; set; } = null;
        public List<Equipment> EquipmentInBag { get; set; } = new List<Equipment>();
        public int Money { get; set; } = 0;

        #region Calculations
        /// <summary>
        /// Calculate Hero MaxHp based on level
        /// </summary>
        /// <param name="level"></param>
        /// <returns>Integer of Max Hp for specified level</returns>
        internal int CalcMaxHp(int level)
        {
            if (level <= 0) throw new ArgumentOutOfRangeException(nameof(level), "Level cannot be negative or 0");

            int baseHp = 7;
            int leveledHp = 3 * level;
            return baseHp + leveledHp;
        }

        /// <summary>
        /// Calculate Hero MaxMana based on level
        /// </summary>
        /// <param name="level"></param>
        /// <returns>Integer of Max Mana for specified level</returns>
        internal int CalcMaxMana(int level)
        {
            if (level <= 0) throw new ArgumentOutOfRangeException(nameof(level), "Level cannot be negative or 0");

            int baseMana = 3;
            int leveledMana = 2 * level;
            return baseMana + leveledMana;
        }

        /// <summary>
        /// Calculate armor value based on equipped armor
        /// </summary>
        /// <returns>Armor value as Int</returns>
        public int CalcAttackPower()
        {
            if (EquippedWeapon != null)
            {
                return Level + EquippedWeapon.AttackPower;
            }
            else { return Level; }
        }

        /// <summary>
        /// Calculate armor value based on equipped armor
        /// </summary>
        /// <returns>Armor value as Int</returns>
        public int CalcArmorValue()
        {
            if (EquippedArmor != null)
            {
                return EquippedArmor.ArmorValue;
            }
            else { return 0; }
        }

        /// <summary>
        /// Calculate level based on xp/level formula
        /// </summary>
        /// <returns>Currene level</returns>
        public int CalcLevel()
        {

            if (Xp < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Xp), "XP cannot be negative.");
            }
            int level = Xp / 10;
            if (level < 1)
            {
                level = 1;
            }
            return level;
        }

        #endregion

        /// <summary>
        /// Sets hero stats (Level, MaxHP, MaxMana, AttackPower according to currect formulas, and sets current hp/mana to max
        /// </summary>
        public void SetStats()
        {
            Level = CalcLevel();
            MaxHP = CalcMaxHp(Level);
            MaxMana = CalcMaxMana(Level);
            CurrentHP = MaxHP;
            CurrentMana = MaxMana;
            AttackPower = CalcAttackPower();
            ArmorValue = CalcArmorValue();
        }

        /// <summary>
        /// Equips input weapon. If another weapon is already equipped, put that one in the bag
        /// </summary>
        /// <param name="weapon"></param>
        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) throw new ArgumentNullException(nameof(weapon), "Weapon cannot be null");

            if (EquippedWeapon != null)
            {
                EquipmentInBag.Add(EquippedWeapon);
            }
            EquippedWeapon = weapon;
            EquipmentInBag.Remove(weapon);
            AttackPower = CalcAttackPower();
        }

        /// <summary>
        /// Equips input armor. If another armor is already equipped, put that one in the bag
        /// </summary>
        /// <param name="armor"></param>
        public void EquipArmor(Armor armor)
        {
            if (armor == null) throw new ArgumentNullException(nameof(armor), "Armor cannot be null");

            if (EquippedArmor != null)
            {
                EquipmentInBag.Add(EquippedArmor);
            }
            EquippedArmor = armor;
            EquipmentInBag.Remove(armor);
            ArmorValue = CalcArmorValue();
        }

        /// <summary>
        /// Uses CalcLevel() to check if Hero should be higher level than currently. If so, sets the new level and recalculates the stats using SetStats()
        /// </summary>
        public void LevelUpCheck()
        {
            int levelCheck = CalcLevel();
            if (levelCheck > Level)
            {
                Console.WriteLine("Level up!");
                Level = levelCheck;
                SetStats();
            }
        }
    }
}
