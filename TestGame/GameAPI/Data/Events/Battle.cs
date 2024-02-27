using GameAPI.Data.Characters;

namespace GameAPI.Data.Events
{
	internal class Battle : Location
    {
        public Enemy Enemy { get; set; }
        public int DamageDoneLastTurn { get; set; }
        public int DamageTakenLastTurn { get; set; }
        public Battle(string name, Enemy enemy) : base(name)
        {
            Enemy = enemy;
        }
    }
}
