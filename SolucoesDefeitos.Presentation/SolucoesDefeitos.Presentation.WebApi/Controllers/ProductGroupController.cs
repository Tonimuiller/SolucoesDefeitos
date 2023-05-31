﻿using Microsoft.AspNetCore.Mvc;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductGroupController : ControllerBase
    {
        private readonly IProductGroupService productGroupService;

        public ProductGroupController(IProductGroupService productGroupService)
        {
            this.productGroupService = productGroupService;
        }
        
        /// <summary>
        /// Get product group by id
        /// </summary>
        /// <param name="id">Product id to fetch</param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken)
        {
            var productGroup = await this.productGroupService.GetByIdAsync(id, cancellationToken);
            if (productGroup == null)
            {
                return NotFound();
            }

            return Ok(new ResponseDto<ProductGroup>(true, productGroup));
        }

        /// <summary>
        /// Get all available product groups
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetListAsync(CancellationToken cancellationToken)
        {
            var productGroups = await this.productGroupService.GetAllAsync(cancellationToken);
            return Ok(new ResponseDto<IEnumerable<ProductGroup>>(true, productGroups));
        }

        /// <summary>
        /// Create a new product group
        /// </summary>
        /// <param name="productGroup">New product group to be created</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ProductGroup productGroup, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newProductGroupResponse = await this.productGroupService.AddAsync(productGroup, cancellationToken);
            return Ok(newProductGroupResponse);
        }

        /// <summary>
        /// Apply changes to an existing product group
        /// </summary>
        /// <param name="id">Changing Product group identifier </param>
        /// <param name="updatedProductGroup">Changes to be applied into the product group</param>
        /// <returns></returns>
        [HttpPut, Route("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] ProductGroup updatedProductGroup, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await this.productGroupService.UpdateAsync(updatedProductGroup, cancellationToken);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Delete an existing product group
        /// </summary>
        /// <param name="id">Identifier of the product group to delete</param>
        /// <returns></returns>
        [HttpDelete, Route("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var productGroup = await this.productGroupService.GetByIdAsync(id, cancellationToken);
            if (productGroup == null)
            {
                return NotFound();
            }
            
            try
            {
                await this.productGroupService.DeleteAsync(productGroup, cancellationToken);
                return Ok(new ResponseDto(true));
            }
            catch (Exception ex)
            {
                var response = new ResponseDto(false, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }
    }
}
