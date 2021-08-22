using System.Collections.Generic;

namespace N_Tier.Application.Models
{
    public class ApiResult<T>
    {
        private ApiResult() { }

        private ApiResult(bool succeeded, int code, T result, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Code = code;
            Result = result;
            Errors = errors;
        }

        public bool Succeeded { get; set; }

        public int Code { get; set; }

        public T Result { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public static ApiResult<T> Success(int code, T result)
        {
            return new(true, code, result, new List<string>());
        }

        public static ApiResult<T> Success200(T result)
        {
            return Success(200, result);
        }

        public static ApiResult<T> Failure(int code, IEnumerable<string> errors)
        {
            return new(false, code, default, errors);
        }
    }
}
