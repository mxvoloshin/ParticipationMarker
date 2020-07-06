using System;

namespace ParticipationMarker.Common.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTimeOffset TableEntityTimeStamp => DateTimeOffset.UtcNow;
    }
}