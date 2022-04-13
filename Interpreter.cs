using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenos
{
    public enum DATATYPES
    {
        DT_INT,
        DT_FLOAT,
        DT_STRING,
        DT_FUNCTION, 
        DT_LIST
    }

    public class SymbolTable
    {
        //protected
        protected Dictionary<string, Val> symbols;
        protected SymbolTable parent;
        //public
        /// <summary>
        /// SymbolTable constructor
        /// </summary>
        /// <param name="parent">SymbolTable referenece</param>
        public SymbolTable(SymbolTable parent = null)
        {
            symbols = new Dictionary<string, Val>();
            this.parent = parent;
        }
        /// <summary>
        /// Returns a string for a provided key if in table, 
        /// table's parent else returns "";
        /// </summary>
        /// <param name="key">Key value</param>
        /// <returns>String</returns>
        public Val getSymbol(string key)
        {
            Val output;
            if(parent != null)
            {
                if(parent.contains(key) == true)
                {
                    return parent.getSymbol(key);
                }
            }
            symbols.TryGetValue(key, out output);
            return output;
        }
        /// <summary>
        /// Adds symbol and it's key if not in table or parent's table
        /// returns true on success and false on failure
        /// </summary>
        /// <param name="key">Key value</param>
        /// <param name="symbol">Symbol value</param>
        /// <returns>Boolean</returns>
        public bool addSymbol(string key, Val symbol)
        {
            if(parent != null)
            {
                if(parent.contains(key) == true)
                {
                    return false;
                }
            }
            if(symbols.ContainsKey(key) == true)
            {
                return false;
            }
            symbols.Add(key, symbol);
            return true;
        }
        /// <summary>
        /// Sets the value in symbol table, checks if key is in
        /// parent table before checking local table. Adds a new
        /// symbol to table if key does not exist in local table
        /// </summary>
        /// <param name="key">Key value</param>
        /// <param name="symbol">Symbol value</param>
        public void setSymbol(string key, Val symbol)
        {
            if (parent != null)
            {
                if (parent.contains(key) == true)
                {
                    parent.setSymbol(key, symbol);
                }
            }
            if (symbols.ContainsKey(key) == false)
            {
                symbols.Add(key, symbol);
            }
            else
            {
                symbols.Remove(key);
                symbols.Add(key, symbol);
            }
        }
        /// <summary>
        /// Checks table if it contains a speficied key
        /// </summary>
        /// <param name="key">Key value</param>
        /// <returns>Boolean</returns>
        public bool contains(string key)
        {
            return symbols.ContainsKey(key);
        }
        /// <summary>
        /// Removes symbol of specified key from table, returns 
        /// true on success and false on failure. Does not remove
        /// from parent table if it exists.
        /// </summary>
        /// <param name="key">Key value</param>
        /// <returns>Boolean</returns>
        public bool remove(string key)
        {
            if(symbols.ContainsKey(key) == true)
            {
                symbols.Remove(key);
                return true;
            }
            return false;
        }
    }

    public class RunTimeResult
    {
        //public
        public Val value;
        public Error error;
        public Val returnValue;
        public bool loopShouldContinue;
        public bool loopShouldBreak;
        public Position start;
        public Position end;

        public RunTimeResult()
        {
            reset();
        }

        public void reset()
        {
            value = null;
            error = null;
            returnValue = null;
            loopShouldContinue = false;
            loopShouldBreak = false;
            start = null;
            end = null;
        }

        public RunTimeResult register(RunTimeResult result)
        {
            value = result.value;
            returnValue = result.returnValue;
            if (result.shouldReturn() == true)
            {
                if (result.error != null)
                {
                    error = result.error;
                    return this;
                }
            }
            if (result.returnValue != null)
            {
                if (result.returnValue is Array)
                {
                    returnValue = new Array((Array)result.value);
                }
                else
                {
                    returnValue = new Val(result.value);
                }
            }
            loopShouldContinue = result.loopShouldContinue;
            loopShouldBreak = result.loopShouldBreak;
            return this;
        }

        public RunTimeResult success(Val value)
        {
            reset();
            this.value = value;
            return this;
        }

        public RunTimeResult successReturn(Val value)
        {
            reset();
            castReturnValue(value);
            this.value = returnValue;
            return this;
        }

        public RunTimeResult successContinue()
        {
            reset();
            loopShouldContinue = true;
            return this;
        }

        public RunTimeResult successBreak()
        {
            reset();
            loopShouldBreak = true;
            return this;
        }

        public RunTimeResult failure(Error error)
        {
            reset();
            this.error = error;
            return this;
        }

        public bool shouldReturn()
        {
            if(error != null)
            {
                return true;
            }
            if(returnValue != null)
            {
                return true;
            }
            if(loopShouldContinue == true)
            {
                return true;
            }
            if(loopShouldBreak == true)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Sets RunTimeResult position 
        /// </summary>
        /// <param name="start">Start Position</param>
        /// <param name="end">End Position</param>
        public void setPos(Position start, Position end)
        {
            this.start = new Position(start);
            this.end = new Position(end);
        }
        /// <summary>
        /// Casts return value into appropriate type
        /// </summary>
        /// <param name="value">Val reference</param>
        public void castReturnValue(Val value)
        {
            if (value is Number)
            {
                returnValue = new Number((Number)value);
            }
            else if (value is Function)
            {
                returnValue = new Function((Function)value);
            }
            else if (value is BaseFunction)
            {
                returnValue = new BaseFunction((BaseFunction)value);
            }
            else if (value is BuiltinFunction)
            {
                returnValue = new BuiltinFunction((BuiltinFunction)value);
            }
            else if (value is String)
            {
                returnValue = new String((String)value);
            }
            else if (value is Array)
            {
                returnValue = new Array((Array)value);
            }
            else
            {
                returnValue = new Number(value.Value);
            }
        }
    }

    public class Context
    {
        //public 
        public string displayName;
        public Context parent;
        public Position parentEntryPos;
        public SymbolTable symbols;

        public Context(string displayName, SymbolTable symbols, Context parent = null, 
            Position parentEntryPos = null)
        {
            this.displayName = displayName;
            this.symbols = symbols;
            this.parent = parent;
            this.parentEntryPos = parentEntryPos;
        }
        /// <summary>
        /// Context copy constructor
        /// </summary>
        /// <param name="obj">Context reference</param>
        public Context(Context obj)
        {
            displayName = obj.displayName;
            parent = obj.parent;
            parentEntryPos = new Position(obj.parentEntryPos);
        }
    }

    public static class Interpreter
    {
        public static RunTimeResult visitNode(SyntaxNode node, Context context)
        {
            RunTimeResult result = new RunTimeResult();
            result.setPos(node.start, node.end);
            if (node is BinOpNode)
            {
                result = visitBinOpNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is UniOpNode)
            {
                result = visitUniOpNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is NumberNode)
            {
                result = visitNumberNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is VarAccessNode)
            {
                result = visitVarAccessNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is VarAssignNode)
            {
                result = visitVarAssignNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is IfNode)
            {
                result = visitIfNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is ForNode)
            {
                result = visitForNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is WhileNode)
            {
                result = visitWhileNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is DefFuncNode)
            {
                result = visitDefFuncNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is CallNode)
            {
                result = visitCallNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is StringNode)
            {
                result = visitStringNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is ListNode)
            {
                result = visitListNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is AccessListNode)
            {
                result = visitAccessListNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is AsignListNode)
            {
                result = visitAsignListNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is RemoveListNode)
            {
                result = visitRemoveListNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is ReturnNode)
            {
                result = visitReturnNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is ContinueNode)
            {
                result = visitContinueNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            else if (node is BreakNode)
            {
                result = visitBreakNode(node, context);
                result.setPos(node.start, node.end);
                return result;
            }
            return result.failure(new RunTimeError("Failed to visit node",
                node.start.context, node.start.fileName, node.start.fileText,
                node.start.Line, node.start.pos.IX, node.end.pos.IX));
        }

        public static RunTimeResult visitNumberNode(SyntaxNode node, Context context)
        {
            Number tmp = null;
            RunTimeResult result = null;
            if (node.token.type == TOKENS.TT_IDENTIFIER)
            {
                tmp = new Number(context.symbols.getSymbol(node.token.value).Value,
                    DATATYPES.DT_INT, node.start, node.end, context);
            }
            else if (node.token.type == TOKENS.TT_INT)
            {
                tmp = new Number(node.token.value,
                    DATATYPES.DT_INT, node.start, node.end, context);
            }
            else if (node.token.type == TOKENS.TT_FLOAT)
            {
                tmp = new Number(node.token.value,
                    DATATYPES.DT_FLOAT, node.start, node.end, context);
            }
            else if (node.token.type == TOKENS.TT_PLUS)
            {
                result = visitNumberNode(node.right, context);
                result.setPos(node.start, node.end);
                return result.success(result.value);
            }
            else if (node.token.type == TOKENS.TT_MINUS)
            {
                result = visitNumberNode(node.right, context);
                result.setPos(node.start, node.end);
                return result.success(result.value);
            }
            else if (node is BinOpNode)
            {
                result = visitBinOpNode(node, context);
                result.setPos(node.start, node.end);
                return result.success(result.value);
            }
            result = new RunTimeResult();
            result.setPos(node.start, node.end);
            return result.success(tmp);
        }

        public static RunTimeResult visitBinOpNode(SyntaxNode node, Context context)
        {
            RunTimeResult left = visitNode(node.left, context);
            RunTimeResult right = visitNode(node.right, context);
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;
            if (node.token.type == TOKENS.TT_POW)
            {
                if (left.value.DT == DATATYPES.DT_STRING)
                {
                    error = new RunTimeError("Not an Int or Float", context,
                        node.left.start.fileName, node.left.start.fileText,
                        node.left.start.Line, node.left.start.pos.IX,
                        node.left.end.pos.IX);
                    return result.failure(error);
                }
                result.setPos(node.start, node.end);
                result = result.success(left.value.powBy((Number)right.value, context));
                return result;
            }
            else if (node.token.type == TOKENS.TT_PLUS)
            {
                if (left.value is Array)
                {
                    if (right.value is Array)
                    {
                        Array temp = ((Array)left.value).contatonate((Array)right.value);
                        result.setPos(node.start, node.end);
                        result = result.success(temp);
                        return result;
                    }
                    else
                    {
                        Array temp = ((Array)left.value).append(right.value);
                        result.setPos(node.start, node.end);
                        result = result.success(temp);
                        return result;
                    }

                }
                else if (left.value.DT == DATATYPES.DT_STRING &&
                    right.value.DT == DATATYPES.DT_STRING)
                {
                    result.setPos(node.start, node.end);
                    result = result.success(((String)left.value).addStr((String)right.value));
                    return result;
                }
                else if (left.value.legalOperation(right.value.DT) == false)
                {
                    error = new RunTimeError("Two or more Strings expected", context,
                        node.left.start.fileName, node.left.start.fileText,
                        node.left.start.Line, node.left.start.pos.IX,
                        node.left.end.pos.IX);
                    return result.failure(error);
                }
                else
                {
                    result.setPos(node.start, node.end);
                    result = result.success(left.value.addTo(right.value.valueAsNumber(), context));
                    return result;
                }
            }
            else if (node.token.type == TOKENS.TT_MINUS)
            {
                if (left.value is Array)
                {
                    if (!(right.value is Number))
                    {
                        error = new RunTimeError("An int value expected", context,
                        node.left.start.fileName, node.left.start.fileText,
                        node.left.start.Line, node.left.start.pos.IX,
                        node.left.end.pos.IX);
                        return result.failure(error);
                    }
                    ((Array)left.value).removeElement((Number)right.value);
                }
                else if (left.value.DT == DATATYPES.DT_STRING)
                {
                    error = new RunTimeError("Not an Int or Float", context,
                        node.left.start.fileName, node.left.start.fileText,
                        node.left.start.Line, node.left.start.pos.IX,
                        node.left.end.pos.IX);
                    return result.failure(error);
                }
                result.setPos(node.start, node.end);
                result = result.success(left.value.subBy(right.value.valueAsNumber(), context));
                return result;
            }
            else if (node.token.type == TOKENS.TT_MUL)
            {
                if (left.value is Array)
                {
                    if (right.value is Array)
                    {
                        result.setPos(node.start, node.end);
                        ((Array)left.value).contatonate((Array)right.value);
                    }
                    else if (!(right.value is Array))
                    {
                        error = new RunTimeError("Two or more Lists expected", context,
                        node.left.start.fileName, node.left.start.fileText,
                        node.left.start.Line, node.left.start.pos.IX,
                        node.left.end.pos.IX);
                        return result.failure(error);
                    }
                }
                else if (left.value.DT == DATATYPES.DT_STRING)
                {
                    result.setPos(node.start, node.end);
                    result = result.success(((String)left.value).mulStr(right.value.valueAsNumber()));
                }
                else
                {
                    result.setPos(node.start, node.end);
                    result = result.success(left.value.mulBy(right.value.valueAsNumber(), context));
                }
                return result;
            }
            else if (node.token.type == TOKENS.TT_DIV)
            {
                if (left.value.DT == DATATYPES.DT_STRING)
                {
                    error = new RunTimeError("Not an Int or Float", context,
                        node.left.start.fileName, node.left.start.fileText,
                        node.left.start.Line, node.left.start.pos.IX,
                        node.left.end.pos.IX);
                    return result.failure(error);
                }
                if ((float)Convert.ToDecimal(right.value.Value) == 0)
                {
                    result.setPos(node.start, node.end);
                    string runError = "Divide by 0 error Line: " +
                        result.start.Line + " Col: " + result.start.Column;
                    result.failure(new RunTimeError(runError,
                        result.start.context, result.start.fileName,
                        result.start.fileText, result.start.Line,
                        result.start.pos.IX, result.end.pos.IX));
                    result.setPos(node.start, node.end);
                    return result;
                }
                result.success(left.value.divBy(right.value.valueAsNumber(), context));
                return result;
            }
            else if (node.token.type == TOKENS.TT_EE)
            {
                result.setPos(node.start, node.end);
                if (left.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.left.start.fileName, node.left.start.fileText, node.left.start.Line,
                        node.left.start.Column, node.left.end.Column);
                    result.failure(error);
                    return result;
                }
                if (right.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.right.start.fileName, node.right.start.fileText, node.right.start.Line,
                        node.right.start.Column, node.right.end.Column);
                    result.failure(error);
                    return result;
                }
                result = result.success(left.value.getCompareEQ(right.value.valueAsNumber()));
                return result;
            }
            else if (node.token.type == TOKENS.TT_NE)
            {
                result.setPos(node.start, node.end);
                if (left.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.left.start.fileName, node.left.start.fileText, node.left.start.Line,
                        node.left.start.Column, node.left.end.Column);
                    result.failure(error);
                    return result;
                }
                if (right.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.right.start.fileName, node.right.start.fileText, node.right.start.Line,
                        node.right.start.Column, node.right.end.Column);
                    result.failure(error);
                    return result;
                }
                result = result.success(left.value.getCompareNE(right.value.valueAsNumber()));
                return result;
            }
            else if (node.token.type == TOKENS.TT_LT)
            {
                result.setPos(node.start, node.end);
                if (left.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.left.start.fileName, node.left.start.fileText, node.left.start.Line,
                        node.left.start.Column, node.left.end.Column);
                    result.failure(error);
                    return result;
                }
                if (right.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.right.start.fileName, node.right.start.fileText, node.right.start.Line,
                        node.right.start.Column, node.right.end.Column);
                    result.failure(error);
                    return result;
                }
                result = result.success(left.value.getCompareLT(right.value.valueAsNumber()));
                return result;
            }
            else if (node.token.type == TOKENS.TT_LTE)
            {
                result.setPos(node.start, node.end);
                if (left.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.left.start.fileName, node.left.start.fileText, node.left.start.Line,
                        node.left.start.Column, node.left.end.Column);
                    result.failure(error);
                    return result;
                }
                if (right.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.right.start.fileName, node.right.start.fileText, node.right.start.Line,
                        node.right.start.Column, node.right.end.Column);
                    result.failure(error);
                    return result;
                }
                result = result.success(left.value.getCompareLTE(right.value.valueAsNumber()));
                return result;
            }
            else if (node.token.type == TOKENS.TT_GT)
            {
                result.setPos(node.start, node.end);
                if (left.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.left.start.fileName, node.left.start.fileText, node.left.start.Line,
                        node.left.start.Column, node.left.end.Column);
                    result.failure(error);
                    return result;
                }
                if (right.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.right.start.fileName, node.right.start.fileText, node.right.start.Line,
                        node.right.start.Column, node.right.end.Column);
                    result.failure(error);
                    return result;
                }
                result = result.success(left.value.getCompareGT(right.value.valueAsNumber()));
                return result;
            }
            else if (node.token.type == TOKENS.TT_GTE)
            {
                result.setPos(node.start, node.end);
                if (left.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.left.start.fileName, node.left.start.fileText, node.left.start.Line,
                        node.left.start.Column, node.left.end.Column);
                    result.failure(error);
                    return result;
                }
                if (right.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.right.start.fileName, node.right.start.fileText, node.right.start.Line,
                        node.right.start.Column, node.right.end.Column);
                    result.failure(error);
                    return result;
                }
                result = result.success(left.value.getCompareGTE(right.value.valueAsNumber()));
                return result;
            }
            else if (node.token.matches(TOKENS.TT_KEYWORD, "and") == true)
            {
                result.setPos(node.start, node.end);
                if (left.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.left.start.fileName, node.left.start.fileText, node.left.start.Line,
                        node.left.start.Column, node.left.end.Column);
                    result.failure(error);
                    return result;
                }
                if (right.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.right.start.fileName, node.right.start.fileText, node.right.start.Line,
                        node.right.start.Column, node.right.end.Column);
                    result.failure(error);
                    return result;
                }
                result = result.success(left.value.andedBy((Number)right.value));
                return result;
            }
            else if (node.token.matches(TOKENS.TT_KEYWORD, "or") == true)
            {
                result.setPos(node.start, node.end);
                if (left.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.left.start.fileName, node.left.start.fileText, node.left.start.Line,
                        node.left.start.Column, node.left.end.Column);
                    result.failure(error);
                    return result;
                }
                if (right.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.right.start.fileName, node.right.start.fileText, node.right.start.Line,
                        node.right.start.Column, node.right.end.Column);
                    result.failure(error);
                    return result;
                }
                result = result.success(left.value.oredBy((Number)right.value));
                return result;
            }
            else
            {
                result.setPos(node.start, node.end);
                if (left.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.left.start.fileName, node.left.start.fileText, node.left.start.Line,
                        node.left.start.Column, node.left.end.Column);
                    result.failure(error);
                    return result;
                }
                if (right.value == null)
                {
                    error = new RunTimeError("Variable not assinged", context,
                        node.right.start.fileName, node.right.start.fileText, node.right.start.Line,
                        node.right.start.Column, node.right.end.Column);
                    result.failure(error);
                    return result;
                }
                result = result.success(left.value.divBy((Number)right.value, context));
                return result;
            }
            result.setPos(node.start, node.end);
            return result;
        }

        public static RunTimeResult visitUniOpNode(SyntaxNode node, Context context)
        {
            Val num = null;
            if (node.right == null)
            {
                num = (Number)visitNumberNode(node, context).value;
            }
            else
            {
                num = (Number)visitNumberNode(node.right, context).value;
            }
            Number neg = new Number("-1", num.DT);
            if (node.token.type == TOKENS.TT_MINUS)
            {
                num = (Number)visitUniOpNode(node.right, context).value;
                num = num.mulBy(neg, context);
            }
            else if (node.token.matches(TOKENS.TT_KEYWORD, "not"))
            {
                num = (Number)visitNode(node.right, context).value;
                num = num.notted();
                num.setContext(context);
            }
            num.setPos(node.start, node.end);
            RunTimeResult result = new RunTimeResult();
            result.success(num);
            return result;
        }

        public static RunTimeResult visitVarAccessNode(SyntaxNode node, Context context)
        {
            RunTimeResult result = new RunTimeResult();
            Val data = context.symbols.getSymbol(node.token.value);
            data.setPos(node.start, node.end);
            if (data == null)
            {
                string runError = "Var not assinged ";
                result.failure(new RunTimeError(runError, node.start.context, node.start.fileName,
                    node.start.fileText, node.start.Line, node.start.pos.IX, node.end.pos.IX));
                return result;
            }
            //Val value = new Val(data);
            result.successReturn(data);
            return result;
        }

        public static RunTimeResult visitVarAssignNode(SyntaxNode node, Context context)
        {
            RunTimeResult result = new RunTimeResult();
            if (node.right == null)
            {
                string runError = "Runtime error";
                result.failure(new RunTimeError(runError, node.start.context,
                    node.start.fileName, node.start.fileText, node.start.Line,
                    node.start.pos.IX, node.end.pos.IX));
                return result;
            }
            result.register(visitNode(node.right, context));
            if (result.shouldReturn() == true)
            {
                return result;
            }
            string name = ((VarAssignNode)node).varName;
            if (result.value.Cont == null)
            {
                result.value.setContext(context);
            }
            context.symbols.setSymbol(name, result.value);
            return result.success(result.value);
        }

        public static RunTimeResult visitIfNode(SyntaxNode node, Context context)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeResult conditionValue = null;
            RunTimeResult exprValue = null;
            RunTimeResult elseValue = null;
            for (int i = 0; i < ((IfNode)node).cases.Count; i++)
            {
                conditionValue = result.register(visitNode(((IfNode)node).cases[i].Item1.node, context));
                if (conditionValue.shouldReturn() == true)
                {
                    return conditionValue;
                }
                if (conditionValue.value.isTrue() == true)
                {
                    exprValue = result.register(visitNode(((IfNode)node).cases[i].Item2.node, context));
                    if (exprValue.shouldReturn() == true)
                    {
                        return exprValue;
                    }
                    return result.success(exprValue.value);
                }
            }
            if (((IfNode)node).elseCase.node != null)
            {
                elseValue = result.register(visitNode(((IfNode)node).elseCase.node, context));
                if (elseValue.shouldReturn() == true)
                {
                    return elseValue;
                }
                return result.success(elseValue.value);
            }
            Number nullResult = Number.Null;
            nullResult.setPos(node.start, node.end);
            nullResult.setContext(context);
            return result.success(nullResult);
        }

        public static RunTimeResult visitForNode(SyntaxNode node, Context context)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeResult startValue = null;
            Number start = null;
            RunTimeResult endValue = null;
            Number end = null;
            RunTimeResult stepValue = null;
            Number step = null;
            Val value = null;
            List<Val> elements = new List<Val>();

            startValue = visitNode(((ForNode)node).startValue, context);
            if (startValue.shouldReturn() == true)
            {
                return startValue;
            }
            start = (Number)startValue.value;

            endValue = visitNode(((ForNode)node).endValue, context);
            if (endValue.shouldReturn() == true)
            {
                return endValue;
            }
            end = (Number)endValue.value;

            if (((ForNode)node).stepValue != null)
            {
                stepValue = visitNode(((ForNode)node).stepValue, context);
                if (stepValue.shouldReturn() == true)
                {
                    return stepValue;
                }
                step = new Number(stepValue.value.Value);
            }
            else
            {
                step = new Number("1");
            }

            Number i = new Number(start.Value);
            context.symbols.addSymbol(((ForNode)node).varNameToken.value, i);
            if (step.isNeg() == true)
            {
                while (end.valueAsInt() <= i.valueAsInt() == true)
                {
                    i = i.addTo(step, context);
                    context.symbols.setSymbol(((ForNode)node).varNameToken.value, i);
                    result = result.register(visitNode(((ForNode)node).bodyNode, context));
                    if (result.shouldReturn() == true &&
                        result.loopShouldContinue == false &&
                        result.loopShouldBreak == false)
                    {
                        return result;
                    }
                    value = result.returnValue;
                    if (result.loopShouldContinue == true)
                    {
                        continue;
                    }
                    if (result.loopShouldBreak == true)
                    {
                        break;
                    }
                    elements.Add(value);
                }
            }
            else
            {
                while (end.valueAsInt() >= i.valueAsInt() == true)
                {
                    i = i.addTo(step, context);
                    context.symbols.setSymbol(((ForNode)node).varNameToken.value, i);
                    result = result.register(visitNode(((ForNode)node).bodyNode, context));
                    if (result.shouldReturn() == true &&
                        result.loopShouldContinue == false &&
                        result.loopShouldBreak == false)
                    {
                        return result;
                    }
                    value = result.value;
                    if (result.loopShouldContinue == true)
                    {
                        continue;
                    }
                    if (result.loopShouldBreak == true)
                    {
                        break;
                    }
                    elements.Add(value);
                }
            }
            context.symbols.remove(((ForNode)node).varNameToken.value);
            return result.success(new Array(elements.ToArray(), node.start, node.end, context));
        }

        public static RunTimeResult visitWhileNode(SyntaxNode node, Context context)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeResult condition = null;
            RunTimeResult expr = null;
            List<Val> elements = new List<Val>();
            while (true)
            {
                condition = result.register(visitNode(((WhileNode)node).conditionNode, context));
                if (result.shouldReturn() == true)
                {
                    return result;
                }
                if (condition.value.isTrue() == false)
                {
                    break;
                }
                expr = result.register(visitNode(((WhileNode)node).expressionNode, context));
                if (expr.shouldReturn() == true &&
                    expr.loopShouldContinue == false &&
                    expr.loopShouldBreak == false)
                {
                    return expr;
                }
                if (result.loopShouldContinue == true)
                {
                    continue;
                }
                if (result.loopShouldBreak == true)
                {
                    break;
                }
                elements.Add(result.value);
            }
            return result.success(new Array(elements.ToArray(), node.start, node.end, context));
        }

        public static RunTimeResult visitDefFuncNode(SyntaxNode node, Context context)
        {
            RunTimeResult result = new RunTimeResult();
            string name = ((DefFuncNode)node).funcName.value;
            SyntaxNode bodyNode = ((DefFuncNode)node).bodyNodes;
            List<string> argNames = new List<string>();
            for (int i = 0; i < ((DefFuncNode)node).argTokens.Count; i++)
            {
                argNames.Add(((DefFuncNode)node).argTokens[i].value);
            }
            Function value = new Function(((DefFuncNode)node).funcName, bodyNode,
                argNames, DATATYPES.DT_FUNCTION, null, null, null,
                ((DefFuncNode)node).shouldAutoReturn);
            value.setContext(context);
            value.setPos(node.start, node.end);

            context.symbols.addSymbol(value.NameToken.value, value);
            result.success(value);
            return result;
        }

        public static RunTimeResult visitCallNode(SyntaxNode node, Context context)
        {
            RunTimeResult result = new RunTimeResult();
            List<Val> args = new List<Val>();
            List<string> argNames = new List<string>();
            RunTimeResult arg = new RunTimeResult();

            BaseFunction valueToCall = (BaseFunction)result.register(visitNode(((CallNode)node).nodeToCall, context)).value;

            valueToCall = makeFunction(valueToCall);
            valueToCall.setContext(context);
            valueToCall.setPos(node.start, node.end);

            for (int i = 0; i < ((CallNode)node).argNodes.Count; i++)
            {
                argNames.Add(valueToCall.ArgNames[i]);
                arg = arg.register(visitNode(((CallNode)node).argNodes[i], context));
                if (arg.error != null)
                {
                    return arg;
                }
                args.Add(arg.value);
            }
            result = result.register(valueToCall.execute(argNames, args));
            if (result.shouldReturn() == true)
            {
                return result;
            }
            return result.success(result.value);
        }

        public static RunTimeResult visitStringNode(SyntaxNode node, Context context)
        {
            RunTimeResult result = new RunTimeResult();
            String str = new String(node.token.value, node.start, node.end, context);
            return result.success(str);
        }

        public static RunTimeResult visitListNode(SyntaxNode node, Context context)
        {
            RunTimeResult result = new RunTimeResult();
            List<Val> elements = new List<Val>();

            for (int i = 0; i < ((ListNode)node).elements.Length; i++)
            {
                result.register(visitNode(((ListNode)node).elements[i], context));
                if (result.shouldReturn() == true)
                {
                    return result;
                }
                elements.Add(result.value);
            }
            return result.success(new Array(elements.ToArray(), node.start, node.end, context));
        }

        public static RunTimeResult visitAccessListNode(SyntaxNode node, Context context)
        {
            RunTimeResult left = visitNode(node.left, context);
            RunTimeResult right = visitNode(node.right, context);
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;

            if (!(left.value is Array))
            {
                error = new RunTimeError("List expected", context,
                node.left.start.fileName, node.left.start.fileText,
                node.left.start.Line, node.left.start.pos.IX,
                node.left.end.pos.IX);
                return result.failure(error);
            }
            if (!(right.value is Number))
            {
                error = new RunTimeError("An int value expected", context,
                node.left.start.fileName, node.left.start.fileText,
                node.left.start.Line, node.left.start.pos.IX,
                node.left.end.pos.IX);
                return result.failure(error);
            }
            return result.success(((Array)left.value).getElement((Number)right.value).value);
        }

        public static RunTimeResult visitAsignListNode(SyntaxNode node, Context context)
        {
            RunTimeResult lis = visitNode(node.left, context);
            RunTimeResult ele = visitNode(node.right, context);
            RunTimeResult value = visitNode(((AsignListNode)node).vNode, context);
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;

            if (!(lis.value is Array))
            {
                error = new RunTimeError("List expected", context,
                node.left.start.fileName, node.left.start.fileText,
                node.left.start.Line, node.left.start.pos.IX,
                node.left.end.pos.IX);
                return result.failure(error);
            }
            if (!(ele.value is Number))
            {
                error = new RunTimeError("An int or list expected", context,
                node.left.start.fileName, node.left.start.fileText,
                node.left.start.Line, node.left.start.pos.IX,
                node.left.end.pos.IX);
                return result.failure(error);
            }
            return result.success(((Array)lis.value).setElement((Number)ele.value, value.value));
        }

        public static RunTimeResult visitRemoveListNode(SyntaxNode node, Context context)
        {
            RunTimeResult lis = visitNode(node.left, context);
            RunTimeResult ele = visitNode(node.right, context);
            RunTimeResult result = new RunTimeResult();
            RunTimeError error = null;

            if (!(lis.value is Array))
            {
                error = new RunTimeError("List expected", context,
                node.left.start.fileName, node.left.start.fileText,
                node.left.start.Line, node.left.start.pos.IX,
                node.left.end.pos.IX);
                return result.failure(error);
            }
            if (!(ele.value is Number))
            {
                error = new RunTimeError("An int value expected", context,
                node.left.start.fileName, node.left.start.fileText,
                node.left.start.Line, node.left.start.pos.IX,
                node.left.end.pos.IX);
                return result.failure(error);
            }
            return result.success((((Array)lis.value).removeElement((Number)ele.value)).value);
        }

        public static RunTimeResult visitReturnNode(SyntaxNode node, Context context)
        {
            RunTimeResult result = new RunTimeResult();
            RunTimeResult value = null;
            if (node != null)
            {
                value = result.register(visitNode(node.right, context));
                value.value.setPos(node.start, node.end);
                value.value.setContext(context);
                if (value.shouldReturn() == true)
                {
                    return value;
                }
            }
            else
            {
                value = new RunTimeResult();
                value.success(Number.Null);
                value.value.setPos(node.start, node.end);
                value.value.setContext(context);
            }
            return result.successReturn(value.value);
        }

        public static RunTimeResult visitContinueNode(SyntaxNode node, Context context)
        {
            RunTimeResult result = new RunTimeResult();
            return result.successContinue();
        }

        public static RunTimeResult visitBreakNode(SyntaxNode node, Context context)
        {
            RunTimeResult result = new RunTimeResult();
            return result.successBreak();
        }

        /// <summary>
        /// Returns true if node is an INT or FLOAT else false
        /// </summary>
        /// <param name="node">SyntaxNode reference</param>
        /// <returns>Boolean</returns>
        public static bool isNumber(SyntaxNode node)
        {
            if(node.token.type == TOKENS.TT_INT)
            {
                return true;
            }
            if(node.token.type == TOKENS.TT_FLOAT)
            {
                return true;
            }
            return false;
        }

        public static DATATYPES getNodeDT(SyntaxNode node)
        {
            switch(node.token.type)
            {
                case TOKENS.TT_FLOAT:
                    return DATATYPES.DT_FLOAT;
                case TOKENS.TT_INT:
                    return DATATYPES.DT_INT;
                case TOKENS.TT_STRING:
                    return DATATYPES.DT_STRING;
            }
            return DATATYPES.DT_INT;
        }

        public static void adjustDataTypes(Val value)
        {
            if(value.Value.Contains(Constants.DIGITS) == true &&
                value.Value.Contains(Constants.LETTERS) == false)
            {
                if(value.Value.Contains(".") == true)
                {
                    value.DT = DATATYPES.DT_FLOAT;
                }
                else
                {
                    value.DT = DATATYPES.DT_INT;
                }
            }
            else
            {
                value.DT = DATATYPES.DT_STRING;
            }
        }

        public static BaseFunction makeFunction(BaseFunction func)
        {
            BaseFunction builtIn = func;
            switch (func.Value)
            {
                case "print":
                    builtIn = new PRINT(func);
                    break;
                case "printRet":
                    builtIn = new PRINT_RET(func);
                    break;
                case "input":
                    builtIn = new INPUT(func);
                    break;
                case "inputInt":
                    builtIn = new INPUT_INT(func);
                    break;
                case "clear":
                    builtIn = new CLEAR(func);
                    break;
                case "isNumber":
                    builtIn = new IS_NUMBER(func);
                    break;
                case "isString":
                    builtIn = new IS_STRING(func);
                    break;
                case "isList":
                    builtIn = new IS_LIST(func);
                    break;
                case "isFunction":
                    builtIn = new IS_FUNCTION(func);
                    break;
                case "append":
                    builtIn = new APPEND(func);
                    break;
                case "pop":
                    builtIn = new POP(func);
                    break;
                case "extend":
                    builtIn = new EXTEND(func);
                    break;
            }
            return builtIn;
        }
    }
}
