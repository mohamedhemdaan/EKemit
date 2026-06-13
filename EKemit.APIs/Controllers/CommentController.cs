using EKemit.APIs.DTOs;
using EKemit.Core;
using EKemit.Core.Entities;
using EKemit.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EKemit.APIs.Controllers
{
    public class CommentController : APIBaseController
    {
        private readonly StoreContext _context;

        public CommentController(StoreContext context)
        {
            _context = context;
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateComment([FromForm] CommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var comment = new Comment
            {
                Text = commentDto.Text,
                BuyerEmail = buyerEmail,
                ProductId = commentDto.ProductId
            };

            _context.Comments.Add(comment);


            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCommentsByProductId(int productId)
        {
            var comments = _context.Comments.Where(c => c.ProductId == productId).ToList();
            if (comments != null && comments.Any())
            {
                return Ok(comments);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
