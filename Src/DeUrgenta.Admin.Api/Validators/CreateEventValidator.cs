﻿using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.Validators
{
    public class CreateEventValidator : IValidateRequest<CreateEvent>
    {
        private readonly DeUrgentaContext _context;

        public CreateEventValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(CreateEvent request)
        {
            var eventTypeExists = await _context.EventTypes.AnyAsync(x => x.Id == request.Event.EventTypeId);

            return eventTypeExists;
        }
    }
}