﻿using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.Validators
{
    public class UpdateBlogPostValidator : IValidateRequest<UpdateBlogPost>
    {
        private readonly DeUrgentaContext _context;

        public UpdateBlogPostValidator(DeUrgentaContext context)
        {
            _context = context;
        }
        
        public async Task<bool> IsValidAsync(UpdateBlogPost request)
        {
            var blogPostExists = await _context.Blogs.AnyAsync(x => x.Id == request.BlogPostId);
            
            return blogPostExists;
        }
    }
}