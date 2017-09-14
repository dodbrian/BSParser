using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BSParser.Data;

namespace BSParser.Writers
{
    public abstract class Writer
    {
		public abstract bool Write(StatementTable data);
    }
}
