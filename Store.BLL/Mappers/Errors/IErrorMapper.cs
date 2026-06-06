using Store.Common;
using FluentValidation.Results;

namespace Store.BLL
{
    public interface IErrorMapper
    {
        Dictionary<string, List<Errors>> MapError(ValidationResult validationResult);
    }
}
