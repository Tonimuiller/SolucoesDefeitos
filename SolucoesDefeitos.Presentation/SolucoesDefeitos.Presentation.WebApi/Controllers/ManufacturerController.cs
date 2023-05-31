using Microsoft.AspNetCore.Mvc;
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
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturerService manufacturerService;

        public ManufacturerController(
            IManufacturerService manufacturerService)
        {
            this.manufacturerService = manufacturerService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Manufacturer newManufacturer, CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manufacturerAddResponse = await this.manufacturerService.AddAsync(newManufacturer, cancellationToken);
            if (manufacturerAddResponse.Success)
            {
                return Ok(new ResponseDto(true));
            }

            return StatusCode((int)HttpStatusCode.InternalServerError, manufacturerAddResponse);
        }

        [HttpPut, Route("{manufacturerId}")]
        public async Task<IActionResult> PutAsync([FromRoute] int manufacturerId, [FromBody] Manufacturer updatedManufacturer, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manufacturer = await this.manufacturerService.GetByIdAsync(manufacturerId, cancellationToken);
            if (manufacturer == null)
            {
                return NotFound();
            }

            manufacturer.Enabled = updatedManufacturer.Enabled;
            manufacturer.Name = updatedManufacturer.Name;

            try
            {
                await this.manufacturerService.UpdateAsync(manufacturer, cancellationToken);
                return Ok(new ResponseDto(true));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseDto(false, ex.Message));
            }
        }

        [HttpGet, Route("{manufacturerId}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int manufacturerId, CancellationToken cancellationToken)
        {
            var manufacturer = await this.manufacturerService.GetByIdAsync(manufacturerId, cancellationToken);
            if (manufacturer == null)
            {
                return NotFound();
            }

            return Ok(new ResponseDto<Manufacturer>(true, manufacturer));
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync(CancellationToken cancellationToken)
        {
            return Ok(new ResponseDto<IEnumerable<Manufacturer>>(true, await this.manufacturerService.GetAllAsync(cancellationToken)));
        }

        [HttpDelete, Route("{manufacturerId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int manufacturerId, CancellationToken cancellationToken)
        {
            var manufacturer = await this.manufacturerService.GetByIdAsync(manufacturerId, cancellationToken);
            if (manufacturer == null)
            {
                return NotFound();
            }

            try
            {
                await this.manufacturerService.DeleteAsync(manufacturer, cancellationToken);
                return Ok(new ResponseDto(true));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseDto(false, ex.Message));
            }
        }
    }
}
