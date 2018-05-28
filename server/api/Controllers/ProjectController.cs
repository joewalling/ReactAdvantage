using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReactAdvantage.API.Services;
using ReactAdvantage.Domain.ViewModels.Projects;

namespace ReactAdvantage.API.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        }

        [HttpGet]
        [Produces(typeof(ProjectListDto))]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                return new ObjectResult(await _projectService.GetAllAsync());
            }
            catch (Exception)
            {
                //Talk about this - ran out of time
                //log ex

                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ProjectDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var projectDto = await _projectService.GetByIdAsync(id);

                if (projectDto.NoData)
                {
                    return NotFound();
                }

                return Ok(projectDto);
            }
            catch (Exception)
            {
                //log ex

                return StatusCode(500);
            }
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProjectEditDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAsync([FromBody] ProjectEditDto project)
        {
            try
            {
                if (project == null)
                {
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                project.Id = await _projectService.CreateAsync(project);

                return CreatedAtAction(nameof(GetByIdAsync), new { id = project.Id }, project);

            }
            catch (Exception)
            {
                //log ex
                return StatusCode(500);
            }
    }

        //Result should be a 204 with no content
        //Client should send entire object content - use patch for deltas
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(int id, [FromBody]ProjectEditDto input)
        {
            try
            {
                if (input == null)
                {
                    return BadRequest();
                }

                var editDto = await _projectService.UpdateAsync(input);

                if (editDto.NoData)
                {
                    return NotFound(input);
                }

                return NoContent();

            }
            catch (Exception)
            {
                //log ex

                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var results = await _projectService.DeleteAsync(id);
                if (!results)
                {
                    return NotFound();
                }

                return NoContent();

            }
            catch (Exception)
            {
                //log ex

                return StatusCode(500);
            }

 
        }
    }
}