using Microsoft.AspNetCore.Mvc;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Controllers.Extension;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolucoesDefeitos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnomalyController : ControllerBase
    {
        private readonly IAnomalyService anomalyService;

        public AnomalyController(
            IAnomalyService anomalyService)
        {
            this.anomalyService = anomalyService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Anomaly newAnomaly)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newAnomalyResponse = await this.anomalyService.AddAsync(newAnomaly);
                return Created(string.Empty, newAnomalyResponse);
            }
            catch(Exception ex)
            {
                return this.InternalServerError(new ResponseDto(false, ex.Message));
            }
        }

        [HttpPut, Route("{anomalyId}")]
        public async Task<IActionResult> PutAsync([FromRoute] int anomalyId, [FromBody] Anomaly updatedAnomaly)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var anomaly = await this.anomalyService.GetByKeyAsync(new { anomalyId });
                if (anomaly == null)
                {
                    return NotFound();
                }

                await this.anomalyService.UpdateAsync(updatedAnomaly);
                return Ok(new ResponseDto(true));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(new ResponseDto(false, ex.Message));
            }
        }

        [HttpGet, Route("{anomalyId}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int anomalyId)
        {
            try
            {
                var anomaly = await this.anomalyService.GetByKeyAsync(new { anomalyId });
                if (anomaly == null)
                {
                    return NotFound();
                }

                return Ok(new ResponseDto<Anomaly>(true, anomaly));
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
                var anomalies = await this.anomalyService.GetAllAsync();
                return Ok(new ResponseDto<IEnumerable<Anomaly>>(true, anomalies));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(new ResponseDto(false, ex.Message));
            }
        }

        [HttpDelete, Route("{anomalyId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int anomalyId)
        {
            try
            {
                var anomaly = await this.anomalyService.GetByKeyAsync(new { anomalyId });
                if (anomaly == null)
                {
                    return NotFound();
                }

                await this.anomalyService.DeleteAsync(anomaly);
                return Ok();
            }
            catch (Exception ex)
            {
                return this.InternalServerError(new ResponseDto(false, ex.Message));
            }
        }
    }
}
