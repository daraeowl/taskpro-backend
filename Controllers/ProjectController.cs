using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using taskpro_api;
using taskpro_api.Models;

namespace taskpro_api.Controllers
{
    [ApiController]
    [Route("api/project")]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllProjects()
        {
            var projects = _context.Projects.ToList();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProjectById(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null)
                {
                    return NotFound();
                }
                return Ok(project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while fetching project with ID {id}: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProject(Project project)
        {
            try
            {
                var existingProject = await _context.Projects.FindAsync(project.Id);
                if (existingProject != null)
                {
                    return Conflict($"Project with ID {project.Id} already exists.");
                }

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while creating the project: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProject(int id, Project updatedProject)
        {
            try
            {
                if (id != updatedProject.Id)
                {
                    return BadRequest("ID in URL does not match ID in the request body");
                }

                var existingProject = await _context.Projects.FindAsync(id);
                if (existingProject == null)
                {
                    return NotFound($"Project with ID {id} not found.");
                }

                _context.Entry(existingProject).CurrentValues.SetValues(updatedProject);
                await _context.SaveChangesAsync();

                return Ok($"Project with ID {id} updated.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound($"Project with ID {id} not found.");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while updating project with ID {id}: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null)
                {
                    return NotFound($"Project with ID {id} not found.");
                }

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                return Ok($"Project with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while deleting project with ID {id}: {ex.Message}");
            }
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
