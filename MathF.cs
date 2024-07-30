// Copyright Eric Chauvin 2024.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



// Math Functions.


using System;




// This class is mainly for things from
// the System.Math class.



public static class MathF
{


internal static double log( double x )
{
// Log to the base e.
return Math.Log( x );
}



internal static double exp( double x )
{
// This is e to the x.
return Math.Exp( x );
}



} // Class
