using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SchoolSecuritySystem.Core.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T? Value { get; }
        public Error Error { get; }

        private Result(T value) { IsSuccess = true; Value = value; Error = null!; }
        private Result(Error error) { IsSuccess = false; Error = error; }

        public static Result<T> Success(T value) => new(value);
        public static Result<T> Failure(Error err) => new(err);
    }
}
