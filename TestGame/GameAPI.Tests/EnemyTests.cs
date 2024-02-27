namespace GameAPI.Tests
{
    public class EnemyTests
    {
        #region Constructor
        private readonly ITestOutputHelper _log;

        public EnemyTests(ITestOutputHelper output)
        {
            _log = output;
        }
        #endregion

        #region CalcNormalDamage
        [Fact]
        public void CalcNormalDamage_ReturnsZero_WhenEnemyIsDead()
        {
            // Arrange
            var enemy = new Enemy();
            enemy.CurrentHP = 0;
            // Act
            int damage = enemy.CalcNormalDamage();
            //Log
            _log.WriteLine($"fienden är död {damage} hp kvar");
            // Assert
            Assert.Equal(0, damage);
        }
        [Fact]
        public void CalcNormalDamage_ReturnsAttackPowerValue()
        {
            //testar det här bara för att se så metoden funkar på fiendes ap

            // Arrange
            var enemy = new Enemy();
            enemy.AttackPower = 25;

            // Act
            int damage = enemy.CalcNormalDamage();

            //Log
            _log.WriteLine($"fiendes attackpower: {damage}");

            // Assert
            Assert.Equal(25, damage);
        }
        #endregion

        #region DropEquipment
        //Negativt scenario
        [Fact]
        public void DropEquipment_ReturnsNull_WhenLootTableIsEmpty()
        {
            // Arrange
            var enemy = new Enemy();
            enemy.LootTable = new List<Equipment>();

            // Act
            var equipment = enemy.DropEquipment();

            //Log
            _log.WriteLine($"dina drops finns inte");

            // Assert
            Assert.Null(equipment);
        }

        [Fact]
        public void DropEquipment_ReturnsNull_WhenLootTableIsNull()
        {
            // Arrange
            var enemy = new Enemy();
            enemy.LootTable = null;

            // Act
            var droppedEquipment = enemy.DropEquipment();

            // Assert
            Assert.Null(droppedEquipment);
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(101)]
        public void DropEquipment_ShouldThrowException_WhenNoDropChanceIsOutOfBounds(int invalidNoDropChance)
        {
            // Arrange
            var enemy = new Enemy();
            enemy.NoDropChance = invalidNoDropChance;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => enemy.DropEquipment());
        }

        //Positivt scenario
        [Fact]
        public void DropEquipment_ReturnsEquipmentFromLootTable()
        {
            // Arrange
            var enemy = new Enemy();
            enemy.NoDropChance = 0;
            var mockEquipment = new Mock<Equipment>().Object;
            mockEquipment.Name = "infantry lasgun";
            enemy.LootTable.Add(mockEquipment);

            // Act
            var droppedEquipment = enemy.DropEquipment();

            //Log
            _log.WriteLine($"Dropped equipment: {droppedEquipment.Name}");

            // Assert
            Assert.Contains(droppedEquipment, enemy.LootTable);
        }

        [Fact]
        public void DropEquipment_ReturnsNull_WhenNoDropChanceIs100()
        {
            // Arrange
            var enemy = new Enemy();
            enemy.NoDropChance = 100;
            var mockEquipment = new Equipment();
            mockEquipment.Name = "TestWeapon";
            enemy.LootTable.Add(mockEquipment);

            // Act
            var droppedEquipment = enemy.DropEquipment();

            // Assert
            Assert.Null(droppedEquipment);
        }

        [Fact]
        public void DropEquipment_CanReturnAnyItemFromLootTable()
        {
            // Arrange
            var enemy = new Enemy();
            enemy.NoDropChance = 0;
            var mockEquipment1 = new Equipment { Name = "Weapon1" };
            var mockEquipment2 = new Equipment { Name = "Weapon2" };
            enemy.LootTable.AddRange(new[] { mockEquipment1, mockEquipment2 });

            // Act & Assert
            var results = new HashSet<string>();
            for (int i = 0; i < 100; i++)
            {
                var droppedEquipment = enemy.DropEquipment();
                results.Add(droppedEquipment.Name);
            }
            Assert.Contains(mockEquipment1.Name, results);
            Assert.Contains(mockEquipment2.Name, results);
        }
        #endregion

        #region Clone
        [Fact]
        public void Clone_ReturnsClone_ButNotIdentical()
        {
            // Arrange
            var originalEnemy = new Enemy
            {
                Id = 1,
                Name = "TestEnemy",
                LootTable = new List<Equipment>
                {
                    new Equipment { Name = "Item1" },
                    new Equipment { Name = "Item2" }
                }
            };

            // Act
            var clonedEnemy = (Enemy)originalEnemy.Clone();

            // Assert
            Assert.NotSame(originalEnemy, clonedEnemy);
        }
        #endregion

    }
}
