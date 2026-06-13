using EKemit.APIs.DTOs;
using EKemit.Core.Entities;
using EKemit.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EKemit.APIs.Controllers
{
   
    public class RatingController : APIBaseController
    {
        private readonly StoreContext _context;

        public RatingController(StoreContext context)
        {
            _context = context;
        }


        [Authorize]
        [HttpPost("{productId}")]
        public async Task<ActionResult> CreateRating([FromRoute] int? productId, [FromForm] RatingDto ratingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var existingRating = _context.Ratings.FirstOrDefault(r => r.ProductId == productId && r.BuyerEmail == buyerEmail);

            if (existingRating != null)
            {
                existingRating.Value = ratingDto.Value;
                _context.Ratings.Update(existingRating);
            }
            else
            {
                var rating = new Rating
                {
                    Value = ratingDto.Value,
                    ProductId = ratingDto.ProductId,
                    BuyerEmail = buyerEmail
                };
                _context.Ratings.Add(rating);
            }
            await _context.SaveChangesAsync();

            var product = _context.Products.Find(productId);
            var ratings = _context.Ratings.Where(r => r.ProductId == productId);
            if (ratings.Any())
            {
                var averageRating = ratings.Average(r => r.Value);
                var score = Math.Round(averageRating, 2);
                product.Rating = score;
                _context.SaveChanges();
            }

            return Ok();
        }








    }
}
