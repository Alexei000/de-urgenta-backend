﻿using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Events.Api.QueryHandlers;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Events.Api.Tests.QueryHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetEventTypesHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetEventTypesHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetEventTypes>>();
            validator
                .IsValidAsync(Arg.Any<GetEventTypes>())
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new GetEventTypesHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new GetEventTypes(), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}
