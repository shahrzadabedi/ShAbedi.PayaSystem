//using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace ShAbedi.PayaSystem.Application.Common.Result;
//public class Result<T>
//{
//    private readonly List<Error> _errors = new();

//    public T? Request { get; protected set; } = default!;
//    public bool IsError => _errors.Any();

//    private Result(T data) => Request = data;
//    private Result(IEnumerable<Error> errors) => AddErrorCore(errors);

//    public Result<T> SetSuccess(T data)
//    {
//        ClearErrorCore();
//        Request = data;
//        return this;
//    }
//    public Result<T> AddError(params Error[] codes)
//    {
//        AddErrorCore(codes);
//        return this;
//    }
//    public Result<T> AddError(IEnumerable<Error> codes)
//    {
//        AddErrorCore(codes);
//        return this;
//    }

//    public static Result<T> Success(T data) => new(data);
//    public static Result<T> Error(params Error[] codes) => new(codes);
//    public static Result<T> Error(IEnumerable<Error> codes) => new(codes);
//    public static Result<T> Error(Exception exception) =>
//        new(new[]
//        {
//            new Error
//            {
//                HttpCode = HttpErrorCode.InternalServerError,
//                Code = ErrorCodeConst.UnknownError,
//                Message = exception.Message
//            }
//        });
//    public static Result<T> ValidationError(List<string> messages) =>
//        new(new[]
//        {
//            new Error
//            {
//                HttpCode = HttpErrorCode.BadRequest,
//                Code = ErrorCodeConst.ValidationError,
//                Message = string.Join("," , messages)
//            }
//        });

//    public static Result<T> ValidationError(params string[] messages) =>
//        new(new[]
//        {
//            new Error
//            {
//                HttpCode = HttpErrorCode.BadRequest,
//                Code = ErrorCodeConst.ValidationError,
//                Message = string.Join("," , messages)
//            }
//        });

//    public static Result<T> DataInsufficientError(params string[] messages) =>
//        new(new[]
//        {
//            new Error
//            {
//                HttpCode = HttpErrorCode.BadRequest,
//                Code = ErrorCodeConst.ValidationError,
//                Message = string.Join("," , messages)
//            }
//        });

//    public static Result<T> DataAlreadyExistsError(params string[] messages) =>
//        new(new[]
//        {
//            new Error
//            {
//                HttpCode = HttpErrorCode.BadRequest,
//                Code = ErrorCodeConst.ValidationError,
//                Message = string.Join("," , messages)
//            }
//        });

//    private void AddErrorCore(IEnumerable<Error> codes) => _errors.AddRange(codes);
//    private void ClearErrorCore() => _errors.Clear();
//}
