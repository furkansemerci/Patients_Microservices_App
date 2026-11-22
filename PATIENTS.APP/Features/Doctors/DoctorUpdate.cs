using System.ComponentModel.DataAnnotations;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PATIENTS.APP.Domain;

namespace PATIENTS.APP.Features.Doctors
{
    public class DoctorUpdateRequest : Record, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")]
        public int? BranchId { get; set; }
        
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters.")]
        public string LastName { get; set; }
        
        public bool IsExpert { get; set; }
        
        public int? UserId { get; set; }
        
        [Required]
        public List<int> PatientIds { get; set; }
    }

    public class DoctorUpdateHandler : DbService<Doctor>, IRequestHandler<DoctorUpdateRequest, CommandResponse>
    {
        public DoctorUpdateHandler(DbContext db) : base(db){}

        protected override IQueryable<Doctor> DbQuery()
        {
            return base.DbQuery().Include(d => d.DoctorPatients);
        }

        public async Task<CommandResponse> Handle(DoctorUpdateRequest request, CancellationToken cancellationToken)
        {
            var doctorEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (doctorEntity is null)
                return Error(0, "Doctor is not found");
            
            DbRemove(doctorEntity.DoctorPatients);

            doctorEntity.BranchId = request.BranchId.Value;
            doctorEntity.FirstName = request.FirstName;
            doctorEntity.LastName = request.LastName;
            doctorEntity.IsExpert = request.IsExpert;
            doctorEntity.UserId = request.UserId;
            doctorEntity.PatientIds = request.PatientIds;
            
            await DbUpdateAsync(doctorEntity, cancellationToken);
            return Success(doctorEntity.Id, "Doctor updated successfully!");
        }
    }
}