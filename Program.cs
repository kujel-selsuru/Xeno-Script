using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XenoLib;

namespace Xenos
{
    class Program
    {
        public static void addArrows(int start, int end)
        {
            Console.WriteLine();
            string arrows = "";
            for(int i = 0; i < start; i++)
            {
                arrows += " ";
            }
            for (int i = start; i < end + 1; i++)
            {
                arrows += "^";
            }
            Console.WriteLine(arrows);
        }
        public static void addBuiltinFunctions(SymbolTable globalSymbols, Context context)
        {
            Token tmpTok = null;
            List<string> argNames = new List<string>();
            Position start = new Position();
            Position end = new Position();

            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "print", start, end);
            argNames.Add("str");
            globalSymbols.addSymbol("print", new PRINT(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "printRet", start, end);
            argNames.Clear();
            argNames.Add("str");
            globalSymbols.addSymbol("printRet", new PRINT_RET(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "input", start, end);
            argNames.Clear();
            globalSymbols.addSymbol("input", new INPUT(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "inputInt", start, end);
            argNames.Clear();
            globalSymbols.addSymbol("inputInt", new INPUT_INT(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "clear", start, end);
            argNames.Clear();
            globalSymbols.addSymbol("clear", new CLEAR(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "isNumber", start, end);
            argNames.Clear();
            argNames.Add("num");
            globalSymbols.addSymbol("isNumber", new IS_NUMBER(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "isString", start, end);
            argNames.Clear();
            argNames.Add("str");
            globalSymbols.addSymbol("isString", new IS_STRING(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "isList", start, end);
            argNames.Clear();
            argNames.Add("str");
            globalSymbols.addSymbol("isList", new IS_LIST(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "isFunction", start, end);
            argNames.Clear();
            argNames.Add("func");
            globalSymbols.addSymbol("isFunction", new IS_FUNCTION(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "append", start, end);
            argNames.Clear();
            argNames.Add("value");
            globalSymbols.addSymbol("append", new APPEND(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "pop", start, end);
            argNames.Clear();
            globalSymbols.addSymbol("pop", new POP(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "extend", start, end);
            argNames.Clear();
            argNames.Add("listA");
            argNames.Add("listB");
            globalSymbols.addSymbol("extend", new EXTEND(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "run", start, end);
            argNames.Clear();
            argNames.Add("fileName");
            globalSymbols.addSymbol("run", new RUN(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "length", start, end);
            argNames.Clear();
            argNames.Add("list");
            globalSymbols.addSymbol("length", new LENGTH(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "createFile", start, end);
            argNames.Clear();
            globalSymbols.addSymbol("createFile", new CREATEFILE(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "writeLine", start, end);
            argNames.Clear();
            argNames.Add("file");
            argNames.Add("str");
            globalSymbols.addSymbol("writeLine", new    WRITELINE(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "readLine", start, end);
            argNames.Clear();
            argNames.Add("file");
            globalSymbols.addSymbol("readLine", new READLINE(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "openFile", start, end);
            argNames.Clear();
            argNames.Add("file");
            globalSymbols.addSymbol("openFile", new OPENFILE(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "closeFile", start, end);
            argNames.Clear();
            argNames.Add("file");
            globalSymbols.addSymbol("closeFile", new CLOSEFILE(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "createFile", start, end);
            argNames.Clear();
            globalSymbols.addSymbol("createFile", new CREATEFILE(tmpTok, argNames, start, end, context));
        }

        static void Main(string[] args)
        {
            string text = "";
            
            SymbolTable globalSymbols = new SymbolTable();
            globalSymbols.addSymbol("null", Number.Null);
            globalSymbols.addSymbol("true", Number.True);
            globalSymbols.addSymbol("false", Number.False);
            
            Context context = new Context("<Program>", globalSymbols, null, new Position(null, "<Program>"));
            addBuiltinFunctions(globalSymbols, context);
            //Lexer lexer = new Lexer(context,"Xenos", text);
            Lexer.init(context, "Xenos", text);
            //Parser parser = new Parser(null);
            Parser.init(null);
            //Interpreter interpreter = new Interpreter();
            RunTimeResult result = null;
            while(true)
            {
                text = Console.ReadLine();
                //Console.WriteLine(text);
                List<Token> tokens = Lexer.processLine(text);
                if(tokens != null)
                {
                    SyntaxNode ast = Parser.parse(tokens);
                    if(Parser.Err == null)
                    {
                        if(ast != null)
                        {
                            result = Interpreter.visitNode(ast, context);
                            if (result != null)
                            {
                                if (result.error != null)
                                {
                                    addArrows(result.start.pos.IX, result.end.pos.IX);
                                    Console.WriteLine(result.error.asString());
                                    Console.WriteLine();
                                }
                                else
                                {
                                    if (result.value != null)
                                    {
                                        result.value.print();
                                        Console.WriteLine();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Loop done");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if(Parser.Err != null)
                            {
                                addArrows(Parser.Err.pos.pos.IX, Parser.Err.pos.pos.IX);
                                Console.WriteLine(Parser.Err.asString());
                            }
                        }
                    }
                    else
                    {

                        addArrows(Parser.Err.pos.pos.IX, Parser.Err.pos.pos.IX);
                        Console.WriteLine(Parser.Err.asString());
                    }
                }
                else
                {

                    addArrows(Lexer.error.pos.pos.IX, Lexer.error.pos.pos.IX);
                    Console.WriteLine(Lexer.error.asString());
                }
                
            }
        }
    }
}
