namespace AICQD.Models
{
    public sealed class ApiResult<T>
    {
        public bool Ok => Error == null;
        public string? Error { get; set; }
        public T? Data { get; set; }
        public static ApiResult<T> FromData(T data) => new() { Data = data };
        public static ApiResult<T> FromError(string err) => new() { Error = err };
    }
}

