// Copyright Eric Chauvin 2024.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



using System;



// namespace



/*
UTF-8 (UCS Transformation Format -8)
UCS: Universal Character Set

C0 Controls and Basic Latin (Basic Latin)
                         (0000007F)
C1 Controls and Latin-1 Supplement (0080 00FF)
Latin Extended-A (0100 017F)
Latin Extended-B (0180 024F)
IPA Extensions (0250 02AF)
Spacing Modifier Letters (02B0 02FF)
Combining Diacritical Marks (0300 036F)
Greek and Coptic (0370 03FF)
Combining Diacritical Marks Supplement
                                 (1DC0 1DFF)
Latin extended additional (1E00 1EFF)
Greek Extended (1F00 1FFF)

Symbols:
General Punctuation (2000206F)
Superscripts and Subscripts (2070209F)
Currency Symbols (20A020CF)
Combining Diacritical Marks for Symbols
                                (20D020FF)
Letterlike Symbols (2100214F)
Number Forms (2150218F)
Arrows (219021FF)
Mathematical Operators (220022FF)
Miscellaneous Technical (230023FF)
Control Pictures (2400243F)
Optical Character Recognition (2440245F)
Enclosed Alphanumerics (246024FF)
Box Drawing (2500257F)
Block Elements (2580259F)
Geometric Shapes (25A025FF)
Miscellaneous Symbols (260026FF)

Dingbats (2700 27BF)

Miscellaneous Mathematical Symbols-A (27C027EF)
Supplemental Arrows-A (27F027FF)
Braille Patterns (280028FF)
Supplemental Arrows-B (2900297F)
Miscellaneous Mathematical Symbols-B (298029FF)
Supplemental Mathematical Operators (2A002AFF)
Miscellaneous Symbols and Arrows (2B002BFF)

Surrogates:
High Surrogates (D800 DB7F)
High Private Use Surrogates (DB80DBFF)
Low Surrogates (DC00DFFF)

*/



