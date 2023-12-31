﻿using BackEndComplaintApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace BackEndComplaintApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ComplaintController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment; 
        }

        // Get All Complaints
        [HttpGet("GetComplaints/{Id}")]
        public IActionResult GetComplaints(int Id)
        {
            if (Id == 1) // Assuming user with ID 1 is the admin
            {
                var userComplaints = _context.Complaints
                               .Include(c => c.Demands) // Include demands related to complaints
                               .ToList();
                return Ok(userComplaints);
            }

            return Unauthorized(new { message = "You don't have permission." });
        }




        // Get All Complaints For single user


        [HttpGet("GetUserComplaints/{id}")]
        public IActionResult GetUserComplaints(int id)
        {
            var userComplaints = _context.Complaints
                .Include(c => c.Demands) // Include demands related to complaints
                .Where(c => c.UserId == id)
                .ToList();

            return Ok(userComplaints);
        }





        // Get Single Complaint with Demands
        [HttpGet("GetSingleComplaint/{id}")]
        public IActionResult GetSingleComplaint(int id)
        {
            var complaint = _context.Complaints
                                   .Include(c => c.Demands) // Eager loading demands
                                   .FirstOrDefault(c => c.Id == id);

            if (complaint == null)
            {
                return NotFound();
            }

            return Ok(complaint);
        }

        [HttpPut("EditComplaint/{id}")]
        public async Task<IActionResult> AcceptView(int id, string isApproved)
        {
            var existingComplaint = await _context.Complaints.FindAsync(id);
            if (existingComplaint == null)
            {
                return NotFound();
            }

            // Check if the complaint is approved, and if it is, prevent editing
            if (existingComplaint.IsApproved=="accepted")
            {
                return BadRequest("Complaint is already approved and cannot be edited.");
            }

            // Update the IsApproved property of the existing complaint
            existingComplaint.IsApproved = isApproved;

            // Save changes to the database asynchronously
            _context.Update(existingComplaint);
            await _context.SaveChangesAsync();

            return Ok(); // Return a success response without the complaint object
        }


        [HttpPost("sendcomplaint")]
        public async Task<IActionResult> sendcomplaint( Complaint complaint)
                {
                    // Save image to a specific directory within the project
                    _context.Complaints.Add(complaint);
                    await _context.SaveChangesAsync();
                    // Return a success response if necessary
                    return Ok("Image uploaded successfully.");
                }
               

        




    }
}
