using GameAPI.Data.Characters;
using GameAPI.Data.Events;
using GameAPI.Data.Items.Equipment;
using GameAPI.Data.Items.Equipment.Armors;
using GameAPI.Data.Items.Equipment.Weapons;

namespace GameAPI
{
    public class GameManager
    {
        private GameState _state;
        public GameManager()
        {
            _state = new GameState();
        }

        public virtual GameState GetGameState()
        {
            return _state;
        }

        public virtual GameState ReturnToTown()
        {
            _state.Location = new Town("Town");
            return _state;
        }

        #region Equip

        /// <summary>
        /// Checks if the hero can currently equip items
        /// </summary>
        /// <returns>True if location is Town, false otherwise</returns>
        internal virtual bool CanEquip()
        {
            if (_state.Location == null) throw new ArgumentNullException("No location in state");
            Location location = _state.Location;
            if (location.GetType() == typeof(Town))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 1) Checks if the hero can currently equip items. 2) Checks the type of equipment. 3) Matches the type to the correct Equip function to equip the item
        /// </summary>
        /// <param name="index"></param>
        /// <returns>GameState</returns>
        public virtual GameState Equip(int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException("index cannot be less than 0");
            if (index > _state.Hero.EquipmentInBag.Count()) throw new ArgumentOutOfRangeException("index cannot be higher than number of items in bag");
            if (_state.Hero.EquipmentInBag == null) throw new ArgumentNullException("Bag does not exist");

            if (CanEquip() == false) { return _state; }

            var item = _state.Hero.EquipmentInBag[index];
            Type itemType = item.GetType();

            if (itemType == typeof(Weapon))
            {
                return EquipWeapon((Weapon)item);
            }
            else if (itemType == typeof(Armor))
            {
                return EquipArmor((Armor)item);
            }
            else
            {
                throw new ArgumentException("Item type not in list of allowed equipment");
            }
        }

        /// <summary>
        /// Equips the specified weapon
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns>GameState</returns>
        internal virtual GameState EquipWeapon(Weapon weapon)
        {
            if (weapon == null) throw new ArgumentNullException("Weapon cannot be null");

            _state.Hero.EquipWeapon(weapon);
            return _state;
        }

        /// <summary>
        /// Equips the specified armor
        /// </summary>
        /// <param name="armor"></param>
        /// <returns>GameState</returns>
        internal virtual GameState EquipArmor(Armor armor)
        {
            if (armor == null) throw new ArgumentNullException("Armor cannot be null");

            _state.Hero.EquipArmor(armor);
            return _state;
        }

        #endregion

        #region Battle admin

        /// <summary>
        /// Creates a Battle location with a random enemy from the EnemyList in GameState, and puts the game there
        /// </summary>
        /// <returns>GameState</returns>
        public virtual GameState StartFight()
        {
            Enemy enemy = ChooseRandomEnemy();

            if (enemy == null) throw new NullReferenceException("Enemy chosen is null");

            _state.Location = new Battle("Battle", enemy);

            return _state;
        }

        /// <summary>
        /// Choses a random enemy from the GameStates EnemyList
        /// </summary>
        /// <returns>Random Enemy</returns>
        internal virtual Enemy ChooseRandomEnemy()
        {
            if (_state.EnemyList == null) throw new NullReferenceException("EnemyList is Null");
            if (_state.EnemyList.Count() == 0) throw new ArgumentOutOfRangeException("EnemyList is empty");

            Random rng = new Random();
            int randomNumber = rng.Next(0, _state.EnemyList.Count);
            Enemy enemy = (Enemy)_state.EnemyList[randomNumber].Clone();

            return enemy;
        }

        /// <summary>
        /// Makes enemy drop loot if possible, gives xp, checks for potential levelup, and sets location back to Town
        /// </summary>
        /// <param name="enemy"></param>
        /// <returns>GameState</returns>
        internal virtual GameState EndBattle(Enemy enemy)
        {
            if (enemy == null) throw new ArgumentNullException("Enemy is null");

            Equipment? loot = enemy.DropEquipment();
            if (loot != null)
            {
                _state.Hero.EquipmentInBag.Add(loot);
            }
            _state.Hero.Xp += enemy.XpValue;
            _state.Hero.Money += enemy.MoneyValue;
            _state.Hero.LevelUpCheck();

            ReturnToTown();

            return _state;
        }

        /// <summary>
        /// Checks if enemy is still alive and either attacks the hero or ends the battle
        /// </summary>
        /// <param name="enemy"></param>
        /// <returns>GameState</returns>
        internal virtual GameState EnemyTurn(Enemy enemy)
        {
            if (enemy == null) throw new ArgumentNullException("Enemy cant be null");
            if (_state.Location == null) throw new ArgumentNullException("Location cant be null");
            if (_state.Location.GetType() != typeof(Battle)) throw new ArgumentException("Location has to be Battle");

            Battle battleLocation = (Battle)_state.Location;

            battleLocation.DamageTakenLastTurn = enemy.Attack(_state.Hero);

            return _state;
        }

        /// <summary>
        /// Checks enemy HP and either lets enemy take a turn, or ends the battle
        /// </summary>
        /// <param name="enemy"></param>
        /// <returns>GameState after EnemyTurn or EndBattle</returns>
        internal virtual GameState EnemyOrEnd(Enemy enemy)
        {
            if (enemy == null) throw new ArgumentNullException("Enemy cant be null");
            if (_state.Location == null) throw new ArgumentNullException("Location cant be null");
            if (_state.Location.GetType() != typeof(Battle)) throw new ArgumentException("Location has to be Battle");

            if (enemy.CurrentHP <= 0)
            {
                EndBattle(enemy);
            }
            else
            {
                EnemyTurn(enemy);
            }

            return _state;
        }

        #endregion

        #region Battle actions
        /// <summary>
        /// Attacks enemy, then calls EnemyTurn() to either further the battle or end it
        /// </summary>
        /// <returns>GameState</returns>
        public virtual GameState Attack()
        {
            if (_state.Location == null) throw new ArgumentNullException("Location cant be null");
            if (_state.Location.GetType() != typeof(Battle)) throw new ArgumentException("Location has to be Battle");
            if (_state.Hero.CurrentHP <= 0) throw new ArgumentException("Hero is dead (0 or less hp)");

            Battle battleLocation = (Battle)_state.Location;
            Enemy enemy = battleLocation.Enemy;

            if (enemy == null) throw new ArgumentNullException("Enemy cant be null");
            if (enemy.CurrentHP <= 0) throw new ArgumentException("Enemy is dead (0 or less hp)");

            battleLocation.DamageDoneLastTurn = _state.Hero.Attack(enemy);

            EnemyOrEnd(enemy);

            return _state;
        }

        /// <summary>
        /// Doubles the heroes armor value, lets the enemy take a swing, then returns to normal value
        /// </summary>
        /// <returns>GameState</returns>
        public virtual GameState Defend()
        {
            if (_state.Location == null) throw new ArgumentNullException("Location cant be null");
            if (_state.Location.GetType() != typeof(Battle)) throw new ArgumentException("Location has to be Battle");
            if (_state.Hero.CurrentHP <= 0) throw new ArgumentException("Hero is dead (0 or less hp)");

            Battle battleLocation = (Battle)_state.Location;
            Enemy enemy = battleLocation.Enemy;

            if (enemy == null) throw new ArgumentNullException("Enemy cant be null");
            if (enemy.CurrentHP <= 0) throw new ArgumentException("Enemy is dead (0 or less hp)");

            int originalArmor = _state.Hero.ArmorValue;
            int defendingArmor = originalArmor * 2;
            _state.Hero.ArmorValue = defendingArmor;

            EnemyOrEnd(enemy);

            _state.Hero.ArmorValue = originalArmor;

            battleLocation.DamageDoneLastTurn = 0;

            return _state;
        }

        /// <summary>
        /// Doubles the heroes dodge chance, lets the enemy take a swing, then revets back to normal dodge chance
        /// </summary>
        /// <returns>GameState</returns>
        public virtual GameState Dodge()
        {
            if (_state.Location == null) throw new ArgumentNullException("Location cant be null");
            if (_state.Location.GetType() != typeof(Battle)) throw new ArgumentException("Location has to be Battle");
            if (_state.Hero.CurrentHP <= 0) throw new ArgumentException("Hero is dead (0 or less hp)");

            Battle battleLocation = (Battle)_state.Location;
            Enemy enemy = battleLocation.Enemy;

            if (enemy == null) throw new ArgumentNullException("Enemy cant be null");
            if (enemy.CurrentHP <= 0) throw new ArgumentException("Enemy is dead (0 or less hp)");

            int originalDodge = _state.Hero.DodgeChance;
            int dodgingChance = originalDodge * 2;
            _state.Hero.DodgeChance = dodgingChance;

            EnemyOrEnd(enemy);

            _state.Hero.DodgeChance = originalDodge;

            battleLocation.DamageDoneLastTurn = 0;

            return _state;
        }

        #endregion

        #region Shopping

        public virtual GameState EnterShop()
        {
            if (_state.Location == null) throw new ArgumentNullException("Must be in Town");
            if (_state.Location.GetType() != typeof(Town)) throw new ArgumentException("Must be in Town");

            Shop shop = new("Shop");
            Armor shield = new()
            {
                Name = "Shield",
                ArmorValue = 1,
                Price = 35
            };
            shop.EquipmentForSale.Add(shield);

            _state.Location = shop;

            return _state;
        }

        public virtual GameState Buy(int index)
        {
            if (_state.Location.GetType() != typeof(Shop)) { return _state; }

            Shop shopLocation = (Shop)_state.Location;

            if (shopLocation.EquipmentForSale.Count == 0) { return _state; }

            Equipment item = shopLocation.EquipmentForSale[index];

            if (_state.Hero.Money < item.Price) { return _state; }

            _state.Hero.Money -= item.Price;
            _state.Hero.EquipmentInBag.Add(item);
            shopLocation.EquipmentForSale.Remove(item);

            return _state;
        }

        public virtual GameState Sell(int index)
        {
            if (_state.Location.GetType() != typeof(Shop)) { return _state; }
            if (index < 0 || index > _state.Hero.EquipmentInBag.Count()) throw new ArgumentOutOfRangeException(nameof(index), "index cannot be less than 0 or greater than the amount of items in the bag");

            Equipment item = _state.Hero.EquipmentInBag[index];
            _state.Hero.Money += item.Price;
            _state.Hero.EquipmentInBag.Remove(item);

            return _state;
        }

        #endregion
    }
}