static class UTF8Str
{

internal static byte[] stringToBytes(
                             string InString )
{
if( InString == null )
  return null;

if( InString.Length == 0 )
  return null;

// UTF-16 is "either one or two 16-bit code _units_ per code point.
// One Char in a Windows string is a code unit.

// But "All code points in the BMP are accessed as a single code unit
// in UTF-16 encoding and can be encoded in one, two or three bytes in
// UTF-8".

// Bits
//  7  U+007F  0xxxxxxx
// 11  U+07FF  110xxxxx  10xxxxxx
// 16  U+FFFF  1110xxxx  10xxxxxx  10xxxxxx

// 21  U+1FFFFF  11110xxx  10xxxxxx  10xxxxxx  10xxxxxx
// 26  U+3FFFFFF  111110xx  10xxxxxx  10xxxxxx  10xxxxxx  10xxxxxx
// 31  U+7FFFFFFF  1111110x  10xxxxxx  10xxxxxx  10xxxxxx  10xxxxxx  10x

// try
byte[] Result = new byte[InString.Length * 3];

int Where = 0;
for( int Count = 0; Count < InString.Length; Count++ )
  {
  char Character = InString[Count];
  if( Character <= 0x7F )
    {
    // Regular ASCII.
    Result[Where] = (byte)Character;
    Where++;
    continue;
    }

  if( Character >= 0xD800 ) // High Surrogates
    {
    // Result[Where] = (byte)'#'; // Ignore anything above high surrogates.
    Where++;
    continue;
    }

  // "the first byte unambiguously indicates the length of the
  // sequence in bytes."
  // "All continuation bytes (byte nos. 26 in the table above) have
  // 10 as their two most-significant bits."

  // character "" = code point U+00A2
  // = 00000000 10100010
  //  11000010 10100010
  //  hexadecimal C2 A2
  // = 00000000 10 100010

  //  7  U+007F  0xxxxxxx
  // 11  U+07FF  110xxxxx  10xxxxxx
  // 16  U+FFFF  1110xxxx  10xxxxxx  10xxxxxx

  if( (Character > 0x7F) && (Character <= 0x7FF) )
    {
    // Notice that this conversion from characters to bytes
    // doesn't involve characters over 0x7F.
    byte SmallByte = (byte)(Character & 0x3F); // Bottom 6 bits.
    byte BigByte = (byte)((Character >> 6) & 0x1F); // Big 5 bits.

    BigByte |= 0xC0; // Mark it as the beginning byte.
    SmallByte |= 0x80; // Mark it as a continuing byte.
    Result[Where] = BigByte;
    Where++;
    Result[Where] = SmallByte;
    Where++;
    }


  // 16  U+FFFF  1110xxxx  10xxxxxx  10xxxxxx
  if( Character > 0x7FF ) // && (Character < 0xD800) )
    {
    byte Byte3 = (byte)(Character & 0x3F); // Bottom 6 bits.
    byte Byte2 = (byte)((Character >> 6) & 0x3F); // Next 6 bits.
    byte BigByte = (byte)((Character >> 12) & 0x0F); // Biggest 4 bits.

    BigByte |= 0xE0; // Mark it as the beginning byte.
    Byte2 |= 0x80; // Mark it as a continuing byte.
    Byte3 |= 0x80; // Mark it as a continuing byte.
    Result[Where] = BigByte;
    Where++;
    Result[Where] = Byte2;
    Where++;
    Result[Where] = Byte3;
    Where++;
    }
  }

Array.Resize( ref Result, Where );
return Result;
}



/*
  internal static string BytesToString( byte[] InBytes, int MaxLen )
    {
    // int Test = 1;
    try
    {
    if( InBytes == null )
      return "";

    if( InBytes.Length == 0 )
      return "";

    if( InBytes[0] == 0 )
      return "";

    if( MaxLen > InBytes.Length )
      MaxLen = InBytes.Length;

    // The constructor has a "suggested capacity" value to start with.
    StringBuilder SBuilder = new StringBuilder( MaxLen );
    // for( int Count = 0; Count < InBytes.Length; Count++ )
    for( int Count = 0; Count < MaxLen; Count++ )
      {
      if( InBytes[Count] == 0 )
        break;

      if( (InBytes[Count] & 0x80) == 0 )
        {
        // It's regular ASCII.
        SBuilder.Append( (char)InBytes[Count] );
        continue;
        }

      if( (InBytes[Count] & 0xC0) == 0x80 )
        {
        // It's a continuing byte that was already taken care of below
        // so skip it here.
        continue;
        }

      if( (InBytes[Count] & 0xC0) == 0xC0 )
        {
        // It's a beginning byte.
        // A beginning byte is either 110xxxxx or 1110xxxx.
        if( (InBytes[Count] & 0xF0) == 0xE0 )
          {
          // Starts with 1110xxxx.
          // It's a 3-byte character.
          if( (Count + 2) >= MaxLen )
            break; // Ignore the garbage.

          char BigByte = (char)(InBytes[Count] & 0x0F); // Biggest 4 bits.
          char Byte2 = (char)(InBytes[Count + 1] & 0x3F); // Next 6 bits.
          char Byte3 = (char)(InBytes[Count + 2] & 0x3F); // Next 6 bits.

          char Character = (char)(BigByte << 12 );
          Character |= (char)(Byte2 << 6);
          Character |= Byte3;

          if( Character < 0xD800 ) // High Surrogates
            SBuilder.Append( Character );

          }

        if( (InBytes[Count] & 0xE0) == 0xC0 )
          {
          // Starts with 110xxxxx.
          // It's a 2-byte character.
          if( (Count + 1) >= MaxLen )
            break; // return ""; // Ignore the garbage.

          char BigByte = (char)(InBytes[Count] & 0x1F); // Biggest 5 bits.
          char Byte2 = (char)(InBytes[Count + 1] & 0x3F); // Next 6 bits.

          char Character = (char)(BigByte << 6);
          Character |= Byte2;
          if( Character < 0xD800 ) // High Surrogates
            SBuilder.Append( Character );

          }

        // If it doesn't match the two above it gets ignored.
        }
      }

    string Result = SBuilder.ToString();
    if( Result == null )
      return "";

    return Result;

    }
    catch( Exception ) // Except )
      {
      // MessageBox.Show( "Error in UTF8Strings.BytesToString(). Test is: " + Test.ToString() + "\r\n" + Except.Message, MainForm.MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop );
      return "";
      }
    }


*/


} // Class
