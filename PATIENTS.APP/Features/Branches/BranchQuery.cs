using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PATIENTS.APP.Domain;

namespace PATIENTS.APP.Features.Branches
{
    public class BranchQueryRequest : Record, IRequest<IQueryable<BranchQueryResponse>>{}

    public class BranchQueryResponse : Record 
    {
        public string Title { get; set; }
        public int DoctorCountR { get; set; }
        public string DoctorNamesR { get; set; }
    }

    public class BranchQueryHandler : DbService<Branch>,
        IRequestHandler<BranchQueryRequest, IQueryable<BranchQueryResponse>>
    {
        public BranchQueryHandler(DbContext db) : base(db){}

        protected override IQueryable<Branch> DbQuery()
        {
            return base.DbQuery().Include(b => b.Doctors). OrderBy(b => b.Title);
        }

        public Task<IQueryable<BranchQueryResponse>> Handle(BranchQueryRequest request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(DbQuery().Select(b => new BranchQueryResponse
            {
                Id = b.Id,
                Title = b.Title,
                DoctorCountR = b.Doctors.Count,
                DoctorNamesR = string.Join(", ",
                    b.Doctors.OrderBy(d => d.LastName).ThenBy(d => d.FirstName)
                        .Select(d => d.FirstName + " " + d.LastName))
            }));
        }
    }
}