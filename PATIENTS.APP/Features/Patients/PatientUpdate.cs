using System.ComponentModel.DataAnnotations;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PATIENTS.APP.Domain;

namespace PATIENTS.APP.Features.Patients
{

    public class PatientUpdateRequest: Record, IRequest<CommandResponse>
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

    public class PatientUpdateHandler : DbService<Patient>, IRequestHandler<PatientUpdateRequest, CommandResponse>
    {
        public PatientUpdateHandler(DbContext db) : base(db)
        {
            
        }

        public async Task<CommandResponse> Handle(PatientUpdateRequest request, CancellationToken cancellationToken)
        {
            var patientEntity = await DbSingleAsync(request.Id, cancellationToken);

            if (patientEntity is null)
            {
                return Error(0,"Patient not found");
                
            }
            
            patientEntity.Height = request.Height;
            patientEntity.Weight = request.Weight;
            patientEntity.FirstName = request.FirstName.Trim();
            patientEntity.LastName = request.LastName.Trim();
            patientEntity.Gender = request.Gender;
            patientEntity.BirthDate = request.BirthDate.Value;
            patientEntity.UserId = request.UserId;
            
            await DbUpdateAsync(patientEntity, cancellationToken);
            
            return Success(patientEntity.Id, "Patient updated successfully");

        }
    }

}