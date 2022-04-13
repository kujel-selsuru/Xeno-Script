using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XenoLib;

namespace Xenos
{

    public static class Constants
    {
        static string digits;
        static string letters;
        static string letters_digits;

        //public
        static Constants()
        {
            digits = "0123456789.";
            letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            letters_digits = letters + digits;
        }

        /// <summary>
        /// Digits property
        /// </summary>
        public static string DIGITS
        {
            get { return digits; }
        }
        /// <summary>
        /// letters property
        /// </summary>
        public static string LETTERS
        {
            get { return letters; }
        }
        /// <summary>
        /// letters_digits property
        /// </summary>
        public static string LETTERS_DIGITS
        {
            get { return letters_digits; }
        }
    }

    public enum TOKENS
    {
        TT_INT,
        TT_FLOAT,
        TT_STRING,
        TT_PLUS,
        TT_MINUS,
        TT_MUL,
        TT_DIV,
        TT_POW,
        TT_EQ,
        TT_LPAREN,
        TT_RPAREN,
        TT_EE,
        TT_NE,
        TT_LT,
        TT_GT,
        TT_LTE,
        TT_GTE,
        TT_EOF,
        TT_KEYWORD,
        TT_IDENTIFIER,
        TT_COMMA,
        TT_LBRACE,
        TT_RBRACE,
        TT_LSQUARE,
        TT_RSQUARE,
        TT_COLON,
        TT_DCOLON,
        TT_SEMI,
        TT_BAR,
        TT_NEWLINE,
        TT_HASH
    }

    public class Token
    {
        //public
        public TOKENS type;
        public string value;
        public Position start;
        public Position end;
        /// <summary>
        /// Token constructor
        /// </summary>
        /// <param name="type">Token type</param>
        /// <param name="value">Token value</param>
        /// <param name="start">Start of Token</param>
        /// <param name="end">End of Token</param>
        public Token(TOKENS type, string value, Position start, Position end)
        {
            this.type = type;
            this.value = value;
            if(start != null)
            {
                this.start = new Position(start);
                this.end = new Position(start);
                this.end.pos.IX += 1;
                this.end.index += 1;
            }

            if(end != null)
            {
                this.end = end;
            }
        }
        /// <summary>
        /// Prints token members
        /// </summary>
        public void print()
        {
            Console.Write("(" + type.ToString() + ": " + value + ") ");
        }
        /// <summary>
        /// Returns true if token is an int or float else returns 
        /// false
        /// </summary>
        /// <returns>Boolean</returns>
        public bool isNumber()
        {
            if(type == TOKENS.TT_INT)
            {
                return true;
            }
            if(type == TOKENS.TT_FLOAT)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if a token's type and value match the parameters
        /// </summary>
        /// <param name="type">TOKENS value</param>
        /// <param name="value">String value</param>
        /// <returns>Boolean</returns>
        public bool matches(TOKENS type, string value)
        {
            if(this.type == type && this.value == value)
            {
                return true;
            }
            return false;
        }
    }

    public class Position
    {
        //public
        public Point2D pos;
        public Context context;
        public int index;
        public string fileName;
        public string fileText;
        /// <summary>
        /// Position constructor
        /// </summary>
        /// <param name="context">Context value</param>
        /// <param name="fileName">File name value</param>
        /// <param name="fileText">File text value</param>
        /// <param name="c">Column value</param>
        /// <param name="l">Line value</param>
        /// <param name="index">Index value</param>
        public Position(Context context = null, string fileName = "", 
            string fileText = "", int c = -1, int l = 0, int index = -1)
        {
            this.context = context;
            pos = new Point2D(c, l);
            this.index = index;
            this.fileName = fileName;
            this.fileText = fileText;
        }
        /// <summary>
        /// Position copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public Position(Position obj)
        {
            context = obj.context;
            pos = new Point2D(obj.pos.IX, obj.pos.IY);
            index = obj.index;
            fileName = obj.fileName;
            fileText = obj.fileText;
        }
        /// <summary>
        /// Advances the position by one character
        /// </summary>
        /// <param name="currentChar">Current character value</param>
        public void advance(char currentChar)
        {
            pos.IX++;
            index++;
            if(currentChar == '\0')
            {
                pos.IY++;
                pos.IX = 0;
            }
        }
        /// <summary>
        /// Line property
        /// </summary>
        public int Line
        {
            get { return pos.IY; }
            set { pos.IY = value; }
        }
        /// <summary>
        /// Column property
        /// </summary>
        public int Column
        {
            get { return pos.IX; }
            set { pos.IX = value; }
        }
    }

