using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Contracts
{
    //We created our message contract for SubmitOrder
    public interface SubmitOrder
    {
        Guid OrderId { get; }
        DateTime Timestamp { get; }
        string CustomerNumber { get; }
    }
}
