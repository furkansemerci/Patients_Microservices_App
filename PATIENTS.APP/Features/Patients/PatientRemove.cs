using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PATIENTS.APP.Domain;

namespace PATIENTS.APP.Features.Patients
{
    public class PatientRemoveRequest : Record, IRequest<CommandResponse>{}

    public class PatientRemoveHandler : DbService<Patient>, IRequestHandler<PatientRemoveRequest, CommandResponse>
    {
        public PatientRemoveHandler(DbContext db) : base(db){}

        protected override IQueryable<Patient> DbQuery()
        {
            return base.DbQuery().Include(u => u.DoctorPatients);
        }

        public async Task<CommandResponse> Handle(PatientRemoveRequest request, CancellationToken cancellationToken)
        {
            var patientEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (patientEntity is null)
                return Error(0,"Patient not found");
            
            DbRemove(patientEntity.DoctorPatients);

            await DbRemoveAsync(patientEntity, cancellationToken);
            
            return Success(patientEntity.Id, "Patient deleted successfully");
        }
    }
}