using System.ComponentModel.DataAnnotations;
using CORE.Domain;

namespace PATIENTS.APP.Domain;

public class Patient : Record
{
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    [Required, StringLength(50)]
    public string FirstName { get; set; }
    
    [Required, StringLength(50)]
    public string LastName { get; set; }
    
    public Genders Gender { get; set; }
    
    public DateTime? BirthDate{ get; set; }
    public int? UserID { get; set; }
    
    public List<DoctorPatient> DoctorPatients { get; set; }
}