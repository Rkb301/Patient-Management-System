using Microsoft.EntityFrameworkCore;
using Patient_Management_System.Models;

namespace Patient_Management_System.Data;
public class PatientContext : DbContext
{
    public PatientContext(DbContextOptions<PatientContext> options) : base(options) { }
    public DbSet<Patient> Patients { get; set; }
}