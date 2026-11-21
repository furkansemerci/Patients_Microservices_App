using System.ComponentModel.DataAnnotations;
using CORE.Domain;

namespace PATIENTS.APP.Domain;

public class Branch : Record
{
    [Required, StringLength(50)]
    public string Title { get; set; }

    public List<Doctor> Doctors { get; set; } = new();
}