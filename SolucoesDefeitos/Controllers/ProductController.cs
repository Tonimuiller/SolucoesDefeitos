using Microsoft.AspNetCore.Mvc;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Controllers.Extension;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolucoesDefeitos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(
            IProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Product newProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newProductResponse = await this.productService.AddAsync(newProduct);
                return Ok(newProductResponse);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(new ResponseDto(false, ex.Message));
            }
        }

        [HttpPut, Route("{productId}")]
        public async Task<IActionResult> PutAsync([FromRoute] int productId, [FromBody] Product updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await this.productService.GetByKeyAsync(new { productId });
            if (product == null)
            {
                NotFound();
            }

            try
            {
                await this.productService.UpdateAsync(updatedProduct);
                return Ok(new ResponseDto(true));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(new ResponseDto(false, ex.Message));
            }
        }

        [HttpGet, Route("{productId}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int productId)
        {
            try
            {
                var product = await this.productService.GetByKeyAsync(new { productId });
                if (product == null)
                {
                    return NotFound();
                }

                return Ok(new ResponseDto<Product>(true, product));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(new ResponseDto(false, ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            try
            {
                var products = await this.productService.GetAllAsync();
                return Ok(new ResponseDto<IEnumerable<Product>>(true, products));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(new ResponseDto(false, ex.Message));
            }
        }

        [HttpDelete, Route("{productId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int productId)
        {
            try
            {
                var product = await this.productService.GetByKeyAsync(new { productId });
                if (product == null)
                {
                    return NotFound();
                }

                await this.productService.DeleteAsync(product);
                return Ok(new ResponseDto(true));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(new ResponseDto(false, ex.Message));
            }
        }
    }
}
