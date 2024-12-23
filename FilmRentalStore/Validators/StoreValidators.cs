using FluentValidation;
using FilmRentalStore.DTO;
namespace FilmRentalStore.Validators
{
    public class StoreValidators : AbstractValidator<StoreDTO>
    {
        public StoreValidators() {

            RuleFor(x => x.ManagerStaffId).NotEmpty().WithMessage("managerStaff is mandatory");
            RuleFor(x => x.AddressId).NotEmpty().WithMessage("addressId is mandatory");

        
        }
    
    }
}
