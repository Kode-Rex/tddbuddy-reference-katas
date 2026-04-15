namespace TimeZoneConverter;

public static class TimeZoneConverter
{
    public static DateTime Convert(DateTime local, TimeSpan fromOffset, TimeSpan toOffset)
    {
        return local - fromOffset + toOffset;
    }
}
