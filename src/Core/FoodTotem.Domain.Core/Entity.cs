namespace FoodTotem.Domain.Core
{
    public class Entity
    {
        public Guid Id { get; private set; }
        public Entity() 
        { 
            Id = Guid.NewGuid();
        }
    }
}