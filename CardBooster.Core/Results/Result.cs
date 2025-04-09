using CardBooster.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Core.Results
{
    public class Result
    {
       public bool IsSuccess { get; private set; }
       public string? ErrorMessage { get; private set; }
       public Exception? Exception { get; private set; }

        public bool IsFailure => !IsSuccess;

        private Result(bool isSuccess, string? errorMessage = null, Exception? exception = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public static Result Success() => new Result(true);
        public static Result Failure(string errorMessage) => 
            new Result(false, errorMessage);
        public static Result Failure(Exception exception) =>
            new Result(false, exception.Message, exception);
        public static Result Failure(string errorMessage, Exception exception) =>
            new Result(false, errorMessage, exception);


    }

    public class Result<TResult> //QueryResult<TResult>
    {
      
        public bool IsSuccess { get; private set; }
        public TResult? Content { get; private set; }
        public string? ErrorMessage { get; private set; }
        public Exception? Exception { get; private set; }

        private Result(bool isSuccess, TResult? content = default, string? errorMessage = null, Exception? exception = null)
        {
            IsSuccess = isSuccess;
            Content = content;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public static Result<TResult> Success(TResult content) => 
            new Result<TResult>(true, content);

        public static Result<TResult> Failure(string errorMessage) =>
            new Result<TResult>(false, default, errorMessage);
        public static Result<TResult> Failure(Exception exception) =>
            new Result<TResult>(false, default, exception.Message, exception);

        public static Result<TResult> Failure(string errorMessage, Exception exception) =>
            new Result<TResult>(false, default, errorMessage, exception);
    }
}
