using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using WellnessDiaryApi.Data;
using WellnessDiaryApi.Models;
using WellnessDiaryApi.Data.Dto;

namespace WellnessDiaryApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticlesController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            if (_context.Articles == null)
            {
                return NotFound();
            }
            return await _context.Articles.ToListAsync();
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            if (_context.Articles == null)
            {
                return NotFound();
            }
            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        // GET: api/Articles/ByCategory/{categoryId}
        [HttpGet("ByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticlesByCategory(int categoryId)
        {
            var articles = await _context.Articles
                                        .Where(a => a.CategoryId == categoryId)
                                        .ToListAsync();

            if (articles == null || articles.Count == 0)
            {
                return NotFound();
            }

            return articles;
        }

        // PUT: api/Articles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, [FromForm] ArticleDTO articleDTO, IFormFile formFile)
        {
            if (id != articleDTO.ArticleId)
            {
                return BadRequest();
            }

            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            // Update the article properties
            article.Title = articleDTO.Title;
            article.Content = articleDTO.Content;
            article.CategoryId = articleDTO.CategoryId;
            article.Author = articleDTO.Author;
            article.PublishedDate = DateTime.Now;

            // Update the image path if a new image is provided
            if (formFile != null && formFile.Length > 0)
            {
                string imagePath = await UploadImage(formFile, articleDTO.ArticleId.ToString());
                articleDTO.ImagePath = imagePath;
                article.ImagePath = imagePath;
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Changes saved successfully." });
        }


        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            if (_context.Articles == null)
            {
                return NotFound();
            }
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return (_context.Articles?.Any(e => e.ArticleId == id)).GetValueOrDefault();
        }

        /// <New Post method>
        /// //////////////////////////////////////////
        /// </summary>        

       

        // Modify the UploadImage method to save the image to wwwroot/Upload/Article
        private async Task<string> UploadImage(IFormFile formFile, string articleId)
        {
            try
            {
                string uploadFolder = Path.Combine(this._webHostEnvironment.WebRootPath, "Upload", "Article");
                Console.WriteLine($"Upload folder: {uploadFolder}"); 

                // Ensure the directory exists or create it
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                    Console.WriteLine("Upload directory created."); 
                }

                // Combine article ID and original image name for the file name
                string fileName = $"{articleId}_{Path.GetFileName(formFile.FileName)}";
                string imagePath = Path.Combine(uploadFolder, fileName);

                // Save the file to disk
                using (FileStream stream = System.IO.File.Create(imagePath))
                {
                    await formFile.CopyToAsync(stream);
                }

                // Construct the full URL for accessing the uploaded image
                string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                string imageUrl = $"{baseUrl}/Upload/Article/{fileName}";

                return imageUrl;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error uploading image: {ex.Message}");
                throw; 
            }
        }

        // POST: api/Articles
        // In the PostArticle method, include the image upload logic
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle([FromForm] ArticleDTO articleDTO, IFormFile formFile)
        {
            try
            {
                if (_context.Articles == null)
                {
                    return Problem("Entity set 'AppDbContext.Articles'  is null.");
                }

                // Upload image if provided
                if (formFile != null && formFile.Length > 0)
                {
                    string imageUrl = await UploadImage(formFile, articleDTO.ArticleId.ToString());
                    articleDTO.ImagePath = imageUrl;
                }
                var article = new Article
                {
                    Title = articleDTO.Title,
                    Content = articleDTO.Content,
                    CategoryId = articleDTO.CategoryId,
                    Author = articleDTO.Author,
                    PublishedDate = articleDTO.PublishedDate,
                    ImagePath = articleDTO.ImagePath
                };
                _context.Articles.Add(article);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetArticle", new { id = articleDTO.ArticleId }, articleDTO);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error creating article: {ex.Message}");
                
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }



    }
}
