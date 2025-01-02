using FluentValidation;
using FilmRentalStore.DTO;
namespace FilmRentalStore.Validators
{
    public class StoreValidators : AbstractValidator<StoreDTO>
    {
        public StoreValidators() {

            RuleFor(x => x.ManagerStaffId).NotEmpty().WithMessage("managerStaff is mandatory");
            RuleFor(x => x.AddressId)
             .Custom((addressId, context) =>
             {
                 
                 if (addressId <= 0)
                 {
                     context.AddFailure("AddressId", "AddressId must be greater than 0.");
                 }
               
             });


        }
    
    }
}
