// Copyright Eric Chauvin 2024.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



using System;



// namespace


public static class Str
{
internal static char charAt( string inS,
                             int where )
{
if( where < 0 )
  throw new Exception( "Str.charAt < 0" );

int last = inS.Length;
if( where >= last )
  throw new Exception( "Str.charAt >= last" );

return inS[where];
}




internal static bool contains( string toCheck,
                               string pattern )
{
return toCheck.Contains( pattern );
}


internal static string replace( string toChange,
                                string toRepl,
                                string replWith )
{
return toChange.Replace( toRepl, replWith );
}


internal static string trim( string toCheck )
{
return toCheck.Trim();
}


internal static string toLower( string toCheck )
{
return toCheck.ToLower();
}


internal static bool startsWith( string line,
                                 string toCheck )
{
return line.StartsWith( toCheck );
}



internal static bool endsWith( string line,
                               string toCheck )
{
return line.EndsWith( toCheck );
}



internal static string cleanAscii( string line )
{
SBuilder sBuild = new SBuilder();

int last = line.Length;
for( int count = 0; count < last; count++ )
  {
  char c = line[count];
  if( c < ' ' )
    continue;

  if( c > '~' ) // ~ is Ascii 126
    continue;

  sBuild.appendChar( c );
  }

return sBuild.toString();
}



} // Class
