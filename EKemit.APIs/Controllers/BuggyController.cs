using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EKemit.APIs.Errors;
using EKemit.Repository.Data;

namespace EKemit.APIs.Controllers
{
   
    public class BuggyController : APIBaseController
    {
        private readonly StoreContext _dbContext;

        public BuggyController(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("NotFound")]
        //baseUrl/api/Buggy/NotFound
        public ActionResult GetNotFoundRequest()
        {
            var Product = _dbContext.Products.Find(100);
            if (Product is null) return NotFound(new ApiResponse(404));

            return Ok(Product);
        }

        [HttpGet("ServerError")]
        //baseUrl/Api/Buggy/ServerError
        public ActionResult GetServerError()
        {
            var Product = _dbContext.Products.Find(100); //Null
            var ProductToReturnDto = Product.ToString();//Error (null --> to String)
            //Will Throw Exception [Null Reference Exception]
            return Ok(ProductToReturnDto);
        }

        [HttpGet("BadRequest")]
        //baseUrl/Api/Buggy/BadRequest
        public ActionResult GetbadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        
        //Validation Erro
        [HttpGet("BadRequest/{id}")]
        //baseUrl/Api/Buggy/BadRequest/1
        public ActionResult GetbadRequest(int id)
        {
            return Ok();
        }
        [HttpGet("UnAuthorized")]
        public ActionResult GetUnAuthorized()
        {
            return Unauthorized(new ApiResponse(401));
        }
    }
}
