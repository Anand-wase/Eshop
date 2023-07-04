using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopping.WebApi.Data;
using OnlineShopping.WebApi.Models;
using OnlineShopping.WebApi.Repository.IRepository;
using System.Data;
using System.Net;

namespace OnlineShopping.WebApi.Controllers
{
    [Route("api/productAPI")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly ICategoryRepository _dbCategory;
        private readonly IProductRepository _dbProduct;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public ProductApiController(IProductRepository dbProduct,IMapper mapper,ICategoryRepository dbCategory)
        {
            _dbCategory = dbCategory;
            _dbProduct = dbProduct;
            _mapper = mapper;
            this._response = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProducts()
        {
            try
            {

                IEnumerable<Product> productList = await _dbProduct.GetAllAsync(includeProperties:"Category");
                _response.Result = _mapper.Map<List<ProductDto>>(productList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }
        [HttpGet("{id:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProduct(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var product = await _dbProduct.GetAsync(u => u.ProductId == id, includeProperties: "Category");

                if (product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ProductDto>(product);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;


        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> CreateProduct([FromBody] CreateProductDto createDto)
        {
            try
            {
                if (await _dbProduct.GetAsync(u => u.ProductName.ToLower() == createDto.ProductName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessage", "Product already Exists!");
                    return BadRequest(ModelState);
                }
                if (await _dbCategory.GetAsync(u => u.CategoryId == createDto.CategoryId) == null)
                {
                    ModelState.AddModelError("ErrorMessage", "Category ID is Invalid!");
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }
                if (createDto.ProductId > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                Product model = _mapper.Map<Product>(createDto);
                await _dbProduct.CreateAsync(model);
                _response.Result = _mapper.Map<ProductDto>(model);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetProduct", new { id = createDto.ProductId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> DeleteProduct(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var product = await _dbProduct.GetAsync(u => u.ProductId == id);
                if (product == null)
                {
                    return NotFound();
                }
                await _dbProduct.RemoveAsync(product);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPut("{id:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> UpdateProduct(int id, [FromBody] UpdateProductDto updateDto)
        {
            try
            {
                if (await _dbProduct.GetAsync(u => u.ProductName.ToLower() == updateDto.ProductName.ToLower() & u.ProductId!=updateDto.ProductId) != null)
                {
                    ModelState.AddModelError("ErrorMessage", "Product already Exists!");
                    return BadRequest(ModelState);
                }
                if (await _dbCategory.GetAsync(u => u.CategoryId == updateDto.CategoryId) == null)
                {
                    ModelState.AddModelError("ErrorMessage", "Category ID is Invalid!");
                    return BadRequest(ModelState);
                }
                if (updateDto == null || id != updateDto.ProductId)
                {
                    return BadRequest();
                }
                Product model = _mapper.Map<Product>(updateDto);
                await _dbProduct.UpdateAsync(model);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;

        }

    }
}