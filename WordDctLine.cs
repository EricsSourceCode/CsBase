// Copyright Eric Chauvin 2024 - 2025.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



using System;



// namespace



public class WordDctLine
{
private MainData mData;
private Word[] valueArray;
private int arrayLast = 0;
private int[] sortIndexAr;



private WordDctLine()
{
}



internal WordDctLine( MainData useMainData )
{
mData = useMainData;

try
{
valueArray = new Word[1];
sortIndexAr = new int[1];
sortIndexAr[0] = 0;

// An array of structs would get initialized,
// but not an array of objects.
valueArray[0] = new Word( mData, "" );
}
catch( Exception ) // Except )
  {
  throw new Exception(
       "Not enough memory for WordDctLine." );

  }
}




//////////
internal void freeAll()
{
arrayLast = 0;
resizeArrays( 1 );
}
/////////




internal int getArrayLast()
{
return arrayLast;
}



internal void clear()
{
arrayLast = 0;
}



void resizeArrays( int newSize )
{
int oldSize = valueArray.Length;

try
{
Array.Resize( ref valueArray, newSize );
Array.Resize( ref sortIndexAr, newSize );

for( int count = 0; count < newSize; count++ )
  sortIndexAr[count] = count;

if( newSize > oldSize )
  {
  for( int count = oldSize; count < newSize;
                                    count++ )
    {
    // An array of structs would get initialized,
    // but not an array of objects.
    valueArray[count] = new
                     Word( mData, "" );
    }
  }
}
catch( Exception ) // Except )
  {
  throw new Exception(
         "Not enough memory for WordDctLine." );

  }
}



private int getPositionOfWord( string word )
{
if( word == null )
  return -1;

if( arrayLast < 1 )
  return -1;

int max = arrayLast;
for( int count = 0; count < max; count++ )
  {
  if( valueArray[count].wordIsEqual( word ))
    return count;

  }

return -1;
}



internal bool keyExists( string word )
{
if( getPositionOfWord( word ) < 0 )
  return false;

return true;
}




internal void setValueAnyID( Word value )
{
// This sets the Word to the new value
// whether it's already there or not.

string word = value.getWord();

// the, and, by, a ...
if( word.Length < 4 )
  return;

int pos = getPositionOfWord( word );
if( pos >= 0 )
  {
  valueArray[pos].copy( value );
  }
else
  {
  int arraySize = valueArray.Length;
  if( arrayLast >= arraySize )
    resizeArrays( arraySize + 16 );

  valueArray[arrayLast].copy( value );
  arrayLast++;
  }
}




internal void getValue( string word,
                        Word toGet )
{
toGet.clear();
int pos = getPositionOfWord( word );
if( pos < 0 )
  return;

toGet.copy( valueArray[pos] );
}




internal void getCopyWordAt( Word toGet,
                             int where )
{
toGet.clear();

if( where < 0 )
  return;

if( where >= arrayLast )
  return;

toGet.copy( valueArray[where] );
}



internal void sortByWord()
{
if( arrayLast < 2 )
  return;

while( true )
  {
  bool swapped = false;

  for( int count = 0; count < (arrayLast - 1);
                                    count++ )
    {
    if( 0 < String.Compare(
          valueArray[sortIndexAr[count]].
                                  getWord(),
          valueArray[sortIndexAr[count + 1]].
                                  getWord(),
                                  true ))
      {
      int tempIndex = sortIndexAr[count];
      sortIndexAr[count] =
                       sortIndexAr[count + 1];
      sortIndexAr[count + 1] = tempIndex;
      swapped = true;
      }
    }

  if( !swapped )
    break;

  }
}



internal void sortByCount()
{
if( arrayLast < 2 )
  return;

while( true )
  {
  bool swapped = false;

  for( int count = 0; count < (arrayLast - 1);
                                    count++ )
    {
    if( valueArray[sortIndexAr[count]].
                                  getCount() <
          valueArray[sortIndexAr[count + 1]].
                                  getCount())
      {
      int tempIndex = sortIndexAr[count];
      sortIndexAr[count] =
                       sortIndexAr[count + 1];
      sortIndexAr[count + 1] = tempIndex;
      swapped = true;
      }
    }

  if( !swapped )
    break;

  }
}




internal void getCopySortedWordAt( Word toGet,
                                   int where )
{
toGet.clear();

if( where < 0 )
  return;

if( where >= arrayLast )
  return;

toGet.copy( valueArray[sortIndexAr[where]] );
}



internal void incCount( string word )
{
int pos = getPositionOfWord( word );
if( pos < 0 )
  return;

valueArray[pos].incCount();
}





} // Class
