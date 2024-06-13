// Copyright Eric Chauvin 2024.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html


using System;



// namespace



public static class ByteHex
{

internal static char intToChar( int c )
{
switch( c )
  {
  case 0: return '0';
  case 1: return '1';
  case 2: return '2';
  case 3: return '3';
  case 4: return '4';
  case 5: return '5';
  case 6: return '6';
  case 7: return '7';
  case 8: return '8';
  case 9: return '9';
  case 10: return 'A';
  case 11: return 'B';
  case 12: return 'C';
  case 13: return 'D';
  case 14: return 'E';
  case 15: return 'F';

  default: return '?';
  }
}



internal static char valToChar( byte c )
{
switch( c )
  {
  case 0: return '0';
  case 1: return '1';
  case 2: return '2';
  case 3: return '3';
  case 4: return '4';
  case 5: return '5';
  case 6: return '6';
  case 7: return '7';
  case 8: return '8';
  case 9: return '9';
  case 10: return 'A';
  case 11: return 'B';
  case 12: return 'C';
  case 13: return 'D';
  case 14: return 'E';
  case 15: return 'F';

  default: return '?';
  }
}



internal static char getRightChar( byte theByte )
{
return valToChar( (byte)(theByte & 0xF) );
}


internal static char getLeftChar( byte theByte )
{
return valToChar( (byte)((theByte >> 4) & 0xF) );
}


internal static byte charToU8( char c )
{
switch( c )
  {
  case '0': return 0;
  case '1': return 1;
  case '2': return 2;
  case '3': return 3;
  case '4': return 4;
  case '5': return 5;
  case '6': return 6;
  case '7': return 7;
  case '8': return 8;
  case '9': return 9;

  case 'a': return 10;
  case 'b': return 11;
  case 'c': return 12;
  case 'd': return 13;
  case 'e': return 14;
  case 'f': return 15;

  case 'A': return 10;
  case 'B': return 11;
  case 'C': return 12;
  case 'D': return 13;
  case 'E': return 14;
  case 'F': return 15;

  default: return 128;
  }
}



internal static bool isValidChar( char c )
{
switch( c )
  {
  case '0': return true;
  case '1': return true;
  case '2': return true;
  case '3': return true;
  case '4': return true;
  case '5': return true;
  case '6': return true;
  case '7': return true;
  case '8': return true;
  case '9': return true;

  case 'a': return true;
  case 'b': return true;
  case 'c': return true;
  case 'd': return true;
  case 'e': return true;
  case 'f': return true;

  case 'A': return true;
  case 'B': return true;
  case 'C': return true;
  case 'D': return true;
  case 'E': return true;
  case 'F': return true;

  default: return false;
  }
}


static string getByteStr( byte val )
{
char c = getLeftChar( val );
string result = "" + c;
c = getRightChar( val );
result += c;

return result;
}


internal static string getUint32Str( uint val )
{
byte aByte = (byte)((val >> 24) & 0xFF);
string result = getByteStr( aByte );

aByte = (byte)((val >> 16) & 0xFF);
result += getByteStr( aByte );

aByte = (byte)((val >> 8) & 0xFF);
result += getByteStr( aByte );

aByte = (byte)(val & 0xFF);
result += getByteStr( aByte );

return result;
}



internal static int byteBufToInt32(
                              ByteBuf inBuf )
{
int result = 0;
int NumbrBase = 16;
int positionBase = 1;
int max = inBuf.getLast();

for( int count = max - 1; count >= 0; count-- )
  {
  int digit = charToU8(
                   (char)inBuf.getU8( count ));
  digit = digit * positionBase;
  result += digit;
  positionBase = positionBase * NumbrBase;

  // Don't multiply too big of a
  // positionBase.
  if( positionBase > 0x3FFFFFF )
    break;

  }

return result;
}


} // Class
