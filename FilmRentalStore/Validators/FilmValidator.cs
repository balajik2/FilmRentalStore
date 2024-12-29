using FilmRentalStore.DTO;
using FilmRentalStore.Validators;

namespace FilmRentalStore.Validators
{
    //public class FilmValidator : AbstractValidator<FilmDTO>
    //{
    //    public FilmValidator()
    //    {
            
    //        RuleFor(x => x.LanguageId).NotNull().WithMessage("Language Id is Required !");
    //        RuleFor(x => x.Rating).Must(rating => new[] { "NC-17", "R", "PG-13", "PG", "G" }.Contains(rating)).WithMessage("Please provide a valid rating: NC-17, R, PG-13, PG, or G.");

    //    }
    //}

    public class CustomFilmValidator 
    {
        public virtual List<string> Validate(FilmDTO film)
        {
            var errors = new List<string>();

           
            if (film.LanguageId == null)
                errors.Add("Language Id is required!");

           
            var validRatings = new[] { "NC-17", "R", "PG-13", "PG", "G" };
            if (string.IsNullOrEmpty(film.Rating) || !validRatings.Contains(film.Rating))
                errors.Add("Please provide a valid rating: NC-17, R, PG-13, PG, or G.");

           
            if (string.IsNullOrWhiteSpace(film.Title))
                errors.Add("Title is required!");
            else if (film.Title.Length < 3)
                errors.Add("Title must be at least 3 characters long.");

            
            if (!string.IsNullOrEmpty(film.Description) && film.Description.Length > 500)
                errors.Add("Description cannot exceed 500 characters.");



           
            if (film.RentalDuration <= 0)
                errors.Add("Rental Duration must be a positive number.");

           
            if (film.RentalRate <= 0)
                errors.Add("Rental Rate must be a positive number.");

           
            if (film.Length.HasValue && film.Length <= 0)
                errors.Add("Length must be greater than zero if provided.");

            return errors;
        }
    }

}
