using System;

namespace Sample.Contracts
{
    //We created our message contract for OrderSubmissionRejected
    public interface OrderSubmissionRejected
    {
        Guid OrderId { get; }
        DateTime Timestamp { get; }
        string CustomerNumber { get; }
        string Reason { get; }
    }
}
