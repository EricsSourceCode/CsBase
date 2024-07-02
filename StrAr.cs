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
private int[] sortIndexArray;
private int last = 0;




internal StrAr()
{
// try
mainAr = new  string[2];
sortIndexArray = new int[2];
}


internal int getLast()
{
return last;
}




// getSortedStrAt()
internal string getStrAt( int where )
{
if( where < 0 )
  return "";

if( where >= last )
  return "";

// sortIndexArray

return mainAr[where];
}



internal void freeAll()
{
last = 0;
resizeArrays( 2 );
}



internal void split( string inS, char delim )
{
Char[] delimArray = new Char[] { delim };

mainAr = inS.Split( delimArray );
last = mainAr.Length;
sortIndexArray = new int[last];
}




private void resizeArrays( int newSize )
{
int oldSize = mainAr.Length;

try
{
Array.Resize( ref mainAr, newSize );
Array.Resize( ref sortIndexArray, newSize );

if( newSize > oldSize )
  {
  for( int count = oldSize; count < newSize;
                                    count++ )
    {
    // An array of structs would get initialized,
    // but not an array of objects.

    // Does this need to be done for strings?
    mainAr[count] = "";
    }
  }
}
catch( Exception ) // Except )
  {
  throw new Exception(
              "Not enough memory for StrAr." );
  }
}




internal void append( string inS )
{
int arSize = mainAr.Length;
if( (last + 1) >= arSize )
  resizeArrays( arSize + 1024 );

mainAr[last] = inS;
last++;
}



} // Class
