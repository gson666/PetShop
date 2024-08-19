using MyPetStore.Validation;

namespace PetSite.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

       [IsTextValid]
        public string Name { get; set; }
      // public List<Animal> AnimalsList { get; set; }
    }
}
