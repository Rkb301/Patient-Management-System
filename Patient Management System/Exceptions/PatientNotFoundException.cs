public class PatientNotFoundException : Exception
{
    public PatientNotFoundException(int id) 
        : base($"Patient with ID {id} not found") { }

    public PatientNotFoundException(string email)
        : base($"Patient with email {email} not found") { }
}