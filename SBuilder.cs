// Copyright Eric Chauvin 2024.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



using System;
using System.Text; // StringBuilder



// namespace


public class SBuilder
{
private StringBuilder sBuild;


public SBuilder()
{
sBuild = new StringBuilder();
}


public void appendChar( char c )
{
sBuild.Append( c );
}


public string toString()
{
return sBuild.ToString();
}


} // Class
