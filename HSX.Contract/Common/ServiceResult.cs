namespace HSX.Contract.Common;

public struct ServiceResult : IResult
{
    public bool Success { get; }
    public string Message { get; }
    public string[] Errors { get; }

    public ServiceResult(bool success, string message, params string[] errors)
    {
        Success = success;
        Message = message;
        Errors = errors;
    }
    public static ServiceResult Ok(string message = Constants.Success) => new ServiceResult(true, message, []);
    public static ServiceResult<T> Ok<T>(T value = default!, string message = Constants.Success) => new ServiceResult<T>(true, message, value, default);
    public static ServiceResult Fail(string message = Constants.Failed, params string[] errors) => new ServiceResult(false, message, errors);
    public static ServiceResult<T> Fail<T>(string message = Constants.Failed, params string[] errors) => new ServiceResult<T>(false, message, default, errors);
    public static ServiceResult NotFound(string message = Constants.NotFound, params string[] errors) => new ServiceResult(false, message, errors);
    public static ServiceResult<T> NotFound<T>(string message = Constants.NotFound, params string[] errors) => new ServiceResult<T>(false, message,default,errors);

    public string GetErrorMessage()
    {
        return $"MSG: {Message} ERROR: {string.Join(",",Errors)}";
    }
}

public struct ServiceResult<T> : IResult
{
    public T Value { get; }
    public bool Success { get; }
    public string Message { get; }
    public string[] Errors { get; }

    public ServiceResult(bool success, string message,T value, params string[] errors)
    {
        Success = success;
        Message = message;
        Errors = errors;
        Value = value;
    }

    public string GetErrorMessage()
    {
        return $"MSG: {Message} ERROR: {string.Join(",", Errors)}";
    }
}
