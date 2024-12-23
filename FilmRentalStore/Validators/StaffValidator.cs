using FilmRentalStore.DTO;
using FluentValidation;

namespace FilmRentalStore.Validators
{
    public class StaffValidator : AbstractValidator<StaffDTO>
    {
        public StaffValidator() 
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Staff Name is required");

        }
    }
}
