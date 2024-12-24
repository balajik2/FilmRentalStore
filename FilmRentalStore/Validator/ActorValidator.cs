using FilmRentalStore.DTO;
using FluentValidation;

namespace FilmRentalStore.Validator
{
    public class ActorValidator : AbstractValidator<ActorDTO>
    {
        public ActorValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is Required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is Required");
            RuleFor(x => x.ActorId).NotEmpty().WithMessage("ActorId is Required");
            

        }

    }
}
