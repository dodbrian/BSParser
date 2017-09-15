using System;
using BSParser.Data;

namespace BSParser.Readers
{
    public enum ReaderType
    {
        RosbankXML,
        VTB24,
        VTB242017,
        Unknown
    }

    public abstract class Reader : IDisposable
    {
        /// <summary>
        /// Source data file name
        /// </summary>
        protected String fileName;

        public bool Eof { get; protected set; }

        public ReaderType Type { get; protected set; }

        /// <summary>
        /// Reads data from source data file
        /// </summary>
        /// <returns></returns>
        public abstract StatementRow Read();

        /// <summary>
        /// Creates an uninitialized Reader object
        /// </summary>
        public Reader()
        {
            Eof = true;
        }

        /// <summary>
        /// Creates a Reader object using source data filename
        /// </summary>
        /// <param name="fileName">Name of a source data file</param>
        public Reader(String fileName)
        {
            Eof = true;
            this.fileName = fileName;
        }

        /// <summary>
        /// Checks if the format of a data file is correct
        /// </summary>
        /// <returns>Returns <c>true</c> if format is correct</returns>
        public abstract bool Test();

        public abstract bool Test(string fileName);

        #region IDisposable Members

        public abstract void Dispose();

        #endregion
    }
}
