using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;


namespace KSH_Lib.Util
{
	public class CSVWriter : IDisposable
	{
		/*--- Constructor ---*/
		public CSVWriter( Stream stream )
		{
			writer = new StreamWriter( stream );
		}
		public CSVWriter( string path )
		{
			writer = new StreamWriter( path );
		}

		/*--- Public Fields ---*/
		public char Delimiter
		{
			get { return specialChars[DelimiterIdx]; }
			set { specialChars[DelimiterIdx] = value; }
		}
		public char Quote
		{
			get { return specialChars[QuoteIdx]; }
			set { specialChars[QuoteIdx] = value; }
		}


		/*--- Private Fields ---*/
		char[] specialChars = new char[] { ',', '"', '\r', '\n' };
		const int DelimiterIdx = 0;
		const int QuoteIdx = 1;

		StreamWriter writer;
		string oneQuote = null;
		string twoQuotes = null;
		string quotedFormat = null;


		/*--- Public Methods ---*/
		public void WriteRow( List<string> col )
		{
			if ( col == null )
			{
				throw new ArgumentNullException( "columns" );
			}
			if ( oneQuote == null || oneQuote[0] != Quote )
			{
				oneQuote = String.Format( "{0}", Quote );
				twoQuotes = String.Format( "{0}{0}", Quote );
				quotedFormat = String.Format( "{0}{{0}}{0}", Quote );
			}

			for ( int i = 0; i < col.Count; i++ )
			{
				if ( i > 0 )
					writer.Write( Delimiter );
				if ( col[i].IndexOfAny( specialChars ) == -1 )
					writer.Write( col[i] );
				else
					writer.Write( quotedFormat, col[i].Replace( oneQuote, twoQuotes ) );
			}
			writer.WriteLine();
		}
		public void Dispose()
		{
			writer.Dispose();
		}
	}
}