namespace GameAPI.Data.Characters
{
    abstract public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int MaxMana { get; set; }
        public int CurrentMana { get; set; }
        public int AttackPower { get; set; }
        public int ArmorValue { get; set; }
        public int CritChance { get; set; }
        public int DodgeChance { get; set; }

        /// <summary>
        /// Attacks input opponent and reduces their max hp
        /// </summary>
        /// <param name="opponent"></param>
        public int Attack(Character opponent)
        {
            if (opponent == null) throw new ArgumentNullException(nameof(opponent), "Opponent cannot be null");
            int unmigitatedDamage = CalcNormalDamage();
            int mitigatedDamage = unmigitatedDamage - opponent.ArmorValue;
            int critMultiplier = CalcCriticalDamage(CritChance);
            int dodgeMultiplier = CalcDodge(opponent.DodgeChance);

            int totalDamageDealt = mitigatedDamage * critMultiplier * dodgeMultiplier;

            if (totalDamageDealt < 0) { totalDamageDealt = 0; } // Stop high armor vs low damage attacks from becoming healing

            opponent.CurrentHP -= totalDamageDealt;

            return totalDamageDealt;
        }
        public int CalcNormalDamage()
        {
            int damage = AttackPower;
            return damage;
        }

        /// <summary>
        /// Takes in crit chance and checks if damage crits
        /// </summary>
        /// <param name="critChance"></param>
        /// <returns>Damage multiplier (1 for failed crit)</returns>
        public int CalcCriticalDamage(int critChance)
        {
            if (critChance < 0) { throw new ArgumentOutOfRangeException(nameof(critChance), "Crit chance cannot be negative"); }
            Random rng = new Random();
            int randomNumber = rng.Next(0, 100);

            if (randomNumber < critChance)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Takes in dodge chance and calculates if damage is negated. Since it returns a multiplier to be used in damage calculation, returning 0 means a sucessful dodge (since dodging multiplies damage by zero and negates it)
        /// </summary>
        /// <param name="dodgeChance"></param>
        /// <returns>Damage multiplier: 0 for sucessful dodge, 1 for failed dodge</returns>
        public int CalcDodge(int dodgeChance)
        {
            if (dodgeChance < 0) { throw new ArgumentOutOfRangeException(nameof(dodgeChance), "Dodge chance cannot be negative"); }
            Random rng = new Random();
            int randomNumber = rng.Next(0, 100);

            if (randomNumber < dodgeChance)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
