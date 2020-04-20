using System;

namespace Sample.Contracts
{
    //We created our message contract for OrderSubmissionAccepted
    public interface OrderSubmissionAccepted
    {
        Guid OrderId { get; }
        DateTime Timestamp { get; }
        string CustomerNumber { get; }
    }
}
