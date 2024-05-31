// Copyright Eric Chauvin 2024.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



using System;
// using System.Text;
// Application, MessageBox, etc.
using System.Windows.Forms;
using System.IO;



// namespace


public class SysIO
{

public SysIO()
{

}



internal bool directoryExists( string dirName )
{
try
{
return Directory.Exists( dirName );
}
catch( Exception )
  {
  MessageBox.Show(
        "Could not check\r\n" + dirName,
        "SysIO",
       MessageBoxButtons.OK);

  return false;
  }
}


internal void createDirectory( string dirName )
{
try
{
if( !Directory.Exists( dirName ))
  Directory.CreateDirectory( dirName );

}
catch( Exception )
  {
  MessageBox.Show(
        "Could not create\r\n" + dirName,
        "SysIO",
       MessageBoxButtons.OK);

  return;
  }
}


} // Class
