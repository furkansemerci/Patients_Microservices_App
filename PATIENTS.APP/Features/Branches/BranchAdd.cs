using System.ComponentModel.DataAnnotations;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PATIENTS.APP.Domain;

namespace PATIENTS.APP.Features.Branches
{
    
    public class BranchAddRequest: Record, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Title { get; set; }
        
    }

    public class BranchAddHandler : DbService<Branch>, IRequestHandler<BranchAddRequest, CommandResponse>
    {
        public BranchAddHandler(DbContext db) : base(db)
        {
            
        }

        public async Task<CommandResponse> Handle(BranchAddRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(branchEntity => branchEntity.Title == request.Title.Trim(), cancellationToken))
                return Error(0, $"\"{request.Title}\" branch exists!");

            var branchEntity = new Branch()
            {
                Title = request.Title.Trim()
            };
            await DbAddAsync(branchEntity, cancellationToken);

            return Success(branchEntity.Id, $"\"{branchEntity.Title}\" branch created successfully.");
        }
    }
        
        
        
}