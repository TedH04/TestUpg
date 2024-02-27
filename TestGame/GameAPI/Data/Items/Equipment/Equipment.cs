using GameAPI.Data.Items.Equipment.Armors;
using GameAPI.Data.Items.Equipment.Weapons;
using System.Text.Json.Serialization;

namespace GameAPI.Data.Items.Equipment
{
	[JsonDerivedType(typeof(Weapon))]
    [JsonDerivedType(typeof(Armor))]
    public class Equipment
    {
        public string Name { get; set; } = null!;
        public int Price { get; set; } = 0;
    }
}
