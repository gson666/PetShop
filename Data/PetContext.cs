using Microsoft.EntityFrameworkCore;
using PetSite.Models;
using System.Reflection.Emit;

namespace PetSite.Data
{
    public class PetContext : DbContext
    {
        public PetContext(DbContextOptions<PetContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>()
                 .HasKey(A => A.AnimalId);

            modelBuilder.Entity<Category>()
                 .HasKey(A => A.CategoryId);

            modelBuilder.Entity<Comment>()
                .HasKey(A => A.CommentId);

            modelBuilder.Entity<Comment>()
                 .HasOne(c => c.CommentAnimal)
                 .WithMany(a => a.CommentsList)
                 .HasForeignKey(f => f.AnimalId);
            //modelBuilder.Entity<Animal>()
            //    .HasOne(a => a.AnimalCategory)
            //    .WithMany(c => c.AnimalsList)
            //    .HasForeignKey(a => a.CategoryId);


            modelBuilder.Entity<Animal>().HasData(
               new Animal { AnimalId = 1, Name = "Dog", Age = 2, ImageUrl = "/Images/dog.jpg" ,Description = "A dog is a domesticated mammal known for its loyalty, companionship, and diverse breeds. They come in various sizes, shapes, and personalities, making them popular pets worldwide. Dogs are highly social animals, forming strong bonds with their human owners and often displaying affection through tail wagging, barking, and cuddling. They serve various roles, including as working dogs, therapy animals, and family pets, and their intelligence allows them to learn commands and perform tasks.", CategoryId = 1 },
               new Animal { AnimalId = 2, Name = "Cat", Age = 3, ImageUrl ="/Images/cat.jpg", Description = "A cat is a small, independent domesticated mammal known for its agility, grooming habits, and playful nature. They come in various breeds and are cherished as pets for their companionship and enigmatic personalities.", CategoryId = 1 },
               new Animal { AnimalId = 3, Name = "Eagle", Age = 1, ImageUrl ="/Images/eagle.jpg", Description = "An eagle is a powerful bird of prey known for its keen eyesight, soaring flight, and hunting prowess. They symbolize strength and are revered for their majestic presence in the sky.", CategoryId = 2 },
               new Animal { AnimalId = 4, Name = "Owl", Age = 5, ImageUrl ="/Images/owl.jpg", Description = "Owls are nocturnal birds of prey known for their night vision, silent flight, and hooting calls. They symbolize wisdom and are skilled hunters with unique adaptations for catching prey.", CategoryId = 2 },
               new Animal { AnimalId = 5, Name = "Goldfish", Age = 20, ImageUrl = "/Images/goldfish.jpg", Description = "A goldfish is a small, colorful freshwater fish often kept as a pet in aquariums. They are known for their bright scales and graceful swimming patterns, bringing beauty and relaxation to home aquariums. Goldfish are relatively easy to care for and have a reputation for their longevity.", CategoryId = 3 },
               new Animal { AnimalId = 6, Name = "Shark", Age = 33, ImageUrl ="/Images/shark.jpg", Description = "A shark is a large, predatory fish known for its sleek, powerful body and sharp teeth. They inhabit oceans worldwide and play a crucial role in marine ecosystems as apex predators. Sharks come in various species, each adapted to specific environments, and they are often regarded with fascination and awe due to their ancient lineage and formidable hunting abilities.", CategoryId = 3 }
               );

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Mammals" },
                new Category { CategoryId = 2, Name = "Birds" },
                new Category { CategoryId = 3, Name = "Fish" }
                );

            modelBuilder.Entity<Comment>().HasData(
                new Comment { CommentId = 1, AnimalId = 1, CommentText = "wow! what a nice creature leaving besides us" },
                new Comment { CommentId = 2, AnimalId = 1, CommentText = "Iwant that guy!" },
                new Comment { CommentId = 3, AnimalId = 1, CommentText = "OMG he's so cute!!" },
                new Comment { CommentId = 4, AnimalId = 2, CommentText = "Whats the requested price for this cutie? :)" },
                new Comment { CommentId = 5, AnimalId = 2, CommentText = "I want to take this guy but he might ruin our home :(" },
                new Comment { CommentId = 6, AnimalId = 3, CommentText = "Damnn hes marvelous" },
                new Comment { CommentId = 7, AnimalId = 4, CommentText = "Lol who who" },
                new Comment { CommentId = 8, AnimalId = 4, CommentText = "Leave this guy alone" }
                );
            modelBuilder.Entity<Animal>().ToTable("Animals");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Comment>().ToTable("Comments");
        }
        public virtual DbSet<Animal> Animals { get; set; }
        public virtual DbSet<Category> Categories { get;set; }
        public virtual DbSet<Comment> Comments { get; set; }

    }
}
