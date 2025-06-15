using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Patient_Management_System.Data;
using Patient_Management_System.Models;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly PatientContext _context;

    public PatientController(PatientContext context)
    {
        _context = context;
    }

    // Existing ID-based endpoints
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetAll()
        => await _context.Patients.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetById(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        return patient == null ? NotFound() : Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> Create(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = patient.PatientId }, patient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Patient patient)
    {
        if (id != patient.PatientId) return BadRequest();
        _context.Entry(patient).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return NotFound();
        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // New email-based endpoints
    [HttpGet("by-email/{email}")]
    public async Task<ActionResult<Patient>> GetByEmail(string email)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);
        return patient == null ? NotFound() : Ok(patient);
    }

    [HttpPut("by-email/{email}")]
    public async Task<IActionResult> UpdateByEmail(string email, Patient updatedPatient)
    {
        if (email != updatedPatient.Email) 
            return BadRequest("Email in URL and body must match");

        var existingPatient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);
        if (existingPatient == null) return NotFound();

        // Update all fields except Email and PatientId
        existingPatient.Name = updatedPatient.Name;
        existingPatient.Phone = updatedPatient.Phone;
        existingPatient.Gender = updatedPatient.Gender;
        existingPatient.BloodGroup = updatedPatient.BloodGroup;
        existingPatient.DateOfBirth = updatedPatient.DateOfBirth;
        existingPatient.Status = updatedPatient.Status;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("by-email/{email}")]
    public async Task<IActionResult> DeleteByEmail(string email)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);
        if (patient == null) return NotFound();
        
        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
