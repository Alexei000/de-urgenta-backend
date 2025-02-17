﻿using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.QueryHandlers
{
    public class GetBackpackCategoryItemsHandler : IRequestHandler<GetBackpackCategoryItems, Result<IImmutableList<BackpackItemModel>, ValidationResult>>
    {
        private readonly IValidateRequest<GetBackpackCategoryItems> _validator;
        private readonly DeUrgentaContext _context;

        public GetBackpackCategoryItemsHandler(IValidateRequest<GetBackpackCategoryItems> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }
        public async Task<Result<IImmutableList<BackpackItemModel>, ValidationResult>> Handle(GetBackpackCategoryItems request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var backpackItems = await _context.BackpackItems
                .Where(item => item.BackpackId == request.BackpackId && item.BackpackCategory == request.CategoryId)
                .Select(item => new BackpackItemModel
                {
                    Id = item.Id,
                    Amount = item.Amount,
                    Name = item.Name,
                    CategoryType = item.BackpackCategory,
                    ExpirationDate = item.ExpirationDate
                })
                .ToListAsync(cancellationToken);

            return backpackItems.ToImmutableList();
        }
    }
}
