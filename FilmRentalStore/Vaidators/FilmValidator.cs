using FilmRentalStore.DTO;
using FluentValidation;

namespace FilmRentalStore.Vaidators
{
    public class FilmValidator : AbstractValidator<FilmDTO>
    {
        public FilmValidator() 
        {
            RuleFor(x => x.FilmId).NotNull().WithMessage("Film Id is Required !");
            RuleFor(x => x.LanguageId).NotNull().WithMessage("Language Id is Required !");
            RuleFor(x => x.Rating).MinimumLength(1).WithMessage("Please give the rating !");
        }
    }
}
