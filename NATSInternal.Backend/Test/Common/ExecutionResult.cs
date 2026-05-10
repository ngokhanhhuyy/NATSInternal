namespace NATSInternal.Test.Common;

public class ExecutionResult<TResponseDto>
{
    #region
    public TResponseDto? ResponseDto { get; set; }
    public Exception? Exception { get; set; }
    #endregion
}