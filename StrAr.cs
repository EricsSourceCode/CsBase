// Copyright Eric Chauvin 2024.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



using System;



// namespace


public class StrAr
{
private string[] mainAr;
// private int[] sortIndexArray;
private int last = 0;




internal StrAr()
{
// try
// mainAr = new  string[arraySize];
// sortIndexArray = new int[arraySize];
}


internal int getLast()
{
return last;
}


internal string getStrAt( int where )
{
if( where < 0 )
  return "";

if( where >= last )
  return "";

return mainAr[where];
}



internal void freeAll()
{
last = 0;
mainAr = null;
// sortIndexArray = null;
}



internal void split( string inS, char delim )
{
Char[] delimArray = new Char[] { delim };

mainAr = inS.Split( delimArray );
last = mainAr.Length;
}




} // Class
