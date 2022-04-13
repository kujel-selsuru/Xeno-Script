using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XenoLib;

namespace Xenos
{
    public class Error
    {
        //public 
        public string errorName;
        public string details;
        public Position pos;
        /// <summary>
        /// Error constructor
        /// </summary>
        /// <param name="errorName">Name of error</param>
        /// <param name="details">Details of error</param>
        /// <param name="context">Context of error</param>
        /// <param name="fileName">File name of error</param>
        /// <param name="fileText">file text of error</param>
        /// <param name="line">Line of error</param>
        /// <param name="start">Start of error</param>
        /// <param name="end">End of error</param>
        public Error(string errorName, string details, Context context, 
            string fileName, string fileText, int line, int start, int end)
        {
            this.errorName = errorName;
            this.details = details;
            this.pos = new Position(context, fileName, fileText, start, line, end);
        }
        /// <summary>
        /// Returns info as a string
        /// </summary>
        /// <returns>String</returns>
        public string asString()
        {
            return pos.fileName + ":: " + errorName + ": " + details;
        }
        /// <summary>
        /// Generates a traceback for errors
        /// </summary>
        public void generateTraceback()
        {
            string result = "";
            Position position = new Position(pos);
            Context context = pos.context;

            Console.WriteLine("Traceback (most recent call last):");

            while (context != null)
            {
                result += "File: " + pos.fileName + " Line: " + pos.Line + " in " + context.displayName;
                position = context.parentEntryPos;
                context = context.parent;
                Console.WriteLine(result);
            }
        }
    }

    public class IllegalCharError : Error
    {
        //public

        public IllegalCharError(string details, Context context, string fileName, string fileText, 
            int line, int start, int end) : base("Illegal Char", details, 
                context, fileName, fileText, line, start, end)
        {

        }
    }

    public class ExpectedCharError : Error
    {
        //public

        public ExpectedCharError(string details, Context context, string fileName, string fileText,
            int line, int start, int end) : base("Expected Char: ", details,
                context, fileName, fileText, line, start, end)
        {

        }
    }

    public class IllegalSyntaxError : Error
    {
        //public

        public IllegalSyntaxError(string details, Context context, string fileName, string fileText,
            int line, int start, int end) : base("Illegal syntax error", details,
                context, fileName, fileText, line, start, end)
        {

        }
    }

    public class RunTimeError : Error
    {
        //public

        public RunTimeError(string details, Context context, string fileName, string fileText,
            int line, int start, int end) : base("Runtime error", details,
                context, fileName, fileText, line, start, end)
        {

        }
    }
}
