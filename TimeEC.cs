// Copyright Eric Chauvin 2024.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



using System;
// System.TimeZoneInfo



// namespace



public class TimeEC
{
private DateTime utcTime;
private DateTime utcStartYear1900;



public TimeEC()
{
setToYear1900();
setStartYear1900Time();
}




public TimeEC( ulong index )
{
setFromIndex( index );
setStartYear1900Time();
}


internal DateTime getUTCDateTime()
{
return utcTime;
}




private void setStartYear1900Time()
{
utcStartYear1900 = new DateTime( 1900,
                      1,
                      1,
                      0,
                      0,
                      0,
                      0,
                      DateTimeKind.Utc );
                      // DateTimeKind.Local

}



internal void setToNow()
{
utcTime = DateTime.UtcNow;
}



internal bool setFromDateTimeString(
                 string dateTimeIn )
{
DateTime localDateTime;
try
{
localDateTime = DateTime.Parse( dateTimeIn );
}
catch
  {
  return false;
  }

utcTime = localDateTime.ToUniversalTime();
return true;
}



internal void addDays( double days )
{
try
{
utcTime = utcTime.AddDays( days );
}
catch( Exception )
  {
  //
  }
}


internal void addHours( double hours )
{
try
{
utcTime = utcTime.AddHours( hours );
}
catch( Exception )
  {
  //
  }
}


internal void addMinutes( int min )
{
try
{
utcTime = utcTime.AddMinutes( min );
}
catch( Exception )
  {
  //
  }
}


internal void addSeconds( int seconds )
{
try
{
utcTime = utcTime.AddSeconds( seconds );
}
catch( Exception )
  {
  //
  }
}


internal void addMilliseconds( int mSecs )
{
try
{
utcTime = utcTime.AddMilliseconds( mSecs );
}
catch( Exception )
  {
  //
  }
}


internal int getYear()
{
return utcTime.Year;
}

internal int getMonth()
{
return utcTime.Month;
}

internal int getDay()
{
return utcTime.Day;
}

internal int getHour()
{
return utcTime.Hour;
}

internal int getMinute()
{
return utcTime.Minute;
}

internal int getSecond()
{
return utcTime.Second;
}

internal int getMillisecond()
{
return utcTime.Millisecond;
}


internal int getLocalDayOfWeek()
{
// Cast the enum to int.
return (int)utcTime.ToLocalTime().DayOfWeek;
}


internal int getLocalYear()
{
return utcTime.ToLocalTime().Year;
}


internal int getLocalMonth()
{
return utcTime.ToLocalTime().Month;
}


internal int getLocalDay()
{
return utcTime.ToLocalTime().Day;
}



internal int getLocalHour()
{
return utcTime.ToLocalTime().Hour;
}


internal int getLocalMinute()
{
return utcTime.ToLocalTime().Minute;
}


internal int getLocalSecond()
{
return utcTime.ToLocalTime().Second;
}


internal string toUTCDateString()
{
return utcTime.ToShortDateString();
}


internal string toUTCTimeString()
{
return utcTime.ToShortTimeString();
}


internal string toLocalDateString()
{
return utcTime.ToLocalTime().
                        ToShortDateString();
}



internal string toLocalDateStringShort()
{
DateTime rightNow = utcTime.ToLocalTime();

int day = rightNow.Day;
int month = rightNow.Month;
int year = rightNow.Year;
year = year % 100;

string dayS = day.ToString();
if( dayS.Length == 1 )
  dayS = "0" + dayS;

string monthS = month.ToString();
if( monthS.Length == 1 )
  monthS = "0" + monthS;

string yearS = year.ToString();
if( yearS.Length == 1 )
  yearS = "0" + yearS;

return monthS + "/" + dayS + "/" + yearS;
}



internal string toLocalDateStringVeryShort()
{
DateTime rightNow = utcTime.ToLocalTime();

int day = rightNow.Day;
int month = rightNow.Month;

string dayS = day.ToString();
if( dayS.Length == 1 )
  dayS = "0" + dayS;

string monthS = month.ToString();
if( monthS.Length == 1 )
  monthS = "0" + monthS;

return monthS + "/" + dayS;
}



/*
  internal string ToUTCDateStringShort()
    {
    // return UTCTime.ToShortDateString();
    int Day = UTCTime.Day;
    int Month = UTCTime.Month;
    int Year = UTCTime.Year;
    Year = Year % 100;

    string DayS = Day.ToString();
    if( DayS.Length == 1 )
      DayS = "0" + DayS;

    string MonthS = Month.ToString();
    if( MonthS.Length == 1 )
      MonthS = "0" + MonthS;

    string YearS = Year.ToString();
    if( YearS.Length == 1 )
      YearS = "0" + YearS;

    return MonthS + "/" + DayS + "/" + YearS;
    }



  internal int GetUTCHour()
    {
    return UTCTime.Hour;
    }



  internal string ToLocalTimeString()
    {
    return UTCTime.ToLocalTime().
                     ToShortTimeString();
    }


  internal string ToLocalTimeStringNoSeconds()
    {
    int Hour = GetLocalHour();
    int Minute = GetLocalMinute();

    string AMPM = "AM";
    if( Hour >= 12 )
      AMPM = "PM";

    string MinS = Minute.ToString();
    if( MinS.Length == 1 )
      MinS = "0" + MinS;

    Hour = Hour % 12;

    return Hour.ToString() + ":" +
                      MinS + " " + AMPM;
    }
*/



internal void copy( TimeEC toCopy )
{
// This won't be quite exact since it's
// to the nearest millisecond.

utcTime = new DateTime( toCopy.getYear(),
                        toCopy.getMonth(),
                        toCopy.getDay(),
                        toCopy.getHour(),
                        toCopy.getMinute(),
                        toCopy.getSecond(),
                        toCopy.getMillisecond(),
                        DateTimeKind.Utc );
                        // DateTimeKind.Local


}




/*
  internal void TruncateToEvenSeconds()
    {
    UTCTime = new DateTime( UTCTime.Year,
                            UTCTime.Month,
                            UTCTime.Day,
                            UTCTime.Hour,
                            UTCTime.Minute,
                            UTCTime.Second,
                            0,
                            DateTimeKind.Utc );
                            // DateTimeKind.Local

    }


//////////
  internal ulong GetTicks()
    {
    return (ulong)UTCTime.Ticks;
    }
/////////
*/



internal ulong getIndex()
{
// 16 bits is enough for the year
//   6,500 or so.  (64K)
ulong Result = (uint)utcTime.Year;

Result <<= 4; // Room for a month up to 16.
Result |= (uint)utcTime.Month;

Result <<= 5; // 32 days.
Result |= (uint)utcTime.Day;

Result <<= 5; // 32 hours.
Result |= (uint)utcTime.Hour;

Result <<= 6; // 64 minutes.
Result |= (uint)utcTime.Minute;

Result <<= 6; // 64 seconds.
Result |= (uint)utcTime.Second;

Result <<= 10;  // 1024 milliseconds.
Result |= (uint)utcTime.Millisecond;

// 16 + 5 + 4 + 5 + 6 + 6 + 10.
// (16 + 5) + (4 + 5) + (6 + 6) + 10.
// 21 +          9 +      12    + 10
// 30 +      22
// 52 bits wide.

return Result;
}


/*
  internal ulong GetTimeIndexShort()
    {
    // The 2-digit year takes up to 7 bits.
    ulong Result = (uint)(UTCTime.Year % 100);

    Result <<= 4; // Room for a month,
                  // from 0 to 15.
    Result |= (uint)UTCTime.Month;

    Result <<= 5; // From 0 to 31.
    Result |= (uint)UTCTime.Day;

    Result <<= 5; // From 0 to 31.
    Result |= (uint)UTCTime.Hour;

    Result <<= 6; // From 0 to 63.
    Result |= (uint)UTCTime.Minute;

    Result <<= 6; // From 0 to 63.
    Result |= (uint)UTCTime.Second;

    // 7 + 4 + 5 + 5 + 6 + 6 = 33
    return Result;
    }
*/



internal void setToYear1900()
{
utcTime = new DateTime( 1900,
                            1,
                            1,
                            0,
                            0,
                            0,
                            0,
                            DateTimeKind.Utc );
                            // DateTimeKind.Local

}



/*


//////////
internal static int GetUTCMonthFromTimeIndex(
                       ulong Index )
    {
    // Index >>= 10;
          // Shift off the milliseconds.
    // Index >>= 6; // Seconds
    // Index >>= 6; // Minutes
    // Index >>= 5; // Hours
    // Index >>= 5; // Days
    Index >>= 32; // All of the above at once.
    Index = Index & 0x0F; // Bottom 4 bits.
    return (int)Index;
    }
/////////



  internal void SetFromLocalDateTime(
                          DateTime SetFrom )
    {
    UTCTime = SetFrom.ToUniversalTime();
    }



  internal DateTime GetAsLocalDateTime()
    {
    return UTCTime.ToLocalTime();
    }
*/




internal void setFromIndex( ulong index )
{
if( index == 0 )
  {
  setToYear1900();
  return;
  }

// 10 bits
int millisecond = (int)(index & 0x3FF);
index >>= 10;

int second = (int)(index & 0x3F);
index >>= 6;

int minute = (int)(index & 0x3F);
index >>= 6;

int hour = (int)(index & 0x1F);
index >>= 5;

int day = (int)(index & 0x1F);
index >>= 5;

int month = (int)(index & 0xF);
index >>= 4;

// 16 bits.
int year = (int)(index & 0xFFFF);
// index >>= 16;

if( year <= 1900 )
  {
  setToYear1900();
  return;
  }

try
{
utcTime = new DateTime( year,
                        month,
                        day,
                        hour,
                        minute,
                        second,
                        millisecond,
                        DateTimeKind.Utc );
                        // DateTimeKind.Local

}
catch( Exception )
  {
  setToYear1900();
  }
}




internal void setFromLocalValues( int year,
                                  int month,
                                  int day,
                                  int hour,
                                  int minute,
                                  int second )
{
if( year <= 1900 )
  {
  setToYear1900();
  return;
  }

try
{
DateTime LocalTime = new DateTime(
                            year,
                            month,
                            day,
                            hour,
                            minute,
                            second,
                            0,
                            DateTimeKind.Local );

utcTime = LocalTime.ToUniversalTime();
}
catch( Exception )
  {
  setToYear1900();
  }
}



/*
  internal double GetDaysToNow()
    {
    DateTime RightNow = DateTime.UtcNow;
    TimeSpan TimeDif = RightNow.Subtract(
                                   UTCTime );
    return TimeDif.TotalDays;
    }


  internal double GetHoursToNow()
    {
    DateTime RightNow = DateTime.UtcNow;
    TimeSpan TimeDif = RightNow.Subtract(
                                   UTCTime );
    return TimeDif.TotalHours;
    }


  internal double GetMinutesToNow()
    {
    DateTime RightNow = DateTime.UtcNow;
    TimeSpan TimeDif = RightNow.Subtract(
                                    UTCTime );
    return TimeDif.TotalMinutes;
    }



  internal double GetSecondsToNow()
    {
    DateTime RightNow = DateTime.UtcNow;
    TimeSpan TimeDif = RightNow.Subtract(
                                    UTCTime );
    return TimeDif.TotalSeconds;
    }




  internal double GetTotalMilliseconds()
    {
    TimeSpan TimeDif = UTCTime.Subtract(
                           UTCStartYear1900 );
    return TimeDif.TotalMilliseconds;
    }



  internal long GetTicks()
    {
    return UTCTime.Ticks;
    }




////////////
  internal static string GetDayOfWeek(
                           DateTime TheDate )
    {
    if( TheDate.DayOfWeek == DayOfWeek.Sunday )
      return "Sun";

    if( TheDate.DayOfWeek == DayOfWeek.Monday )
      return "Mon";

    if( TheDate.DayOfWeek == DayOfWeek.Tuesday )
      return "Tue";

    if( TheDate.DayOfWeek == DayOfWeek.Wednesday )
      return "Wed";

    if( TheDate.DayOfWeek == DayOfWeek.Thursday )
      return "Thu";

    if( TheDate.DayOfWeek == DayOfWeek.Friday )
      return "Fri";

    if( TheDate.DayOfWeek == DayOfWeek.Saturday )
      return "Sat";

    // Didn't find a match.
    return "";
    }
/////////////




//////////
  internal static string GetTimeString(
                             DateTime TheTime )
    {
    int Hour = TheTime.Hour;
    string AmPm = "AM";
    if( Hour > 12 )
      {
      Hour -= 12;
      AmPm = "PM";
      }

    string HourS = Hour.ToString();
    if( HourS.Length == 1 )
      HourS = " " + HourS;

    string MinS = TheTime.Minute.ToString();
    if( MinS.Length == 1 )
      MinS = "0" + MinS;

    string SecS = TheTime.Second.ToString();
    if( SecS.Length == 1 )
      SecS = "0" + SecS;

    return HourS + ":" + MinS + ":" + SecS +
                                  " " + AmPm;
    }
////////////





// See this in the old ECTime.
// MakeTimeZonesList()



  internal string GetLocalMillisecondTime()
    {
    DateTime RightNow = UTCTime.ToLocalTime();

    // This won't be quite exact since it's
                   // to the nearest millisecond.
    string HourS = RightNow.Hour.ToString();
    string MinuteS = RightNow.Minute.ToString();
    string SecondS = RightNow.Second.ToString();
    string MillisecS = RightNow.Millisecond.
                                   ToString();

    if( MinuteS.Length == 1 )
      MinuteS = "0" + MinuteS;

    if( SecondS.Length == 1 )
      SecondS = "0" + SecondS;

    if( MillisecS.Length == 1 )
      MillisecS = "0" + MillisecS;

    if( MillisecS.Length == 2 )
      MillisecS = "0" + MillisecS;

    string Result = HourS + ":" +
                    MinuteS + ":" +
                    SecondS + ":" +
                    MillisecS;

    return Result;
    }

*/



internal string toDelimStr()
{
// 2024;3;7;10;13;17;877828000

return "" + getLocalYear() + ";" +
       getLocalMonth() + ";" +
       getLocalDay() + ";" +
       getLocalHour() + ";" +
       getLocalMinute() + ";" +
       getLocalSecond() + ";" +
       "0;"; // millisec.  0 to 999.

}



internal void setFromDelim( string inS )
{
try
{
setToYear1900();

int year = 1;
int month = 1;
int day = 1;
int hour = 0;
int minute = 0;
int second = 0;
int millisec = 0;

StrAr fields = new StrAr();
fields.split( inS, ';' );
int last = fields.getLast();
if( last < 1 )
  return;

year = Int32.Parse( fields.getStrAt( 0 ));

if( last > 1 )
month = Int32.Parse( fields.getStrAt( 1 ));

if( last > 2 )
  day = Int32.Parse( fields.getStrAt( 2 ));

if( last > 3 )
  hour = Int32.Parse( fields.getStrAt( 3 ));

if( last > 4 )
  minute = Int32.Parse( fields.getStrAt( 4 ));

if( last > 5 )
  second = Int32.Parse( fields.getStrAt( 5 ));


// millisec has to be 0 through 999.
// if( last > 6 )
//   millisec = Int32.Parse( fields.getStrAt( 6 ));


utcTime = new DateTime( year,
                        month,
                        day,
                        hour,
                        minute,
                        second,
                        millisec,
                        DateTimeKind.Local );

}
catch( Exception )
  {
  throw new Exception( "TimeEC setFromDelim" );
  }
}





} // Class
