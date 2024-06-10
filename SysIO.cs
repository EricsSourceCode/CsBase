// Copyright Eric Chauvin 2024.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



using System;
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


} // Class
