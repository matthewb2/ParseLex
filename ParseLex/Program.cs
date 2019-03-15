
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ParserLex
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("calc> ");
            string text = Console.ReadLine();

            Lexer lexer = new Lexer(text);
            Interpreter interpreter = new Interpreter(lexer);
            //Console.WriteLine(text);

            int result = interpreter.expr();
            Console.WriteLine(result);

        }
    }



    public class Token
    {
        public string type;
        public string value;

        public Token()
        {
        }
        public Token(string type, string value)
        {

            this.type = type;
            this.value = value;
        }

        public Token(string type, int value)
        {

            this.type = type;
            this.value = value.ToString();
        }


        public string __str__()
        {

            return String.Format("Token({0}, {1})", this.type, this.value);

        }

        public string __repr__()
        {

            return this.__str__();

        }


    }







    public class Lexer
    {
        public string text;
        public int pos;
        public string current_char;
        public Token current_token;


        public Lexer(string text)
        {
            this.pos = 0;
            this.text = text;
            this.current_char = this.text.Substring(this.pos, 1);
            this.current_token = new Token();


        }

        public void error()
        {
            Console.WriteLine("Error parsing input");
        }

        public void advance()
        {
            this.pos += 1;
            Token token = new Token();
            if (this.pos > text.Length - 1)
            {
                this.current_char = null;

            }
            else
            {
                this.current_char = this.text.Substring(this.pos, 1);
            }
        }
        public void skip_whitespace()
        {
            while (this.current_char != null && string.IsNullOrEmpty(this.current_char))
            {
                this.advance();
            }
        }

        public string integer()
        {
            string result = "";

            while (this.current_char != null && Regex.IsMatch(this.current_char, @"^\d+$"))
            {
                result += this.current_char;
                //Console.WriteLine(result);
                this.advance();

            }

            return result;
        }

        public Token get_next_token()
        {
            /* Lexical analyzer (also known as scanner or tokenizer)
            This method is responsible for breaking a sentence
            apart into tokens.One token at a time.
            */

            Token token = new Token();


            //Console.WriteLine(this.pos);
            //Console.WriteLine(this.current_char);

            while (this.current_char != null)
            {
                // Console.WriteLine(this.pos);
                if (string.IsNullOrEmpty(this.current_char))
                {
                    this.skip_whitespace();
                    continue;
                }
                else if (Regex.IsMatch(this.current_char, @"^\d{1}$"))
                {

                    token = new Token("INTEGER", this.integer());


                    return token;
                }
                else if (this.current_char.Contains('*'))
                {
                    this.advance();
                    token = new Token("MUL", "*");
                    return token;
                }
                else if (this.current_char.Contains('/'))
                {
                    this.advance();
                    token = new Token("DIV", "/");
                    return token;
                }
                else if (this.current_char.Contains('+'))
                {
                    this.advance();
                    token = new Token("PLUS", "+");
                    return token;
                }
                else if (this.current_char.Contains('-'))
                {
                    this.advance();
                    token = new Token("MINUS", "-");
                    return token;
                }
                else if (this.current_char.Contains('('))
                {
                    this.advance();
                    token = new Token("LPAREN", "(");
                    return token;
                }
                else if (this.current_char.Contains(')'))
                {
                    this.advance();
                    token = new Token("RPAREN", ")");
                    return token;
                }
                else error();
            }
            return token;

        }
    }
    public class Interpreter
    {
        Lexer lexer;
        Token current_token;

        public Interpreter(Lexer lexer)
        {
            this.lexer = lexer;
            this.current_token = this.lexer.get_next_token();
        }
        public void error()
        {

        }
        /*#########################################################
        # Parser / Interpreter code                              #
        #########################################################*/
        public void eat(string token_type)
        {
            /* compare the current token type with the passed token
            // type and if they match then "eat" the current token
            // and assign the next token to the self.current_token,
            // otherwise raise an exception.
            */
            if (this.current_token.type == token_type)
            {
                this.current_token = this.lexer.get_next_token();
            }
            else error();


        }

        public int factor()
        {
            //factor : INTEGER | LPAREN expr RPAREN"""

            Token token;
            int result;

            token = this.current_token;

            if (token.type == "INTEGER")
            {
                eat("INTEGER");
                return Convert.ToInt32(token.value);
            }
            else if (token.type == "LPAREN")
            {
                eat("LPAREN");
                result = expr();
                eat("RPAREN");
                return result;
            }
            return 0;
        }

        public int term()
        {
            //this.current_token = this.lexer.get_next_token();

            int result = factor();

            while (current_token.type == "MUL" || current_token.type == "DIV")
            {

                Token token = current_token;
                if (token.type == "MUL")
                {
                    eat("MUL");
                    result = result * factor();
                }
                else if (token.type == "DIV")
                {
                    eat("DIV");
                    result = result / factor();
                }
            }


            return result;

        }

        public int expr()
        {
            //this.current_token = this.lexer.get_next_token();

            int result = term();

            while (current_token.type == "PLUS" || current_token.type == "MINUS")
            {

                Token token = current_token;
                if (token.type == "PLUS")
                {
                    eat("PLUS");
                    result = result + term();
                }
                else if (token.type == "MINUS")
                {
                    eat("MINUS");
                    result = result - term();
                }
            }


            return result;

        }

    }



}
