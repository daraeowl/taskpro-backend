﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using taskpro_api;
using taskpro_api.Models;
using Task = taskpro_api.Models.Task;

namespace backend.Controllers
{   

    [ApiController]
    [Route("api/task")]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            var tasks = _context.Tasks.ToList();

            return Ok(tasks);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ha ocurrido un error al intentar obtener el Task con el ID {id}: {ex.Message}");
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTask(Task task)
        {
            try
            {
                // Check if the task object is null
                if (task == null)
                {
                    return BadRequest("Task object is null");
                }

                // Check if the Title is provided and not empty
                if (string.IsNullOrEmpty(task.Title))
                {
                    return BadRequest("Task Title is missing or empty");
                }

                // Check if the Description is provided and not empty
                if (string.IsNullOrEmpty(task.Description))
                {
                    return BadRequest("Task Description is missing or empty");
                }

                // Check if the Status is provided and not empty
                if (string.IsNullOrEmpty(task.Status))
                {
                    return BadRequest("Task Status is missing or empty");
                }

                // Since the ID is auto-generated by the database, exclude it from the ModelState
                ModelState.Remove("Id");

                // Check if there are any validation errors
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Add the task to the database
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                // Return the created task
                return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
            }
            catch (DbUpdateException ex)
            {
                // Log the inner exception for details
                Console.WriteLine("Inner Exception: " + ex.InnerException.Message);

                return StatusCode(500, $"An error occurred while trying to create the task: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while trying to create the task: {ex.Message}");
            }
        }




        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTask(int id, Task updatedTask)
        {
            try
            {
                if (id != updatedTask.Id)
                {
                    return BadRequest("El ID en el URL no es igual al ID del request body");
                }

                var existingTask = await _context.Tasks.FindAsync(id);
                if (existingTask == null)
                {
                    return NotFound($"Task con el ID {id} no se ha encontrado.");
                }

                _context.Entry(existingTask).CurrentValues.SetValues(updatedTask);
                await _context.SaveChangesAsync();

                return Ok($"Task con el ID {id} actualizado.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound($"Task con el ID {id} No encontrado.");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ha ocurrido un error al intentar actualizar el task con ID {id}: {ex.Message}");
            }
        }


        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound($"Task con el ID {id} no encontrado.");
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return Ok($"Task con el ID {id} eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ha ocurrido un error al intentar el task con ID {id}: {ex.Message}");
            }
        }




    }
}
