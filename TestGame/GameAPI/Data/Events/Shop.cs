using GameAPI.Data.Items.Equipment;

namespace GameAPI.Data.Events
{
	public class Shop : Location
	{
		public List<Equipment> EquipmentForSale { get; set; }
		public Shop(string name) : base(name)
		{
			EquipmentForSale = new List<Equipment>();
		}
	}
}
