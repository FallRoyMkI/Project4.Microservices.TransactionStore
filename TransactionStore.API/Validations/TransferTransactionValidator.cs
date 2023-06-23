using FluentValidation;
using TransactionStore.Models.Dtos;

namespace TransactionStore.API.Validations
{
    public class TransferTransactionValidator : AbstractValidator<TransferTransactionDtoRequest>
    {
        private readonly List<string> _moneyTypes = new() { "RUB", "USD", "EUR", "JPY", "CNY", "RSD", "BGN", "ARS" }; 
        public TransferTransactionValidator()
        {
            RuleFor(request => request.Amount)
               .Must(x => x > 0).WithMessage("Invalid Amount");
            RuleFor(request => request.AccountId)
                .Must(x => x > 0).WithMessage("Invalid AccountId");
            RuleFor(request => request)
                .Must(x => x.TargetAccountId > 0 && x.TargetAccountId != x.AccountId).WithMessage("Invalid TargetAccountId");
            RuleFor(request => request.MoneyType)
                .Must(x => _moneyTypes.Contains(x)).WithMessage("Invalid MoneyType");
            RuleFor(request => request.TargetMoneyType)
                .Must(x => _moneyTypes.Contains(x)).WithMessage("Invalid TargetMoneyType");
            RuleFor(request => request)
               .Must(x => x.MoneyType != x.TargetMoneyType).WithMessage("Invalid MoneyTypes");
        }
    }
}
