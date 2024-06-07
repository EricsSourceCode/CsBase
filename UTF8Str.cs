// Copyright Eric Chauvin 2024.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



// See https://ericssourcecode.github.io/
// For guides and information.



using System;



// namespace



/*
UTF-8 (UCS Transformation Format -8)
UCS: Universal Character Set


One-byte ASCII:
7  U+007F  0xxxxxxx

Two-byte characters:
11  U+07FF  110xxxxx  10xxxxxx

All continuation bytes (byte nos. 26 in the table above) have 10 as their
two most-significant bits (bits 76); in contrast, the first byte never
has 10 as its two most-significant bits. As a result, it is immediately
obvious whether any given byte anywhere in a (valid) UTF8 stream represents
the first byte of a byte sequence corresponding to a single character, or
a continuation byte of such a byte sequence.

If the character is encoded by a sequence of more than one byte, the first
byte has as many leading "1" bits as the total number of bytes in the
sequence, followed by a "0" bit, and the succeeding bytes are all marked
by a leading "10" bit pattern.

So the first 128 characters (US-ASCII) need one byte. The next 1,920
characters need two bytes to encode. This includes Latin letters with
diacritics and characters from the Greek, Cyrillic, Coptic, Armenian,
Hebrew, Arabic, Syriac and Tna alphabets. Three bytes are needed for
the rest of the Basic Multilingual Plane (which contains virtually all
characters in common use). Four bytes are needed for characters in the
other planes of Unicode, which include less common CJK characters and
various historic scripts.

[Red cells from table] must never appear in a valid UTF-8 sequence. The
first two (C0 and C1) could only be used for overlong encoding of basic
ASCII characters. The remaining red cells indicate start bytes of sequences
that could only encode numbers larger than the 0x10FFFF limit of Unicode.
The byte 244 (hex 0xF4) could also encode some values greater than
0x10FFFF; such a sequence is also invalid.

Many earlier decoders would happily try to decode these. Carefully
crafted invalid UTF-8 could make them either skip or create ASCII
characters such as NUL, slash, or quotes. Invalid UTF-8 has been used to
bypass security validations in high profile products including Microsoft's
IIS web server[11] and Apache's Tomcat servlet container.

RFC 3629 states "Implementations of the decoding algorithm MUST protect
against decoding invalid sequences." The Unicode Standard requires
decoders to "...treat any ill-formed code unit sequence as an error
condition. This guarantees that it will neither interpret nor emit an
ill-formed code unit sequence."

Many Windows programs (including Windows Notepad) add the bytes 0xEF, 0xBB,
0xBF at the start of any document saved as UTF-8. This is the UTF-8 encoding
of the Unicode byte order mark (BOM), and is commonly referred to as a
UTF-8 BOM, even though it is not relevant to byte order. The BOM can
also appear if another encoding with a BOM is translated to UTF-8 without
stripping it.  The Unicode standard recommends against the BOM for UTF-8.

Sorting of UTF-8 strings as arrays of unsigned bytes will produce the
same results as sorting them based on Unicode code points.

If an odd number of bytes is missing from UTF-16, the whole rest of the
string will be meaningless text. Any bytes missing from UTF-8 will still
allow the text to be recovered accurately starting with the next
character after the missing bytes. If any partial character is removed
the corruption is always recognizable.

The first plane (code points U+0000 to U+FFFF) contains the most
frequently used characters and is called the Basic Multilingual Plane or BMP.

In practice most software defaults to little-endian,
Big Endian:    UTF-16BE
Little Endian: UTF-16LE

Java originally used UCS-2, and added UTF-16 supplementary character
support in J2SE 5.0. However, non-BMP characters require the individual
surrogate halves to be entered individually, for example: "\uD834\uDD1E"
for U+1D11E.

UTF-16 is used by the .NET environments; Mac OS X's Cocoa and Core
Foundation frameworks.

[Windows] functions use UTF-16 (wide character) encoding, which is the
most common encoding of Unicode and the one used for native Unicode
encoding on Windows operating systems.

The older UCS-2 (2-byte Universal Character Set) is a similar character
encoding that was superseded by UTF-16 in version 2.0 of the Unicode
standard in July 1996.

It produces a fixed-length format by simply using the code point as the
16-bit code unit and produces exactly the same result as UTF-16 for 96.9%
of all the code points in the range 0-0xFFFF, including all characters that
had been assigned a value at that time.

Basic Multilingual Plane
As of Unicode 6.0, the BMP comprises the following blocks:

    C0 Controls and Basic Latin (Basic Latin) (0000007F)
    C1 Controls and Latin-1 Supplement (008000FF)
    Latin Extended-A (0100017F)
    Latin Extended-B (0180024F)
    IPA Extensions (025002AF)
    Spacing Modifier Letters (02B002FF)
    Combining Diacritical Marks (0300036F)
    Greek and Coptic (037003FF)
    Cyrillic (040004FF)
    Cyrillic Supplement (0500052F)
    Armenian (0530058F)
    Hebrew (059005FF)
    Arabic (060006FF)
    Syriac (0700074F)
    Arabic Supplement (0750077F)
    Thaana (078007BF)
    NKo (07C007FF)
    Samaritan (0800083F)
    Mandaic (0840085F)

    Indic scripts:
        Devanagari (0900097F)
        Bengali (098009FF)
        Gurmukhi (0A000A7F)
        Gujarati (0A800AFF)
        Oriya (0B000B7F)
        Tamil (0B800BFF)
        Telugu (0C000C7F)
        Kannada (0C800CFF)
        Malayalam (0D000D7F)
        Sinhala (0D800DFF)

    Thai (0E000E7F)
    Lao (0E800EFF)
    Tibetan (0F000FFF)
    Myanmar (1000109F)
    Georgian (10A010FF)
    Hangul Jamo (110011FF)
    Ethiopic (1200137F)
    Ethiopic Supplement (1380139F)
    Cherokee (13A013FF)
    Unified Canadian Aboriginal Syllabics (1400167F)
    Ogham (1680169F)
    Runic (16A016FF)

    Philippine scripts:
        Tagalog (1700171F)
        Hanunoo (1720173F)
        Buhid (1740175F)
        Tagbanwa (1760177F)

    Khmer (178017FF)
    Mongolian (180018AF)
    Unified Canadian Aboriginal Syllabics Extended (18B018FF)
    Limbu (1900194F)
    Tai Le (1950197F)
    Tai Lue (198019DF)
    Khmer Symbols (19E019FF)
    Buginese (1A001A1F)
    Tai Tham (1A201AAF)

    Balinese (1B001B7F)
    Sundanese (1B801BBF)
    Batak (1BC01BFF)
    Lepcha (1C001C4F)
    Ol Chiki (1C501C7F)
    Vedic Extensions (1CD01CFF)
    Phonetic Extensions (1D001D7F)
    Phonetic Extensions Supplement (1D801DBF)
    Combining Diacritical Marks Supplement (1DC01DFF)
    Latin extended additional (1E001EFF)
    Greek Extended (1F001FFF)
    Symbols:
        General Punctuation (2000206F)
        Superscripts and Subscripts (2070209F)
        Currency Symbols (20A020CF)
        Combining Diacritical Marks for Symbols (20D020FF)
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
        Dingbats (270027BF)
        Miscellaneous Mathematical Symbols-A (27C027EF)
        Supplemental Arrows-A (27F027FF)
        Braille Patterns (280028FF)
        Supplemental Arrows-B (2900297F)
        Miscellaneous Mathematical Symbols-B (298029FF)
        Supplemental Mathematical Operators (2A002AFF)
        Miscellaneous Symbols and Arrows (2B002BFF)

    Glagolitic (2C002C5F)
    Latin Extended-C (2C602C7F)
    Coptic (2C802CFF)
    Georgian Supplement (2D002D2F)
    Tifinagh (2D302D7F)
    Ethiopic Extended (2D802DDF)
    Cyrillic Extended-A (2DE02DFF)
    Supplemental Punctuation (2E002E7F)
    East Asian scripts and symbols:
        CJK Radicals Supplement (2E802EFF)
        Kangxi Radicals (2F002FDF)
        Ideographic Description Characters (2FF02FFF)
        CJK Symbols and Punctuation (3000303F)
        Hiragana (3040309F)
        Katakana (30A030FF)
        Bopomofo (3100312F)
        Hangul Compatibility Jamo (3130318F)
        Kanbun (3190319F)
        Bopomofo Extended (31A031BF)
        CJK Strokes (31C031EF)
        Katakana Phonetic Extensions (31F031FF)
        Enclosed CJK Letters and Months (320032FF)
        CJK Compatibility (330033FF)
        CJK Unified Ideographs Extension A (34004DBF)
        Yijing Hexagram Symbols (4DC04DFF)
        CJK Unified Ideographs (4E009FFF)

    Yi Syllables (A000A48F)
    Yi Radicals (A490A4CF)
    Lisu (A4D0A4FF)
    Vai (A500A63F)
    Cyrillic Extended-B (A640A69F)
    Bamum (A6A0A6FF)
    Modifier Tone Letters (A700A71F)
    Latin Extended-D (A720A7FF)
    Syloti Nagri (A800A82F)
    Common Indic Number Forms (A830A83F)
    Phags-pa (A840A87F)
    Saurashtra (A880A8DF)
    Devanagari Extended (A8E0A8FF)
    Kayah Li (A900A92F)
    Rejang (A930A95F)
    Hangul Jamo Extended-A (A960A97F)
    Javanese (A980A9DF)
    Cham (AA00AA5F)
    Myanmar Extended-A (AA60AA7F)
    Tai Viet (AA80AADF)
    Ethiopic Extended-A (AB00AB2F)
    Meetei Mayek (ABC0ABFF)
    Hangul Syllables (AC00D7AF)
    Hangul Jamo Extended-B (D7B0D7FF)

    Surrogates:
        High Surrogates (D800DB7F)
        High Private Use Surrogates (DB80DBFF)
        Low Surrogates (DC00DFFF)

    Private Use Area (E000F8FF)

    CJK Compatibility Ideographs (F900FAFF)
    Alphabetic Presentation Forms (FB00FB4F)
    Arabic Presentation Forms-A (FB50FDFF)
    Variation Selectors (FE00FE0F)
    Vertical Forms (FE10FE1F)
    Combining Half Marks (FE20FE2F)
    CJK Compatibility Forms (FE30FE4F)
    Small Form Variants (FE50FE6F)
    Arabic Presentation Forms-B (FE70FEFF)
    Halfwidth and Fullwidth Forms (FF00FFEF)
    Specials (FFF0FFFF)

*/



  static class UTF8Strings
  {

  internal static byte[] StringToBytes( string InString )
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
