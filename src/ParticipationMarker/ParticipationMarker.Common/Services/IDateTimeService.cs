using System;

namespace ParticipationMarker.Common.Services
{
    public interface IDateTimeService
    {
        DateTimeOffset TableEntityTimeStamp { get; }
    }
}