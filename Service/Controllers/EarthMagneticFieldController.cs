using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OSDC.DotnetLibraries.General.DataManagement;
using NORCE.Drilling.EarthMagneticField.Model;
using NORCE.Drilling.EarthMagneticField.Service.Managers;

namespace NORCE.Drilling.EarthMagneticField.Service.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class EarthMagneticFieldController : ControllerBase
    {
        private readonly ILogger<EarthMagneticFieldManager> _logger;
        private readonly EarthMagneticFieldManager _earthMagneticFieldManager;

        public EarthMagneticFieldController(ILogger<EarthMagneticFieldManager> logger, SqlConnectionManager connectionManager)
        {
            _logger = logger;
            _earthMagneticFieldManager = EarthMagneticFieldManager.GetInstance(_logger, connectionManager);
        }

        /// <summary>
        /// Returns the list of Guid of all EarthMagneticField present in the microservice database at endpoint EarthMagneticField/api/EarthMagneticField
        /// </summary>
        /// <returns>the list of Guid of all EarthMagneticField present in the microservice database at endpoint EarthMagneticField/api/EarthMagneticField</returns>
        [HttpGet(Name = "GetAllEarthMagneticFieldId")]
        public ActionResult<IEnumerable<Guid?>> GetAllEarthMagneticFieldId()
        {
            var ids = _earthMagneticFieldManager.GetAllEarthMagneticFieldId();
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
        /// Returns the list of MetaInfo of all EarthMagneticField present in the microservice database, at endpoint EarthMagneticField/api/EarthMagneticField/MetaInfo
        /// </summary>
        /// <returns>the list of MetaInfo of all EarthMagneticField present in the microservice database, at endpoint EarthMagneticField/api/EarthMagneticField/MetaInfo</returns>
        [HttpGet("MetaInfo", Name = "GetAllEarthMagneticFieldMetaInfo")]
        public ActionResult<IEnumerable<MetaInfo?>> GetAllEarthMagneticFieldMetaInfo()
        {
            var vals = _earthMagneticFieldManager.GetAllEarthMagneticFieldMetaInfo();
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
        /// Returns the EarthMagneticField identified by its Guid from the microservice database, at endpoint EarthMagneticField/api/EarthMagneticField/id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>the EarthMagneticField identified by its Guid from the microservice database, at endpoint EarthMagneticField/api/EarthMagneticField/id</returns>
        [HttpGet("{id}", Name = "GetEarthMagneticFieldById")]
        public ActionResult<Model.EarthMagneticField?> GetEarthMagneticFieldById(Guid id)
        {
            if (!id.Equals(Guid.Empty))
            {
                var val = _earthMagneticFieldManager.GetEarthMagneticFieldById(id);
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
        /// Returns the list of all EarthMagneticField present in the microservice database, at endpoint EarthMagneticField/api/EarthMagneticField/HeavyData
        /// </summary>
        /// <returns>the list of all EarthMagneticField present in the microservice database, at endpoint EarthMagneticField/api/EarthMagneticField/HeavyData</returns>
        [HttpGet("HeavyData", Name = "GetAllEarthMagneticField")]
        public ActionResult<IEnumerable<Model.EarthMagneticField?>> GetAllEarthMagneticField()
        {
            var vals = _earthMagneticFieldManager.GetAllEarthMagneticField();
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
        /// Returns the list of all EarthMagneticField present in the microservice database, at endpoint EarthMagneticField/api/EarthMagneticField/HeavyData
        /// </summary>
        /// <returns>the list of all EarthMagneticField present in the microservice database, at endpoint EarthMagneticField/api/EarthMagneticField/HeavyData</returns>
        [HttpGet("Completed", Name = "GetAllCompletedEarthMagneticField")]
        public ActionResult<IEnumerable<Model.EarthMagneticField?>> GetAllCompletedEarthMagneticField(bool isCompleted)
        {
            var vals = _earthMagneticFieldManager.GetAllCompletedEarthMagneticField(isCompleted);
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
        /// Performs calculation on the given EarthMagneticField and adds it to the microservice database, at the endpoint EarthMagneticField/api/EarthMagneticField
        /// </summary>
        /// <param name="earthMagneticField"></param>
        /// <returns>true if the given EarthMagneticField has been added successfully to the microservice database, at the endpoint EarthMagneticField/api/EarthMagneticField</returns>
        [HttpPost(Name = "PostEarthMagneticField")]
        public ActionResult PostEarthMagneticField([FromBody] Model.EarthMagneticField? data)
        {
            if (data != null && data.MetaInfo != null && data.MetaInfo.ID != Guid.Empty)
            {
                var existingData = _earthMagneticFieldManager.GetEarthMagneticFieldById(data.MetaInfo.ID);
                if (existingData == null)
                {
                    if (_earthMagneticFieldManager.AddEarthMagneticField(data))
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
                    _logger.LogWarning("The given EarthMagneticField already exists and will not be added");
                    return StatusCode(StatusCodes.Status409Conflict);
                }
            }
            else
            {
                _logger.LogWarning("The given EarthMagneticField is null or its ID is empty");
                return BadRequest();
            }
        }

        /// <summary>
        /// Performs calculation on the given EarthMagneticField and updates it in the microservice database, at the endpoint EarthMagneticField/api/EarthMagneticField/id
        /// </summary>
        /// <param name="earthMagneticField"></param>
        /// <returns>true if the given EarthMagneticField has been updated successfully to the microservice database, at the endpoint EarthMagneticField/api/EarthMagneticField/id</returns>
        [HttpPut("{id}", Name = "PutEarthMagneticFieldById")]
        public ActionResult PutEarthMagneticFieldById(Guid id, [FromBody] Model.EarthMagneticField data)
        {
            if (data != null && data.MetaInfo != null && data.MetaInfo.ID.Equals(id))
            {
                var existingData = _earthMagneticFieldManager.GetEarthMagneticFieldById(id);
                if (existingData != null)
                {
                    if (_earthMagneticFieldManager.UpdateEarthMagneticFieldById(id, data))
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
                    _logger.LogWarning("The given EarthMagneticField has not been found in the database");
                    return NotFound();
                }
            }
            else
            {
                _logger.LogWarning("The given EarthMagneticField is null or its does not match the ID to update");
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes the EarthMagneticField of given ID from the microservice database, at the endpoint EarthMagneticField/api/EarthMagneticField/id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>true if the EarthMagneticField was deleted from the microservice database, at the endpoint EarthMagneticField/api/EarthMagneticField/id</returns>
        [HttpDelete("{id}", Name = "DeleteEarthMagneticFieldById")]
        public ActionResult DeleteEarthMagneticFieldById(Guid id)
        {
            if (_earthMagneticFieldManager.GetEarthMagneticFieldById(id) != null)
            {
                if (_earthMagneticFieldManager.DeleteEarthMagneticFieldById(id))
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
                _logger.LogWarning("The EarthMagneticField of given ID does not exist");
                return NotFound();
            }
        }
    }
}
