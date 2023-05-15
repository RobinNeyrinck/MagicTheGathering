namespace Howest.MagicCards.MinimalAPI.Validator;

public class CardCustomValidator : AbstractValidator<MongoCardDTO>
{
	public CardCustomValidator()
	{
		RuleFor(c => c.Name)
			.NotNull()
			.NotEmpty()
			.WithMessage("Name is required")
			.Length(1, 100)
			.WithMessage("Name must be between 1 and 100 characters");

		RuleFor(c => c.Amount)
			.NotNull()
			.NotEmpty()
			.WithMessage("Amount is required")
			.GreaterThanOrEqualTo(0)
			.WithMessage("Amount must be greater than or equal to 0");			
	}
}
