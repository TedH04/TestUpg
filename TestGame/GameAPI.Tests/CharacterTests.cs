namespace GameAPI.Tests
{
    public class TestCharacter : Character
    {
        //på grund av att Character är abstract så kan vi inte skapa en instans av den, därför skapar vi en ny klass som ärver från Character
        //för att skapa upp  metoderna som vi kan testa
        private int? _testDamage;
        public void TestCalcNormalDamage(int value)
        {
            _testDamage = value;
        }

        public virtual int CalcNormalDamage()
        {
            return _testDamage ?? base.CalcNormalDamage();
        }
    }

    public class CharacterTests
    {
        #region Constructor
        private readonly ITestOutputHelper _log;

        public CharacterTests(ITestOutputHelper output)
        {
            _log = output;
        }
        #endregion

        #region Attack


        [Fact]
        public void Attack_ShouldReduceEnemyCurrentHPByCorrectAmount()
        {
            // Arrange
            var attacker = new TestCharacter();
            var enemy = new TestCharacter();

            attacker.TestCalcNormalDamage(40);
            attacker.AttackPower = 40;
            enemy.ArmorValue = 10;
            enemy.CurrentHP = 100;

            // Act
            attacker.Attack(enemy);

            //Log
            _log.WriteLine($"din attackpower till en början: {attacker.AttackPower}");
            _log.WriteLine($"fiendes armorvalue: {enemy.ArmorValue}");
            _log.WriteLine($"fiendes currenthp: 100");
            _log.WriteLine($"hur mycket damage du gör på fiende: {attacker.CalcNormalDamage()}");
            _log.WriteLine($"fiendes hp efter du attackerat: {enemy.CurrentHP} MATHS: 100 + 10armor - 40 damage = 70");

            // Assert
            Assert.Equal(70, enemy.CurrentHP); // 100 - (40 - 10) = 70 fiende borde ha 70hp
        }

        [Fact]
        public void Attack_ShouldNotHealEnemy_WhenHighArmorAndLowAttackPower()
        {
            // Arrange
            var attacker = new TestCharacter();
            var enemy = new TestCharacter();

            attacker.TestCalcNormalDamage(10);
            attacker.AttackPower = 10;
            enemy.ArmorValue = 50;
            enemy.CurrentHP = 100;

            // Act
            attacker.Attack(enemy);

            // Assert
            Assert.Equal(100, enemy.CurrentHP); // dmg borde inte heal enemy
        }

        [Fact]
        public void Attack_ShouldThrowException_WhenOpponentIsNull()
        {
            // Arrange
            var attacker = new TestCharacter();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => attacker.Attack(null));
        }

        [Fact]
        public void Attack_ShouldDealZeroDamage_WhenArmorEqualsDamage()
        {
            //arrange
            var attacker = new TestCharacter();
            var enemy = new TestCharacter();
            attacker.AttackPower = 50;
            enemy.ArmorValue = 50;
            enemy.CurrentHP = 100;
            //act
            attacker.Attack(enemy);

            _log.WriteLine($"enemy first hp {enemy.MaxHP}");
            _log.WriteLine($"enemy current hp: {enemy.CurrentHP}");

            //Assert
            Assert.Equal(100, enemy.CurrentHP);
        }

        [Fact]
        public void Attack_WithNegativeAttackPower_ShouldNotIncreaseEnemyHP()
        {
            //Arrange
            var attacker = new TestCharacter();
            var enemy = new TestCharacter();
            attacker.AttackPower = -10;
            enemy.CurrentHP = 100;
            //Act
            attacker.Attack(enemy);

            _log.WriteLine($"enemy first hp {enemy.MaxHP}");
            _log.WriteLine($"enemy current hp: {enemy.CurrentHP}");

            //Assert
            Assert.Equal(100, enemy.CurrentHP);
        }

        [Fact]
        public void Attack_ReturnsCorrectDamageDealt()
        {
            //Arrange
            var attacker = new TestCharacter();
            var enemy = new TestCharacter();
            attacker.AttackPower = 40;
            enemy.ArmorValue = 10;
            enemy.CurrentHP = 100;
            //Act
            int damageDealt = attacker.Attack(enemy);

            _log.WriteLine($"enemy first hp {enemy.MaxHP}");
            _log.WriteLine($"enemy current hp: {enemy.CurrentHP}");

            //Assert
            Assert.Equal(30, damageDealt);
        }

        #endregion

        #region CalcNormalDamage

        [Fact]
        public void CalcNormalDamage_ShouldReturnAttackPower()
        {
            // Arrange
            var character = new TestCharacter();
            character.AttackPower = 20;

            // Act
            int damage = character.CalcNormalDamage();

            // Assert
            Assert.Equal(20, damage);
        }

        #endregion

        #region CalcCriticalDamage
        [Theory]
        [InlineData(25)]
        [InlineData(75)]
        [InlineData(50)]
        public void CalcCriticalDamage_ShouldBeDeterministic_GivenFixedCritChance(int critChance)
        {
            // Arrange
            var attacker = new TestCharacter();
            attacker.CritChance = critChance;

            // Act & Assert
            for (int i = 0; i < 100; i++)
            {
                int damageMultiplier = attacker.CalcCriticalDamage(attacker.CritChance);
                Assert.True(damageMultiplier == 1 || damageMultiplier == 2);
            }
        }

        [Fact]
        public void CalcCriticalDamage_ReturnsHigherMultiplier_IfCritOccurs()
        {
            // Arrange
            var attacker = new TestCharacter();
            attacker.CritChance = 100;

            // Act
            int damageMultiplier = attacker.CalcCriticalDamage(attacker.CritChance);

            // Assert
            Assert.True(damageMultiplier > 1);
        }

        [Fact]
        public void CalcCriticalDamage_ReturnsOne_IfCritFails()
        {
            // Arrange
            var attacker = new TestCharacter();
            attacker.CritChance = 0;

            // Act
            int damageMultiplier = attacker.CalcCriticalDamage(attacker.CritChance);

            // Assert
            Assert.True(damageMultiplier == 1);
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(-10)]
        public void CalcCriticalDamage_ShouldThrowException_WhenCritChanceOutOfBounds(int invalidCritChance)
        {
            // Arrange
            var attacker = new TestCharacter();

            _log.WriteLine($"invalidCritChance: {invalidCritChance}");

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => attacker.CalcCriticalDamage(invalidCritChance));
        }
        #endregion

        #region CalcDodge
        [Theory]
        [InlineData(25)]
        [InlineData(75)]
        [InlineData(50)]
        public void CalcDodge_ShouldProduceConsistentDodgeResults(int dodgeChance)
        {
            // Arrange
            var attacker = new TestCharacter();
            attacker.DodgeChance = dodgeChance;

            _log.WriteLine($"dodgeChance: {dodgeChance}");

            // Act & Assert
            for (int i = 0; i < 100; i++)
            {
                int damageMultiplier = attacker.CalcDodge(attacker.DodgeChance);
                Assert.True(damageMultiplier == 0 || damageMultiplier == 1);
            }
        }

        [Fact]
        public void CalcDodge_ReturnsZero_IfDodgeWorks()
        {
            // Arrange
            var attacker = new TestCharacter();
            attacker.DodgeChance = 100;

            // Act
            int damageMultiplier = attacker.CalcDodge(attacker.DodgeChance);

            _log.WriteLine($"dmg multiplier: {damageMultiplier}");

            // Assert
            Assert.True(damageMultiplier == 0);
        }

        [Fact]
        public void CalcDodge_ReturnsOne_IfFailsOccurs()
        {
            // Arrange
            var attacker = new TestCharacter();
            attacker.DodgeChance = 0;


            // Act
            int damageMultiplier = attacker.CalcDodge(attacker.DodgeChance);

            _log.WriteLine($"dmg multiplier: {damageMultiplier}");

            // Assert
            Assert.True(damageMultiplier == 1);
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(-10)]
        public void CalcDodge_ShouldThrowException_WhenDodgeChanceOutOfBounds(int invalidDodgeChance)
        {
            // Arrange
            var attacker = new TestCharacter();

            _log.WriteLine($"invalid dodge chance: {invalidDodgeChance}");

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => attacker.CalcDodge(invalidDodgeChance));
        }
        [Fact]
        public void CalcDodgeDefender_ReturnsZero_IfDodgeWorks()
        {
            //Arrange
            var defender = new TestCharacter();
            defender.DodgeChance = 100;
            //Act
            int dodgeMultiplier = defender.CalcDodge(defender.DodgeChance);

            _log.WriteLine($"dodge multiplier: {dodgeMultiplier}");

            //Assert
            Assert.True(dodgeMultiplier == 0);
        }
        [Fact]
        public void CalcDodgeDefender_ReturnsOne_IfDodgeFails()
        {
            //Arrange
            var defender = new TestCharacter();
            defender.DodgeChance = 0;
            //Act
            int dodgeMultiplier = defender.CalcDodge(defender.DodgeChance);

            _log.WriteLine($"dodge multiplier: {dodgeMultiplier}");

            //Assert
            Assert.True(dodgeMultiplier == 1);
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(-10)]
        public void CalcDodgeDefender_ShouldThrowException_WhenDodgeChanceOutOfBounds(int invalidDodgeChance)
        {
            //Arrange
            var defender = new TestCharacter();

            _log.WriteLine($"invalid dodge chance: {invalidDodgeChance}");

            //Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => defender.CalcDodge(invalidDodgeChance));
        }
        #endregion

    }
}
