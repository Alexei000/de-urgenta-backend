﻿using System;
using DeUrgenta.Domain.Api.Entities;

namespace DeUrgenta.Backpack.Api.Models
{
    public sealed record BackpackItemRequest
    {
        public string Name { get; init; }

        public uint Amount { get; init; }

        public DateTime? ExpirationDate { get; init; }

        public BackpackCategoryType CategoryType { get; init; }
        
        public ulong Version { get; init; }
    }
}