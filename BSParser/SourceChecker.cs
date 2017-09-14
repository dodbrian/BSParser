using BSParser.Readers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BSParser
{
    public class SourceChecker
    {
        protected List<Reader> Readers;

        public bool Init()
        {
            Readers = new List<Reader>
                {
                    new RosbankStatementReader(),
                    new VTB24StatementReader()
                };

            return true;
        }

        /// <summary>
        /// Detects appropriate Reader for a data file
        /// </summary>
        /// <param name="fileName">Path to a data file</param>
        /// <returns>Reader or null if no appropriate Reader exists for a given file</returns>
        public Reader Detect(String fileName)
        {
            if (Readers == null)
                Init();

            return Readers?.FirstOrDefault(reader => reader.Test(fileName));
        }
    }
}
