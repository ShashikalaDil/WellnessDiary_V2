using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellnessDiaryApi.Data;
using WellnessDiaryApi.Data.Dto;
using WellnessDiaryApi.Models;

namespace WellnessDiaryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalTestsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MedicalTestsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        // GET: api/MedicalTests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalTest>>> GetMedicalTests()
        {
            if (_context.MedicalTests == null)
            {
                return NotFound();
            }
            return await _context.MedicalTests.ToListAsync();
        }

        // GET: api/MedicalTests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalTest>> GetMedicalTest(int id)
        {
            if (_context.MedicalTests == null)
            {
                return NotFound();
            }
            var medicalTest = await _context.MedicalTests.FindAsync(id);

            if (medicalTest == null)
            {
                return NotFound();
            }

            return medicalTest;
        }


        // GET: api/MedicalTests/ByCategory/{categoryId}
        [HttpGet("ByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<MedicalTest>>> GetMedicalTestsByCategory(int categoryId)
        {
            var medicalTests = await _context.MedicalTests
                                        .Where(a => a.CategoryId == categoryId)
                                        .ToListAsync();

            if (medicalTests == null || medicalTests.Count == 0)
            {
                return NotFound();
            }

            return medicalTests;
        }


        // PUT: api/MedicalTests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicalTest(int id, [FromForm] MedicalTestDTO medicalTestDTO, IFormFile formFile)
        {
            if (id != medicalTestDTO.TestId)
            {
                return BadRequest("Mismatch between provided ID and test ID in the data.");
            }

            var medicalTest = await _context.MedicalTests.FindAsync(id);
            if (medicalTest == null)
            {
                return NotFound("Medical test not found.");
            }

            medicalTest.TestName = medicalTestDTO.TestName;
            medicalTest.CategoryId = medicalTestDTO.CategoryId;
            medicalTest.Description = medicalTestDTO.Description;

            // Update the image path if a new image is provided
            if (formFile != null && formFile.Length > 0)
            {
                string imagePath = await UploadImage(formFile, medicalTestDTO.TestId.ToString());
                medicalTest.ImagePath = imagePath;
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Changes saved successfully." });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log or handle the concurrency exception appropriately
                return StatusCode(500, $"An error occurred while saving changes: {ex.Message}");
            }
        }
                   

        // DELETE: api/MedicalTests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalTest(int id)
        {
            if (_context.MedicalTests == null)
            {
                return NotFound();
            }
            var medicalTest = await _context.MedicalTests.FindAsync(id);
            if (medicalTest == null)
            {
                return NotFound();
            }

            _context.MedicalTests.Remove(medicalTest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicalTestExists(int id)
        {
            return (_context.MedicalTests?.Any(e => e.TestId == id)).GetValueOrDefault();
        }

        ////////////////////////////
        ///

        // Modify the UploadImage method to save the image to wwwroot/Upload/Article
        private async Task<string> UploadImage(IFormFile formFile, string testId)
        {
            try
            {
                string uploadFolder = Path.Combine(this._webHostEnvironment.WebRootPath, "Upload", "TestsArticle");
                Console.WriteLine($"Upload folder: {uploadFolder}");

                // Ensure the directory exists or create it
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                    Console.WriteLine("Upload directory created.");
                }

                // Combine article ID and original image name for the file name
                string fileName = $"{testId}_{Path.GetFileName(formFile.FileName)}";
                string imagePath = Path.Combine(uploadFolder, fileName);

                // Save the file to disk
                using (FileStream stream = System.IO.File.Create(imagePath))
                {
                    await formFile.CopyToAsync(stream);
                }

                // Construct the full URL for accessing the uploaded image
                string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                string imageUrl = $"{baseUrl}/Upload/TestsArticle/{fileName}";

                return imageUrl;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error uploading image: {ex.Message}");
                throw;
            }
        }

        // POST: api/MedicalTests
        // In the PostArticle method, include the image upload logic
        [HttpPost]
        public async Task<ActionResult<MedicalTest>> PostMedicalTest([FromForm] MedicalTestDTO medicalTestDTO, IFormFile formFile)
        {
            try
            {
                if (_context == null || _context.MedicalTests == null)
                {
                    return Problem("DbContext or DbSet 'MedicalTests' is null.");
                }

                if (formFile != null && formFile.Length > 0)
                {
                    string imageUrl = await UploadImage(formFile, medicalTestDTO.TestId.ToString());
                    medicalTestDTO.ImagePath = imageUrl;
                }

                var medicalTest = new MedicalTest
                {
                    TestName = medicalTestDTO.TestName,
                    CategoryId = medicalTestDTO.CategoryId,
                    Description = medicalTestDTO.Description,
                    ImagePath = medicalTestDTO.ImagePath
                };

                _context.MedicalTests.Add(medicalTest);

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMedicalTest", new { id = medicalTestDTO.TestId }, medicalTestDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating medical test: {ex.Message}");

                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


    }
}
