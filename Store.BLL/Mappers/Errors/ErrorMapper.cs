using Store.Common;
using FluentValidation.Results;

namespace Store.BLL
{
    public class ErrorMapper : IErrorMapper
    {
        public Dictionary<string, List<Errors>> MapError(ValidationResult validationResult)
        {
            return validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => new Errors
                    {
                        Code = e.ErrorCode,
                        Description = e.ErrorMessage
                    }).ToList()
                );
        }
    }
}
