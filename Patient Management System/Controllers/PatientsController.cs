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
    }

    // Get All
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetAll()
    {
        _logger.LogInformation("Fetching all patients");
        try
        {
            var patients = await _context.Patients.ToListAsync();
            _logger.LogInformation("Successfully fetched {PatientCount} patients", patients.Count);
            return Ok(patients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all patients");
            throw; // middleware will handle errors
        }
    }

    

    // ID Get
    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetById(int id)
    {
        _logger.LogInformation("Fetching patient with ID {PatientId}", id);
        try
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found", id);
                throw new PatientNotFoundException(id);
            }
            _logger.LogInformation("Fetched patient with ID {PatientId}", id);
            return Ok(patient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching patient with ID {PatientId}", id);
            throw;
        }
    }



    // ID Create
    [HttpPost]
    public async Task<ActionResult<Patient>> Create(Patient patient)
    {
        _logger.LogInformation("Creating new patient with email {Email}", patient.Email);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid patient data: {@Errors}", ModelState.Values);
            return ValidationProblem(ModelState);
        }

        try
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created patient with ID {PatientId}", patient.PatientId);
            return CreatedAtAction(nameof(GetById), new { id = patient.PatientId }, patient);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error creating patient with email {Email}", patient.Email);
            throw new DbUpdateException("Failed to create patient due to database error", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating patient");
            throw;
        }
    }



    // ID Update
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Patient patient)
    {
        _logger.LogInformation("Updating patient with ID {PatientId}", id);
        if (id != patient.PatientId)
        {
            _logger.LogWarning("ID mismatch: {RouteId} vs {BodyId}", id, patient.PatientId);
            return BadRequest("ID in route does not match ID in body");
        }

        try
        {
            var existingPatient = await _context.Patients.FindAsync(id);
            if (existingPatient == null)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found for update", id);
                throw new PatientNotFoundException(id);
            }

            _context.Entry(existingPatient).CurrentValues.SetValues(patient);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated patient with ID {PatientId}", id);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error updating patient with ID {PatientId}", id);
            throw new DbUpdateException("Patient was modified by another user", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient with ID {PatientId}", id);
            throw;
        }
    }



    // ID Delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting patient with ID {PatientId}", id);
        try
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found for deletion", id);
                throw new PatientNotFoundException(id);
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted patient with ID {PatientId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient with ID {PatientId}", id);
            throw;
        }
    }

    // Email Get
    [HttpGet("by-email/{email}")]
    public async Task<ActionResult<Patient>> GetByEmail(string email)
    {
        _logger.LogInformation("Fetching patient with email {Email}", email);
        try
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);
            if (patient == null)
            {
                _logger.LogWarning("Patient with email {Email} not found", email);
                throw new PatientNotFoundException(email);
            }
            _logger.LogInformation("Fetched patient with email {Email}", email);
            return Ok(patient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching patient with email {Email}", email);
            throw;
        }
    }
    
    
    // Email Update
    [HttpPut("by-email/{email}")]
    public async Task<IActionResult> UpdateByEmail(string email, Patient updatedPatient)
    {
        _logger.LogInformation("Updating patient with email {Email}", email);
        if (email != updatedPatient.Email)
        {
            _logger.LogWarning("Email mismatch: {RouteEmail} vs {BodyEmail}", email, updatedPatient.Email);
            return BadRequest("Email in URL and body must match");
        }

        try
        {
            var existingPatient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);
            if (existingPatient == null)
            {
                _logger.LogWarning("Patient with email {Email} not found for update", email);
                throw new PatientNotFoundException(email);
            }

            _context.Entry(existingPatient).CurrentValues.SetValues(updatedPatient);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated patient with email {Email}", email);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error updating patient with email {Email}", email);
            throw new DbUpdateException("Patient was modified by another user", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient with email {Email}", email);
            throw;
        }
    }



    // Email Delete
    [HttpDelete("by-email/{email}")]
    public async Task<IActionResult> DeleteByEmail(string email)
    {
        _logger.LogInformation("Deleting patient with email {Email}", email);
        try
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);
            if (patient == null)
            {
                _logger.LogWarning("Patient with email {Email} not found for deletion", email);
                throw new PatientNotFoundException(email);
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted patient with email {Email}", email);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient with email {Email}", email);
            throw;
        }
    }

    // btw i like cats :3
    
    // General querying
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Patient>>> GetWithParams([FromQuery] QueryParams param)
    {
        _logger.LogInformation("Searching patients with params {@Params}", param);
        try
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

            var results = await query.ToListAsync();
            _logger.LogInformation("Found {Count} patients matching search", results.Count);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching patients with params {@Params}", param);
            throw;
        }
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
