// Copyright Eric Chauvin 2024.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



using System;



// namespace


public static class RangeT
{
internal static void test( int where,
                           int min, int max,
                           string showS )
{
if( where < min )
  throw new Exception(
             "Range test min: " + showS );

if( where > max )
  throw new Exception(
            "Range test mex: " + showS );

}



} // Class
