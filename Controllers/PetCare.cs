using Azure.Core;
using Azure.Messaging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using PetSite.Data;
using PetSite.Models;
namespace PetSite.Controllers
{
    public class PetCare : Controller
    {
        private readonly PetContext _context;
        public PetCare(PetContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var Comments = await _context.Animals
                .Include(c => c.CommentsList)
                .OrderByDescending(a => a.CommentsList.Count)
                .ToListAsync();
            var Topcomments = Comments.Take(2).ToList();
            return View(Topcomments);
        }
        public async Task<IActionResult> Catalog(int? choice)
        {
            var animals = await _context.Animals.ToListAsync();
            var selectedanimals = animals.Where(a => a.CategoryId == choice).ToList();

            return View("Index2", selectedanimals);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var Adetails = await _context.Animals
            .Include(a => a.CommentsList)
            .Include(a => a.AnimalCategory)
            .Select(a => new
            {
                Animal = a,
                CategoryName = a.AnimalCategory.Name,
                a.AnimalId
            })
            .SingleOrDefaultAsync(a => a.AnimalId == id);

            if (Adetails != null)
            {
                var animal = Adetails.Animal;
                var categoryName = Adetails.CategoryName;

                ViewBag.CategoryName = categoryName;

                return View("Index3", animal);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Animal(int animalId, string commentText)
        {

            var animal = await _context.Animals.FindAsync(animalId);

            if (animal != null && !string.IsNullOrEmpty(commentText))
            {
                var newComment = new Comment
                {
                    CommentText = commentText,
                    AnimalId = animalId
                };

                _context.Comments.Add(newComment);
                await _context.SaveChangesAsync();
            }
            return View("Index3", animal);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var AnimalToEdit = await _context.Animals
                .Include(a => a.AnimalCategory)
                .SingleOrDefaultAsync(a => a.AnimalId == id);
            if (AnimalToEdit == null)
            {
                return NotFound();
            }
            return View(AnimalToEdit);
        }
        
        [HttpPost]
        public async Task<IActionResult> Update(Animal ani, IFormFile imageFile)
        {
            var animal = await _context.Animals
                .Include(a => a.AnimalCategory)
                .FirstOrDefaultAsync(a => a.AnimalId == ani.AnimalId);

            if (animal == null)
            {
                return NotFound();
            }
            animal.Name = ani.Name;
            animal.Age = ani.Age;
            animal.Description = ani.Description;

            if (imageFile != null && imageFile.Length > 0)
            {
                var originalFileName = imageFile.FileName;
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                var PhotoPath = Path.Combine(imagePath, originalFileName);

                using (var stream = new FileStream(PhotoPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                animal.ImageUrl = "/Images/" + originalFileName;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Admin");
        }


        [HttpGet]
        public async Task<IActionResult> AddNew()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
            var animal = new Animal();
            return View(animal);
        }


        [HttpPost]
        public IActionResult AddNew(Animal newAnimal, IFormFile ImageUrl, int CategoryId)
        {
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;

            var categoryExists = _context.Categories.Any(c => c.CategoryId == CategoryId);
            if(categoryExists)
            {
                newAnimal.CategoryId = CategoryId;
            }

                string uploadedFileName = null;

                if (ImageUrl != null && ImageUrl.Length > 0)
                {
                    var uniqueFileName = ImageUrl.FileName;
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");

                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    var filePath = Path.Combine(uploadDir, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageUrl.CopyTo(stream);
                    }

                    uploadedFileName = uniqueFileName;
                }

                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(uploadedFileName))
                    {
                        newAnimal.ImageUrl = "/Images/" + uploadedFileName;
                    }

                    _context.Animals.Add(newAnimal);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            

            return View(newAnimal);
        }


        public async Task<IActionResult> Remove(int id)
        {
            var ToRemove = await _context.Animals.FindAsync(id);
            if (ToRemove == null)
            {
                return NotFound();
            }
            _context.Animals.Remove(ToRemove);
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        public async Task<IActionResult> Admin(int choice)
        {
            var Spec = await _context.Animals.ToListAsync();
            var select = Spec.Where(a => a.CategoryId == choice).ToList();
            if (choice == 4)
            {
                var all = await _context.Animals
                    .OrderBy(a => a.CategoryId)
                    .ToListAsync();

                return View(all);
            }
            return View(select);
        }





    }
}
