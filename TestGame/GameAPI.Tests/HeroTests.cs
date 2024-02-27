namespace GameAPI.Tests
{
    public class HeroTests
    {
        //använd _log för att logga i test explorer
        #region Constructor
        private readonly ITestOutputHelper _log;
        private Hero _hero;

        public HeroTests(ITestOutputHelper output)
        {
            _log = output;
            _hero = new Hero(1, "Test Hero");
        }
        #endregion

        #region CalcMaxHp
        [Theory]
        [InlineData(1, 10)]  // 7 baseHp + 3*1 = 10 : level 1 blir maxHp 10
        [InlineData(2, 13)]  // 7 baseHp + 3*2 = 13
        [InlineData(3, 16)]  // 7 baseHp + 3*3 = 16
        [InlineData(10, 37)] // 7 baseHp + 3*10 = 37
        public void CalcMaxHp_AtVariousLevels_ShouldReturnCorrectHp(int level, int expectedHp)
        {
            // Arrange
            // Act
            int maxHp = _hero.CalcMaxHp(level);

            //Log
            _log.WriteLine($"Hero baseHp: {_hero.MaxHP} Hero level: {level} Hero Hp: {maxHp}");

            // Assert
            Assert.Equal(expectedHp, maxHp);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void CalcMaxHp_WhenLevelIsNegativeOrZero_ThrowsArgumentOutOfRangeException(int level)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _hero.CalcMaxHp(level));
        }

        #endregion

        #region CalcMaxMana
        [Theory]
        [InlineData(1, 5)] // 3 baseMana + 2*1 = 5 //level 1 blir baseMana 5
        [InlineData(2, 7)] // 3 baseMana + 2*2 = 7
        [InlineData(10, 23)] // 3 baseMana + 2*10 = 23
        public void CalcMaxMana_AtVariousLevels_ShouldReturnCorrectMp(int level, int expectedMana)
        {
            //Arrange
            //Act
            int maxMana = _hero.CalcMaxMana(level);

            //Log
            _log.WriteLine($"Level: {level} Mp: {maxMana} baseMp: {_hero.MaxMana}");

            //Assert
            Assert.Equal(expectedMana, maxMana);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void CalcMaxMana_WhenLevelIsNegativeOrZero_ThrowsArgumentOutOfRangeException(int level)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _hero.CalcMaxMana(level));
        }

        #endregion

        #region CalcArmorValue
        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(30)]
        public void CalcArmorValue_WithVariousArmors_ShouldReturnUpdatedArmorValues(int armorVal)
        {
            //Arrange
            Armor testArmor = new Armor
            {
                ArmorValue = armorVal
            };
            _hero.EquipArmor(testArmor);

            // Act
            int armorValue = _hero.CalcArmorValue();

            //Log
            _log.WriteLine($"Armor value med armor: {_hero.ArmorValue} ArmorValue utan armor: {armorValue}");

            // Assert
            Assert.Equal(testArmor.ArmorValue, armorValue);
        }
        #endregion

        #region CalcLevel
        [Theory]
        [InlineData(10, 1)]
        [InlineData(20, 2)]
        [InlineData(100, 10)]
        public void CalcLevel_XpLessThan10_ShouldReturnLevelOne(int heroxp, int expectedLevel)
        {
            // Arrange
            _hero.Xp = heroxp;

            // Act
            int level = _hero.CalcLevel();

            //Log
            _log.WriteLine($"xp du får in: {heroxp} vilket level du är/blir: {level}");

            // Assert
            Assert.Equal(expectedLevel, level);
        }

        [Fact]
        public void CalcLevel_WhenXpIsLessThanTen_ReturnsOne()
        {
            _hero.Xp = 5;
            var result = _hero.CalcLevel();
            Assert.Equal(1, result);
        }
        #endregion

        #region SetStats
        [Theory]
        // xp, expectedLevel, expectedMaxHp, expectedMaxMana, expectedAttackPower
        [InlineData(0, 1, 10, 5, 1)]
        [InlineData(5, 1, 10, 5, 1)]
        [InlineData(10, 1, 10, 5, 1)] // vid 10 xp är man fortfarande lvl 1, så denna är för lvl 1
        [InlineData(20, 2, 13, 7, 2)]
        public void SetStats_GivenXp_ShouldSetCorrectStats(int xp, int expectedLevel, int expectedMaxHp, int expectedMaxMana, int expectedAttackPower)
        {
            // Arrange
            _hero.Xp = xp;

            // Act
            _hero.SetStats();

            //Log
            _log.WriteLine($"level: {_hero.Level} max hp: {_hero.MaxHP} max mana: {_hero.MaxMana} attackpower: {_hero.AttackPower}");

            // Assert
            Assert.Equal(expectedLevel, _hero.Level);
            Assert.Equal(expectedMaxHp, _hero.MaxHP);
            Assert.Equal(expectedMaxMana, _hero.MaxMana);
            Assert.Equal(expectedMaxHp, _hero.CurrentHP);
            Assert.Equal(expectedMaxMana, _hero.CurrentMana);
            Assert.Equal(expectedAttackPower, _hero.AttackPower);
        }

        [Fact]
        public void SetStats_ShouldSetLevel_Correctly()
        {
            // Arrange
            int expectedLevel = 5;
            _hero.Xp = 50; // Assuming 10 XP per level

            // Act
            _hero.SetStats();

            _log.WriteLine($"level: {_hero.Level}");

            // Assert
            Assert.Equal(expectedLevel, _hero.Level);
        }

        [Fact]
        public void SetStats_ShouldSetHP_Correctly()
        {
            // Arrange
            int expectedLevel = 3;
            _hero.Xp = 30;
            int expectedMaxHP = 7 + 3 * expectedLevel;

            // Act
            _hero.SetStats();

            _log.WriteLine($"hero hp: {_hero.CurrentHP}");

            // Assert
            Assert.Equal(expectedMaxHP, _hero.MaxHP);
            Assert.Equal(expectedMaxHP, _hero.CurrentHP);
        }

        [Fact]
        public void SetStats_ShouldSetMana_Correctly()
        {
            // Arrange
            int expectedLevel = 4;
            _hero.Xp = 40;
            int expectedMaxMana = 3 + 2 * expectedLevel;

            // Act
            _hero.SetStats();

            _log.WriteLine($"hero mana: {_hero.CurrentMana}");

            // Assert
            Assert.Equal(expectedMaxMana, _hero.MaxMana);
            Assert.Equal(expectedMaxMana, _hero.CurrentMana);
        }

        [Fact]
        public void SetStats_ShouldSetAttackPower_Correctly_WithoutWeapon()
        {
            // Arrange
            int expectedLevel = 6;
            _hero.Xp = 60;
            _hero.EquippedWeapon = null;

            // Act
            _hero.SetStats();

            _log.WriteLine($"hero attackpower: {_hero.AttackPower}");

            // Assert
            Assert.Equal(expectedLevel, _hero.AttackPower);
        }

        [Theory]
        [InlineData(2, 5, 7)]
        [InlineData(4, 10, 14)]
        public void SetStats_ShouldSetAttackPower_Correctly_WithWeapon(int heroLevel, int weaponPower, int expectedAttackPower)
        {
            // Arrange
            _hero.Xp = heroLevel * 10;
            _hero.EquippedWeapon = new Weapon { AttackPower = weaponPower };

            // Act
            _hero.SetStats();

            _log.WriteLine($"hero attackpower: {_hero.AttackPower}");

            // Assert
            Assert.Equal(expectedAttackPower, _hero.AttackPower);
        }

        [Fact]
        public void SetStats_ShouldSetArmorValue_Correctly_WithoutArmor()
        {
            // Arrange
            _hero.EquippedArmor = null;

            // Act
            _hero.SetStats();

            _log.WriteLine($"hero armorvalue: {_hero.ArmorValue}");

            // Assert
            Assert.Equal(0, _hero.ArmorValue);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        public void SetStats_ShouldSetArmorValue_Correctly_WithArmor(int armorValue)
        {
            // Arrange
            _hero.EquippedArmor = new Armor { ArmorValue = armorValue };

            // Act
            _hero.SetStats();

            _log.WriteLine($"hero armorvalue: {_hero.ArmorValue}");

            // Assert
            Assert.Equal(armorValue, _hero.ArmorValue);
        }

        [Fact]
        public void SetStats_ShouldThrowException_WhenCalcLevelThrowsException()
        {
            // Arrange
            _hero.Xp = -10; // Assuming this will cause CalcLevel to throw an exception

            _log.WriteLine($"hero level: {_hero.Level}");

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _hero.SetStats()); // Replace with your specific exception type
        }

        [Fact]
        public void SetStats_ShouldThrowException_WhenCalcMaxHpThrowsException()
        {
            // Arrange
            _hero.Xp = -10; // Assuming this will cause CalcMaxHp to throw an exception

            _log.WriteLine($"hero max hp: {_hero.MaxHP}");

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _hero.SetStats()); // Replace with your specific exception type
        }
        #endregion

        #region CalcNormalDamage
        [Fact]
        public void CalcNormalDamage_WithWeaponEquipped_ShouldReturnCombinedDamage()
        {
            // Arrange
            Weapon SulfuronHammer = new Weapon { AttackPower = 5 };
            _hero.EquipWeapon(SulfuronHammer);
            int expectedDamage = _hero.Level + SulfuronHammer.AttackPower;

            // Act
            int damage = _hero.CalcNormalDamage();

            //Log
            _log.WriteLine($"Basedamage: {_hero.Level} AttackPower med vapen som har 5 AP:{damage}");

            // Assert
            Assert.Equal(expectedDamage, damage);
        }

        [Fact]
        public void CalcNormalDamage_WithoutWeaponEquipped_ShouldReturnHeroAttackPower()
        {
            // Arrange
            int expectedDamage = _hero.AttackPower;

            // Act
            int damage = _hero.CalcNormalDamage();

            //Log
            _log.WriteLine($"Basedamage: {_hero.AttackPower} AttackPower utan vapen:{damage}");

            // Assert
            Assert.Equal(expectedDamage, damage);
        }
        #endregion

        #region LevelUpCheck
        //Positivt test scenario
        [Fact]
        public void LevelUpCheck_WhenXpIsEnough_ShouldLevelUp()
        {
            // Arrange
            // Act: ökar xp så _hero borde lvla upp (10 xp för att lvla upp)
            _hero.Xp = 20; //Behövs 20 xp för lvl 2 (19 räcker inte)
            var oldLevel = _hero.Level;
            _hero.LevelUpCheck();

            //Log
            _log.WriteLine($"Ny level för _hero: {_hero.Level} Gammal level för _hero: {oldLevel}");

            // Assert
            Assert.True(_hero.Level > oldLevel, "Du lvlade inte upp");
        }

        //Negativt test scenario
        [Fact]
        public void LevelUpCheck_WhenXpIsNotEnough_ShouldNotLevelUp()
        {
            // Arrange
            // Act öka inte xp, borde inte lvla upp
            var oldLevel = _hero.Level;
            _hero.LevelUpCheck();

            //Log
            _log.WriteLine($"Ny level för _hero: {_hero.Level} Gammal level för _hero: {oldLevel}");

            // Assert
            Assert.Equal(oldLevel, _hero.Level);
        }
        #endregion

        #region EquipWeapon
        [Fact]
        public void EquipWeapon_ShouldEquipWeapon_WhenNoWeaponEquipped()
        {
            // Arrange
            _hero.EquippedWeapon = null;
            Weapon newWeapon = new Weapon { Name = "TestWeapon" };
            _hero.EquipmentInBag.Add(newWeapon);
            int amountInBagAtStart = _hero.EquipmentInBag.Count();

            // Act
            _hero.EquipWeapon(newWeapon);

            //Log
            _log.WriteLine($"_hero weapon: {_hero.EquippedWeapon?.Name}");

            // Assert
            Assert.Equal(_hero.EquippedWeapon?.Name, newWeapon.Name);
        }

        [Fact]
        public void EquipWeapon_ShouldRemoveWeaponFromBag_WhenWeaponEquipped()
        {
            // Arrange
            _hero.EquippedWeapon = null;
            Weapon newWeapon = new Weapon { Name = "TestWeapon" };
            _hero.EquipmentInBag.Add(newWeapon);
            int amountInBagAtStart = _hero.EquipmentInBag.Count();

            // Act
            _hero.EquipWeapon(newWeapon);

            //Log
            _log.WriteLine($"_hero weapon: {_hero.EquippedWeapon?.Name}");

            // Assert
            Assert.Equal(amountInBagAtStart - 1, _hero.EquipmentInBag.Count);
        }

        [Fact]
        public void EquipWeapon_ShouldSwapWeapons_WhenWeaponEquipped()
        {
            // Arrange
            Weapon equippedWeapon = new Weapon { Name = "InitiallyEquippedWeapon" };
            Weapon weaponToBeEquipped = new Weapon { Name = "WeaponInitiallyInBag" };
            _hero.EquippedWeapon = equippedWeapon;
            _hero.EquipmentInBag.Add(weaponToBeEquipped);
            int amountInBagAtStart = _hero.EquipmentInBag.Count();

            // Act
            _hero.EquipWeapon(weaponToBeEquipped);

            //Log
            _log.WriteLine($"_hero weapon: {_hero.EquippedWeapon?.Name}");

            // Assert
            Assert.Equal(weaponToBeEquipped, _hero.EquippedWeapon);
            Assert.Equal(amountInBagAtStart, _hero.EquipmentInBag.Count);
            Assert.Contains(_hero.EquipmentInBag, equipment => equipment is Weapon w && w.Name == equippedWeapon.Name);
            Assert.DoesNotContain(_hero.EquipmentInBag, equipment => equipment is Weapon w && w.Name == weaponToBeEquipped.Name);
        }

        [Fact]
        public void EquipWeapon_ShouldNotChangeAttackPower_WhenWeaponNotInBag()
        {
            // Arrange
            Weapon initialEquippedWeapon = new Weapon { Name = "InitiallyEquippedWeapon" };
            Weapon weaponNotInBag = new Weapon { Name = "WeaponNotInBag" };
            _hero.EquippedWeapon = initialEquippedWeapon;

            // Act
            _hero.EquipWeapon(weaponNotInBag);

            // Assert
            Assert.Equal(initialEquippedWeapon.AttackPower, _hero.EquippedWeapon.AttackPower);
            Assert.DoesNotContain(_hero.EquipmentInBag, equipment => equipment is Weapon w && w.Name == weaponNotInBag.Name);
        }

        [Fact]
        public void EquipWeapon_ShouldNotChange_WhenEquippingSameWeapon()
        {
            // Arrange
            Weapon initiallyEquippedWeapon = new Weapon { Name = "WeaponToEquipAgain" };
            _hero.EquippedWeapon = initiallyEquippedWeapon;

            // Act
            _hero.EquipWeapon(initiallyEquippedWeapon);

            // Assert
            Assert.Equal(initiallyEquippedWeapon, _hero.EquippedWeapon);
            Assert.DoesNotContain(_hero.EquipmentInBag, equipment => equipment is Weapon w && w.Name == initiallyEquippedWeapon.Name);
        }

        [Fact]
        public void EquipWeapon_ShouldSwapWeapons_WhenBagIsFull()
        {
            // Arrange
            Weapon equippedWeapon = new Weapon { Name = "InitiallyEquippedWeapon" };
            Weapon weaponToBeEquipped = new Weapon { Name = "WeaponToEquipWhenFull" };
            // sätter en hypotetisk max kapacitet på baggen till 10 för testet
            for (int i = 0; i < 10; i++)
            {
                _hero.EquipmentInBag.Add(new Weapon { Name = $"DummyWeapon{i}" });
            }
            _hero.EquippedWeapon = equippedWeapon;
            _hero.EquipmentInBag.Add(weaponToBeEquipped);

            // Act
            _hero.EquipWeapon(weaponToBeEquipped);

            // Assert
            Assert.Equal(weaponToBeEquipped, _hero.EquippedWeapon);
            Assert.Contains(_hero.EquipmentInBag, equipment => equipment is Weapon w && w.Name == equippedWeapon.Name);
        }

        [Fact]
        public void EquipWeapon_WhenWeaponIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _hero.EquipWeapon(null));
        }
        #endregion

        #region EquipArmor
        [Fact]
        public void EquipArmor_ShouldEquipArmor_WhenNoArmorEquipped()
        {
            // Arrange
            _hero.EquippedWeapon = null;
            Armor newArmor = new Armor { Name = "TestArmor" };
            _hero.EquipmentInBag.Add(newArmor);
            int amountInBagAtStart = _hero.EquipmentInBag.Count();

            // Act
            _hero.EquipArmor(newArmor);

            //Log
            _log.WriteLine($"_hero armor: {_hero.EquippedArmor?.Name}");

            // Assert
            Assert.Equal(_hero.EquippedArmor, newArmor);
            Assert.Equal(amountInBagAtStart - 1, _hero.EquipmentInBag.Count);
        }

        [Fact]
        public void EquipArmor_ShouldSwapArmors_WhenArmorEquipped()
        {
            // Arrange
            Armor equippedArmor = new Armor { Name = "InitiallyEquippedArmor" };
            Armor armorToBeEquipped = new Armor { Name = "ArmorInitiallyInBag" };
            _hero.EquippedArmor = equippedArmor;
            _hero.EquipmentInBag.Add(armorToBeEquipped);
            int amountInBagAtStart = _hero.EquipmentInBag.Count();

            // Act
            _hero.EquipArmor(armorToBeEquipped);

            //Log
            _log.WriteLine($"_hero weapon: {_hero.EquippedWeapon?.Name}");

            // Assert
            Assert.Equal(armorToBeEquipped, _hero.EquippedArmor);
            Assert.Equal(amountInBagAtStart, _hero.EquipmentInBag.Count);
            Assert.Contains(_hero.EquipmentInBag, equipment => equipment is Armor a && a.Name == equippedArmor.Name);
            Assert.DoesNotContain(_hero.EquipmentInBag, equipment => equipment is Armor a && a.Name == armorToBeEquipped.Name);
        }

        [Fact]
        public void EquipArmor_ShouldNotChangeArmorValue_WhenArmorNotInBag()
        {
            // Arrange
            Armor initialEquippedArmor = new Armor { Name = "InitiallyEquippedArmor" };
            Armor armorNotInBag = new Armor { Name = "ArmorNotInBag" };
            _hero.EquippedArmor = initialEquippedArmor;

            // Act
            _hero.EquipArmor(armorNotInBag);

            // Assert
            Assert.Equal(initialEquippedArmor.ArmorValue, _hero.EquippedArmor.ArmorValue);
            Assert.DoesNotContain(_hero.EquipmentInBag, equipment => equipment is Armor a && a.Name == armorNotInBag.Name);
        }

        [Fact]
        public void EquipArmor_ShouldSwapArmors_WhenBagIsFull()
        {
            // Arrange
            Armor equippedArmor = new Armor { Name = "InitiallyEquippedArmor" };
            Armor armorToBeEquipped = new Armor { Name = "ArmorToEquipWhenFull" };

            // sätter en hypotetisk max kapacitet på baggen till 10 för testet
            for (int i = 0; i < 10; i++)
            {
                _hero.EquipmentInBag.Add(new Armor { Name = $"DummyArmor{i}" });
            }
            _hero.EquippedArmor = equippedArmor;
            _hero.EquipmentInBag.Add(armorToBeEquipped);

            // Act
            _hero.EquipArmor(armorToBeEquipped);

            // Assert
            Assert.Equal(armorToBeEquipped, _hero.EquippedArmor);
            Assert.Contains(_hero.EquipmentInBag, equipment => equipment is Armor a && a.Name == equippedArmor.Name);
        }

        [Fact]
        public void EquipArmor_ShouldNotChange_WhenEquippingSameArmor()
        {
            // Arrange
            Armor initiallyEquippedArmor = new Armor { Name = "ArmorToEquipAgain" };
            _hero.EquippedArmor = initiallyEquippedArmor;

            // Act
            _hero.EquipArmor(initiallyEquippedArmor);

            // Assert
            Assert.Equal(initiallyEquippedArmor, _hero.EquippedArmor);
            Assert.DoesNotContain(_hero.EquipmentInBag, equipment => equipment is Armor a && a.Name == initiallyEquippedArmor.Name);
        }

        [Fact]
        public void EquipArmor_ShouldUpdateArmorValue_Correctly()
        {
            // Arrange
            Armor armorToBeEquipped = new Armor { Name = "ArmorWithSpecificValue", ArmorValue = 10 };
            _hero.EquipmentInBag.Add(armorToBeEquipped);

            // Act
            _hero.EquipArmor(armorToBeEquipped);

            // Assert
            Assert.Equal(10, _hero.ArmorValue);
        }

        [Fact]
        public void EquipArmor_WhenArmorIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _hero.EquipArmor(null));
        }
        #endregion

    }
}