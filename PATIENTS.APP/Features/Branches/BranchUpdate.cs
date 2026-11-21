using System.ComponentModel.DataAnnotations;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PATIENTS.APP.Domain;

namespace PATIENTS.APP.Features.Branches
{
    public class BranchUpdateRequest : Record, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Title { get; set; }
    }

    public class BranchUpdateHandler : DbService<Branch>, IRequestHandler<BranchUpdateRequest, CommandResponse>
    {
        public BranchUpdateHandler(DbContext db) : base(db){}

        public async Task<CommandResponse> Handle(BranchUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery()
                    .AnyAsync(
                        branchEntity => branchEntity.Id != request.Id && branchEntity.Title == request.Title.Trim(),
                        cancellationToken))
                return Error(0,$"\"{request.Title}\" branch exists!");
            var branchEntity = await DbSingleAsync(request.Id, cancellationToken);

            if (branchEntity is null)
                return Error(0, "Branch not found!");
            
            branchEntity.Title = request.Title.Trim();
            await DbUpdateAsync(branchEntity, cancellationToken);
            
            return Success(branchEntity.Id, $"\"{branchEntity.Title}\" branch updated successfully.");

        }
    }
}