using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.OtherSubjectInformationModel;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtherSubjectInformationController : ControllerBase
    {
        private readonly InterfaceOtherSubjectInformationRepository _interfaceOtherSubjectInformationRepository;
        public OtherSubjectInformationController(InterfaceOtherSubjectInformationRepository interfaceOtherSubjectInformationRepository)
        {
            this._interfaceOtherSubjectInformationRepository = interfaceOtherSubjectInformationRepository;
        }
        //POST
        #region
        [HttpPost("OtherSubjectInformationCreate")]
        public async Task<IActionResult> OtherSubjectInformationCreate([FromBody] OtherSubjectInformationModelCreate model)
        {
            try
            {
                return Ok(await _interfaceOtherSubjectInformationRepository.CreateNew(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error OtherSubjectInformationModelCreate: {ex.Message}"
                });
            }
        }
        #endregion
        //PUT
        #region
        [HttpPut("UpdateOtherSubjectInformation/{id}")]
        public async Task<IActionResult> UpdateOtherSubjectInformation(string id, [FromBody] OtherSubjectInformationModelUpdate model)
        {
            try
            {
                return Ok(await _interfaceOtherSubjectInformationRepository.UpdateById(id, model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error UpdateOtherSubjectInformation: {ex.Message}"
                });
            }
        }
        #endregion
    }
}
