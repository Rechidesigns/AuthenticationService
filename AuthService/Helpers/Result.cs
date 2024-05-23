using AuthService.Data.UserDatas.DTOs;

namespace AuthService.Helpers
{
    public class Result<T>
    {
        public bool Succeeded { get; set; }
        public T Value { get; set; }
        public string Error { get; set; }
        public string Message { get; private set; }

        public static Result<T> Success(T value) => new Result<T> { Succeeded = true, Value = value };
        public static Result<T> Success(T value, string message) => new() { Succeeded = true, Value = value, Message = message };

        public static Result<T> Failure(string error) => new Result<T> { Succeeded = false, Error = error, Message = error };
        public static Result<T> ValidationError(List<string> errors) => new Result<T> { Succeeded = false, Error = string.Join(",", errors) };

        internal static Result<LoginResponseDto> Failure(object invalid_User_Password)
        {
            throw new NotImplementedException();
        }
    }
}
