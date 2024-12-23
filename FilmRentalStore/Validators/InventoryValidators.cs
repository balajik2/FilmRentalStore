using FilmRentalStore.DTO;
using FluentValidation;

namespace FilmRentalStore.Validators
{
    public class InventoryValidators : AbstractValidator<InventoryDTO>
    {
        public InventoryValidators()
        {
            RuleFor(x => x.FilmId).NotEmpty().WithMessage("FilmId is required");
            RuleFor(x => x.StoreId).NotEmpty().WithMessage("storeId is required");
        }
    }
}
