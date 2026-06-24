using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OSDC.DotnetLibraries.General.DataManagement;
using NORCE.Drilling.EarthMagneticField.Service.Managers;

namespace NORCE.Drilling.EarthMagneticField.Service.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class EarthMagneticFieldCalculationOrderController : ControllerBase
    {
        private readonly ILogger<EarthMagneticFieldCalculationOrderManager> _logger;
        private readonly EarthMagneticFieldCalculationOrderManager _earthMagneticFieldCalculationOrderManager;

        public EarthMagneticFieldCalculationOrderController(ILogger<EarthMagneticFieldCalculationOrderManager> logger, SqlConnectionManager connectionManager)
        {
            _logger = logger;
            _earthMagneticFieldCalculationOrderManager = EarthMagneticFieldCalculationOrderManager.GetInstance(_logger, connectionManager);
        }

        /// <summary>
        /// Returns the list of Guid of all EarthMagneticFieldCalculationOrder present in the microservice database at endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder
        /// </summary>
        /// <returns>the list of Guid of all EarthMagneticFieldCalculationOrder present in the microservice database at endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder</returns>
        [HttpGet(Name = "GetAllEarthMagneticFieldCalculationOrderId")]
        public ActionResult<IEnumerable<Guid>> GetAllEarthMagneticFieldCalculationOrderId()
        {
            var ids = _earthMagneticFieldCalculationOrderManager.GetAllEarthMagneticFieldCalculationOrderId();
            if (ids != null)
            {
                return Ok(ids);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Returns the list of MetaInfo of all EarthMagneticFieldCalculationOrder present in the microservice database, at endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder/MetaInfo
        /// </summary>
        /// <returns>the list of MetaInfo of all EarthMagneticFieldCalculationOrder present in the microservice database, at endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder/MetaInfo</returns>
        [HttpGet("MetaInfo", Name = "GetAllEarthMagneticFieldCalculationOrderMetaInfo")]
        public ActionResult<IEnumerable<MetaInfo>> GetAllEarthMagneticFieldCalculationOrderMetaInfo()
        {
            var vals = _earthMagneticFieldCalculationOrderManager.GetAllEarthMagneticFieldCalculationOrderMetaInfo();
            if (vals != null)
            {
                return Ok(vals);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Returns the EarthMagneticFieldCalculationOrder identified by its Guid from the microservice database, at endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder/id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>the EarthMagneticFieldCalculationOrder identified by its Guid from the microservice database, at endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder/id</returns>
        [HttpGet("{id}", Name = "GetEarthMagneticFieldCalculationOrderById")]
        public ActionResult<Model.EarthMagneticFieldCalculationOrder?> GetEarthMagneticFieldCalculationOrderById(Guid id)
        {
            if (!id.Equals(Guid.Empty))
            {
                var val = _earthMagneticFieldCalculationOrderManager.GetEarthMagneticFieldCalculationOrderById(id);
                if (val != null)
                {
                    return Ok(val);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns the list of all EarthMagneticFieldCalculationOrderLight present in the microservice database, at endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder/LightData
        /// </summary>
        /// <returns>the list of all EarthMagneticFieldCalculationOrderLight present in the microservice database, at endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder/LightData</returns>
        [HttpGet("LightData", Name = "GetAllEarthMagneticFieldCalculationOrderLight")]
        public ActionResult<IEnumerable<Model.EarthMagneticFieldCalculationOrderLight>> GetAllEarthMagneticFieldCalculationOrderLight()
        {
            var vals = _earthMagneticFieldCalculationOrderManager.GetAllEarthMagneticFieldCalculationOrderLight();
            if (vals != null)
            {
                return Ok(vals);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Returns the list of all EarthMagneticFieldCalculationOrder present in the microservice database, at endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder/HeavyData
        /// </summary>
        /// <returns>the list of all EarthMagneticFieldCalculationOrder present in the microservice database, at endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder/HeavyData</returns>
        [HttpGet("HeavyData", Name = "GetAllEarthMagneticFieldCalculationOrder")]
        public ActionResult<IEnumerable<Model.EarthMagneticFieldCalculationOrder?>> GetAllEarthMagneticFieldCalculationOrder()
        {
            var vals = _earthMagneticFieldCalculationOrderManager.GetAllEarthMagneticFieldCalculationOrder();
            if (vals != null)
            {
                return Ok(vals);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Performs calculation on the given EarthMagneticFieldCalculationOrder and adds it to the microservice database, at the endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder
        /// </summary>
        /// <param name="earthMagneticFieldCalculationOrder"></param>
        /// <returns>true if the given EarthMagneticFieldCalculationOrder has been added successfully to the microservice database, at the endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder</returns>
        [HttpPost(Name = "PostEarthMagneticFieldCalculationOrder")]
        public ActionResult PostEarthMagneticFieldCalculationOrder([FromBody] Model.EarthMagneticFieldCalculationOrder? data)
        {
            // Check if earthMagneticFieldCalculationOrder exists in the database through ID
            if (data != null && data.MetaInfo != null && data.MetaInfo.ID != Guid.Empty)
            {
                var existingData = _earthMagneticFieldCalculationOrderManager.GetEarthMagneticFieldCalculationOrderById(data.MetaInfo.ID);
                if (existingData == null)
                {   
                    //  If earthMagneticFieldCalculationOrder was not found, call AddEarthMagneticFieldCalculationOrder, where the earthMagneticFieldCalculationOrder.Calculate()
                    // method is called. 
                    if (_earthMagneticFieldCalculationOrderManager.AddEarthMagneticFieldCalculationOrder(data))
                    {
                        return Ok(); // status=OK is used rather than status=Created because NSwag auto-generated controllers use 200 (OK) rather than 201 (Created) as return codes
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
                else
                {
                    _logger.LogWarning("The given EarthMagneticFieldCalculationOrder already exists and will not be added");
                    return StatusCode(StatusCodes.Status409Conflict);
                }
            }
            else
            {
                _logger.LogWarning("The given EarthMagneticFieldCalculationOrder is null, badly formed, or its ID is empty");
                return BadRequest();
            }
        }

        /// <summary>
        /// Performs calculation on the given EarthMagneticFieldCalculationOrder and updates it in the microservice database, at the endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder/id
        /// </summary>
        /// <param name="earthMagneticFieldCalculationOrder"></param>
        /// <returns>true if the given EarthMagneticFieldCalculationOrder has been updated successfully to the microservice database, at the endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder/id</returns>
        [HttpPut("{id}", Name = "PutEarthMagneticFieldCalculationOrderById")]
        public ActionResult PutEarthMagneticFieldCalculationOrderById(Guid id, [FromBody] Model.EarthMagneticFieldCalculationOrder? data)
        {
            // Check if EarthMagneticFieldCalculationOrder is in the data base
            if (data != null && data.MetaInfo != null && data.MetaInfo.ID.Equals(id))
            {
                var existingData = _earthMagneticFieldCalculationOrderManager.GetEarthMagneticFieldCalculationOrderById(id);
                if (existingData != null)
                {
                    if (_earthMagneticFieldCalculationOrderManager.UpdateEarthMagneticFieldCalculationOrderById(id, data))
                    {
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
                else
                {
                    _logger.LogWarning("The given EarthMagneticFieldCalculationOrder has not been found in the database");
                    return NotFound();
                }
            }
            else
            {
                _logger.LogWarning("The given EarthMagneticFieldCalculationOrder is null, badly formed, or its does not match the ID to update");
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes the EarthMagneticFieldCalculationOrder of given ID from the microservice database, at the endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder/id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>true if the EarthMagneticFieldCalculationOrder was deleted from the microservice database, at the endpoint EarthMagneticField/api/EarthMagneticFieldCalculationOrder/id</returns>
        [HttpDelete("{id}", Name = "DeleteEarthMagneticFieldCalculationOrderById")]
        public ActionResult DeleteEarthMagneticFieldCalculationOrderById(Guid id)
        {
            if (_earthMagneticFieldCalculationOrderManager.GetEarthMagneticFieldCalculationOrderById(id) != null)
            {
                if (_earthMagneticFieldCalculationOrderManager.DeleteEarthMagneticFieldCalculationOrderById(id))
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                _logger.LogWarning("The EarthMagneticFieldCalculationOrder of given ID does not exist");
                return NotFound();
            }
        }
    }
}
