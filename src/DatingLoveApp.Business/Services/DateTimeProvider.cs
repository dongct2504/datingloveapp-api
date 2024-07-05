﻿using DatingLoveApp.DataAccess.Interfaces;

namespace DatingLoveApp.Business.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;

    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime LocalVietnamDateTimeNow
    {
        get
        {
            string userTimeZoneId = "SE Asia Standard Time";
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(userTimeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(UtcNow, timeZone);
        }
    }
}
