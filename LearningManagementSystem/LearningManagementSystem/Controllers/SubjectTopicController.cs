﻿using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.OtherSubjectInformationModel;
using LearningManagementSystem.Models.SubjectTopicModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectTopicController : ControllerBase
    {
        private readonly InterfaceSubjectTopicRepository _interfaceSubjectTopicRepository;
        public SubjectTopicController(InterfaceSubjectTopicRepository interfaceSubjectTopicRepository)
        {
            this._interfaceSubjectTopicRepository = interfaceSubjectTopicRepository;
        }
        //GET
        #region
        #endregion
        //POST
        #region
        [HttpPost("SubjectTopicCreate")]
        public async Task<IActionResult> SubjectTopicCreate([FromBody] SubjectTopicModelCreate model)
        {
            try
            {
                return Ok(await _interfaceSubjectTopicRepository.CreateNew(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error SubjectTopicCreate: {ex.Message}"
                });
            }
        }
        #endregion
        //PUT
        #region
        [HttpPut("UpdateSubjectTopic/{id}")]
        public async Task<IActionResult> UpdateSubjectTopic(string id, [FromBody] SubjectTopicModelUpdate model)
        {
            try
            {
                return Ok(await _interfaceSubjectTopicRepository.UpdateById(id, model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error UpdateSubjectTopic: {ex.Message}"
                });
            }
        }
        #endregion
    }
}