// Copyright Eric Chauvin 2024.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



using System;
using System.Text;
// using System.Windows.Forms;
using System.IO;



// namespace


public class SysIO
{

public SysIO()
{

}



static internal bool directoryExists(
                              string dirName )
{
try
{
return Directory.Exists( dirName );
}
catch( Exception )
  {
  string showS =
    "SysIO could not check dir: " + dirName;

  throw new Exception( showS );
  }
}


static internal void createDirectory(
                            string dirName )
{
try
{
if( !Directory.Exists( dirName ))
  Directory.CreateDirectory( dirName );

}
catch( Exception )
  {
  string showS =
    "SysIO could not create dir: " + dirName;

  throw new Exception( showS );
  }
}



static internal bool fileExists( string inFile )
{
return File.Exists( inFile );
}


static internal void fileDelete( string inFile )
{
File.Delete( inFile );
}


static internal void fileCopy( string fromFile,
                               string toFile,
                               bool overWrite )
{
File.Copy( fromFile, toFile, overWrite );
}


static internal string readAllText(
                        string fromFile )
{
return File.ReadAllText( fromFile,
                         Encoding.UTF8 );
}



static internal void writeAllText(
                        string toFile,
                        string toWrite )
{
File.WriteAllText( toFile, toWrite,
                   Encoding.UTF8 );
}


} // Class
