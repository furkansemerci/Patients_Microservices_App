using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PATIENTS.APP.Domain;

namespace PATIENTS.APP.Features.Doctors
{
    public class DoctorRemoveRequest : Record , IRequest<CommandResponse> {}

    public class DoctorRemoveHandler : DbService<Doctor>, IRequestHandler<DoctorRemoveRequest, CommandResponse>
    {
        public DoctorRemoveHandler(DbContext db) : base(db) {}

        protected override IQueryable<Doctor> DbQuery()
        {
            return base.DbQuery().Include(u => u.DoctorPatients);
        }

        public async Task<CommandResponse> Handle(DoctorRemoveRequest request, CancellationToken cancellationToken)
        {
            var doctorEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (doctorEntity is null)
            {
                Error(0, "Doctor not found");
            }
            
            DbRemove(doctorEntity.DoctorPatients);
            await DbRemoveAsync(doctorEntity,cancellationToken);
            return Success(doctorEntity.Id, "Doctor deleted successfully");
        }
    }
}