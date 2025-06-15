namespace Patient_Management_System.Models;

public class Patient
{
    public int PatientId { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public Gender Gender { get; set; }
    public BloodGroup BloodGroup { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Status Status { get; set; }
}

public enum BloodGroup
{
    APos,
    ANeg,
    BPos,
    BNeg,
    AbPos,
    AbNeg,
    OPos,
    ONeg
}

public enum Gender
{
    M,
    F,
    Other
}

public enum Status
{
    Admitted,
    Discharged,
    Outpatient
}