// Copyright Eric Chauvin 2024.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



using System;



// namespace


public static class Str
{
internal static bool contains( string toCheck,
                               string pattern )
{
return toCheck.Contains( pattern );
}


internal static string forTest( string toTest )
{
return toTest + "this and that.";
} 



} // Class


