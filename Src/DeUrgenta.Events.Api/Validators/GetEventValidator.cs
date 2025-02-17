﻿using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Events.Api.Queries;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Events.Api.Validators
{
    public class GetEventValidator : IValidateRequest<GetEvent>
    {
        private readonly DeUrgentaContext _context;

        public GetEventValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(GetEvent request)
        {
            var eventTypeExists = await _context.EventTypes.AnyAsync(x => x.Id == request.Filter.EventTypeId);

            return eventTypeExists ? ValidationResult.Ok : ValidationResult.GenericValidationError;
        }
    }
}
