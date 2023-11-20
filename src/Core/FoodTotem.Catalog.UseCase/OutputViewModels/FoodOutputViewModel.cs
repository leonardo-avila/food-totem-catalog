namespace FoodTotem.Catalog.UseCase.OutputViewModels
{
	public class FoodOutputViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Category { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public string ImageUrl { get; set; }
	}
}