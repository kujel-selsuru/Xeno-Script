using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenos
{
    public class SyntaxNode
    {
        public SyntaxNode left;
        public SyntaxNode right;
        public Token token;
        public Position start;
        public Position end;

        public SyntaxNode(Token token)
        {
            left = null;
            right = null;
            this.token = token;
            start = new Position(token.start);
            end = start;
        }
        /// <summary>
        /// Prints out node values
        /// </summary>
        public void print()
        {
            Console.Write("(");
            if (left != null)
            {
                left.print();
            }
            Console.Write(token.value.ToString());
            if (right != null)
            {

                right.print();
            }
            Console.Write(")");
        }
        /// <summary>
        /// Returns the type of token
        /// </summary>
        /// <returns>TOKENS</returns>
        public TOKENS type()
        {
            return token.type;
        }
    }
    public class NumberNode : SyntaxNode
    {
        public NumberNode(Token token) : base(token)
        {

        }
    }
    public class StringNode : SyntaxNode
    {
        public StringNode(Token token) : base(token)
        {

        }
    }
    public class BinOpNode : SyntaxNode
    {
        public BinOpNode(SyntaxNode left, Token token,
            SyntaxNode right) :
            base(token)
        {
            this.left = left;
            this.right = right;
            start = new Position(left.start);
            end = new Position(right.end);
        }
    }
    public class UniOpNode : SyntaxNode
    {
        public UniOpNode(Token token, SyntaxNode node) :
            base(token)
        {
            this.right = node;
            start = new Position(token.start);
            end = new Position(node.end);
        }
    }
    public class VarAccessNode : SyntaxNode
    {
        public string varName;

        public VarAccessNode(Token token) :
            base(token)
        {
            this.varName = token.value;
            start = new Position(token.start);
            end = new Position(token.end);
        }
    }
    public class VarAssignNode : SyntaxNode
    {
        public string varName;

        public VarAssignNode(string varName, Token token, SyntaxNode node) :
            base(token)
        {
            this.varName = varName;
            this.right = node;
            start = new Position(token.start);
            if (node != null)
            {
                end = new Position(node.end);
            }
            else
            {
                end = new Position(start);
            }
        }
    }
    public class IfNode : SyntaxNode
    {
        public List<Tuple<ParseResult, ParseResult>> cases;
        public ParseResult elseCase;
        public ParseResult eCase;

        public IfNode(Token token, List<Tuple<ParseResult, ParseResult>> cases,
            ParseResult elseCase) : base(token)
        {
            if (cases != null)
            {
                this.cases = new List<Tuple<ParseResult, ParseResult>>();
                for (int i = 0; i < cases.Count; i++)
                {
                    ParseResult a = new ParseResult(cases[i].Item1);
                    ParseResult b = new ParseResult(cases[i].Item2);
                    this.cases.Add(new Tuple<ParseResult, ParseResult>(a, b));
                }
            }
            else
            {
                this.cases = cases;
            }
            this.elseCase = elseCase;
            eCase = new ParseResult();
            if (elseCase != null)
            {
                eCase.success(elseCase.node);
            }
            this.elseCase = eCase;
            if (cases.Count > 0)
            {
                this.start = new Position(cases[0].Item1.node.start);
            }
            else if(elseCase != null)
            {
                this.start = new Position(elseCase.node.start);
            }
            else
            {
                this.start = new Position(token.start);
            }
            if (elseCase != null)
            {
                if (elseCase.node != null)
                {
                    this.end = new Position(elseCase.node.end);
                }
                else
                {
                    this.end = new Position(cases[cases.Count - 1].Item1.node.end);
                }
            }
        }
    }
    public class ForNode : SyntaxNode
    {
        public Token varNameToken;
        public NumberNode startValue;
        public NumberNode endValue;
        public NumberNode stepValue;
        public SyntaxNode bodyNode;


        public ForNode(Token token, Token varNameTok, NumberNode startValue,
            NumberNode endValue, NumberNode stepValue, SyntaxNode bodyNode) :
            base(token)
        {
            this.varNameToken = varNameTok;
            this.startValue = startValue;
            this.endValue = endValue;
            this.stepValue = stepValue;
            this.bodyNode = bodyNode;

            start = new Position(varNameTok.start);
            end = new Position(bodyNode.end);
        }
    }
    public class WhileNode : SyntaxNode
    {
        public SyntaxNode conditionNode;
        public SyntaxNode expressionNode;

        public WhileNode(Token token, SyntaxNode conditionNode, 
            SyntaxNode expressionNode) : base(token)
        {
            this.conditionNode = conditionNode;
            this.expressionNode = expressionNode;

            start = new Position(conditionNode.start);
            end = new Position(expressionNode.end);
        }
    }
    public class ArgNode : SyntaxNode
    {
        public string argName;
        public SyntaxNode argValue;

        public ArgNode(Token token, string argName, SyntaxNode argValue) : 
            base(token)
        {
            this.argName = argName;
            this.argValue = argValue;
        }
    }
    public class DefFuncNode : SyntaxNode
    {
        public Token funcName;
        public List<Token> argTokens;
        public SyntaxNode bodyNodes;
        public bool shouldAutoReturn;

        public DefFuncNode(Token token, Token funcName, 
            List<Token> argTokens, SyntaxNode bodyNodes, bool shouldAutoReturn = false) : base(token)
        {
            this.funcName = funcName;
            this.argTokens = argTokens;
            this.bodyNodes = bodyNodes;
            this.shouldAutoReturn = shouldAutoReturn;
            if (argTokens.Count > 0)
            {
                start = new Position(argTokens[0].start);
            }
            else
            {
                start = new Position(funcName.start);
            }
            end = new Position(bodyNodes.end);
            //end = new Position(bodyNodes[bodyNodes.Count - 1].end);
        }
    }
    public class CallNode : SyntaxNode
    {
        public SyntaxNode nodeToCall;
        public List<SyntaxNode> argNodes;

        public CallNode(Token token, SyntaxNode nodeToCall, 
            List<SyntaxNode> argNodes) : base(token)
        {
            this.nodeToCall = nodeToCall;
            this.argNodes = argNodes;

            start = new Position(nodeToCall.start);
            if(argNodes.Count > 0)
            {
                end = new Position(argNodes[argNodes.Count - 1].end);
            }
            else
            {
                end = new Position(nodeToCall.end);
            }
        }
    }
    public class ListNode : SyntaxNode
    {
        public SyntaxNode[] elements;

        public ListNode(Token token, SyntaxNode[] elements, 
            Position start, Position end) : base(token)
        {
            this.elements = new SyntaxNode[elements.Length];
            for(int i = 0; i < elements.Length; i++)
            {
                this.elements[i] = elements[i];
            }
            this.start = new Position(start);
            this.end = new Position(end);
        }
    }
    public class AccessListNode : SyntaxNode
    {
        public AccessListNode(Token token, SyntaxNode lNode, 
            SyntaxNode nNode) : base(token)
        {
            left = lNode;
            right = nNode;
        }
    }
    public class AsignListNode : SyntaxNode
    {
        public SyntaxNode vNode;

        public AsignListNode(Token token, SyntaxNode lNode,
            SyntaxNode nNode, SyntaxNode vNode) : base(token)
        {
            left = lNode;
            right = nNode;
            this.vNode = vNode;
        }
    }
    public class RemoveListNode : SyntaxNode
    {

        public RemoveListNode(Token token, SyntaxNode lNode,
            SyntaxNode nNode) : base(token)
        {
            left = lNode;
            right = nNode;
        }
    }
    public class ReturnNode : SyntaxNode
    {
        public ReturnNode(Token token, SyntaxNode node) :
            base(token)
        {
            this.right = node;
            start = new Position(token.start);
            end = new Position(node.end);
        }
    }
    public class BreakNode : SyntaxNode
    {
        public BreakNode(Token token, Position start) :
            base(token)
        {
            this.start = new Position(start);
            this.end = new Position(start);
        }
    }
    public class ContinueNode : SyntaxNode
    {
        public ContinueNode(Token token, Position start) :
            base(token)
        {
            this.start = new Position(start);
            this.end = new Position(start);
        }
    }

    public class ParseResult
    {
        //public 
        public Error error;
        public SyntaxNode node;
        public int registeredAdvanceCount;
        public int advanceCount;
        public int attemptedAdvances;
        public int toReverseCount;
        public bool shouldReturnNull;

        public ParseResult()
        {
            error = null;
            node = null;
            registeredAdvanceCount = 0;
            advanceCount = 0;
            attemptedAdvances = 0;
            toReverseCount = 0;
            shouldReturnNull = false;
        }

        public ParseResult(ParseResult obj)
        {
            this.error = obj.error;
            this.node = obj.node;
            this.advanceCount = obj.advanceCount;
        }

        public void registerAdvancement()
        {
            registeredAdvanceCount = 1;
            advanceCount++;
        }

        public ParseResult register(ParseResult result)
        {
            if(result.error != null)
            {
                error = result.error;
            }
            registeredAdvanceCount = result.advanceCount;
            advanceCount += result.advanceCount;
            node = result.node;
            return this;
        }

        public ParseResult success(SyntaxNode node, bool shouldReturnNull = false)
        {
            this.node = node;
            this.shouldReturnNull = shouldReturnNull;
            return this;
        }

        public ParseResult failure(Error error)
        {
            this.error = error;
            return this;
        }

        public ParseResult tryRegister(ParseResult result)
        {
            if(result.error != null)
            {
                toReverseCount = result.advanceCount;
                return null;
            }
            return register(result);
        }
    }

    public static class Parser
    {
        //protected
        private static List<Token> tokens;
        private static int tokIdx;
        private static Token curTok;
        private static Error error;

        //public 

        static Parser()
        {
            tokens = null;
            tokIdx = -1;
            curTok = null;
            error = null;
        }

        public static void init(List<Token> toks)
        {
            tokens = toks;
            tokIdx = -1;
            curTok = null;
            advance();
            error = null;
        }
        /// <summary>
        /// Advances current token index and sets curTok
        /// </summary>
        public static void advance()
        {
            tokIdx++;
            updateCurrentToken();
        }

        public static void reverse(int toReverseCount)
        {
            tokIdx -= toReverseCount;
            updateCurrentToken();
        }

        public static void updateCurrentToken()
        {
            if (tokens != null)
            {
                if (tokIdx >= 0 && tokIdx < tokens.Count)
                {
                    curTok = tokens[tokIdx];
                }
            }
        }

        public static SyntaxNode parse(List<Token> toks)
        {
            if (toks.Count > 0)
            {
                tokens = toks;
                tokIdx = -1;
                curTok = null;
                advance();
                error = null;
                Token opTok = curTok;

                ParseResult result = statements(opTok);
                result.registerAdvancement();
                advance();
                if (result.error != null)
                {
                    error = result.error;
                }
                else if(curTok.type != TOKENS.TT_EOF)
                {
                    string illegalSyntax = "Line: " + curTok.start.Line +
                        "Col: " + curTok.start.Column +
                        " Expected '+', '-', '*', or '/'";
                    error = new IllegalSyntaxError(illegalSyntax, curTok.start.context, 
                        curTok.start.fileName, curTok.start.fileText, curTok.start.Line, 
                        curTok.start.Column, curTok.start.Column);
                }
                return result.node;
            }
            else
            {
                return null;
            }
        }

        public static ParseResult atom()
        {
            ParseResult result = new ParseResult();
            Token tok = curTok;
            SyntaxNode node = null;
            if (isTerm(tok) == true)
            {
                advance();
                result.registerAdvancement();
                if (tok.type == TOKENS.TT_STRING)
                {
                    node = new StringNode(tok);
                }
                else
                {
                    node = new NumberNode(tok);
                }
                result.success(node);
                return result;
            }
            else if(curTok.type == TOKENS.TT_IDENTIFIER)
            {
                advance();
                result.registerAdvancement();
                result.register(result);
                result.success(new VarAccessNode(tok));
                return result;
            }
            else if (curTok.type == TOKENS.TT_LPAREN)
            {
                advance();
                result.registerAdvancement();
                result = result.register(expression());
                if (result.error != null)
                {
                    return result;
                }
                if (curTok.type == TOKENS.TT_RPAREN)
                {
                    advance();
                    result.registerAdvancement();
                    return result.success(result.node);
                }
                else
                {
                    return result.failure(syntaxErrorMsg(tok, "Line: " + curTok.start.Line +
                        " Col: " + curTok.start.Column + " Expected ')'"));
                }
            }
            else if(curTok.type == TOKENS.TT_LSQUARE)
            {
                ParseResult lExpr = result.register(listExpr());
                if(lExpr.error != null)
                {
                    return lExpr;
                }
                return result.success(lExpr.node);
            }
            else if(curTok.matches(TOKENS.TT_KEYWORD, "if"))
            {
                result = result.register(ifCases());
                if(result.error != null)
                {
                    return result;
                }
                return result.success(result.node);
            }
            else if (curTok.matches(TOKENS.TT_KEYWORD, "for"))
            {
                result = result.register(forStatement());
                if (result.error != null)
                {
                    return result;
                }
                return result.success(result.node);
            }
            else if (curTok.matches(TOKENS.TT_KEYWORD, "while"))
            {
                result = result.register(whileStatement());
                if (result.error != null)
                {
                    return result;
                }
                return result.success(result.node);
            }
            else if (curTok.matches(TOKENS.TT_KEYWORD, "func"))
            {
                result = result.register(funcDef());
                if (result.error != null)
                {
                    return result;
                }
                return result.success(result.node);
            }
            else if (curTok.matches(TOKENS.TT_KEYWORD, "var"))
            {
                result = result.register(expression());
                if (result.error != null)
                {
                    return result;
                }
                return result.success(result.node);
            }
            return result.failure(syntaxErrorMsg(tok, "Line: " + curTok.start.Line +
                        " Col: " + curTok.start.Column + " Expected INT or FLOAT"));
        }

        public static ParseResult power(SyntaxNode left, Token tok, SyntaxNode right)
        {
            ParseResult result = new ParseResult();
            if (left == null)
            {
                string illegalSyntax = "Identifier, int or float expected";
                result.failure(new IllegalSyntaxError(illegalSyntax, curTok.start.context,
                    curTok.start.fileName, curTok.start.fileText, curTok.start.Line,
                    curTok.start.pos.IX, curTok.end.pos.IX));
                return result;
            }
            if (right == null)
            {
                string illegalSyntax = "Identifier, int or float expected";
                result.failure(new IllegalSyntaxError(illegalSyntax, curTok.start.context,
                    curTok.start.fileName, curTok.start.fileText, curTok.start.Line,
                    curTok.start.pos.IX, curTok.end.pos.IX));
                return result;
            }
            result.success(new BinOpNode(left, tok, right));
            return result;
        }

        public static ParseResult factor()
        {
            ParseResult result = new ParseResult();
            ParseResult result2 = null;
            Token tok = curTok;

            if(tok.matches(TOKENS.TT_KEYWORD, "var"))
            {
                result.registerAdvancement();
                advance();
                tok = curTok;
            }

            if(isAtom(tok) == true)
            {
                if(tok.type == TOKENS.TT_IDENTIFIER)
                {
                    if(tokens[tokIdx + 1].type == TOKENS.TT_LPAREN)
                    {
                        result = call();
                        if(result.error != null)
                        {
                            return result;
                        }
                        result.success(result.node);
                        return result;
                    }
                    result.success(new VarAccessNode(tok));
                    advance();
                    result.registerAdvancement();
                    return result;
                }
                advance();
                result.registerAdvancement();
                result = factor();
                if (result.error != null)
                {
                    return result;
                }
                if (result.node.token.type == TOKENS.TT_IDENTIFIER)
                {
                    return result.success(new VarAccessNode(result.node.token));
                }
                return result.success(new UniOpNode(tok, result.node));
            }
            else if (tok.matches(TOKENS.TT_KEYWORD, "not"))
            {
                Token opTok = curTok;
                advance();
                result.registerAdvancement();
                result = factor();
                if (result.error != null)
                {
                    return result;
                }
                return result.success(new UniOpNode(opTok, result.node));
            }
            else
            {
                result2 = atom();
                tok = curTok;
                if (tok.type == TOKENS.TT_POW)
                {
                    advance();
                    result.registerAdvancement();
                    result2 = power(result2.node, tok, factor().node);
                }
                return result2;
            }
        }

        public static ParseResult term()
        {
            ParseResult result = new ParseResult();
            ParseResult left = factor();
            if(left.error != null)
            {
                return left;
            }
            ParseResult right = null;
            Token opTok = null;

            while(isFactor(curTok) == true)
            {
                opTok = curTok;
                advance();
                result.registerAdvancement();
                right = factor();
                if(right.error != null)
                {
                    return right;
                }
                left.success(new BinOpNode(left.node, opTok, right.node));
            }
            if(left == null)
            {
                string illegalSyntax = "Identifier, int or float expected";
                result.failure(new IllegalSyntaxError(illegalSyntax, curTok.start.context,
                    curTok.start.fileName, curTok.start.fileText, curTok.start.Line,
                    curTok.start.pos.IX, curTok.end.pos.IX));
                return result;
            }
            return left;
        }

        public static ParseResult expression()
        {
            ParseResult result = new ParseResult();
            ParseResult left = term();
            if (left.error != null)
            {
                return left;
            }
            ParseResult right = null;
            Token opTok = null;

            if(curTok.matches(TOKENS.TT_KEYWORD, "var") == true)
            {
                result.register(left);
                advance();
                result.registerAdvancement();
                if(curTok.type != TOKENS.TT_IDENTIFIER)
                {
                    string illegalSyntax = "Identifier expected";
                    result.failure(new IllegalSyntaxError(illegalSyntax, curTok.start.context, 
                        curTok.start.fileName, curTok.start.fileText, curTok.start.Line, 
                        curTok.start.pos.IX, curTok.end.pos.IX));
                    return result;
                }
                opTok = curTok;
                advance();
                result.registerAdvancement();
                if(curTok.type != TOKENS.TT_EQ)
                {
                    string illegalSyntax = "'=' expected";
                    result.failure(new IllegalSyntaxError(illegalSyntax, curTok.start.context,
                        curTok.start.fileName, curTok.start.fileText, curTok.start.Line,
                        curTok.start.pos.IX, curTok.end.pos.IX));
                    return result;
                }
                result.register(expression());
                if(result.error != null)
                {
                    return result;
                }
                return result.success(result.node);
            }
            else if (curTok.matches(TOKENS.TT_KEYWORD, "not"))
            {
                opTok = curTok;
                result.registerAdvancement();
                advance();

                ParseResult node = compExpression();
                if (node.error != null)
                {
                    return node;
                }
                return result.success(new UniOpNode(opTok, node.node));
            }
            else if (curTok.type == TOKENS.TT_EQ)
            {
                Token prevTok = tokens[tokIdx - 1];
                if (prevTok.type != TOKENS.TT_IDENTIFIER)
                {
                    string illegalSyntax = "Identifier expected";
                    result.failure(new IllegalSyntaxError(illegalSyntax, prevTok.start.context,
                        prevTok.start.fileName, prevTok.start.fileText, prevTok.start.Line,
                        prevTok.start.pos.IX, prevTok.end.pos.IX));
                    return result;
                }
                advance();
                result.registerAdvancement();
                result.register(result);
                result.success(new VarAssignNode(left.node.token.value, curTok, expression().node));
                return result;
            }
            else if(curTok.type == TOKENS.TT_COLON)
            {
                opTok = curTok;
                result.registerAdvancement();
                advance();
                result.register(result);
                result.success(new AccessListNode(opTok, left.node, expression().node));
                return result;
            }
            else if (curTok.type == TOKENS.TT_DCOLON)
            {
                opTok = curTok;
                result.registerAdvancement();
                advance();
                right = expression();
                result.register(result);
                result.success(new AsignListNode(opTok, left.node, right.node, expression().node));
                return result;
            }
            else if (curTok.type == TOKENS.TT_BAR)
            {
                opTok = curTok;
                result.registerAdvancement();
                advance();
                right = expression();
                result.register(result);
                result.success(new RemoveListNode(opTok, left.node, right.node));
                return result;
            }
            else if (isComparison(curTok.type) == true)
            {
                opTok = curTok;
                result.registerAdvancement();
                advance();
                right = expression();
                if (right.error != null)
                {
                    return right;
                }
                return left.success(new BinOpNode(left.node, opTok, right.node));
            }

            while(isAtom(curTok) == true)
            {
                opTok = curTok;
                right = term();
                if (right.error != null)
                {
                    return right;
                }
                left.success(new BinOpNode(left.node, opTok, right.node));
            }
            return left;
        }

        public static ParseResult compExpression()
        {
            ParseResult result = new ParseResult();
            ParseResult node = new ParseResult();
            ParseResult left = null;
            ParseResult right = null;
            
            if(curTok.type == TOKENS.TT_EE)
            {
                left = arithExpr();
                if(left.error != null)
                {
                    return left;
                }
                advance();
                right = arithExpr();
                if (right.error != null)
                {
                    return right;
                }
                return node.success(new BinOpNode(left.node, curTok, right.node));
            }
            else if (curTok.type == TOKENS.TT_NE)
            {
                left = arithExpr();
                if (left.error != null)
                {
                    return left;
                }
                advance();
                right = arithExpr();
                if (right.error != null)
                {
                    return right;
                }
                return node.success(new BinOpNode(left.node, curTok, right.node));
            }
            else if (curTok.type == TOKENS.TT_LT)
            {
                left = arithExpr();
                if (left.error != null)
                {
                    return left;
                }
                advance();
                right = arithExpr();
                if (right.error != null)
                {
                    return right;
                }
                return node.success(new BinOpNode(left.node, curTok, right.node));
            }
            else if (curTok.type == TOKENS.TT_GT)
            {
                left = arithExpr();
                if (left.error != null)
                {
                    return left;
                }
                advance();
                right = arithExpr();
                if (right.error != null)
                {
                    return right;
                }
                return node.success(new BinOpNode(left.node, curTok, right.node));
            }
            else if (curTok.type == TOKENS.TT_LTE)
            {
                left = arithExpr();
                if (left.error != null)
                {
                    return left;
                }
                advance();
                right = arithExpr();
                if (right.error != null)
                {
                    return right;
                }
                return node.success(new BinOpNode(left.node, curTok, right.node));
            }
            else if (curTok.type == TOKENS.TT_GTE)
            {
                left = arithExpr();
                if (left.error != null)
                {
                    return left;
                }
                advance();
                right = arithExpr();
                if (right.error != null)
                {
                    return right;
                }
                return node.success(new BinOpNode(left.node, curTok, right.node));
            }
            string compError = "expected '==', '!=', '<', '<=', '>' or '>='";
            return node.failure(syntaxErrorMsg(curTok, compError));
        }

        public static ParseResult arithExpr()
        {
            ParseResult result = new ParseResult();
            ParseResult left = term();
            if (left.error != null)
            {
                return left;
            }
            Token opTok = curTok;
            result.registerAdvancement();
            advance();
            ParseResult right = term();
            if (right.error != null)
            {
                return right;
            }
            return result.success(new BinOpNode(left.node, opTok, right.node));
        }

        public static ParseResult ifExpression()
        {
            ParseResult result = new ParseResult();
            //result = ifExprCases("if");
            result = ifCases();
            ParseResult allCases = result.register(result);
            return result.success(allCases.node);
        }

        public static ParseResult ifCases()
        {
            ParseResult result = new ParseResult();
            ParseResult condition = new ParseResult();
            ParseResult expr = new ParseResult();
            List<Tuple<ParseResult, ParseResult>> cases =
                new List<Tuple<ParseResult, ParseResult>>();
            ParseResult elseCase = new ParseResult();
            Tuple<ParseResult, ParseResult> tup = null;
            Token opTok = curTok;

            if (curTok.matches(TOKENS.TT_KEYWORD, "if") == true)
            {
                result.registerAdvancement();
                advance();

                condition.register(statement());
                if(condition.error != null)
                {
                    return condition;
                }

                if(curTok.matches(TOKENS.TT_KEYWORD, "then") == false)
                {
                    string ifError = "expected 'then' ";
                    return result.failure(syntaxErrorMsg(curTok, ifError));
                }

                result.registerAdvancement();
                advance();

                if(curTok.type != TOKENS.TT_LBRACE)
                {
                    string ifError = "expected '{' ";
                    return result.failure(syntaxErrorMsg(curTok, ifError));
                }

                result.registerAdvancement();
                advance();

                expr.register(statements(opTok));
                if(expr.error != null)
                {
                    return expr;
                }

                while(curTok.type == TOKENS.TT_SEMI ||
                curTok.type == TOKENS.TT_NEWLINE)
                {
                    result.registerAdvancement();
                    advance();
                }

                if(curTok.type != TOKENS.TT_RBRACE)
                {
                    string ifError = "expected '}' ";
                    return result.failure(syntaxErrorMsg(curTok, ifError));
                }

                tup = new Tuple<ParseResult, ParseResult>(
                    new ParseResult(condition), new ParseResult(expr));
                cases.Add(tup);
            }

            result.registerAdvancement();
            advance();

            while(curTok.matches(TOKENS.TT_KEYWORD, "elif") == true)
            {
                result.registerAdvancement();
                advance();

                condition.register(statement());
                if (condition.error != null)
                {
                    return condition;
                }

                if (curTok.matches(TOKENS.TT_KEYWORD, "then") == false)
                {
                    string ifError = "expected 'then' ";
                    return result.failure(syntaxErrorMsg(curTok, ifError));
                }

                result.registerAdvancement();
                advance();

                if (curTok.type != TOKENS.TT_LBRACE)
                {
                    string ifError = "expected '{' ";
                    return result.failure(syntaxErrorMsg(curTok, ifError));
                }

                result.registerAdvancement();
                advance();

                expr.register(statements(opTok));
                if (expr.error != null)
                {
                    return expr;
                }

                while (curTok.type == TOKENS.TT_SEMI ||
                curTok.type == TOKENS.TT_NEWLINE)
                {
                    result.registerAdvancement();
                    advance();
                }

                if (curTok.type != TOKENS.TT_RBRACE)
                {
                    string ifError = "expected '}' ";
                    return result.failure(syntaxErrorMsg(curTok, ifError));
                }

                tup = new Tuple<ParseResult, ParseResult>(
                    new ParseResult(condition), new ParseResult(expr));
                cases.Add(tup);

                result.registerAdvancement();
                advance();
            }

            if (curTok.matches(TOKENS.TT_KEYWORD, "else") == true)
            {
                result.registerAdvancement();
                advance();

                if (curTok.type != TOKENS.TT_LBRACE)
                {
                    string ifError = "expected '{' ";
                    return result.failure(syntaxErrorMsg(curTok, ifError));
                }

                result.registerAdvancement();
                advance();

                expr.register(statements(opTok));
                if (expr.error != null)
                {
                    return expr;
                }

                while (curTok.type == TOKENS.TT_SEMI ||
                curTok.type == TOKENS.TT_NEWLINE)
                {
                    result.registerAdvancement();
                    advance();
                }
                
                if (curTok.type != TOKENS.TT_RBRACE)
                {
                    string ifError = "expected '}' ";
                    return result.failure(syntaxErrorMsg(curTok, ifError));
                }
                
                elseCase = result.register(expr);
            }
            return result.success(new IfNode(opTok, cases, elseCase));
        }

        public static ParseResult forStatement()
        {
            ParseResult result = new ParseResult();
            ParseResult expr = new ParseResult();
            Token varNameTok = null;
            NumberNode startValue = null;
            NumberNode endValue = null;
            NumberNode stepValue = null;
            Token opTok = curTok;

            if (curTok.matches(TOKENS.TT_KEYWORD, "for") == true)
            {
                result.registerAdvancement();
                advance();
            }

            if(curTok.type == TOKENS.TT_IDENTIFIER)
            {
                varNameTok = curTok;
            }
            else
            {
                string err = "Expected identifier";
                return result.failure(syntaxErrorMsg(curTok, err));
            }

            result.registerAdvancement();
            advance();

            if(curTok.type != TOKENS.TT_EQ)
            {
                string err = "Expected '='";
                return result.failure(syntaxErrorMsg(curTok, err));
            }

            result.registerAdvancement();
            advance();

            expr = result.register(expression());
            if (expr.error != null)
            {
                return expr;
            }
            startValue = new NumberNode(expr.node.token);

            if (curTok.matches(TOKENS.TT_KEYWORD, "to") == false)
            {
                string ifError = "expected 'to' ";
                return result.failure(syntaxErrorMsg(curTok, ifError));
            }

            result.registerAdvancement();
            advance();

            expr = result.register(expression());
            if (expr.error != null)
            {
                return expr;
            }
            endValue = new NumberNode(expr.node.token);

            if (curTok.matches(TOKENS.TT_KEYWORD, "step") == false)
            {
                string ifError = "expected 'step' ";
                return result.failure(syntaxErrorMsg(curTok, ifError));
            }

            result.registerAdvancement();
            advance();

            expr = result.register(expression());
            if (expr.error != null)
            {
                return expr;
            }
            stepValue = new NumberNode(expr.node.token);

            if (curTok.matches(TOKENS.TT_KEYWORD, "then") == false)
            {
                string ifError = "expected 'then' ";
                return result.failure(syntaxErrorMsg(curTok, ifError));
            }

            result.registerAdvancement();
            advance();

            if (curTok.type != TOKENS.TT_LBRACE)
            {
                string ifError = "expected '{' ";
                return result.failure(syntaxErrorMsg(curTok, ifError));
            }

            result.registerAdvancement();
            advance();

            expr = result.register(statements(opTok));
            if (expr.error != null)
            {
                return expr;
            }

            while (curTok.type == TOKENS.TT_SEMI ||
            curTok.type == TOKENS.TT_NEWLINE)
            {
                result.registerAdvancement();
                advance();
            }
            
            if (curTok.type != TOKENS.TT_RBRACE)
            {
                string ifError = "expected '}' ";
                return result.failure(syntaxErrorMsg(curTok, ifError));
            }
            return result.success(new ForNode(opTok, varNameTok, startValue, endValue, stepValue, expr.node));
        }

        public static ParseResult whileStatement()
        {
            ParseResult result = new ParseResult();
            ParseResult condition = new ParseResult();
            SyntaxNode con = null;
            ParseResult expr = new ParseResult();
            Token opTok = curTok;

            if (curTok.matches(TOKENS.TT_KEYWORD, "while") == true)
            {
                result.registerAdvancement();
                advance();
            }

            condition = result.register(expression());
            if (condition.error != null)
            {
                return condition;
            }
            con = condition.node;

            if (curTok.matches(TOKENS.TT_KEYWORD, "then") == false)
            {
                string ifError = "expected 'then' ";
                return result.failure(syntaxErrorMsg(curTok, ifError));
            }

            result.registerAdvancement();
            advance();

            if (curTok.type != TOKENS.TT_LBRACE)
            {
                string ifError = "expected '{' ";
                return result.failure(syntaxErrorMsg(curTok, ifError));
            }

            result.registerAdvancement();
            advance();

            expr = result.register(statements(opTok));
            if (expr.error != null)
            {
                return expr;
            }
            
            if (curTok.type != TOKENS.TT_RBRACE)
            {
                string ifError = "expected '}' ";
                return result.failure(syntaxErrorMsg(curTok, ifError));
            }
            
            result.registerAdvancement();
            advance();

            return result.success(new WhileNode(opTok, con, expr.node));
        }

        public static bool isControlFlow()
        {
            if(curTok.matches(TOKENS.TT_KEYWORD, "if") == true)
            {
                return true;
            }
            if (curTok.matches(TOKENS.TT_KEYWORD, "for") == true)
            {
                return true;
            }
            if (curTok.matches(TOKENS.TT_KEYWORD, "while") == true)
            {
                return true;
            }
            return false;
        }

        /*
        public ParseResult ifExprCases(string keyword)
        {
            ParseResult result = new ParseResult();
            ParseResult condition = new ParseResult();
            ParseResult expr = new ParseResult();
            ParseResult exprTmp = null;
            List<Tuple<ParseResult, ParseResult>> cases =
                new List<Tuple<ParseResult, ParseResult>>();
            ParseResult elseCase = new ParseResult();
            ParseResult elifExpr = new ParseResult();
            ParseResult allCases = null;
            Token opTok = curTok;
            Tuple<ParseResult, ParseResult> tup = null;

            if (curTok.matches(TOKENS.TT_KEYWORD, keyword) == false)
            {
                string ifError = "expected '" + keyword + "' ";
                return result.failure(syntaxErrorMsg(curTok, ifError));
            }
            result.registerAdvancement();
            advance();

            condition = condition.register(statement());
            if(condition.error != null)
            {
                return condition;
            }

            if(curTok.matches(TOKENS.TT_KEYWORD, "then") == false)
            {
                string ifError = "expected 'then' ";
                return result.failure(syntaxErrorMsg(curTok, ifError));
            }

            result.registerAdvancement();
            advance();

            if(curTok.type != TOKENS.TT_LBRACE)
            {
                string ifError = "expected '{' ";
                return result.failure(syntaxErrorMsg(curTok, ifError));
            }

            result.registerAdvancement();
            advance();

            if (curTok.type == TOKENS.TT_SEMI ||
                curTok.type == TOKENS.TT_NEWLINE)
            {
                result.registerAdvancement();
                advance();
                
                expr = result.register(statements(opTok));
                if(expr.error != null)
                {
                    return expr;
                }
                tup = new Tuple<ParseResult, ParseResult>(condition, expr);
                cases.Add(tup);

                if(curTok.type == TOKENS.TT_RBRACE)
                {
                    result.registerAdvancement();
                    advance();
                }
                else
                {
                    allCases = result.register(ifExprBorC(opTok));
                    if(result.error != null)
                    {
                        return allCases;
                    }
                    for(int i = 0; i < ((IfNode)allCases.node).cases.Count; i++)
                    {
                        cases.Add(((IfNode)allCases.node).cases[i]);
                    }
                }
            }
            else
            {
                exprTmp = result.register(statements(opTok));
                if(exprTmp.error != null)
                {
                    return exprTmp;
                }
                tup = new Tuple<ParseResult, ParseResult>(new ParseResult(condition), new ParseResult(exprTmp));
                cases.Add(tup);
                
                if(curTok.type == TOKENS.TT_RBRACE)
                {
                    result.registerAdvancement();
                    advance();
                }
                while(curTok.matches(TOKENS.TT_KEYWORD, "elif") == true)
                {

                }
                if (allCases != null)
                {
                    if (allCases.node is IfNode)
                    {
                        for (int i = 0; i < ((IfNode)allCases.node).cases.Count; i++)
                        {
                            cases.Add(((IfNode)allCases.node).cases[i]);
                        }
                    }
                    else
                    {
                        elseCase = allCases;
                    }
                }
            }
            SyntaxNode tmp = new IfNode(opTok, cases, elseCase);
            return result.success(tmp);
        }

        public ParseResult ifExprB()
        {
            return ifExprCases("elif");
        }

        public ParseResult ifExprC()
        {
            ParseResult result = new ParseResult();
            ParseResult lines = new ParseResult();
            ParseResult expr = new ParseResult();
            ParseResult elseCase = new ParseResult();
            Token opTok = curTok;

            if(curTok.matches(TOKENS.TT_KEYWORD, "else"))
            {
                result.registerAdvancement();
                advance();

                if(curTok.type == TOKENS.TT_LBRACE)
                {
                    result.registerAdvancement();
                    advance();
                }

                lines = result.register(statements(opTok));
                if(lines.error != null)
                {
                    return lines;
                }
                elseCase = lines;
                if (curTok.type != TOKENS.TT_RBRACE)
                {
                    string ifError = "expected '}' ";
                    return result.failure(syntaxErrorMsg(curTok, ifError));
                }
                result.registerAdvancement();
                advance();
            }
            else
            {
                expr = result.register(statements(opTok));
                if(expr.error != null)
                {
                    return expr;
                }
                elseCase = expr;
            }
            return result.success(elseCase.node);
        }

        public ParseResult ifExprBorC(Token opTok)
        {
            ParseResult result = new ParseResult();
            List<Tuple<ParseResult, ParseResult>> cases =
                new List<Tuple<ParseResult, ParseResult>>();
            ParseResult elseCase = null;

            if(curTok.matches(TOKENS.TT_KEYWORD, "elif") == true)
            {
                result = result.register(ifExprB());
                if(result.error != null)
                {
                    return result;
                }
                for (int i = 0; i < ((IfNode)result.node).cases.Count; i++)
                {
                    cases.Add(((IfNode)result.node).cases[i]);
                }
            }
            else if(curTok.matches(TOKENS.TT_KEYWORD, "else") == true)
            {
                elseCase = result.register(ifExprC());
                return elseCase;
            }
            return result.success(new IfNode(opTok, cases, elseCase));
        }
        */

        public static ParseResult forExpression()
        {
            ParseResult result = new ParseResult();
            Token opTok = curTok;
            Token varNameTok = null;
            NumberNode startValue = null;
            NumberNode endValue = null;
            NumberNode stepValue = null;

            if(curTok.matches(TOKENS.TT_KEYWORD, "for") == false)
            {
                string forError = "Expected 'for' ";
                return result.failure(syntaxErrorMsg(curTok, forError));
            }

            result.registerAdvancement();
            advance();

            if(curTok.type != TOKENS.TT_IDENTIFIER)
            {
                string forError = "Expected identifier ";
                return result.failure(syntaxErrorMsg(curTok, forError));
            }

            varNameTok = curTok;
            result.registerAdvancement();
            advance();

            if(curTok.type != TOKENS.TT_EQ)
            {
                string forError = "Expected '=' ";
                return result.failure(syntaxErrorMsg(curTok, forError));
            }

            result.registerAdvancement();
            advance();

            result = result.register(statement());
            if(result.error != null)
            {
                return result;
            }
            startValue = new NumberNode(result.node.token);

            if(curTok.matches(TOKENS.TT_KEYWORD, "to") == false)
            {
                string forError = "Expected 'to' ";
                return result.failure(syntaxErrorMsg(curTok, forError));
            }

            result.registerAdvancement();
            advance();

            result = result.register(statement());
            if(result.error != null)
            {
                return result;
            }
            endValue = new NumberNode(result.node.token);

            if(curTok.matches(TOKENS.TT_KEYWORD, "step") == true)
            {
                result.registerAdvancement();
                advance();
                result = result.register(statement());
                if (result.error != null)
                {
                    return result;
                }
                stepValue = new NumberNode(result.node.token);
            }
            else
            {
                stepValue = null;
            }

            if(curTok.matches(TOKENS.TT_KEYWORD, "then") == false)
            {
                string forError = "Expected 'then' ";
                return result.failure(syntaxErrorMsg(curTok, forError));
            }

            result.registerAdvancement();
            advance();

            if (curTok.type != TOKENS.TT_LBRACE)
            {
                string forError = "Expected '{' ";
                return result.failure(syntaxErrorMsg(curTok, forError));
            }
            
            while(curTok.type == TOKENS.TT_SEMI || 
                curTok.type == TOKENS.TT_NEWLINE)
            {
                result.registerAdvancement();
                advance();
            }

            result.registerAdvancement();
            advance();

            result = result.register(statements(opTok));
            if(result.error != null)
            {
                return result;
            }

            if(curTok.type != TOKENS.TT_RBRACE)
            {
                string funcError = "Expected '}' ";
                return result.failure(syntaxErrorMsg(curTok, funcError));
            }

            result.success(new ForNode(opTok, varNameTok, startValue, 
                endValue, stepValue, result.node));
            return result;
        }

        public static ParseResult whileExpression()
        {
            ParseResult result = new ParseResult();
            Token opTok = curTok;
            ParseResult condition = null;
            ParseResult expr = null;

            if (curTok.matches(TOKENS.TT_KEYWORD, "while") == false)
            {
                string forError = "Expected 'while' ";
                return result.failure(syntaxErrorMsg(curTok, forError));
            }

            result.registerAdvancement();
            advance();

            condition = statement();
            if(condition.error != null)
            {
                return condition;
            }

            if (curTok.matches(TOKENS.TT_KEYWORD, "then") == false)
            {
                string forError = "Expected 'then' ";
                return result.failure(syntaxErrorMsg(curTok, forError));
            }

            result.registerAdvancement();
            advance();

            if (curTok.type != TOKENS.TT_LBRACE)
            {
                string funcError = "Expected '{' ";
                return result.failure(syntaxErrorMsg(curTok, funcError));
            }

            result.registerAdvancement();
            advance();

            while(curTok.type == TOKENS.TT_NEWLINE ||
                curTok.type == TOKENS.TT_SEMI)
            {
                result.registerAdvancement();
                advance();
            }

            expr = statements(opTok);
            if (expr.error != null)
            {
                return expr;
            }

            if (curTok.type != TOKENS.TT_RBRACE)
            {
                string funcError = "Expected '}' ";
                return result.failure(syntaxErrorMsg(curTok, funcError));
            }

            result = result.success(new WhileNode(opTok, condition.node, expr.node));
            return result;
        }

        public static ParseResult funcDef()
        {
            ParseResult result = new ParseResult();
            Token opTok = curTok;
            Token funcName = null;
            List<Token> argTokens = new List<Token>();
            ParseResult bodyNode = null;

            if (curTok.matches(TOKENS.TT_KEYWORD, "func") == false)
            {
                string funcError = "Expected 'func' ";
                return result.failure(syntaxErrorMsg(curTok, funcError));
            }

            result.registerAdvancement();
            advance();

            if(curTok.type != TOKENS.TT_IDENTIFIER)
            {
                string funcError = "Expected identifier ";
                return result.failure(syntaxErrorMsg(curTok, funcError));
            }

            funcName = curTok;

            result.registerAdvancement();
            advance();

            if (curTok.type != TOKENS.TT_LPAREN)
            {
                string funcError = "Expected '(' ";
                return result.failure(syntaxErrorMsg(curTok, funcError));
            }

            result.registerAdvancement();
            advance();

            if (curTok.type == TOKENS.TT_IDENTIFIER)
            {
                argTokens.Add(curTok);
                result.registerAdvancement();
                advance();


                while (curTok.type == TOKENS.TT_COMMA)
                {
                    result.registerAdvancement();
                    advance();

                    if (curTok.type != TOKENS.TT_IDENTIFIER)
                    {
                        string funcError = "Expected identifier ";
                        return result.failure(syntaxErrorMsg(curTok, funcError));
                    }
                    argTokens.Add(curTok);
                    result.registerAdvancement();
                    advance();
                }

                if (curTok.type != TOKENS.TT_RPAREN)
                {
                    string funcError = "Expected ',' or ')' ";
                    return result.failure(syntaxErrorMsg(curTok, funcError));
                }
            }
            else
            {
                if (curTok.type != TOKENS.TT_RPAREN)
                {
                    string funcError = "Expected ',' or ')' ";
                    return result.failure(syntaxErrorMsg(curTok, funcError));
                }
            }

            result.registerAdvancement();
            advance();

            if (curTok.type != TOKENS.TT_LBRACE)
            {
                string funcError = "Expected '{' ";
                return result.failure(syntaxErrorMsg(curTok, funcError));
            }

            result.registerAdvancement();
            advance();

            bodyNode = statements(opTok);
            if(bodyNode.error != null)
            {
                return bodyNode;
            }

            if (curTok.type != TOKENS.TT_RBRACE)
            {
                string funcError = "Expected '}' ";
                return result.failure(syntaxErrorMsg(curTok, funcError));
            }

            result.registerAdvancement();
            advance();

            result.success(new DefFuncNode(opTok, funcName, argTokens, bodyNode.node, false));
            return result;
        }

        public static ParseResult call()
        {
            ParseResult result = new ParseResult();
            ParseResult node = new ParseResult();
            ParseResult atomed = new ParseResult();
            Token opTok = curTok;
            List<SyntaxNode> argNodes = new List<SyntaxNode>();
            atomed = atomed.register(atom());
            if(atomed.error != null)
            {
                return atomed;
            }

            if(curTok.type == TOKENS.TT_LPAREN)
            {
                result.registerAdvancement();
                advance();

                if(curTok.type == TOKENS.TT_RPAREN)
                {
                    result.registerAdvancement();
                    advance();
                }
                else
                {
                    node = result.register(expression());
                    argNodes.Add(node.node);
                    if(result.error != null)
                    {
                        string callError = "Expected ')', 'if', 'for', 'while', 'func', int, float or identifier ";
                        return result.failure(syntaxErrorMsg(curTok, callError));
                    }

                    while(curTok.type == TOKENS.TT_COMMA)
                    {
                        result.registerAdvancement();
                        advance();

                        node = result.register(expression());
                        argNodes.Add(node.node);
                        if (result.error != null)
                        {
                            string callError = "Expected ')', 'if', 'for', 'while', 'func', int, float or identifier ";
                            return result.failure(syntaxErrorMsg(curTok, callError));
                        }
                    }
                }
                /*
                if (curTok.type != TOKENS.TT_RPAREN)
                {
                    string callError = "Expected ',' or ')' ";
                    return result.failure(syntaxErrorMsg(curTok, callError));
                }
                */

                result.registerAdvancement();
                advance();

                return result.success(new CallNode(opTok, atomed.node, argNodes));
            }
            return result.success(atomed.node);
        }

        public static ParseResult listExpr()
        {
            ParseResult result = new ParseResult();
            ParseResult node = null;
            Token opTok = curTok;
            List<SyntaxNode> elements = new List<SyntaxNode>();
            Position start = new Position(curTok.start);
            IllegalSyntaxError error = null;

            if(curTok.type != TOKENS.TT_LSQUARE)
            {
                error = new IllegalSyntaxError("Expected '[' ", 
                    curTok.start.context, curTok.start.fileName, 
                    curTok.start.fileText, curTok.start.Line, 
                    curTok.start.pos.IX, curTok.end.pos.IX);
            }

            result.registerAdvancement();
            advance();

            if(curTok.type == TOKENS.TT_RSQUARE)
            {
                result.registerAdvancement();
                advance();
            }
            else
            {
                node = result.register(expression());
                elements.Add(node.node);
                if (result.error != null)
                {
                    string callError = "Expected ']', 'if', 'for', 'while', 'func', int, float or identifier ";
                    return result.failure(syntaxErrorMsg(curTok, callError));
                }

                while (curTok.type == TOKENS.TT_COMMA)
                {
                    result.registerAdvancement();
                    advance();

                    node = result.register(expression());
                    elements.Add(node.node);
                    if (result.error != null)
                    {
                        string callError = "Expected ']', 'if', 'for', 'while', 'func', int, float or identifier ";
                        return result.failure(syntaxErrorMsg(curTok, callError));
                    }
                }

                if (curTok.type != TOKENS.TT_RSQUARE)
                {
                    error = new IllegalSyntaxError("Expected ']' ",
                        curTok.start.context, curTok.start.fileName,
                        curTok.start.fileText, curTok.start.Line,
                        curTok.start.pos.IX, curTok.end.pos.IX);
                }

                result.registerAdvancement();
                advance();

            }
            ListNode list = new ListNode(opTok, elements.ToArray(), start, curTok.end);
            return result.success(list);
        }

        public static ParseResult statements(Token opTok)
        {
            ParseResult result = new ParseResult();
            ParseResult state = null;
            List<ParseResult> lines = new List<ParseResult>();
            List<SyntaxNode> nodes = new List<SyntaxNode>();
            Position start = new Position(curTok.start);

            while(curTok.type == TOKENS.TT_SEMI || 
                curTok.type == TOKENS.TT_NEWLINE)
            {
                result.registerAdvancement();
                advance();
            }

            state = result.register(statement());
            lines.Add(new ParseResult(state));
            if (state.error != null)
            {
                return state;
            }

            bool moreStatements = true;
            while(true)
            {
                int newLines = 0;
                while(curTok.type == TOKENS.TT_SEMI || 
                    curTok.type == TOKENS.TT_NEWLINE)
                {
                    result.registerAdvancement();
                    advance();
                    newLines++;
                }
                if(newLines == 0)
                {
                    moreStatements = false;
                }
                if(moreStatements == false)
                {
                    break;
                }
                state = result.tryRegister(statement());
                if(state == null)
                {
                    reverse(result.toReverseCount);
                    moreStatements = false;
                    continue;
                }
                lines.Add(new ParseResult(state));
            }
            for(int i = 0; i < lines.Count; i++)
            {
                nodes.Add(lines[i].node);
            }
            
            return result.success(new ListNode(opTok, nodes.ToArray(), start, curTok.end));
        }

        public static ParseResult instructions()
        {
            ParseResult result = new ParseResult();
            ParseResult expr = null;
            Position start = new Position(curTok.start);
            List<SyntaxNode> lines = new List<SyntaxNode>();
            Token opTok = curTok;

            while(curTok.type != TOKENS.TT_RBRACE)
            {
                if(curTok.matches(TOKENS.TT_KEYWORD, "if") == true)
                {
                    expr = ifCases();
                    if(expr.error != null)
                    {
                        return expr;
                    }
                    lines.Add(expr.node);
                }
                if (curTok.matches(TOKENS.TT_KEYWORD, "for") == true)
                {
                    expr = forExpression();
                    if (expr.error != null)
                    {
                        return expr;
                    }
                    lines.Add(expr.node);
                }
                if (curTok.matches(TOKENS.TT_KEYWORD, "while") == true)
                {
                    expr = whileExpression();
                    if (expr.error != null)
                    {
                        return expr;
                    }
                    lines.Add(expr.node);
                }
                expr = expression();
                if(curTok.type != TOKENS.TT_SEMI)
                {
                    string error = "Expected ';' ";
                    return result.failure(syntaxErrorMsg(curTok, error));
                }
                if (expr.error != null)
                {
                    return expr;
                }
                lines.Add(expr.node);
            }
            return result.success(new ListNode(opTok, lines.ToArray(), opTok.start, curTok.end));
        }

        public static ParseResult statement()
        {
            ParseResult result = new ParseResult();
            ParseResult expr = null;
            Position start = new Position(curTok.start);
            Token opTok = curTok;

            if(curTok.matches(TOKENS.TT_KEYWORD, "return"))
            {
                result.registerAdvancement();
                advance();

                expr = result.tryRegister(expression());
                if(expr == null)
                {
                    reverse(result.toReverseCount);
                }
                return result.success(new ReturnNode(opTok, expr.node));
            }

            if(curTok.matches(TOKENS.TT_KEYWORD, "continue"))
            {
                result.registerAdvancement();
                advance();
                return result.success(new ContinueNode(opTok, start));
            }

            if(curTok.matches(TOKENS.TT_KEYWORD, "break"))
            {
                result.registerAdvancement();
                advance();
                return result.success(new BreakNode(opTok, start));
            }

            expr = result.register(expression());
            if(expr.error != null)
            {
                string error = "Illegal syntax: Expected 'return', 'continue'," +
                    "'break', 'if', 'for', 'while', 'int', 'float', 'identifier'," +
                    "'+', '-', '*', '/', '^', '=', '{', '(', '{', or 'not'";
                return result.failure(syntaxErrorMsg(opTok, error));
            }
            return result.success(expr.node);
        }
        /// <summary>
        /// Returns true if token type is TT_INT, TT_FLOAT or TT_STRING
        /// </summary>
        /// <param name="token">Token reference</param>
        /// <returns>Boolean</returns>
        public static bool isTerm(Token token)
        {
            if(token.type == TOKENS.TT_INT)
            {
                return true;
            }
            if (token.type == TOKENS.TT_FLOAT)
            {
                return true;
            }
            if(token.type == TOKENS.TT_STRING)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns true if token type is TT_MUL or TT_DIV
        /// </summary>
        /// <param name="token">Token reference</param>
        /// <returns>Boolean</returns>
        public static bool isFactor(Token token)
        {
            if (token.type == TOKENS.TT_MUL)
            {
                return true;
            }
            if (token.type == TOKENS.TT_DIV)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns true if token is +, -, *, / or ^
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Boolean</returns>
        public static bool isArith(Token token)
        {
            if (token.type == TOKENS.TT_PLUS)
            {
                return true;
            }
            if (token.type == TOKENS.TT_MINUS)
            {
                return true;
            }
            if (token.type == TOKENS.TT_MUL)
            {
                return true;
            }
            if (token.type == TOKENS.TT_DIV)
            {
                return true;
            }
            if (token.type == TOKENS.TT_POW)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns true if token type is TT_PLUS or TT_MINUS
        /// </summary>
        /// <param name="token">Token reference</param>
        /// <returns>Boolean</returns>
        public static bool isAtom(Token token)
        {
            if (token.type == TOKENS.TT_PLUS)
            {
                return true;
            }
            if (token.type == TOKENS.TT_MINUS)
            {
                return true;
            }
            if (token.type == TOKENS.TT_IDENTIFIER)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if a token type is a comparison type
        /// </summary>
        /// <param name="type">TOKENS value</param>
        /// <returns>Boolean</returns>
        public static bool isComparison(TOKENS type)
        {
            if(type == TOKENS.TT_EE)
            {
                return true;
            }
            else if (type == TOKENS.TT_GT)
            {
                return true;
            }
            else if (type == TOKENS.TT_GTE)
            {
                return true;
            }
            else if (type == TOKENS.TT_LT)
            {
                return true;
            }
            else if (type == TOKENS.TT_LTE)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if a token's operation matches a spcified type
        /// </summary>
        /// <param name="token">Token to check</param>
        /// <param name="type">Type of token operation</param>
        /// <returns>Boolean</returns>
        public static bool operationMatches(Token token, TOKENS type)
        {
            if(token.type == type)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Generates syntax error
        /// </summary>
        /// <param name="tok">Token reference</param>
        /// <param name="msg">details string</param>
        /// <returns>Error object</returns>
        public static Error syntaxErrorMsg(Token tok, string msg)
        {
            return new IllegalSyntaxError(msg, tok.start.context, 
                tok.start.fileName, tok.start.fileText, tok.start.Line, 
                tok.start.Column, tok.end.Column);
        }
        /// <summary>
        /// Clears the current error
        /// </summary>
        public static void clearError()
        {
            error = null;
        }
        /// <summary>
        /// Error property
        /// </summary>
        public static Error Err
        {
            get { return error; }
            set { error = value; }
        }
    }
}
