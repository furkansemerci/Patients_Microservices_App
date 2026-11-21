using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PATIENTS.APP.Domain;

namespace PATIENTS.APP.Features.Doctors
{
    public class DoctorQueryRequest : Record, IRequest<IQueryable<DoctorQueryResponse>>{}

    public class DoctorQueryResponse : Record
    {
        public int BranchId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsExpert{get; set;}
        public int? UserId { get; set; }
        public string FullNameR{get; set;}
        public string IsExpertR{get; set;}
        public string BranchTitleR{get; set;}
        public List<int> PatientIds{get; set;}
        public string PatientNamesR{get; set;}
    }

    public class DoctorQueryHandler : DbService<Doctor>,
        IRequestHandler<DoctorQueryRequest, IQueryable<DoctorQueryResponse>>
    {
        public DoctorQueryHandler(DbContext db) : base(db){}

        protected override IQueryable<Doctor> DbQuery()
        {
            return base.DbQuery()
                .Include(d => d.Branch)
                .Include(d => d.DoctorPatients)
                .ThenInclude(dp => dp.Patient)
                .OrderBy(d => d.LastName)
                .ThenBy(d => d.FirstName);
        }
        
        public Task<IQueryable<DoctorQueryResponse>> Handle(
            DoctorQueryRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(DbQuery().Select(d => new DoctorQueryResponse
            {
                Id = d.Id,
                BranchId = d.BranchId,
                FirstName = d.FirstName,
                LastName = d.LastName,
                IsExpert = d.IsExpert,
                UserId = d.UserId,
                FullNameR = d.FirstName + " " + d.LastName,
                IsExpertR = d.IsExpert ? "Yes" : "No",
                BranchTitleR = d.Branch.Title,
                PatientIds = d.DoctorPatients.Select(dp => dp.PatientId).ToList(),
                PatientNamesR = string.Join(", ", d.DoctorPatients
                    .OrderBy(dp => dp.Patient.LastName)
                    .ThenBy(dp => dp.Patient.FirstName)
                    .Select(dp => dp.Patient.FirstName + " " + dp.Patient.LastName))
            }));
        }
    }
}