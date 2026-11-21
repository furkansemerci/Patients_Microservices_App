using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CORE.Domain;

namespace PATIENTS.APP.Domain;

public class Doctor : Record
{
    public int BranchId { get; set; }
    
    [Required, StringLength(50)]
    public string FirstName { get; set; }
    [Required, StringLength(50)]
    public string LastName { get; set; }

    public bool IsExpert{get; set; }
    
    public int? UserId { get; set; }
    
    public List<DoctorPatient> DoctorPatients { get; set; }

    [NotMapped]
    public List<int> PatientIds
    {
        get => DoctorPatients.Select(doctorPatientEntity => doctorPatientEntity.PatientId).ToList();
        set => DoctorPatients = value.Select(patientIdValue => new DoctorPatient
        {
            PatientId = patientIdValue
        }).ToList();
    }
}