using System.Text.Json.Serialization;

namespace Store.Common
{
    public class GeneralResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, List<Errors>>? Errors { get; set; }

        public static GeneralResult SuccessResult(string message = "Success")
            => new() { Success = true, Message = message };

        public static GeneralResult FailureResult(string message, Dictionary<string, List<Errors>>? errors = null)
            => new() { Success = false, Message = message, Errors = errors };
    }

    public class GeneralResult<T> : GeneralResult
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        public static GeneralResult<T> SuccessResult(T data, string message = "Success")
            => new() { Success = true, Message = message, Data = data };

        public static new GeneralResult<T> FailureResult(string message, Dictionary<string, List<Errors>>? errors = null)
            => new() { Success = false, Message = message, Errors = errors };
    }
}
