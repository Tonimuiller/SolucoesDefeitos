using Microsoft.AspNetCore.Mvc;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Controllers.Extension;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System;
using System.Collections.Generic;
using System.Threading;
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
        public async Task<IActionResult> PostAsync([FromBody] Product newProduct, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newProductResponse = await this.productService.AddAsync(newProduct, cancellationToken);
                return Created(string.Empty, newProductResponse);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(new ResponseDto(false, ex.Message));
            }
        }

        [HttpPut, Route("{productId}")]
        public async Task<IActionResult> PutAsync([FromRoute] int productId, [FromBody] Product updatedProduct, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await this.productService.GetByIdAsync(productId, cancellationToken);
            if (product == null)
            {
                NotFound();
            }

            try
            {
                await this.productService.UpdateAsync(updatedProduct, cancellationToken);
                return Ok(new ResponseDto(true));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(new ResponseDto(false, ex.Message));
            }
        }

        [HttpGet, Route("{productId}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int productId, CancellationToken cancellationToken)
        {
            try
            {
                var product = await this.productService.GetByIdAsync(productId, cancellationToken);             
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
        public async Task<IActionResult> GetListAsync(CancellationToken cancellationToken)
        {
            try
            {
                var products = await this.productService.GetAllAsync(cancellationToken);
                return Ok(new ResponseDto<IEnumerable<Product>>(true, products));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(new ResponseDto(false, ex.Message));
            }
        }

        [HttpDelete, Route("{productId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int productId, CancellationToken cancellationToken)
        {
            try
            {
                var product = await this.productService.GetByIdAsync(productId, cancellationToken);              
                if (product == null)
                {
                    return NotFound();
                }

                await this.productService.DeleteAsync(product, cancellationToken);
                return Ok(new ResponseDto(true));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(new ResponseDto(false, ex.Message));
            }
        }
    }
}
