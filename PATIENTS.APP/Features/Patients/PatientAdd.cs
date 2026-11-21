using System.ComponentModel.DataAnnotations;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PATIENTS.APP.Domain;

namespace PATIENTS.APP.Features.Patients
{

    public class PatientAddRequest: Record, IRequest<CommandResponse>
    {
        public decimal? Height { get; set; }
        
        public decimal? Weight { get; set; }
        
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "{0} must be maximum of {1} characters")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "{0} must be maximum of {1} characters")]
        public string LastName { get; set; }
        
        public Genders Gender { get; set; }
        
        [Required(ErrorMessage = "{0} is required")]
        public DateTime? BirthDate { get; set; }
        
        public int? UserId { get; set; }
        
        
    }

    public class PatientAddHandler : DbService<Patient>, IRequestHandler<PatientAddRequest, CommandResponse>
    {
        public PatientAddHandler(DbContext db) : base(db)
        {
            
        }

        public async Task<CommandResponse> Handle(PatientAddRequest request, CancellationToken cancellationToken)
        {
            var patientEntity = new Patient
            {
                Height = request.Height,
                Weight = request.Weight,
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Gender = request.Gender,
                BirthDate = request.BirthDate.Value,
                UserID = request.UserId
            };

            await DbAddAsync(patientEntity, cancellationToken);
            
            return Success(patientEntity.Id, "Patient created successfully");
        }
    }

}