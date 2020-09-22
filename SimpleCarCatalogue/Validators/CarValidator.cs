using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCarCatalogue.Validators
{
    public class CarValidator : AbstractValidator<Car>, IValidator<Car>
    {
        private readonly IProducerService _producerService;

        public string[] ValidationMessages { get; private set; }

        public CarValidator(IProducerService producerService)
        {
            _producerService = producerService;

            RuleFor(n => n.Name)
                .NotNull().WithMessage("Name can not be null")
                .Length(1, 256).WithMessage("Name lenght has to bo in [1, 255] range");

            RuleFor(n => n.Year)
                .GreaterThanOrEqualTo(1900).WithMessage("Year must be greater than 1900")
                .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage($"Year must be lower than {DateTime.UtcNow.Year}");

            var availableProducersIds = _producerService.GetAll().Select(n => n.Id).ToList();
            RuleFor(n => n.ProducerId)
                .Must(n => availableProducersIds.Any(m => m == n)).WithMessage("ProducerId is out of range");
        }

        bool IValidator<Car>.Validate(Car itemToValidate)
        {
            var validationResult = Validate(itemToValidate);
            ValidationMessages = validationResult.Errors.Select(n => n.ErrorMessage).ToArray();
            return validationResult.IsValid;
        }
    }
}