    public static class Lexer
    {
        //protected
        private static string text;
        private static Position pos;
        private static char currentChar;
        private static Dictionary<char, char> escapeChars;

        private static string[] keywords =
        {
            "var",
            "if",
            "then",
            "else",
            "elif",
            "for",
            "step",
            "to",
            "while",
            "and",
            "or",
            "not",
            "func",
            "return",
            "break",
            "continue"
        };
        //public
        public static Error error;
        static Lexer()
        {
        }
        public static void init(Context context, string fileName, string txt)
        { 
            text = txt;
            pos = new Position(context, fileName, text);
            currentChar = '\0';
            error = null;
            escapeChars = new Dictionary<char, char>();
            escapeChars.Add('n', '\n');
            escapeChars.Add('t', '\t');
        }
        /// <summary>
        /// Advances the lexer position
        /// </summary>
        public static void advance()
        {
            pos.advance(currentChar);
            if(pos.index < text.Length)
            {
                currentChar = text[pos.index];
            }
            else
            {
                currentChar = '\0';
            }
        }
        /// <summary>
        /// Processes the current line of text
        /// </summary>
        /// <param name="text">String of text to process</param>
        /// <returns>List of token objects</returns>
        public static List<Token> processLine(string txt)
        {
            text = txt;
            pos.Column = -1;
            pos.index = -1;
            pos.Line = 0;
            error = null;
            advance();
            return makeTokens();
        }
        /// <summary>
        /// Makes a list of tokens from a string of text
        /// </summary>
        /// <returns>List of Token objects</returns>
        public static List<Token> makeTokens()
        {
            List<Token> tokens = new List<Token>();
            while(currentChar != '\0')
            {
                if(currentChar == ' ')
                {
                    advance();
                }
                else if (currentChar == '\t')
                {
                    advance();
                }
                else if (currentChar == '#')
                {
                    skipComment();
                }
                else if (Constants.DIGITS.Contains(currentChar) == true)
                {
                    tokens.Add(makeNumber());
                }
                else if (Constants.LETTERS_DIGITS.Contains(currentChar) == true)
                {
                    tokens.Add(makeIdentifier());
                }
                else if (currentChar == '"')
                {
                    tokens.Add(makeString());
                }
                else if (currentChar == '+')
                {
                    tokens.Add(new Token(TOKENS.TT_PLUS, "PLUS", new Position(pos), new Position(pos)));
                    advance();
                }
                else if (currentChar == '-')
                {
                    tokens.Add(new Token(TOKENS.TT_MINUS, "MINUS", new Position(pos), new Position(pos)));
                    advance();
                }
                else if (currentChar == '*')
                {
                    tokens.Add(new Token(TOKENS.TT_MUL, "MUL", new Position(pos), new Position(pos)));
                    advance();
                }
                else if (currentChar == '/')
                {
                    tokens.Add(new Token(TOKENS.TT_DIV, "DIV", new Position(pos), new Position(pos)));
                    advance();
                }
                else if (currentChar == '^')
                {
                    tokens.Add(new Token(TOKENS.TT_POW, "POW", new Position(pos), new Position(pos)));
                    advance();
                }
                else if (currentChar == ':')
                {
                    tokens.Add(makeColon());
                }
                else if (currentChar == ';')
                {
                    tokens.Add(new Token(TOKENS.TT_SEMI, "SEMI", new Position(pos), new Position(pos)));
                    advance();
                }
                else if (currentChar == '\n')
                {
                    tokens.Add(new Token(TOKENS.TT_NEWLINE, "NEWLINE", new Position(pos), new Position(pos)));
                    advance();
                }
                else if (currentChar == '|')
                {
                    tokens.Add(new Token(TOKENS.TT_BAR, "BAR", new Position(pos), new Position(pos)));
                    advance();
                }
                else if (currentChar == '(')
                {
                    tokens.Add(new Token(TOKENS.TT_LPAREN, "LPAREN", new Position(pos), new Position(pos)));
                    advance();
                }
                else if (currentChar == ')')
                {
                    tokens.Add(new Token(TOKENS.TT_RPAREN, "RPAREN", new Position(pos), new Position(pos)));
                    advance();
                }
                else if (currentChar == '[')
                {
                    tokens.Add(new Token(TOKENS.TT_LSQUARE, "LSQUARE", new Position(pos), new Position(pos)));
                    advance();
                }
                else if (currentChar == ']')
                {
                    tokens.Add(new Token(TOKENS.TT_RSQUARE, "RSQUARE", new Position(pos), new Position(pos)));
                    advance();
                }
                else if (currentChar == ',')
                {
                    tokens.Add(new Token(TOKENS.TT_COMMA, "COMMA", new Position(pos), new Position(pos)));
                    advance();
                }
                else if (currentChar == '{')
                {
                    tokens.Add(new Token(TOKENS.TT_LBRACE, "LBRACE", new Position(pos), new Position(pos)));
                    advance();
                }
                else if (currentChar == '}')
                {
                    tokens.Add(new Token(TOKENS.TT_RBRACE, "RBRACE", new Position(pos), new Position(pos)));
                    advance();
                }
                else if (currentChar == ',')
                {
                    tokens.Add(new Token(TOKENS.TT_COMMA, "COMMA", new Position(pos), new Position(pos)));
                    advance();
                }
                else if(currentChar == '!')
                {
                    Tuple<Token, Error> tup = makeNotEquals();
                    if(tup.Item2 != null)
                    {
                        error = charErrorMsg(pos, "Expected '=' " + "Line: " + (pos.Line - 1).ToString() +
                        " Col: " + (pos.Column).ToString());
                        return null;
                    }
                    tokens.Add(tup.Item1);
                }
                else if (currentChar == '=')
                {
                    tokens.Add(makeEquals());
                }
                else if (currentChar == '>')
                {
                    tokens.Add(makeGreaterThan());
                }
                else if (currentChar == '<')
                {
                    tokens.Add(makeLessThan());
                }
                else
                {
                    Position p = new Position(pos);
                    pos.advance(currentChar);
                    error = charErrorMsg(pos, "'" + currentChar.ToString() + 
                        "' " + "Line: " + (pos.Line - 1).ToString() + 
                        " Col: " + (pos.Column).ToString());
                    return null;
                }
            }
            tokens.Add(new Token(TOKENS.TT_EOF, "", pos, pos));
            return tokens;
        }
        /// <summary>
        /// Makes a number token
        /// </summary>
        /// <returns>Token</returns>
        public static Token makeNumber()
        {
            Position tmp = new Position(pos);
            string numStr = "";
            int dotsCount = 0;
            Token token = null;
            while(currentChar != '\0' &&
                Constants.DIGITS.Contains(currentChar) == true)
            {
                if(currentChar == '.')
                {
                    dotsCount++;
                    if(dotsCount > 1)
                    {
                        break;
                    }
                }
                numStr += currentChar;
                advance();
            }
            if(dotsCount == 0 )
            {
                token = new Token(TOKENS.TT_INT, numStr, tmp, new Position(pos));
            }
            else
            {
                token = new Token(TOKENS.TT_FLOAT, numStr, tmp, new Position(pos));
            }
            return token;
        }
        /// <summary>
        /// Makes keyword and identifier tokens
        /// </summary>
        /// <returns>Token object</returns>
        public static Token makeIdentifier()
        {
            string str = "";
            Position start = new Position(pos);
            Token tok = null;
            while(currentChar != '\0' &&
                Constants.LETTERS_DIGITS.Contains(currentChar) == true)
            {
                str += currentChar;
                advance();
            }
            if(checkKeywords(str) == true)
            {
                tok = new Token(TOKENS.TT_KEYWORD, str, start, new Position(pos));
            }
            else
            {
                tok = new Token(TOKENS.TT_IDENTIFIER, str, start, new Position(pos));
            }
            return tok;
        }
        /// <summary>
        /// Returns true if value provided is a keyword else returns 
        /// false
        /// </summary>
        /// <param name="value">String value to check</param>
        /// <returns>Boolean</returns>
        public static bool checkKeywords(string value)
        {
            for(int i = 0; i < keywords.Length; i++)
            {
                if(value == keywords[i])
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Returns a tuple with either a Token or Error depending
        /// on if successfully generated a not equals token
        /// </summary>
        /// <returns>Tuple of Token and Error</returns>
        public static Tuple<Token, Error> makeNotEquals()
        {
            Position start = new Position(pos);
            advance();
            if(currentChar == '=')
            {
                Token tok = new Token(TOKENS.TT_NE, "!=", start, new Position(pos));
                return new Tuple<Token, Error>(tok, null);
            }
            error = charErrorMsg(pos, "'=' " + "Line: " + (pos.Line - 1).ToString() +
                        " Col: " + (pos.Column).ToString());
            return new Tuple<Token, Error>(null, error);
        }
        /// <summary>
        /// Makes either an asign or equals token depending on 
        /// if first equals sign fallowed by second equals sign
        /// </summary>
        /// <returns>Token</returns>
        public static Token makeEquals()
        {
            Position start = new Position(pos);
            advance();
            Token tok = null;
            if (currentChar == '=')
            {
                tok = new Token(TOKENS.TT_EE, "==", start, new Position(pos));
                advance();
                return tok;
            }
            tok = new Token(TOKENS.TT_EQ, "=", start, new Position(pos));
            return tok;
        }
        /// <summary>
        /// Makes either a greater than or greater than or equals 
        /// token depending on if greater than sign fallowed by 
        /// equals sign
        /// </summary>
        /// <returns>Token</returns>
        public static Token makeGreaterThan()
        {
            Position start = new Position(pos);
            advance();
            Token tok = null;
            if (currentChar == '=')
            {
                tok = new Token(TOKENS.TT_GTE, ">=", start, new Position(pos));
                advance();
            }
            tok = new Token(TOKENS.TT_GT, ">", start, new Position(pos));
            return tok;
        }
        /// <summary>
        /// Makes either a less than or less than or equals token 
        /// depending on if less than sign fallowed by equals
        /// sign
        /// </summary>
        /// <returns>Token</returns>
        public static Token makeLessThan()
        {
            Position start = new Position(pos);
            advance();
            Token tok = null;
            if (currentChar == '=')
            {
                tok = new Token(TOKENS.TT_LTE, "<=", start, new Position(pos));
                advance();
            }
            tok = new Token(TOKENS.TT_LT, "<", start, new Position(pos));
            return tok;
        }
        /// <summary>
        /// Makes a string token
        /// </summary>
        /// <returns>Token object</returns>
        public static Token makeString()
        {
            string str = "";
            Position start = new Position(pos);
            bool escapeChar = false;
            advance();
            
            while(currentChar != '\0' && (currentChar != '"' || escapeChar == true))
            {
                if (escapeChar == true)
                {
                    char escape;
                    escapeChars.TryGetValue(currentChar, out escape);
                    if(escape == '\0')
                    {
                        escape = currentChar;
                    }
                    str += escape;
                }
                else
                {
                    if (currentChar == '\\')
                    {
                        escapeChar = true;
                    }
                    else
                    {
                        str += currentChar;
                        advance();
                    }
                }
                escapeChar = false;
            }

            advance();
            return new Token(TOKENS.TT_STRING, str, start, pos);
        }
        /// <summary>
        /// Makes an array access token
        /// </summary>
        /// <returns></returns>
        public static Token makeColon()
        {
            Position start = new Position(pos);
            advance();
            Token tok = null;
            if (currentChar == ':')
            {
                tok = new Token(TOKENS.TT_DCOLON, "::", start, new Position(pos));
                advance();
                return tok;
            }
            tok = new Token(TOKENS.TT_COLON, ":", start, new Position(pos));
            return tok;
        }
        /// <summary>
        /// skips all fallowing characters until a new line character
        /// is encountered
        /// </summary>
        public static void skipComment()
        {
            advance();
            
            while(currentChar != '\n' && currentChar != '#')
            {
                if (currentChar == '\\')
                {
                    advance();
                    if(currentChar == 'n')
                    {
                        break;
                    }
                }
                advance();
            }

            advance();
        }
        /// <summary>
        /// Generates char error
        /// </summary>
        /// <param name="pos">Position reference</param>
        /// <param name="msg">details string</param>
        /// <returns>Error object</returns>
        public static Error charErrorMsg(Position pos, string msg)
        {
            return new IllegalCharError(msg, pos.context,
                pos.fileName, pos.fileText, pos.Line,
                pos.Column, pos.Column);
        }
        /// <summary>
        /// Generates char error
        /// </summary>
        /// <param name="pos">Position reference</param>
        /// <param name="msg">details string</param>
        /// <returns>Error object</returns>
        public static Error expectedErrorMsg(Position pos, string msg)
        {
            return new ExpectedCharError(msg, pos.context,
                pos.fileName, pos.fileText, pos.Line,
                pos.Column, pos.Column);
        }
    }
}
