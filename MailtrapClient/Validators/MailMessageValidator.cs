using FluentValidation;

namespace Mailtrap;

internal class MailMessageValidator : AbstractValidator<MailMessage>
{
    public MailMessageValidator()
    {
        RuleFor(mailMessage => mailMessage.From.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(mailMessage => mailMessage.To.Length)
            .NotEmpty()
            .WithMessage("There must be at least 1 recipient address");

        RuleFor(mailMessage => mailMessage.To[0].Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(mailMessage => mailMessage.Subject)
            .NotEmpty()
            .WithMessage("The subject must consist of at least 1 character");

        RuleFor(mailMessage => mailMessage.Text)
            .NotEmpty()
            .When(mailMessage => string.IsNullOrEmpty(mailMessage.Html))
            .WithMessage("Text or Html must be specified or both");

        RuleFor(mailMessage => mailMessage.Html)
            .NotEmpty()
            .When(mailMessage => string.IsNullOrEmpty(mailMessage.Text))
            .WithMessage("Text or Html must be specified or both");
    }
}
