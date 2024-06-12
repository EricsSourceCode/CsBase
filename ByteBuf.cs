// Copyright Eric Chauvin 2024.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html




using System;



// namespace



public class ByteBuf
{
private ByteArray bArray;
private int last = 0;


internal ByteBuf()
{
try
{
bArray = new ByteArray();
}
catch( Exception ) //  Except )
  {
  string showS = "ByteBuf: not" +
     " enough memory.";
     //   + Except.Message;

  throw new Exception( showS );
  }
}


internal void setSize( int howBig )
{
last = 0;
bArray.setSize( howBig );
}




internal int getLast()
{
return last;
}


internal void truncateLast( int setTo )
{
if( setTo < 0 )
  throw new Exception(
             "ByteBuf.truncateLast < zero." );

if( setTo > last )
  throw new Exception(
        "ByteBuf.truncateLast too big." );

last = setTo;
}


internal void clear()
{
last = 0;
}


/*
  void copyToCharArray( ByteArray& copyTo );

  void copyToOpenCharArrayNoNull(
                  OpenCharArray& copyTo ) const;

  void copyToOpenCharArrayNull(
                  OpenCharArray& copyTo ) const;

  void copyFromOpenCharArrayNoNull(
                  const OpenCharArray& copyFrom );

  void copyFromOpenCharArrayNull(
                  const OpenCharArray& copyFrom );

  void appendChar( const char toSet );

  void appendCharPt( const char* pStr );

  void appendCharArray( const ByteArray& toAdd,
                        const Int32 howMany );

  void appendCharBuf( const CharBuf& charBuf );

  inline char getC( const Int32 where ) const
    {
    if( where >= last )
      throw "getC() past last.";

    return cArray.getC( where );
    }
*/



internal byte getU8( int where )
{
if( where >= last )
  throw new Exception( 
               "getU8() past last." );

return bArray.getVal( where );
}



/*
  inline void setC( const Int32 where,
                    const char toSet )
    {
    if( where >= last )
      throw "CharBuf.setC where past last.";

    cArray.setC( where, toSet );
    }


  inline void setU8( const Int32 where,
                     const Uint8 toSet )
    {
    if( where >= last )
      throw "CharBuf.setU8 where past last.";

    cArray.setU8( where, toSet );
    }


  inline void fillBytes( const Uint8 toSet,
                         const Int32 howMany )
    {
    clear();
    for( Int32 count = 0; count < howMany; count++ )
      appendU8( toSet );

    }

  void setFromHexTo256( const CharBuf& hexBuf );

  inline void xorFrom( const CharBuf& fromBuf )
    {
    if( last != fromBuf.last )
      throw "CharBuf::xorFrom last != from.";

    const Int32 max = last;
    for( Int32 count = 0; count < max; count++ )
      {
      char fromByte = fromBuf.getC( count );
      char toSet = fromByte xor getC( count );
      cArray.setC( count, toSet );
      }
    }

  bool searchMatches( const Int32 position,
                  const CharBuf& toFind ) const;

  Int32 findText( const CharBuf& toFind,
                  const Int32 startAt ) const;

  static inline char toLower(
                       const char fromChar )
    {
    if( !((fromChar >= 'A') &&
                          (fromChar <= 'Z')) )
      return fromChar;

    // 'A' is 65 and 'a' is 97.
    // A lower case letter is 32 plus the
    // upper case letter.  So that is the
    // fifth bit.
    const char fifth = 'a' - 'A';
    return fromChar + fifth;
    // Or xor that fifth bit.
    // return fromChar xor fifth;
    }

  bool contains( const CharBuf& toFind ) const;

  };



// Copyright Eric Chauvin 2022 - 2024.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



#include "CharBuf.h"
#include "StIO.h"
#include "Casting.h"
#include "ByteHex.h"



CharBuf::CharBuf( const char* pStr )
{
setFromCharPoint( pStr );
}


#include "../CppMem/MemoryWarnTop.h"

void CharBuf::setFromCharPoint(
                         const char* pStr )
{
last = 0;
if( pStr == nullptr )
  return;

const char* sizePoint = pStr;

Int32 strSize = 0;

// Make it a reasonable loop count so it
// doesn't go forever if it never finds null.

bool foundNull = false;
for( Int32 count = 0; count < 5000; count++ )
  {
  char c = *sizePoint;
  if( c == 0 )
    {
    foundNull = true;
    break;
    }

  sizePoint++;
  strSize++;
  }

if( !foundNull )
  return;

const Int32 max = strSize;

setSize( max + 1 );

for( Int32 count = 0; count < max; count++ )
  {
  char c = *pStr;
  cArray.setC( last, c );
  last++;
  pStr++;
  }
}



#include "../CppMem/MemoryWarnBottom.h"




void CharBuf::setFromInt64( const Int64 inN )
{
Int64 n = inN;

// Clears it too.
setSize( 1024 );

if( n == 0 )
  {
  appendChar( '0' );
  return;
  }

ByteArray tempBuf;
tempBuf.setSize( 1024 );

Int32 lastTemp = 0;
bool isNegative = false;
if( n < 0 )
  {
  isNegative = true;
  n = n * -1;
  }

Int64 toDivide = n;
while( toDivide != 0 )
  {
  Int64 digit = toDivide % 10;
  // Ascii values go from '0' up to '9'.
  tempBuf.setC( lastTemp, Casting::i32ToChar(
               Casting::i64ToI32((
                '0' + digit ))));
  lastTemp++;

  toDivide = toDivide / 10;
  }

if( isNegative )
  {
  tempBuf.setC( lastTemp, '-' );
  lastTemp++;
  }

// Clears it too.
setSize( lastTemp );

// Reverse it.
for( Int32 count = lastTemp - 1; count >= 0;
                                       count-- )
  {
  appendChar( tempBuf.getC( count ));
  }
}



void CharBuf::setFromUint64( const Uint64 inN )
{
Uint64 n = inN;

// Clears it too.
setSize( 1024 );

if( n == 0 )
  {
  appendChar( '0' );
  return;
  }

ByteArray tempBuf;
tempBuf.setSize( 1024 );

Int32 lastTemp = 0;

Uint64 toDivide = n;
while( toDivide != 0 )
  {
  Uint64 digit = toDivide % 10;
  // Ascii values go from '0' up to '9'.
  tempBuf.setC( lastTemp, Casting::i32ToChar(
          '0' + Casting::u64ToI32(digit )));
  lastTemp++;

  toDivide = toDivide / 10;
  }

// Clears it too.
setSize( lastTemp );

// Reverse it.
for( Int32 count = lastTemp - 1; count >= 0;
                                       count-- )
  {
  appendChar( tempBuf.getC( count ));
  }
}



CharBuf::CharBuf( const CharBuf& in )
{
if( in.testForCopy )
  return;

throw "CharBuf copy constructor called.";
}


void CharBuf::setSize( const Int32 howBig )
{
last = 0;
cArray.setSize( howBig );
}


void CharBuf::increaseSize( const Int32 howMuch )
{
cArray.increaseSize( howMuch );
}



void CharBuf::appendChar( const char toSet )
{
// It's good if you can set the size ahead
// of time.
if( (last + 1) >= cArray.getSize() )
  increaseSize( 1024 * 16 );

cArray.setC( last, toSet );
last++;
}
*/



internal void appendU8( byte toSet )
{
// It's good if you can set the size ahead
// of time.
if( (last + 1) >= bArray.getSize() )
  bArray.increaseSize( 1024 * 16 );

bArray.setVal( last, toSet );
last++;
}



/*
void CharBuf::appendU16( const Uint32 toSet )
{
if( (last + 1) >= cArray.getSize() )
  increaseSize( 1024 * 16 );

cArray.setU8( last, ((toSet >> 8) & 0xFF) );
last++;

cArray.setU8( last, (toSet & 0xFF) );
last++;
}
*/



internal void appendU32( uint toSet )
{
// Big endian.
byte toAdd = (byte)(toSet >> (3 * 8));
appendU8( toAdd );

toAdd = (byte)( toSet >> (2 * 8));
appendU8( toAdd );

toAdd = (byte)( toSet >> (1 * 8));
appendU8( toAdd );

toAdd = (byte)( toSet );
appendU8( toAdd );
}




internal void appendU64( ulong toSet )
{
// Big endian.
byte toAdd = (byte)(toSet >> (7 * 8));
appendU8( toAdd );

/*
toAdd = Casting::u64ToU8( toSet >> (6 * 8) );
appendU8( toAdd );

toAdd = Casting::u64ToU8( toSet >> (5 * 8) );
appendU8( toAdd );

toAdd = Casting::u64ToU8( toSet >> (4 * 8) );
appendU8( toAdd );

toAdd = Casting::u64ToU8( toSet >> (3 * 8) );
appendU8( toAdd );

toAdd = Casting::u64ToU8( toSet >> (2 * 8) );
appendU8( toAdd );

toAdd = Casting::u64ToU8( toSet >> (1 * 8) );
appendU8( toAdd );

toAdd = Casting::u64ToU8( toSet );
appendU8( toAdd );
*/
}



internal uint getU32( int where )
{
// Big endian.
uint toSet = getU8( where );
toSet <<= 8;

uint nextC = getU8( where + 1 );
toSet |= nextC;
toSet <<= 8;

nextC = getU8( where + 2 );
toSet |= nextC;
toSet <<= 8;

nextC = getU8( where + 3 );
toSet |= nextC;

return toSet;
}


/*
Uint64 CharBuf::getU64( const Int32 where ) const
{
// Big endian.
Uint64 toSet = getU8( where );
toSet <<= 8;

Uint64 nextC = getU8( where + 1 );
toSet |= nextC;
toSet <<= 8;

nextC = getU8( where + 2 );
toSet |= nextC;
toSet <<= 8;

nextC = getU8( where + 3 );
toSet |= nextC;
toSet <<= 8;

nextC = getU8( where + 4 );
toSet |= nextC;
toSet <<= 8;

nextC = getU8( where + 5 );
toSet |= nextC;
toSet <<= 8;

nextC = getU8( where + 6 );
toSet |= nextC;
toSet <<= 8;

nextC = getU8( where + 7 );
toSet |= nextC;

return toSet;
}



// Used in Base64 encoding.
Uint32 CharBuf::get24Bits( const Int32 where ) const
{
// Big endian.
Uint32 toSet = getU8( where );
toSet <<= 8;

Uint32 nextC = getU8( where + 1 );
toSet |= nextC;
toSet <<= 8;

nextC = getU8( where + 2 );
toSet |= nextC;

return toSet;
}



Uint32 CharBuf::get16Bits( const Int32 where ) const
{
// Big endian.
Uint32 toSet = getU8( where );
toSet <<= 8;

Uint32 nextC = getU8( where + 1 );
toSet |= nextC;

return toSet;
}



void CharBuf::append24Bits( const Uint32 toSet )
{
// Big endian.
Uint8 toAdd = Casting::u32ToU8(
                         toSet >> (2 * 8) );
appendU8( toAdd );

toAdd = Casting::u32ToU8(
                         toSet >> (1 * 8) );
appendU8( toAdd );

toAdd = Casting::u32ToU8( toSet );
appendU8( toAdd );
}


void CharBuf::appendCharArray(
                       const ByteArray& toAdd,
                       const Int32 howMany )
{
if( (last + howMany + 2) >= cArray.getSize() )
  increaseSize( howMany + (1024 * 16) );

for( Int32 count = 0; count < howMany; count++ )
  {
  cArray.setC( last, toAdd.getC( count ) );
  last++;
  }
}


void CharBuf::appendCharBuf( const CharBuf& charBuf )
{
const Int32 howMany = charBuf.getLast();

if( (last + howMany + 2) >= cArray.getSize() )
  increaseSize( howMany + (1024 * 16) );

for( Int32 count = 0; count < howMany; count++ )
  {
  cArray.setC( last, charBuf.getC( count ));
  last++;
  }
}
*/



internal void copy( ByteBuf toCopy )
{
last = toCopy.last;

bArray.copy( toCopy.bArray );
}



/*
void CharBuf::copyToCharArray( ByteArray& copyTo )
{
const Int32 max = getLast();
copyTo.setSize( max );
for( Int32 count = 0; count < max; count++ )
  copyTo.setC( count, cArray.getC( count ));

}



void CharBuf::copyToOpenCharArrayNoNull(
                     OpenCharArray& copyTo ) const
{
const Int32 max = getLast();
copyTo.setSize( max );

// Memory::copy()  Memory.cpp

for( Int32 count = 0; count < max; count++ )
  copyTo.setC( count, cArray.getC( count ));

}


void CharBuf::copyToOpenCharArrayNull(
                     OpenCharArray& copyTo ) const
{
const Int32 max = getLast();
copyTo.setSize( max + 1 );

// Memory::copy()  Memory.cpp

for( Int32 count = 0; count < max; count++ )
  copyTo.setC( count, cArray.getC( count ));

copyTo.setC( max, 0 );
}


void CharBuf::copyFromOpenCharArrayNoNull(
                 const OpenCharArray& copyFrom )
{
const Int32 max = copyFrom.getSize();
cArray.setSize( max + 1024);

// Memory::copy()  Memory.cpp

for( Int32 count = 0; count < max; count++ )
  cArray.setC( count, copyFrom.getC( count ));

last = max;
}



void CharBuf::copyFromOpenCharArrayNull(
                 const OpenCharArray& copyFrom )
{
const Int32 max = copyFrom.getSize();
setSize( max + 1024);

// Memory::copy()  Memory.cpp

for( Int32 count = 0; count < max; count++ )
  {
  char c = copyFrom.getC( count );
  if( c == 0 )
    return;

  appendChar( c );
  }
}



void CharBuf::testBasics( void )
{
Uint64 test1 = 135;
appendU64( test1 );

Uint64 test2 = 0xF123345678543219ULL;
appendU64( test2 );

Uint64 test = getU64( 0 );
if( test != test1 )
  throw "CharBuf test basics.";

// Make sure the offset is right.
// A multiple of 4 for Uint32.
// 8 for Uint64.

test = getU64( 8 );
if( test != test2 )
  throw "CharBuf test basics.";

}



// === Move this into ByteArray.
Int32 CharBuf::findChar( const Int32 start,
                         const char toFind )
{
const Int32 max = last;
if( start < 0 )
  return -1;

if( start >= max )
  return -1;

for( Int32 count = start; count < max; count++ )
  {
  if( cArray.getC( count ) == toFind )
    return count;

  }

return -1; // Didn't find it.
}


bool CharBuf::isEqual( const CharBuf& toCheck )
                                          const
{
if( last != toCheck.last )
  return false;

const Int32 max = last;
for( Int32 count = 0; count < max; count++ )
  {
  if( cArray.getC( count ) !=
                  toCheck.cArray.getC( count ) )
    return false;

  }

return true;
}


#include "../CppMem/MemoryWarnTop.h"


void CharBuf::appendCharPt( const char* pStr )
{
if( pStr == nullptr )
  return;

const char* sizePoint = pStr;
Int32 strSize = 0;

bool foundNull = false;
for( Int32 count = 0; count < 5000; count++ )
  {
  char c = *sizePoint;
  if( c == 0 )
    {
    foundNull = true;
    break;
    }

  sizePoint++;
  strSize++;
  }

if( !foundNull )
  return;

const Int32 max = strSize;

for( Int32 count = 0; count < max; count++ )
  {
  appendChar( *pStr );
  pStr++;
  }
}


#include "../CppMem/MemoryWarnBottom.h"



void CharBuf::reverse( void )
{
const Int32 max = getLast();

ByteArray tempBuf;
tempBuf.setSize( max );

// Reverse it.
Int32 where = 0;
for( Int32 count = max - 1; count >= 0; count-- )
  {
  tempBuf.setC( where, getC( count ));
  where++;
  }

clear();
for( Int32 count = 0; count < max; count++ )
  appendChar( tempBuf.getC( count ));

}
*/



internal string getHexStr()
{
string result = "";

int max = getLast();
for( int count = 0; count < max; count++ )
  {
  byte oneByte = getU8( count );
/*
  char leftC = ByteHex::getLeftChar( oneByte );
  StIO::putChar( leftC );
  char rightC = ByteHex::getRightChar( oneByte );
  StIO::putChar( rightC );
  StIO::putChar( ' ' );
*/
  }


return result;
}



/*
void CharBuf::showAscii( void ) const
{
const Int32 max = getLast();
for( Int32 count = 0; count < max; count++ )
  {
  Uint8 oneByte = getU8( count );
  if( oneByte == 10 ) // Line Feed.
    {
    StIO::putLF();
    continue;
    }

  if( (oneByte < 127) && (oneByte >= 32))
    {
    char showC = oneByte & 0x7F;
    StIO::putChar( showC );
    }
  }

StIO::putLF();
}



// From a string like this:
// const char* theBytes =
// "b4 bb 49 8f 82 79 30 3d 98 08 36 39"


void CharBuf::setFromHexTo256(
                       const CharBuf& hexBuf )
{
CharBuf cleanBuf;

// StIO::putS( "setFromHexTo256()" );

const Int32 max = hexBuf.getLast();
cleanBuf.setSize( max ); // max / 2

for( Int32 count = 0; count < max; count++ )
  {
  char c = hexBuf.getC( count );

  if( !ByteHex::isValidChar( c ))
    continue;

  cleanBuf.appendChar( c );

  // StIO::putChar( c );
  }

// StIO::putLF();

const Int32 maxC = cleanBuf.getLast();
setSize( maxC ); // max / 2

for( Int32 count = 0; (count + 1) < maxC;
                                     count += 2 )
  {
  char c = cleanBuf.getC( count );
  Uint8 firstHalf = ByteHex::charToU8( c );
  c = cleanBuf.getC( count + 1 );
  Uint8 secondHalf = ByteHex::charToU8( c );

  Uint8 oneByte = (firstHalf << 4) & 0xFF;
  oneByte |= secondHalf;
  appendU8( oneByte );
  }
}



bool CharBuf::searchMatches(
                  const Int32 position,
                  const CharBuf& toFind ) const
{
const Int32 findLength = toFind.getLast();
if( findLength < 1 )
  return false;

if( (position + findLength - 1) >= last )
  return false;

for( Int32 count = 0; count < findLength;
                                     count++ )
  {
  char asciiChar = cArray.getC(
                           position + count );
  asciiChar = toLower( asciiChar );

  if( asciiChar != toFind.getC( count ) )
    return false;

  }

return true;
}



Int32 CharBuf::findText( const CharBuf& toFind,
                    const Int32 startAt ) const
{
const Int32 max = last;

if( startAt >= max )
  return -1;

const Int32 toFindLen = toFind.getLast();
CharBuf toFindLow;
for( Int32 count = startAt; count < toFindLen;
                                         count++ )
  {
  toFindLow.appendChar( toLower(
                     toFind.getC( count )) );
  }

for( Int32 count = startAt; count < max; count++ )
  {
  if( searchMatches( count, toFindLow ))
    return count;

  }

return -1;
}


bool CharBuf::contains(
                   const CharBuf& toFind ) const
{
if( findText( toFind, 0 ) >= 0 )
  return true;

return false;
}


*/



} // Class
