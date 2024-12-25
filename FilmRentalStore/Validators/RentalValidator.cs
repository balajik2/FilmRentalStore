using FilmRentalStore.DTO;
using FluentValidation;

namespace FilmRentalStore.Validators
{
    public class RentalValidator : AbstractValidator<RentalDTO>
    {
        public RentalValidator()
        {
            RuleFor(x => x.RentalDate).NotEmpty().WithMessage("RentalDate is required");



            RuleFor(x => x.InventoryId).NotEmpty().WithMessage("InventoryId is required");
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required");

        }
    }
}
