using FilmRentalStore.DTO;
using FluentValidation;

namespace FilmRentalStore.Validators
{
    public class RentalValidator : AbstractValidator<RentalDTO>
    {
        public RentalValidator()
        {
            RuleFor(x => x.ReturnDate).NotEmpty().WithMessage("ReturnDate must not be empty").GreaterThan(DateTime.MinValue).WithMessage("ReturnDate must be a valid date");
            RuleFor(x => x.StaffId).NotEmpty().WithMessage("StaffId is required").GreaterThan(0).WithMessage("StaffId must be greater than 0");
            RuleFor(x => x.RentalDate).LessThanOrEqualTo(x => x.ReturnDate).WithMessage("RentalDate must be before ReturnDate");
            RuleFor(x => x.InventoryId).NotEmpty().WithMessage("InventoryId is required").GreaterThan(0).WithMessage("InventoryId must be greater than 0");
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required").GreaterThan(0).WithMessage("CustomerId must be greater than 0");

        }
    }
}
