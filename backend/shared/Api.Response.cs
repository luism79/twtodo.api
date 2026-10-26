namespace backend.shared
{
    public record ApiResponse<T>(bool success, string message, T? data);

    public record ApiErrorResponse(bool success, string message, string error);

    public static class ApiResponse
    {
        public static ApiResponse<T> Success<T>(T data, string message) => new(true, message, data);

        public static ApiErrorResponse Error(string message, string error) => new(false, message, error);
    }
}
