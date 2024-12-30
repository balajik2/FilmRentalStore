using FilmRentalStore.DTO;
using FluentValidation;

namespace FilmRentalStore.Validators
{
    public class InventoryValidators : AbstractValidator<InventoryDTO>
    {
        public InventoryValidators()
        {

            RuleFor(x => x.FilmId)
               .NotEmpty().WithMessage("FilmId is required.")
               .Custom((filmId, context) =>
               {

                   if (filmId <= 0)
                   {
                       context.AddFailure("FilmId", "FilmId must be greater than 0.");
                   }
                   RuleFor(x => x.StoreId).NotEmpty().WithMessage("storeId is required");
               });
               
        }
    }
}
