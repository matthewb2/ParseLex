
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

            Interpreter interpreter = new Interpreter(text);
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


        string __str__() {
        
        return String.Format("Token({0}, {1})", this.type, this.value);
        
        }

        string __repr__() {

            return this.__str__();

        }


    }







    public class Interpreter
    {
        public string text;
        public int pos;
        public string current_char;
        public Token current_token;


        public Interpreter(string text)
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
            while (this.current_char != null && string.IsNullOrWhiteSpace(this.current_char))
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

            while (this.current_char!=null) {
               // Console.WriteLine(this.pos);
                if (string.IsNullOrWhiteSpace(this.current_char))
                {
                    this.skip_whitespace();
                    continue;
                } else if (Regex.IsMatch(this.current_char, @"^\d{1}$"))
                {

                    token = new Token("INTEGER", this.integer());
                  

                    return token;
                }
                else if (this.current_char.Contains('+'))
                {
                    this.advance();
                    token = new Token("PLUS", this.current_char);

                    return token;
                } else if (this.current_char.Contains('-'))
                {
                    this.advance();

                    token = new Token("MINUS", this.current_char);
                    return token;
                }
                else error();
            }
            return token;
           
        }

        public void eat(string token_type)
        {
            /* compare the current token type with the passed token
            // type and if they match then "eat" the current token
            // and assign the next token to the self.current_token,
            // otherwise raise an exception.
            */
            if (this.current_token.type == token_type)
            {
                this.current_token = this.get_next_token();
            }
            else error();

            
        }

        public int expr()
        {
            this.current_token = this.get_next_token();


            Token left = this.current_token;
            //Console.WriteLine(left.value);
            this.eat("INTEGER");


            Token op = this.current_token;

            if (op.type == "PLUS")
            {
                this.eat("PLUS");
            } else  {
                this.eat("MINUS");
            }

            Token right = this.current_token;
            //Console.WriteLine(right.value);


            this.eat("INTEGER");

            int result;
            if (op.type == "PLUS") {
                
                result = Convert.ToInt32(left.value) + Convert.ToInt32(right.value);

            }
            else {
                result = Convert.ToInt32(left.value) - Convert.ToInt32(right.value);

            }
           
            return result;
        
        }

    }



}