using System.ComponentModel.DataAnnotations;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PATIENTS.APP.Domain;

namespace PATIENTS.APP.Features.Doctors
{
    public class DoctorAddRequest : Record, IRequest<CommandResponse>
    {
        [Required(ErrorMessage ="{0} is required!")]
        public int? BranchId { get; set; }
        
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50,ErrorMessage = "{0} must be maximum {1} characters!")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50,ErrorMessage = "{0} must be maximum {1} characters!")]
        public string LastName { get; set; }
        
        public bool IsExpert { get; set; }
        public int? UserId { get; set; }
        
        [Required]
        public List<int> PatientIds{get; set;}
    }

    public class DoctorAddHandler : DbService<Doctor>, IRequestHandler<DoctorAddRequest, CommandResponse>
    {
        public DoctorAddHandler(DbContext db) : base(db){}

        public async Task<CommandResponse> Handle(DoctorAddRequest request, CancellationToken cancellationToken)
        {
            var doctorEntity = new Doctor
            {
                BranchId = request.BranchId.Value,
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                IsExpert = request.IsExpert,
                UserId = request.UserId,
                PatientIds = request.PatientIds
            };

            await DbAddAsync(doctorEntity, cancellationToken);
            
            return Success(doctorEntity.Id, "Doctor created successfully!");
        }
    }
}