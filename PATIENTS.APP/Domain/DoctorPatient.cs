using CORE.Domain;

namespace PATIENTS.APP.Domain;

public class DoctorPatient : Record
{
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    public int PatientId { get; set; }
    public Patient Patient { get; set; }
}