using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Patient_Management_System.Data;
using Patient_Management_System.Models;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly PatientContext _context;
    private readonly ILogger<PatientController> _logger;


    // Context creation for db
    public PatientController(PatientContext context, ILogger<PatientController> logger)
    {
        _context = context;
        _logger = logger;
        _logger.LogInformation("PatientController initialized");
    }

    // Get All
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetAll()
    {
        _logger.LogInformation("Fetching all patients");
        try
        {
            var patients = await _context.Patients.ToListAsync();
            _logger.LogInformation("Fetched {PatientCount} patients", patients.Count);
            return Ok(patients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch patients");
            return StatusCode(500, "Internal server error");
        }
    }
    

    // ID Get
    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetById(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        return patient == null ? NotFound() : Ok(patient);
    }


    // ID Create
    [HttpPost]
    public async Task<ActionResult<Patient>> Create(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = patient.PatientId }, patient);
    }


    // ID Update
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Patient patient)
    {
        if (id != patient.PatientId) return BadRequest();
        _context.Entry(patient).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }


    // ID Delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return NotFound();
        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Email GET
    [HttpGet("by-email/{email}")]
    public async Task<ActionResult<Patient>> GetByEmail(string email)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);
        return patient == null ? NotFound() : Ok(patient);
    }


    // Email Update
    [HttpPut("by-email/{email}")]
    public async Task<IActionResult> UpdateByEmail(string email, Patient updatedPatient)
    {
        if (email != updatedPatient.Email) 
            return BadRequest("Email in URL and body must match");

        var existingPatient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);
        if (existingPatient == null) return NotFound();

        // Update other fields
        existingPatient.Name = updatedPatient.Name;
        existingPatient.Phone = updatedPatient.Phone;
        existingPatient.Gender = updatedPatient.Gender;
        existingPatient.BloodGroup = updatedPatient.BloodGroup;
        existingPatient.DateOfBirth = updatedPatient.DateOfBirth;
        existingPatient.Status = updatedPatient.Status;

        await _context.SaveChangesAsync();
        return NoContent();
    }


    // Email Delete
    [HttpDelete("by-email/{email}")]
    public async Task<IActionResult> DeleteByEmail(string email)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);
        if (patient == null) return NotFound();
        
        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    
    // General querying
    [HttpGet("/search")]
    public async Task<ActionResult<IEnumerable<Patient>>> GetWithParams([FromQuery] QueryParams param)
    {
        var query = _context.Patients.AsQueryable();

        if (param.Id != null && param.Id.Any())
            query = query.Where(p => param.Id.Contains(p.PatientId));
        
        if (param.Name != null && param.Name.Any())
            query = query.Where(p => param.Name.Contains(p.Name));
        
        if (param.Email != null && param.Email.Any())
            query = query.Where(p => param.Email.Contains(p.Email));
        
        if (param.Phone != null && param.Phone.Any())
            query = query.Where(p => param.Phone.Contains(p.Phone));
        
        if (param.Gender != null && param.Gender.Any())
            query = query.Where(p => param.Gender.Contains(p.Gender));
        
        if (param.Blood != null && param.Blood.Any())
            query = query.Where(p => param.Blood.Contains(p.BloodGroup));
        
        if (param.Dob != null && param.Dob.Any())
            query = query.Where(p => param.Dob.Contains(p.DateOfBirth));
        
        if (param.Status != null && param.Status.Any())
            query = query.Where(p => param.Status.Contains(p.Status));


        return await query.ToListAsync();
    }
    
    public class QueryParams
    {
        public List<int>? Id { get; set; }
        public List<string>? Name { get; set; }
        public List<string>? Email { get; set; }
        public List<string>? Phone { get; set; }
        public List<Gender>? Gender { get; set; }
        public List<BloodGroup>? Blood { get; set; }
        public List<DateTime>? Dob { get; set; }
        public List<Status>? Status { get; set; }
    }
}
