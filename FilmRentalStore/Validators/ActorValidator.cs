using FilmRentalStore.DTO;
using FluentValidation;

namespace FilmRentalStore.Validators
{
    public class ActorValidator : AbstractValidator<ActorDTO>
    {
        public ActorValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is Required").Matches("^[a-zA-Z]+$").WithMessage("FirstName should only contain letters");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is Required").Matches("^[a-zA-Z]+$").WithMessage("LastName should only contain letters");
            RuleFor(x => x.ActorId).NotEmpty().WithMessage("ActorId is Required").Must(id => int.TryParse(id.ToString(), out _)).WithMessage("ActorId should only contain numbers");



        }

    }
}
