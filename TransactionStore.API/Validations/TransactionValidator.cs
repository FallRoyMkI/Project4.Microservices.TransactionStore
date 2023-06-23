using FluentValidation;
using TransactionStore.Models.Dtos;

namespace TransactionStore.API.Validations
{
    public class TransactionValidator : AbstractValidator<TransactionDtoRequest>
    {
        public TransactionValidator() 
        {
            RuleFor(request => request.Amount)
               .Must(x => x != 0).WithMessage("Invalid Amount");
            RuleFor(request => request.AccountId)
                .Must(x => x > 0).WithMessage("Invalid AccountId");
        }
    }


}
