using FluentValidation;
using FilmRentalStore.DTO;
namespace FilmRentalStore.Validators
{
    public class CustomerValidator : AbstractValidator<CustomerDTO>
    {
        public CustomerValidator()
        {
         
           RuleFor(x => x.StoreId).NotEmpty().NotNull().WithMessage("StoreId is required");
             RuleFor(x => x.AddressId).NotEmpty().WithMessage("AddressId is required");
            RuleFor(x => x.FirstName).NotEmpty().NotNull().WithMessage("FirstName is required");
            RuleFor(x => x.LastName).NotEmpty().NotNull().WithMessage("LastName is required");
        }
    }
}
