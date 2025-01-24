// Copyright Eric Chauvin 2024 - 2025.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



using System;



// namespace



public class WordDct
{
private MainData mData;
private WordDctLine[] lineArray;
int highestIdNum = 0;
int minArIndex = 0;


private const int keySize = 0x3FF;



private WordDct()
{
}



internal WordDct( MainData useMainData )
{
mData = useMainData;

minArIndex = getArIndex( "aa" );

try
{
lineArray = new WordDctLine[keySize];
for( int count = 0; count < keySize; count++ )
  lineArray[count] = new WordDctLine( mData );

}
catch( Exception ) // Except )
  {
  throw new Exception(
           "Not enough memory for WordDct." );
  }
}



internal void clear()
{
for( int count = 0; count < keySize; count++ )
  lineArray[count].clear();

}



internal int getArIndex( string wordIn )
{
wordIn = Str.trim( wordIn );
wordIn = Str.toLower( wordIn );

int len = wordIn.Length;
if( len == 0 )
  return 0;

char char1 = wordIn[0];
char char2 = (char)96;  // Little 'a' is 97.

if( len >= 2 )
  char2 = wordIn[1];

// Little 'a' is 97.

if( char1 < 'a' )
  char1 = (char)96;

if( char2 < 'a' )
  char2 = (char)96;

if( char1 > 'z' )
  char1 = 'z';

if( char2 > 'z' )
  char2 = 'z';

// 'a' - 96 is 1.  So there can be a zero.
int index1 = char1 - 96;
int index2 = char2 - 96;

if( index1 > 27 )
  {
  throw new Exception(
         "This can't happen in WordDct." );
  }

if( index1 < 0 )
  {
  throw new Exception( "index1 < 0." );
  }

if( index2 < 0 )
  {
  throw new Exception( "index2 < 0." );
  }

// 5 bits.
// int mask = 16 + 8 + 4 + 2 + 1;
// if( mask != 0x1F )
//  throw new Exception( "Mask is not right." );

int index = index1;
index <<= 5;
index |= index2;

if( index < 0 )
  throw new Exception( "index < 0" );

if( index >= keySize )
  throw new Exception( "index is too big." );

// index = index & keySize;
// if( index == keySize )
//  throw new Exception( "index == keySize." );

return index;
}




internal void setValueAnyID( Word value )
{
try
{
string word = value.getWord();
if( word == null )
  return;

// See this in WordDctLine
// the, and, by, a ...
// if( word.Length < 4 )

int arIndex = getArIndex( word );

lineArray[arIndex].setValueAnyID( value );
}
catch( Exception )
  {
  throw new Exception(
       "WordDct Exception in setValue()." );
  }
}



internal void getValue( string word,
                        Word toGet )
{
toGet.clear();

if( word == null )
  return;

if( word.Length < 1 )
  return;

int arIndex = getArIndex( word );
if( lineArray[arIndex].getArrayLast() == 0 )
  return;

lineArray[arIndex].getValue( word, toGet );
}



internal bool keyExists( string word )
{
if( word == null )
  return false;

word = Str.trim( word );
if( word.Length < 1 )
  return false;

int arIndex = getArIndex( word );
if( lineArray[arIndex] == null )
  return false;

return lineArray[arIndex].keyExists( word );
}




internal void readAllFromFile( string fileName )
{
clear();

mData.showStatus( "Reading words file:" );
mData.showStatus( fileName );

if( !SysIO.directoryExists(
                  mData.getDataDirectory()))
  {
  mData.showStatus( "No data directory." );
  return;
  }

if( !SysIO.fileExists( fileName ))
  {
  mData.showStatus( "No words file." );
  return;
  }

string fileS = SysIO.readAllText( fileName );

StrAr lines = new StrAr();
lines.split( fileS, MarkersAI.WordListDelim );
int last = lines.getLast();

for( int count = 0; count < last; count++ )
  {
  if( (count % 100) == 0 )
    {
    mData.showStatus( "Reading words..." );
    if( !mData.checkEvents())
      return;

    }

  string line = lines.getStrAt( count );
  // line = Str.trim( line );

  Word word = new Word( mData, "" );
  if( !word.setFromString( line ))
    continue;

  string wordS = word.getWord();
  int idNum = word.getIdNum();
  if( highestIdNum < idNum )
    highestIdNum = idNum;

  // if( count > 50 )
    // break;

  int arIndex = getArIndex( word.getWord());
  if( arIndex >= minArIndex )
    lineArray[arIndex].setValueAnyID( word );

  }
}




internal void writeFileS( string fileName,
                          string toWrite )
{

if( !SysIO.directoryExists(
                  mData.getDataDirectory()))
  {
  mData.showStatus( "No data directory." );
  return;
  }

mData.showStatus( " " );
mData.showStatus( "Writing: " + fileName );

SysIO.writeAllText( fileName, toWrite );
}



internal void writeAllToFile( string fileName,
                              int minCount )
{
SBuilder sBuild = new SBuilder();

mData.showStatus( "Writing words to file." );

Word word = new Word( mData, "" );

int howMany = 0;
for( int count = 0; count < keySize; count++ )
  {
  if( (count % 100) == 0 )
    {
    if( !mData.checkEvents())
      return;

    }

  // if( howMany > 20 )
    // break;

  int last = lineArray[count].getArrayLast();
  if( last < 1 )
    continue;

  // mData.showStatus( "Last: " + last );
  for( int countR = 0; countR < last; countR++ )
    {
    lineArray[count].getCopyWordAt( word,
                                    countR );

    // It doesn't have a valid idNum.
    if( word.getIdNum() < 1 )
      {
      throw new Exception(
                "Word valid index on write." );
      }

    if( word.getCount() < minCount )
      continue;

    // ulong dateIndex = story.getDateIndex();
    // if( dateIndex < oldIndex )
      // continue;

    sBuild.appendStr( word.toString() +
                      MarkersAI.WordListDelim );

    howMany++;
    }
  }

string toWrite = sBuild.toString();
writeFileS( fileName, toWrite );

mData.showStatus( "Words: " + howMany );
mData.showStatus( "Finished writing file." );
}



/*
Break a sentence in to separate words and
add each word separately.

internal void addWordsLine( string line )
{
if( line == null )
  return;

line = Str.toLower( line );
line = Str.trim( line );

if( line.Length < 1 )
  return;

StrAr words = new StrAr();
words.split( line, ' ' );
int last = words.getLast();

for( int count = 0; count < last; count++ )
  {
  string word = words.getStrAt( count );
  word = removePunctuation( word );
  word = Str.trim( word );
  if( word.Length < 1 )
    continue;

  if( isBadWord( word ))
    continue;

  addWord( word );
  }
}
*/




/*
internal string removePunctuation( string word )
{
string result = "";

if( Str.endsWith( word, "\'" ))
  word = Str.replace( word, "\'", "" );

// if( Str.startsWith( word, "\'" ))
  // word = Str.replace( word, "\'", "" );

int last = word.Length;
for( int count = 0; count < last; count++ )
  {
  char letter = word[count];
  if( letter == '.' )
    continue;

  if( letter == ',' )
    continue;

  if( letter == ';' )
    continue;

  if( letter == ':' )
    continue;

  if( letter == '!' )
    continue;

  if( letter == '?' )
    continue;

  if( letter == '(' )
    continue;

  if( letter == ')' )
    continue;

  if( letter == '[' )
    continue;

  if( letter == ']' )
    continue;

  if( letter == '{' )
    continue;

  if( letter == '}' )
    continue;


  if( letter == '$' )
    continue;

  // For AI it would matter that these stay
  // like they are.
  // '  you've
  // digital's word ends with: 's
  // walz's  china's
  // And AI needs the context of the whole
  // word, like provided, and not just provide.
  // delayed means something different from
  // delay.


  // Ampersand is like this: &#8217;
  // Not just this: #2024
  if( letter == '#' )
    continue;

  // These are really one word.
  // opt-out
  // real-time

  if( letter == '\"' )
    continue;

  // 0 to 9
  if( (letter >= '0') && (letter <= '9') )
    continue;

  result += word[count];
  }

return result;
}
*/



/*
internal bool isBadWord( string word )
{
// An email address:
if( Str.contains( word, "@" ))
  return true;

return false;
}
*/



internal void addWord( string wordS )
{
if( wordS == null )
  return;

wordS = Str.toLower( wordS );
wordS = Str.trim( wordS );
if( wordS.Length < 1 )
  return;

int arIndex = getArIndex( wordS );
// if( arIndex < minArIndex )
  // return;

if( lineArray[arIndex].keyExists( wordS ))
  {
  lineArray[arIndex].incCount( wordS );
  return;
  }

// mData.showStatus( "New word: " + wordS );

Word toAdd = new Word( mData, wordS );
toAdd.setCount( 1 );

// It is at the highest idNum it found,
// so add one to make a new higher idNum
// than what is in the data.
highestIdNum++;
toAdd.setIdNum( highestIdNum );
setValueAnyID( toAdd );
}




internal void checkUniqueID()
{
mData.showStatus( "Unique check." );

Word word = new Word( mData, "" );
Word word2 = new Word( mData, "" );

for( int count = 0; count < keySize; count++ )
  {
  // mData.showStatus(
  //          "Unique count: " + count );

  if( (count % 20) == 0 )
    {
    if( !mData.checkEvents())
      return;

    }

  int last = lineArray[count].getArrayLast();
  if( last < 1 )
    continue;

  for( int countR = 0; countR < last; countR++ )
    {
    lineArray[count].getCopyWordAt( word,
                                    countR );
    int idNum1 = word.getIdNum();
    if( idNum1 < 1 )
      throw new Exception( "idNum1 < 1" );

    // Start on the next row at count.
    // This misses any duplicates in row count.
    for( int count2 = count + 1;
                    count2 < keySize; count2++ )
      {
      int last2 = lineArray[count2].
                               getArrayLast();
        {
        if( last2 < 1 )
          continue;

        for( int countR2 = 0; countR2 < last2;
                                    countR2++ )
          {
          lineArray[count2].getCopyWordAt(
                              word2, countR2 );
          int idNum2 = word2.getIdNum();
          if( idNum2 < 1 )
            throw new Exception( "idNum2 < 1" );

          if( idNum1 == idNum2 )
            {
            throw new Exception(
                "idNum1 == idNum2 " + idNum1 );
            }
          }
        }
      }
    }
  }

mData.showStatus( "Finished Unique check." );
}




internal void sortWords()
{
mData.showStatus( "Sorting words." );

for( int count = 0; count < keySize; count++ )
  {
  if( (count % 100) == 0 )
    {
    if( !mData.checkEvents())
      return;

    }

  int last = lineArray[count].getArrayLast();
  if( last < 1 )
    continue;

  lineArray[count].sortByWord();
  }

mData.showStatus( "Finished sorting words." );
}




internal void showSortedWords()
{
sortWords();
mData.showStatus( " " );
mData.showStatus( " " );

SBuilder sBuild = new SBuilder();

Word word = new Word( mData, "" );

int howMany = 0;
for( int count = 0; count < keySize; count++ )
  {
  // mData.showStatus( " " );
  // mData.showStatus( "Count: " + count );
  // mData.showStatus( " " );

  if( (count % 100) == 0 )
    {
    if( !mData.checkEvents())
      return;

    }

  // if( howMany > 20 )
    // break;

  int last = lineArray[count].getArrayLast();
  if( last < 1 )
    continue;

  // mData.showStatus( "Last: " + last );
  for( int countR = 0; countR < last; countR++ )
    {
    lineArray[count].getCopySortedWordAt(
                               word, countR );

    // ulong dateIndex = story.getDateIndex();
    // if( dateIndex < oldIndex )
      // continue;

    mData.showStatus( word.getWord()//  +
                      // ": " + word.getIdNum() +
                      // ": " + word.getCount()
                      );
    howMany++;
    }
  }

mData.showStatus( " " );
mData.showStatus( "Words: " + howMany );
}



internal void showSortByCount( int minCount )
{
mData.showStatus( " " );
mData.showStatus( "Sorting by count." );

WordDctLine sortLineArray = new WordDctLine(
                                      mData );

Word word = new Word( mData, "" );

for( int count = 0; count < keySize; count++ )
  {
  if( (count % 20) == 0 )
    {
    if( !mData.checkEvents())
      return;

    }

  // if( howMany > 20 )
    // break;

  int last = lineArray[count].getArrayLast();
  if( last < 1 )
    continue;

  // mData.showStatus( "Last: " + last );
  for( int countR = 0; countR < last; countR++ )
    {
    lineArray[count].getCopyWordAt(
                               word, countR );

    if( word.getCount() >= minCount )
      sortLineArray.setValueAnyID( word );

    }
  }

sortLineArray.sortByCount();

int lastS = sortLineArray.getArrayLast();
for( int count = 0; count < lastS; count++ )
  {
  sortLineArray.getCopySortedWordAt(
                             word, count );

  mData.showStatus( "" + (count + 1) + ") " +
                    word.getWord() + ": " +
                    word.getCount());
  }

mData.showStatus( "Finished sort by count." );
}




} // Class
