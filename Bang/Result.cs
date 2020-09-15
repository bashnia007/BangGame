using System;

namespace Bang
{
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string Reason { get; private set; }

        private Result()
        {
        }

        public static Result Success() => new Result {IsSuccess = true};

        public static Result Error(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            return new Result {Reason = message};
        }
        
        public static implicit operator bool(Result result)
        {
            return result.IsSuccess;
        }
    }

    public class Result<T>
    {
        public T Value { get; private set; }
        public string Reason { get; private set; }
        public bool IsSuccess { get; private set; }

        private Result() {}
        private Result(T value) => new Result<T>() {Value = value, IsSuccess = true};

        public static Result<T> Success(T value) => new Result<T> {Value = value, IsSuccess = true};
        public static Result<T> Error(string message) => new Result<T> {Reason = message, IsSuccess = false};
    }
}