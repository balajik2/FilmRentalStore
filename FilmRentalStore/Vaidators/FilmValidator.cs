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
            RuleFor(x => x.Rating).Must(rating => new[] { "NC-17", "R", "PG-13", "PG", "G" }.Contains(rating)).WithMessage("Please provide a valid rating: NC-17, R, PG-13, PG, or G.");

        }
    }

}
