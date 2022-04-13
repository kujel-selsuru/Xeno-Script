using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenos
{
    public class Val
    {
        //protected
        protected string value;
        protected DATATYPES dt;
        protected Position start;
        protected Position end;
        protected Context context;
        protected Error error;

        //public
        public Val(string value, DATATYPES dt, Position
            start = null, Position end = null, Context context = null)
        {
            this.value = value;
            this.dt = dt;
            switch (dt)
            {
                case DATATYPES.DT_INT:
                case DATATYPES.DT_FLOAT:
                case DATATYPES.DT_STRING:
                    break;
                default:
                    this.dt = DATATYPES.DT_INT;
                    break;
            }
            this.start = start;
            this.end = end;
            setContext(context);
            error = null;
        }
        public Val(string value, Position
            start = null, Position end = null, Context context = null)
        {
            this.value = value;
            if (value.Contains(Constants.LETTERS) == true)
            {
                dt = DATATYPES.DT_STRING;
            }
            else if (value.Contains(Constants.DIGITS))
            {
                if (value.Contains('.') == true)
                {
                    dt = DATATYPES.DT_FLOAT;
                }
                else
                {
                    dt = DATATYPES.DT_INT;
                }
            }
            this.start = start;
            this.end = end;
            setContext(context);
            error = null;
        }
        public Val(Val obj)
        {
            this.value = obj.value;
            this.dt = obj.dt;
            this.start = new Position(obj.start);
            this.end = new Position(obj.end);
            this.context = new Context(obj.context);
            this.error = obj.error;
        }
        /// <summary>
        /// Add a number to self and return result
        /// </summary>
        /// <param name="num">Number reference</param>
        /// <returns>Number object</returns>
        public Number addTo(Number num, Context context)
        {
            if (num.legalOperation(num.DT) == false)
            {
                error = new RunTimeError("Illegal operation", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX);
                return null;
            }
            Number tmp = new Number("", dt, start, end, context);
            if (dt == DATATYPES.DT_INT)
            {
                int i1 = Convert.ToInt32(value);
                int i2 = Convert.ToInt32(num.Value);
                int i3 = i1 + i2;
                tmp.value = i3.ToString();
            }
            else
            {
                float f1 = (float)Convert.ToDecimal(value);
                float f2 = (float)Convert.ToDecimal(num.Value);
                float f3 = f1 + f2;
                tmp.value = f3.ToString();
            }
            return tmp;
        }
        /// <summary>
        /// Subtract a number from self and return result
        /// </summary>
        /// <param name="num">Number reference</param>
        /// <returns>Number object</returns>
        public Number subBy(Number num, Context context)
        {
            if (num.legalOperation(num.DT) == false)
            {
                error = new RunTimeError("Illegal operation", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX);
                return null;
            }
            Number tmp = new Number("", dt, start, end, context);
            if (dt == DATATYPES.DT_INT)
            {
                int i1 = Convert.ToInt32(value);
                int i2 = Convert.ToInt32(num.Value);
                int i3 = i1 + i2;
                tmp.value = i3.ToString();
            }
            else
            {
                float f1 = (float)Convert.ToDecimal(value);
                float f2 = (float)Convert.ToDecimal(num.Value);
                float f3 = f1 - f2;
                tmp.value = f3.ToString();
            }
            return tmp;
        }
        /// <summary>
        /// Multiply a number by self and return result
        /// </summary>
        /// <param name="num">Number reference</param>
        /// <returns>Number object</returns>
        public Number mulBy(Number num, Context context)
        {
            if (num.legalOperation(num.DT) == false)
            {
                error = new RunTimeError("Illegal operation", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX);
                return null;
            }
            Number tmp = new Number("", dt, start, end, context);
            if (dt == DATATYPES.DT_INT)
            {
                int i1 = Convert.ToInt32(value);
                int i2 = Convert.ToInt32(num.Value);
                int i3 = i1 * i2;
                tmp.value = i3.ToString();
            }
            else
            {
                float f1 = (float)Convert.ToDecimal(value);
                float f2 = (float)Convert.ToDecimal(num.Value);
                float f3 = f1 * f2;
                tmp.value = f3.ToString();
            }
            return tmp;
        }
        /// <summary>
        /// Divide a number by self and return result
        /// </summary>
        /// <param name="num">Number reference</param>
        /// <returns>Number object</returns>
        public Number divBy(Number num, Context context)
        {
            if (num.legalOperation(num.DT) == false)
            {
                error = new RunTimeError("Illegal operation", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX);
                return null;
            }
            Number tmp = new Number("", dt, start, end, context);
            if (dt == DATATYPES.DT_INT)
            {
                int i1 = Convert.ToInt32(value);
                int i2 = Convert.ToInt32(num.Value);
                int i3 = i1 / i2;
                tmp.value = i3.ToString();
            }
            else
            {
                float f1 = (float)Convert.ToDecimal(value);
                float f2 = (float)Convert.ToDecimal(num.Value);
                float f3 = f1 / f2;
                tmp.value = f3.ToString();
            }
            return tmp;
        }
        /// <summary>
        /// Performs power operation on a number by value and 
        /// returns result
        /// </summary>
        /// <param name="num">Power value</param>
        /// <param name="context">Context reference</param>
        /// <returns>Number object</returns>
        public Number powBy(Number num, Context context)
        {
            if (num.legalOperation(num.DT) == false)
            {
                error = new RunTimeError("Illegal operation", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX);
                return null;
            }
            Number tmp = new Number("", dt, start, end, context);
            if (dt == DATATYPES.DT_INT)
            {
                int i1 = Convert.ToInt32(value);
                int i2 = Convert.ToInt32(num.Value);
                int i3 = (int)Math.Pow(i1, i2);
                tmp.value = i3.ToString();
            }
            else
            {
                float f1 = (float)Convert.ToDecimal(value);
                int f2 = Convert.ToInt32(num.Value);
                float f3 = (float)Math.Pow(f1, f2);
                tmp.value = f3.ToString();
            }
            return tmp;
        }
        /// <summary>
        /// Negate number and return result
        /// </summary>
        /// <returns>Number object</returns>
        public Number negate(Context context)
        {
            Number tmp = new Number("", dt, start, end, context);
            if (dt == DATATYPES.DT_INT)
            {
                int i1 = Convert.ToInt32(value);
                int i2 = -1;
                int i3 = i1 * i2;
                tmp.value = i3.ToString();
            }
            else
            {
                float f1 = (float)Convert.ToDecimal(value);
                float f2 = -1;
                float f3 = f1 * f2;
                tmp.value = f3.ToString();
            }
            return tmp;
        }
        /// <summary>
        /// Compares if number is equal to another number and returns
        /// a new Number valued at either 1 for true or 0 for false
        /// </summary>
        /// <param name="other">Number reference</param>
        /// <returns>Number object</returns>
        public Number getCompareEQ(Number other)
        {
            if (other.legalOperation(other.DT) == false)
            {
                error = new RunTimeError("Illegal operation", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX);
                return null;
            }
            Number result = new Number("1", new Position(start),
                new Position(end), context);
            if (value == other.Value)
            {
                return result;
            }
            result.Value = "0";
            return result;
        }
        /// <summary>
        /// Compares if number is not equal to another number and returns
        /// a new Number valued at either 1 for true or 0 for false
        /// </summary>
        /// <param name="other">Number reference</param>
        /// <returns>Number object</returns>
        public Number getCompareNE(Number other)
        {
            if (other.legalOperation(other.DT) == false)
            {
                error = new RunTimeError("Illegal operation", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX);
                return null;
            }
            Number result = new Number("1", new Position(start),
                new Position(end), context);
            if (value != other.Value)
            {
                return result;
            }
            result.Value = "0";
            return result;
        }
        /// <summary>
        /// Compares if number is less than another number and returns
        /// a new Number valued at either 1 for true or 0 for false
        /// </summary>
        /// <param name="other">Number reference</param>
        /// <returns>Number object</returns>
        public Number getCompareLT(Number other)
        {
            if (other.legalOperation(other.DT) == false)
            {
                error = new RunTimeError("Illegal operation", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX);
                return null;
            }
            Number result = new Number("1", new Position(start),
                new Position(end), context);
            if (dt == DATATYPES.DT_INT)
            {
                if (valueAsInt() < other.valueAsInt())
                {
                    return result;
                }
            }
            else
            {
                if (valueAsFloat() < other.valueAsFloat())
                {
                    return result;
                }
            }
            result.Value = "0";
            return result;
        }
        /// <summary>
        /// Compares if number is less than or equal another 
        /// number and returns a new Number valued at either 
        /// 1 for true or 0 for false
        /// </summary>
        /// <param name="other">Number reference</param>
        /// <returns>Number object</returns>
        public Number getCompareLTE(Number other)
        {
            if (other.legalOperation(other.DT) == false)
            {
                error = new RunTimeError("Illegal operation", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX);
                return null;
            }
            Number result = new Number("1", new Position(start),
                new Position(end), context);
            if (dt == DATATYPES.DT_INT)
            {
                if (valueAsInt() <= other.valueAsInt())
                {
                    return result;
                }
            }
            else
            {
                if (valueAsFloat() <= other.valueAsFloat())
                {
                    return result;
                }
            }
            result.Value = "0";
            return result;
        }
        /// <summary>
        /// Compares if number is greater than another number and 
        /// returns a new Number valued at either 1 for true or 
        /// 0 for false
        /// </summary>
        /// <param name="other">Number reference</param>
        /// <returns>Number object</returns>
        public Number getCompareGT(Number other)
        {
            if (other.legalOperation(other.DT) == false)
            {
                error = new RunTimeError("Illegal operation", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX);
                return null;
            }
            Number result = new Number("1", new Position(start),
                new Position(end), context);
            if (dt == DATATYPES.DT_INT)
            {
                if (valueAsInt() > other.valueAsInt())
                {
                    return result;
                }
            }
            else
            {
                if (valueAsFloat() > other.valueAsFloat())
                {
                    return result;
                }
            }
            result.Value = "0";
            return result;
        }
        /// <summary>
        /// Compares if number is greater than or equal another 
        /// number and returns a new Number valued at either 
        /// 1 for true or 0 for false
        /// </summary>
        /// <param name="other">Number reference</param>
        /// <returns>Number object</returns>
        public Number getCompareGTE(Number other)
        {
            if (other.legalOperation(other.DT) == false)
            {
                error = new RunTimeError("Illegal operation", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX);
                return null;
            }
            Number result = new Number("1", new Position(start),
                new Position(end), context);
            if (dt == DATATYPES.DT_INT)
            {
                if (valueAsInt() <= other.valueAsInt())
                {
                    return result;
                }
            }
            else
            {
                if (valueAsFloat() <= other.valueAsFloat())
                {
                    return result;
                }
            }
            result.Value = "0";
            return result;
        }
        /// <summary>
        /// Ands a number with another number
        /// </summary>
        /// <param name="other">Number reference</param>
        /// <returns>Number object</returns>
        public Number andedBy(Number other)
        {
            if (other.legalOperation(other.DT) == false)
            {
                error = new RunTimeError("Illegal operation", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX);
                return null;
            }
            Number result = new Number("1", new Position(start),
                new Position(end), context);
            if (value == "1" && other.Value == "1")
            {
                return result;
            }
            else if (value == "0" && other.Value == "0")
            {
                return result;
            }
            result.Value = "0";
            return result;
        }
        /// <summary>
        /// Ors a number with another number
        /// </summary>
        /// <param name="other">Number reference</param>
        /// <returns>Number object</returns>
        public Number oredBy(Number other)
        {
            if (other.legalOperation(other.DT) == false)
            {
                error = new RunTimeError("Illegal operation", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX);
                return null;
            }
            Number result = new Number("1", new Position(start),
                new Position(end), context);
            if (value == "1" && other.Value == "0")
            {
                return result;
            }
            else if (value == "0" && other.Value == "1")
            {
                return result;
            }
            else if (value == "1" && other.Value == "1")
            {
                return result;
            }
            result.Value = "0";
            return result;
        }
        /// <summary>
        /// Returns a value of zero if number's value is one or greater
        /// and a value of one if number's value is zero;
        /// </summary>
        /// <returns>Number object</returns>
        public Number notted()
        {
            Number result = new Number(value, new Position(start),
                new Position(end), context);
            if (value == "1")
            {
                result.Value = "0";
            }
            else if (value == "0")
            {
                result.Value = "1";
            }
            else
            {
                result.Value = "0";
            }
            return result;
        }
        /// <summary>
        /// Returns true if value is greater then or equal to one
        /// </summary>
        /// <returns>Boolean</returns>
        public bool isTrue()
        {
            if (valueAsInt() >= 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if a value is negative
        /// </summary>
        /// <returns>Boolean</returns>
        public bool isNeg()
        {
            if (valueAsFloat() < 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Set number's position
        /// </summary>
        /// <param name="start">Start Position</param>
        /// <param name="end">End Position</param>
        public void setPos(Position start, Position end)
        {
            this.start = start;
            this.end = end;
        }
        /// <summary>
        /// Sets Number Context value
        /// </summary>
        /// <param name="context">Context reference</param>
        public void setContext(Context context = null)
        {
            this.context = context;
        }
        /// <summary>
        /// Returns value as an integer
        /// </summary>
        /// <returns>Integer</returns>
        public int valueAsInt()
        {
            return Convert.ToInt32(value);
        }
        /// <summary>
        /// Returns value as a float
        /// </summary>
        /// <returns>Float</returns>
        public float valueAsFloat()
        {
            return (float)Convert.ToDecimal(value);
        }
        /// <summary>
        /// Returns value as a number object
        /// </summary>
        /// <returns></returns>
        public Number valueAsNumber()
        {
            return new Number(value, start, end, context);
        }
        /// <summary>
        /// Prints number to console window
        /// </summary>
        public virtual void print()
        {
            Console.Write(value);
        }
        /// <summary>
        /// Checks that value types for operation is legal
        /// </summary>
        /// <param name="otherDT">DATATYPES value</param>
        /// <returns>Boolean</returns>
        public bool legalOperation(DATATYPES otherDT)
        {
            switch (dt)
            {
                case DATATYPES.DT_INT:
                case DATATYPES.DT_FLOAT:
                    if (otherDT == DATATYPES.DT_INT ||
                        otherDT == DATATYPES.DT_FLOAT)
                    {
                        return true;
                    }
                    return false;
                case DATATYPES.DT_STRING:
                    if (otherDT == DATATYPES.DT_STRING)
                    {
                        return true;
                    }
                    return false;
            }
            return false;
        }
        /// <summary>
        /// Virtual execute method
        /// </summary>
        /// <param name="args">List of SyntaxNodes</param>
        /// <returns>Val object</returns>
        public virtual RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            return null;
        }
        /// <summary>
        /// Value property
        /// </summary>
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
        /// <summary>
        /// DT property
        /// </summary>
        public DATATYPES DT
        {
            get { return dt; }
            set
            {
                dt = value;
                switch (dt)
                {
                    case DATATYPES.DT_INT:
                    case DATATYPES.DT_FLOAT:
                        break;
                    default:
                        dt = DATATYPES.DT_INT;
                        break;
                }
            }
        }
        /// <summary>
        /// Start property
        /// </summary>
        public Position Start
        {
            get { return start; }
            set { start = value; }
        }
        /// <summary>
        /// End property
        /// </summary>
        public Position End
        {
            get { return end; }
            set { end = value; }
        }
        /// <summary>
        /// Context property
        /// </summary>
        public Context Cont
        {
            get { return context; }
        }
    }

    public class Number : Val
    {
        //protected

        //public
        public Number(string value, DATATYPES dt, Position
            start = null, Position end = null, Context context = null) :
            base(value, dt, start, end, context)
        {

        }

        public Number(string value, Position start = null,
            Position end = null, Context context = null) :
            base(value, DATATYPES.DT_INT, start, end, context)
        {
            this.value = value;
            this.start = start;
            this.end = end;
            setContext(context);
        }

        public Number(Number obj) :
            base(obj)
        {

        }

        public static Number Null
        {
            get { return new Number("0"); }
        }

        public static Number False
        {
            get { return new Number("0"); }
        }

        public static Number True
        {
            get { return new Number("1"); }
        }
    }

    public class String : Val
    {
        public String(string value, Position start, Position end,
            Context context) :
            base(value, DATATYPES.DT_STRING, start, end, context)
        {

        }

        public String(String obj) : base(obj)
        {

        }
        /// <summary>
        /// Adds another string to this object's value and returns the result
        /// </summary>
        /// <param name="str"></param>
        /// <returns>String object</returns>
        public String addStr(String str)
        {
            return new String(value + str.Value, start, str.End, context);
        }
        /// <summary>
        /// Returns true if the other string's value is the same 
        /// else returns false
        /// </summary>
        /// <param name="str">String reference</param>
        /// <returns>Number object</returns>
        public Number compare(String str)
        {
            if (value.Length != str.Value.Length)
            {
                return new Number("0", start, end, context);
            }
            for (int i = 0; i < str.Value.Length; i++)
            {
                if (str.Value[i] != value[i])
                {
                    return new Number("0", start, end, context);
                }
            }
            return new Number("1", start, end, context);
        }
        /// <summary>
        /// Returns a string made of X copies of value
        /// </summary>
        /// <param name="num">Number of copies of value to add</param>
        /// <returns>String object</returns>
        public String mulStr(Number num)
        {
            String str = new String("", start, end, context);
            for (int i = 0; i < num.valueAsInt(); i++)
            {
                str = str.addStr(this);
            }
            return str;
        }
        /// <summary>
        /// Returns true if value length is greater than 0
        /// </summary>
        /// <returns>Boolean</returns>
        public new bool isTrue()
        {
            if (value.Length > 0)
            {
                return true;
            }
            return false;
        }
    }

    public class XenoFile : Val
    {
        protected string fileName;
        protected bool isOpen;
        protected bool fileNotFound;
        protected int lineIndex;
        protected System.IO.StreamReader sr;
        protected System.IO.StreamWriter sw;

        public XenoFile(string value, Position start, Position end,
            Context context) :
            base(value, DATATYPES.DT_STRING, start, end, context)
        {
            fileName = value;
            isOpen = false;
            fileNotFound = false;
            lineIndex = 0;
            sr = null;
            sw = null;
        }

        public XenoFile(XenoFile obj) : base(obj)
        {
            this.fileName = obj.FileName;
            this.isOpen = obj.IsOpen;
            this.fileNotFound = obj.FileNotFound;
            this.lineIndex = obj.LineIndex;
            this.sr = obj.SR;
            this.sw = obj.SW;
        }
        
        public void open(string fileName)
        {
            reset();
            if(System.IO.File.Exists(fileName) == false)
            {
                fileNotFound = true;
            }
            else
            {
                isOpen = true;
            }
        }

        public void close()
        {
            if(sr != null)
            {
                sr.Close();
            }
            if (sw != null)
            {
                sw.Close();
            }
        }

        public String readLine()
        {
            String line = new String("", start, end, context);
            if (isOpen == true)
            {
                line.Value = sr.ReadLine();
                lineIndex++;
            }
            return line;
        }

        public void writeLine(String line)
        {
            if (isOpen == true)
            {
                sw.WriteLine(line.Value);
                lineIndex++;
            }
        }

        public void writeLine(Number line)
        {
            if (isOpen == true)
            {
                sw.WriteLine(line.Value);
                lineIndex++;
            }
        }

        public void reset()
        {
            fileName = "";
            isOpen = false;
            fileNotFound = false;
            sr = null;
            sw = null;
            lineIndex = 0;
        }

        public string FileName
        {
            get { return fileName; }
        }

        public bool IsOpen
        {
            get { return isOpen; }
        }

        public bool FileNotFound
        {
            get { return fileNotFound; }
        }

        public System.IO.StreamReader SR
        {
            get { return sr; }
        }

        public System.IO.StreamWriter SW
        {
            get { return sw; }
        }

        public int LineIndex
        {
            get { return lineIndex; }
        }
    }

    public class BaseFunction : Val
    {
        //protected
        protected Token nameToken;
        protected SyntaxNode bodyNodes;
        protected List<string> argNames;
        protected bool shouldAutoReturn;
        //public
        public BaseFunction(Token nameToken, SyntaxNode bodyNodes,
            List<string> argNames, DATATYPES dt,
            Position start, Position end, Context context, bool shouldAutoReturn = false) :
            base(nameToken.value, dt, start, end, context)
        {
            this.nameToken = nameToken;
            this.bodyNodes = bodyNodes;
            this.argNames = new List<string>();
            for(int i = 0; i < argNames.Count; i++)
            {
                this.argNames.Add(argNames[i]);
            }
            this.shouldAutoReturn = shouldAutoReturn;
            dt = DATATYPES.DT_FUNCTION;
        }

        public BaseFunction(BaseFunction obj) : base(obj)
        {
            this.nameToken = obj.NameToken;
            this.bodyNodes = obj.BodyNodes;
            this.argNames = obj.ArgNames;
            this.shouldAutoReturn = obj.ShouldAutoReturn;
        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();

            SymbolTable symbols = setSymbols(context);
            Context newContext = generateNewContext(symbols);

            RunTimeResult popResult = checkAndPopulateArgs(argNames, args, newContext);
            if (popResult.error != null)
            {
                return popResult;
            }

            result = result.register(Interpreter.visitNode(bodyNodes, newContext));
            Val retValue = null;
            if(result.shouldReturn() == true && 
                result.returnValue == null)
            {
                return result;
            }
            if (result.shouldReturn() == true &&
                result.returnValue != null)
            {
                retValue = result.returnValue;
            }
            else
            {
                retValue = Number.Null;
            }
            return result.success(retValue);
        }

        public Context generateNewContext(SymbolTable symbols)
        {
            return new Context(nameToken.value, symbols, context, start);
        }

        public RunTimeResult checkArgs(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            if (args.Count > ArgNames.Count)
            {
                string errorDets = (ArgNames.Count - args.Count) +
                    " too many arguments passed into " +
                    nameToken.value;
                RunTimeError error = new RunTimeError(errorDets, context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX);
                return result.failure(error);
            }
            if (args.Count < ArgNames.Count)
            {
                string errorDets = (args.Count - ArgNames.Count) +
                    " too few arguments passed into " +
                    nameToken.value;
                RunTimeError error = new RunTimeError(errorDets, context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX);
                return result.failure(error);
            }
            return result.success(null);
        }

        public void populateArgs(List<string> argNames, List<Val> args, Context newContext)
        {
            string argName = "";
            Val argVal = null;
            for (int i = 0; i < args.Count; i++)
            {
                argName = argNames[i];
                argVal = new Val(args[i].Value, args[i].Start, args[i].End, newContext);
                newContext.symbols.addSymbol(argName, argVal);
            }
        }

        public SymbolTable setSymbols(Context context)
        {
            SymbolTable symbols = null;
            if (context.parent == null)
            {
                symbols = new SymbolTable(context.symbols);
            }
            else
            {
                symbols = new SymbolTable(context.parent.symbols);
            }
            return symbols;
        }

        public RunTimeResult checkAndPopulateArgs(List<string> argNames, List<Val> args, Context newContext)
        {
            RunTimeResult checkedArgs = new RunTimeResult();
            checkedArgs.register(checkArgs(argNames, args));
            if (checkedArgs.error != null)
            {
                return checkedArgs;
            }

            populateArgs(argNames, args, newContext);
            return checkedArgs.success(null);
        }
        /// <summary>
        /// Overrides Val print method
        /// </summary>
        public override void print()
        {
            Console.Write(nameToken.value);
        }
        /// <summary>
        /// NameToken property
        /// </summary>
        public Token NameToken
        {
            get { return nameToken; }
        }
        /// <summary>
        /// BodyNodes property
        /// </summary>
        public SyntaxNode BodyNodes
        {
            get { return bodyNodes; }
        }
        /// <summary>
        /// ArgNodes property
        /// </summary>
        public List<string> ArgNames
        {
            get { return argNames; }
        }
        /// <summary>
        /// ShouldAutoReturn property
        /// </summary>
        public bool ShouldAutoReturn
        {
            get { return shouldAutoReturn; }
        }
    }

    public class Function : BaseFunction
    {
        public Function(Token nameToken, SyntaxNode bodyNodes, 
            List<string> argNames, DATATYPES dt, Position start, 
            Position end, Context context, bool shouldAutoReturn = false) : 
            base(nameToken, bodyNodes, argNames, dt, start, end, context, 
                shouldAutoReturn)
        {
            dt = DATATYPES.DT_FUNCTION;
        }

        public Function(Function obj) : base(obj)
        {

        }
    }

    public class BuiltinFunction : BaseFunction
    {
        public BuiltinFunction(Token nameToken, SyntaxNode bodyNodes, 
            List<string>argNames, DATATYPES dt, 
            Position start, Position end, Context context, 
            bool shouldAutoReturn = false) : base(nameToken, bodyNodes, 
                argNames, dt, start, end, context)
        {
            dt = DATATYPES.DT_FUNCTION;
        }

        public BuiltinFunction(BuiltinFunction obj) : base(obj)
        {

        }

        public BuiltinFunction(BaseFunction obj) : base(obj)
        {

        }

        public RunTimeResult execute(string funcName, List<string> argNames, List<Val> args, Position start)
        {
            RunTimeResult result = new RunTimeResult();
            switch (funcName)
            {
                case "PRINT":
                    return PRINT(argNames, args);
                case "PRINT_RET":
                    return PRINT_RET(argNames, args);
                case "INPUT":
                    return INPUT(argNames, args);
                case "INPUT_INT":
                    return INPUT_INT(argNames, args);
                case "CLEAR":
                    return CLEAR(argNames, args);
                case "IS_NUMBER":
                    return IS_NUMBER(argNames, args);
                case "IS_STRING":
                    return IS_STRING(argNames, args);
                case "IS_LIST":
                    return IS_LIST(argNames, args);
                case "IS_FUNCTION":
                    return IS_FUNCTION(argNames, args);
                case "APPEND":
                    return APPEND(argNames, args);
                case "POP":
                    return POP(argNames, args);
                case "EXTEND":
                    return EXTEND(argNames, args);
            }
            return result;
        }

        protected RunTimeResult PRINT(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            Console.Write(args[0].Value);
            return result.success(new Val("", start, end, context));
        }

        protected RunTimeResult PRINT_RET(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            return result.success(args[0]);
        }

        protected RunTimeResult INPUT(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            string str = null;
            str = Console.ReadLine();
            String st = new String(str, start, start, context);
            st.End.pos.IX += str.Length;
            st.End.index += str.Length;
            return result.success(st);
        }

        protected RunTimeResult INPUT_INT(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            string str = null;
            str = Console.ReadLine();
            while(str.Contains(Constants.LETTERS) == true)
            {
                str = Console.ReadLine();
            }
            Number num = new Number(str, start, start, context);
            num.End.pos.IX += str.Length;
            num.End.index += str.Length;
            return result.success(num);
        }

        protected RunTimeResult CLEAR(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            
            if (argNames.Count < 0)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 0)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 0)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 0)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            Console.Clear();
            return result.success(new Val("1", start, end, context));
        }

        protected RunTimeResult IS_NUMBER(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args[0].DT == DATATYPES.DT_INT ||
                args[0].DT == DATATYPES.DT_FLOAT)
            {
                return result.success(new Val("1", start, end, context));
            }
            return result.success(new Val("0", start, end, context));
        }

        protected RunTimeResult IS_STRING(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args[0].DT == DATATYPES.DT_STRING)
            {
                return result.success(new Val("1", start, end, context));
            }
            return result.success(new Val("0", start, end, context));
        }

        protected RunTimeResult IS_LIST(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args[0].DT == DATATYPES.DT_LIST)
            {
                return result.success(new Val("1", start, end, context));
            }
            return result.success(new Val("0", start, end, context));
        }

        protected RunTimeResult IS_FUNCTION(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args[0].DT == DATATYPES.DT_FUNCTION)
            {
                return result.success(new Val("1", start, end, context));
            }
            return result.success(new Val("0", start, end, context));
        }

        protected RunTimeResult APPEND(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            Array arr = null;
            if (argNames.Count < 2)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 2)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 2)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 2)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args[0].DT == DATATYPES.DT_LIST)
            {
                arr = ((Array)args[0]).append(args[1]);
                return result.success(arr);
            }
            return result.success(args[0]);
        }

        protected RunTimeResult POP(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            Val value = null;
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args[0].DT == DATATYPES.DT_LIST)
            {
                value = ((Array)args[0]).pop();
                return result.success(new Val("1", start, end, context));
            }
            return result.success(args[0]);
        }

        protected RunTimeResult EXTEND(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            Array arr = null;
            if (argNames.Count < 2)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 2)
            {
                error = new RunTimeError("Too few arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 2)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 2)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args[0].DT == DATATYPES.DT_LIST &&
                args[1].DT == DATATYPES.DT_LIST)
            {
                arr = ((Array)args[0]).contatonate((Array)args[1]);
                return result.success(arr);
            }
            return result.success(arr);
        }
    }

    public class PRINT : BuiltinFunction
    {
        public PRINT(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public PRINT(BuiltinFunction obj) : base(obj)
        {

        }

        public PRINT(BaseFunction obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            Console.Write(args[0].Value);
            return result.success(new Val("", start, end, exContext));
        }
    }

    public class PRINT_RET : BuiltinFunction
    {
        public PRINT_RET(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public PRINT_RET(BuiltinFunction obj) : base(obj)
        {

        }

        public PRINT_RET(BaseFunction obj) : base(obj)
        {

        }

        public PRINT_RET(PRINT_RET obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            return result.success(args[0]);
        }
    }

    public class INPUT : BuiltinFunction
    {
        public INPUT(Token nameToken, List<string> argNames, Position start, Position end, 
            Context context) : base(nameToken, null, argNames, 
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public INPUT(BuiltinFunction obj) : base(obj)
        {

        }

        public INPUT(BaseFunction obj) : base(obj)
        {

        }

        public INPUT(INPUT obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            string str = null;
            str = Console.ReadLine();
            String st = new String(str, start, start, exContext);
            st.End.pos.IX += str.Length;
            st.End.index += str.Length;
            return result.success(st);
        }
    }

    public class INPUT_INT : BuiltinFunction
    {
        public INPUT_INT(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public INPUT_INT(BuiltinFunction obj) : base(obj)
        {

        }

        public INPUT_INT(BaseFunction obj) : base(obj)
        {

        }

        public INPUT_INT(INPUT_INT obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            string str = null;
            str = Console.ReadLine();
            while (str.Contains(Constants.LETTERS) == true)
            {
                str = Console.ReadLine();
            }
            Number num = new Number(str, start, start, exContext);
            num.End.pos.IX += str.Length;
            num.End.index += str.Length;
            return result.success(num);
        }
    }

    public class CLEAR : BuiltinFunction
    {
        public CLEAR(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public CLEAR(BuiltinFunction obj) : base(obj)
        {

        }

        public CLEAR(BaseFunction obj) : base(obj)
        {

        }

        public CLEAR(CLEAR obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            RunTimeError error = null;

            if (argNames.Count < 0)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 0)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 0)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 0)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            Console.Clear();
            return result.success(new Val("1", start, end, exContext));
        }
    }

    public class IS_NUMBER : BuiltinFunction
    {
        public IS_NUMBER(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public IS_NUMBER(BuiltinFunction obj) : base(obj)
        {

        }

        public IS_NUMBER(BaseFunction obj) : base(obj)
        {

        }

        public IS_NUMBER(IS_NUMBER obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args[0].DT == DATATYPES.DT_INT ||
                args[0].DT == DATATYPES.DT_FLOAT)
            {
                return result.success(new Val("1", start, end, exContext));
            }
            return result.success(new Val("0", start, end, exContext));
        }
    }

    public class IS_STRING : BuiltinFunction
    {
        public IS_STRING(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public IS_STRING(BuiltinFunction obj) : base(obj)
        {

        }

        public IS_STRING(BaseFunction obj) : base(obj)
        {

        }

        public IS_STRING(IS_STRING obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args[0].DT == DATATYPES.DT_STRING)
            {
                return result.success(new Val("1", start, end, exContext));
            }
            return result.success(new Val("0", start, end, exContext));
        }
    }

    public class IS_LIST : BuiltinFunction
    {
        public IS_LIST(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public IS_LIST(BuiltinFunction obj) : base(obj)
        {

        }

        public IS_LIST(BaseFunction obj) : base(obj)
        {

        }

        public IS_LIST(IS_LIST obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args[0].DT == DATATYPES.DT_LIST)
            {
                return result.success(new Val("1", start, end, exContext));
            }
            return result.success(new Val("0", start, end, exContext));
        }
    }

    public class IS_FUNCTION : BuiltinFunction
    {
        public IS_FUNCTION(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public IS_FUNCTION(BuiltinFunction obj) : base(obj)
        {

        }

        public IS_FUNCTION(BaseFunction obj) : base(obj)
        {

        }

        public IS_FUNCTION(IS_FUNCTION obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args[0].DT == DATATYPES.DT_FUNCTION)
            {
                return result.success(new Val("1", start, end, exContext));
            }
            return result.success(new Val("0", start, end, exContext));
        }
    }

    public class APPEND : BuiltinFunction
    {
        public APPEND(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public APPEND(BuiltinFunction obj) : base(obj)
        {

        }

        public APPEND(BaseFunction obj) : base(obj)
        {

        }

        public APPEND(APPEND obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            Array arr = null;
            if (argNames.Count < 2)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 2)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 2)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 2)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args[0].DT == DATATYPES.DT_LIST)
            {
                arr = ((Array)args[0]).append(args[1]);
                return result.success(arr);
            }
            return result.success(args[0]);
        }
    }

    public class POP : BuiltinFunction
    {
        public POP(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public POP(BuiltinFunction obj) : base(obj)
        {

        }

        public POP(BaseFunction obj) : base(obj)
        {

        }

        public POP(POP obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            Val value = null;
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args[0].DT == DATATYPES.DT_LIST)
            {
                value = ((Array)args[0]).pop();
                return result.success(new Val("1", start, end, exContext));
            }
            return result.success(args[0]);
        }
    }

    public class EXTEND : BuiltinFunction
    {
        public EXTEND(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public EXTEND(BuiltinFunction obj) : base(obj)
        {

        }

        public EXTEND(BaseFunction obj) : base(obj)
        {

        }

        public EXTEND(EXTEND obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            Array arr = null;
            if (argNames.Count < 2)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 2)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 2)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 2)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args[0].DT == DATATYPES.DT_LIST &&
                args[1].DT == DATATYPES.DT_LIST)
            {
                arr = ((Array)args[0]).contatonate((Array)args[1]);
                return result.success(arr);
            }
            return result.success(arr);
        }
    }

    public class LENGTH : BuiltinFunction
    {
        public LENGTH(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public LENGTH(BuiltinFunction obj) : base(obj)
        {

        }

        public LENGTH(BaseFunction obj) : base(obj)
        {

        }

        public LENGTH(LENGTH obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            Number len = null;
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args[0].DT != DATATYPES.DT_LIST)
            {
                RunTimeError err = new RunTimeError("No list provided ", context, 
                    start.fileName, start.fileText, start.Line, start.pos.IX, 
                    end.pos.IX);
                return result.failure(err);
            }
            len = new Number(((Array)args[0]).elements.Length.ToString());
            return result.success(len);
        }
    }

    public class RUN : BuiltinFunction
    {
        public RUN(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public RUN(BuiltinFunction obj) : base(obj)
        {

        }

        public RUN(BaseFunction obj) : base(obj)
        {

        }

        public RUN(RUN obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            string fileName = context.symbols.getSymbol(args[0].Value).Value;
            string scriptName = fileName;
            fileName = AppDomain.CurrentDomain.BaseDirectory + fileName + ".ds";
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            string script = null;
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }

            if(System.IO.File.Exists(fileName) == false)
            {
                string err = "Failed to open file @" + scriptName;
                return result.failure(new RunTimeError(err, context, start.fileName, 
                    start.fileText, start.Line, start.pos.IX, end.pos.IX));
            }
            System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
            script = sr.ReadToEnd();
            sr.Close();
            RunTimeResult code = Interpreter.visitNode(Parser.parse(Lexer.processLine(script)), context);
            return result.success(code.value);
            //return result.success(new String(script, start, end, context));
        }
    }

    public class WRITELINE : BuiltinFunction
    {
        public WRITELINE(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public WRITELINE(BuiltinFunction obj) : base(obj)
        {

        }

        public WRITELINE(BaseFunction obj) : base(obj)
        {

        }

        public WRITELINE(WRITELINE obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            string fileName = context.symbols.getSymbol(args[0].Value).Value;
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            if(argNames.Count < 2)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if(args.Count < 2)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if(argNames.Count > 2)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if(args.Count > 2)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }

            if(System.IO.File.Exists(((XenoFile)args[0]).FileName) == false)
            {
                string err = "Failed to open file @" + fileName;
                return result.failure(new RunTimeError(err, context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX));
            }
            if (((XenoFile)args[0]).IsOpen == false)
            {
                string err = "Open file before reading or writing " + fileName;
                return result.failure(new RunTimeError(err, context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX));
            }
            if (args[1] is String)
            {
                ((XenoFile)args[0]).writeLine((String)args[1]);
            }
            else if(args[1] is Number)
            {
                ((XenoFile)args[0]).writeLine((Number)args[1]);
            }
            else
            {
                error = new RunTimeError("Failed to write line ", context, 
                    start.fileName, start.fileText, start.Line, start.pos.IX, 
                    end.pos.IX);
                return result.failure(error);
            }

            return result.success(Number.True);
        }
    }

    public class READLINE : BuiltinFunction
    {
        public READLINE(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public READLINE(BuiltinFunction obj) : base(obj)
        {

        }

        public READLINE(BaseFunction obj) : base(obj)
        {

        }

        public READLINE(WRITELINE obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            string fileName = context.symbols.getSymbol(args[0].Value).Value;
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }

            
            if(!(args[0] is XenoFile))
            {
                error = new RunTimeError("No file provided ", context,
                    start.fileName, start.fileText, start.Line, start.pos.IX,
                    end.pos.IX);
                return result.failure(error);
            }
            if (System.IO.File.Exists(fileName) == false)
            {
                string err = "Failed to open file @ " + fileName;
                return result.failure(new RunTimeError(err, context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX));
            }
            if (((XenoFile)args[0]).IsOpen == false)
            {
                string err = "Open file before reading or writing " + fileName;
                return result.failure(new RunTimeError(err, context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX));
            }
            return result.success(((XenoFile)args[0]).readLine());
        }
    }

    public class OPENFILE : BuiltinFunction
    {
        public OPENFILE(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public OPENFILE(BuiltinFunction obj) : base(obj)
        {

        }

        public OPENFILE(BaseFunction obj) : base(obj)
        {

        }

        public OPENFILE(OPENFILE obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            string fileName = context.symbols.getSymbol(args[0].Value).Value;
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }


            if (!(args[0] is XenoFile))
            {
                error = new RunTimeError("No file provided ", context,
                    start.fileName, start.fileText, start.Line, start.pos.IX,
                    end.pos.IX);
                return result.failure(error);
            }
            if (System.IO.File.Exists(fileName) == false)
            {
                string err = "Failed to open file @ " + fileName;
                return result.failure(new RunTimeError(err, context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX));
            }
            ((XenoFile)args[0]).open(((XenoFile)args[0]).FileName);
            return result.success(Number.True);
        }
    }

    public class CLOSEFILE : BuiltinFunction
    {
        public CLOSEFILE(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public CLOSEFILE(BuiltinFunction obj) : base(obj)
        {

        }

        public CLOSEFILE(BaseFunction obj) : base(obj)
        {

        }

        public CLOSEFILE(CLOSEFILE obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            string fileName = context.symbols.getSymbol(args[0].Value).Value;
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            if (argNames.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 1)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 1)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }


            if (!(args[0] is XenoFile))
            {
                error = new RunTimeError("No file provided ", context,
                    start.fileName, start.fileText, start.Line, start.pos.IX,
                    end.pos.IX);
                return result.failure(error);
            }
            if (System.IO.File.Exists(fileName) == false)
            {
                string err = "Failed to open file @ " + fileName;
                return result.failure(new RunTimeError(err, context, start.fileName,
                    start.fileText, start.Line, start.pos.IX, end.pos.IX));
            }
            ((XenoFile)args[0]).close();
            return result.success(Number.True);
        }
    }

    public class CREATEFILE : BuiltinFunction
    {
        public CREATEFILE(Token nameToken, List<string> argNames, Position start, Position end,
            Context context) : base(nameToken, null, argNames,
                DATATYPES.DT_FUNCTION, start, end, context)
        {

        }

        public CREATEFILE(BuiltinFunction obj) : base(obj)
        {

        }

        public CREATEFILE(BaseFunction obj) : base(obj)
        {

        }

        public CREATEFILE(CLOSEFILE obj) : base(obj)
        {

        }

        public override RunTimeResult execute(List<string> argNames, List<Val> args)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            string fileName = context.symbols.getSymbol(args[0].Value).Value;
            SymbolTable symbols = setSymbols(context);
            Context exContext = generateNewContext(symbols);
            if (argNames.Count < 0)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count < 0)
            {
                error = new RunTimeError("Too few arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (argNames.Count > 0)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            if (args.Count > 0)
            {
                error = new RunTimeError("Too many arguments", exContext, start.fileName,
                    start.fileText, start.Line, start.pos.IX, start.pos.IX);
                return result.failure(error);
            }
            
            return result.success(new XenoFile("", start, end, context));
        }
    }

    public class Array : Val
    {
        public Val[] elements;

        public Array(Val[] elements, Position start, Position end,
            Context context) : base("array", start, end, context)
        {
            this.elements = new Val[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                this.elements[i] = elements[i];
            }
            dt = DATATYPES.DT_LIST;
        }

        public Array(Array obj) : base(obj)
        {
            elements = new Val[obj.elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                this.elements[i] = new Val(obj.elements[i]);
            }
            dt = DATATYPES.DT_LIST;
        }

        public Array contatonate(Array other)
        {
            Val[] a = new Val[elements.Length + other.elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                a[i] = elements[i];
            }
            for (int i = 0; i < other.elements.Length; i++)
            {
                a[i + elements.Length] = other.elements[i];
            }
            Array newList = new Array(a, start, end, context);
            return newList;
        }

        public Array append(Val value)
        {
            Val[] newList = new Val[elements.Length + 1];
            for(int i = 0; i < elements.Length; i++)
            {
                newList[i] = elements[i];
            }
            newList[elements.Length] = value;
            return new Array(newList, start, end, context);
        }

        public Array setElement(Number num, Val value)
        {
            Array newList = null;
            Val[] a = new Val[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                a[i] = elements[i];
            }
            a[num.valueAsInt()] = value;
            newList = new Array(a, start, end, context);
            return newList;
        }

        public Val pop()
        {
            if(elements.Length == 0)
            {
                return new Val("", start, end, context);
            }
            Val element = elements[0];
            if (elements.Length > 1)
            {
                Val[] newList = new Val[elements.Length - 1];
                for (int i = 1; i < elements.Length; i++)
                {
                    newList[i - 1] = elements[i];
                }
                elements = newList;
            }
            else
            {
                elements = new Val[0];
            }
            return element;
        }

        public void clear()
        {
            elements = new Val[0];
        }

        public RunTimeResult removeElement(Number num)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            Array newList = null;
            Val[] a = new Val[elements.Length - 1];
            if(num.valueAsInt() < 0)
            {
                error = new RunTimeError("Index value outside list range", 
                    context, start.fileName, start.fileText, start.Line, 
                    start.pos.IX, end.pos.IY);
                return result.failure(error);
            }
            if (num.valueAsInt() > elements.Length - 1)
            {
                error = new RunTimeError("Index value outside list range",
                    context, start.fileName, start.fileText, start.Line,
                    start.pos.IX, end.pos.IY);
                return result.failure(error);
            }
            if (num.valueAsInt() != 0)
            {
                for (int i = 0; i < num.valueAsInt(); i++)
                {
                    a[i] = elements[i];
                }
                for (int i = num.valueAsInt() + 1; i < elements.Length; i++)
                {
                    a[i - 1] = elements[i];
                }
            }
            else
            {
                for (int i = 0; i < elements.Length - 1; i++)
                {
                    a[i] = elements[i + 1];
                }
            }
            newList = new Array(a, start, end, context);
            return result.success(newList);
        }

        public RunTimeResult getElement(Number num)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            if (num.valueAsInt() < 0)
            {
                error = new RunTimeError("Index value outside list range",
                    context, start.fileName, start.fileText, start.Line,
                    start.pos.IX, end.pos.IY);
                return result.failure(error);
            }
            if (num.valueAsInt() > elements.Length - 1)
            {
                error = new RunTimeError("Index value outside list range",
                    context, start.fileName, start.fileText, start.Line,
                    start.pos.IX, end.pos.IY);
                return result.failure(error);
            }
            return result.success(elements[num.valueAsInt()]);
        }

        public RunTimeResult insertElement(Number num, Val value)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            Array newList = null;
            Val[] a = new Val[elements.Length + 1];
            if (num.valueAsInt() < 0)
            {
                error = new RunTimeError("Index value outside list range",
                    context, start.fileName, start.fileText, start.Line,
                    start.pos.IX, end.pos.IY);
                return result.failure(error);
            }
            if (num.valueAsInt() > elements.Length - 1)
            {
                error = new RunTimeError("Index value outside list range",
                    context, start.fileName, start.fileText, start.Line,
                    start.pos.IX, end.pos.IY);
                return result.failure(error);
            }
            if (num.valueAsInt() != 0)
            {
                for (int i = 0; i < num.valueAsInt(); i++)
                {
                    a[i] = elements[i];
                }
                a[num.valueAsInt()] = value;
                for (int i = num.valueAsInt(); i < elements.Length; i++)
                {
                    a[i + 1] = elements[i];
                }
            }
            else
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    a[i + 1] = elements[i];
                }
                a[0] = value;
            }
            newList = new Array(a, start, end, context);
            return result.success(newList);
        }

        public override void print()
        {
            //Console.Write("[");
            for(int i = 0; i < elements.Length; i++)
            {
                if(elements[0] != null)
                {
                    if (elements[i].DT == DATATYPES.DT_LIST)
                    {
                        if (((Array)elements[i]).DT == DATATYPES.DT_LIST)
                        {
                            ((Array)elements[i]).print();
                        }
                    }
                    else
                    {
                        Console.Write(elements[i].Value);
                    }
                }
                if(i < elements.Length - 1)
                {
                    Console.Write(", ");
                }
            }
        }
    }
}
