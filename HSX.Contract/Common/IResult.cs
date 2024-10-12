namespace HSX.Contract.Common;

public interface IResult
{
    public bool Success { get; }
    public string Message { get; }
    public string[] Errors { get; }
    public string GetErrorMessage();
}
