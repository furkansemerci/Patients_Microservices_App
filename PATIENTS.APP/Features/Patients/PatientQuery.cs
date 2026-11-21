using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PATIENTS.APP.Domain;

namespace PATIENTS.APP.Features.Patients
{
    public class PatientQueryRequest : Record, IRequest<IQueryable<PatientQueryResponse>>
    {
    }

    public class PatientQueryResponse : Record
    {
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Genders Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int? UserId { get; set; }
        public string FullNameR { get;  set; }
        public string GenderR { get; set; }
        public string BirthDateR { get; set; }
        public string HeightR { get; set; }
        public string WeightR { get; set; }
        public string DoctorNamesR { get; set; }
    }

    public class PatientQueryHandler : DbService<Patient>,
        IRequestHandler<PatientQueryRequest, IQueryable<PatientQueryResponse>>
    {
        public PatientQueryHandler(DbContext db) : base(db){}

        protected override IQueryable<Patient> DbQuery()
        {
            return base.DbQuery()
                .Include(p => p.DoctorPatients)
                .ThenInclude(dp => dp.Doctor)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName);
        }

        public Task<IQueryable<PatientQueryResponse>> Handle(PatientQueryRequest request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(DbQuery().Select(p => new PatientQueryResponse
            {
                Id = p.Id,
                Height = p.Height,
                Weight = p.Weight,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Gender = p.Gender,
                BirthDate = p.BirthDate,
                UserId = p.UserId,
                FullNameR = p.FirstName + " " + p.LastName,
                GenderR = p.Gender.ToString(),
                BirthDateR = p.BirthDate.ToShortDateString(),
                HeightR = p.Height.HasValue ? p.Height.Value.ToString("N2") : string.Empty,
                WeightR = p.Weight.HasValue ? p.Weight.Value.ToString("N2") : string.Empty,
                DoctorNamesR = string.Join(", ", p.DoctorPatients
                    .OrderBy(dp => dp.Doctor.LastName)
                    .ThenBy(dp => dp.Doctor.FirstName)
                    .Select(dp => dp.Doctor.FirstName + " " + dp.Doctor.LastName))
            }));
        }
    }
}