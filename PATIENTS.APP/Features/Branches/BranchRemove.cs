using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PATIENTS.APP.Domain;


namespace PATIENTS.APP.Features.Branches
{
    public class BranchRemoveRequest: Record, IRequest<CommandResponse>
    {
    }

    public class BranchRemoveHandler : DbService<Branch>, IRequestHandler<BranchRemoveRequest, CommandResponse>
    {
        public BranchRemoveHandler(DbContext db) : base(db)
        {
            
        }

        protected override IQueryable<Branch> DbQuery()
        {
            return base.DbQuery().Include(branchEntity => branchEntity.Doctors);
        }

        public async Task<CommandResponse> Handle(BranchRemoveRequest request, CancellationToken cancellationToken)
        {
            var branchEntity = await DbSingleAsync(request.Id, cancellationToken);
            
            if(branchEntity is null)
                return Error(0,"Branch not found");

            if (branchEntity.Doctors.Any())
            {
                return Error(0,"Branch has relational doctors");
            }

            await DbRemoveAsync(branchEntity, cancellationToken);
            
            return Success(branchEntity.Id, "Branch deleted successfully");

        }
    }
}