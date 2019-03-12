
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

        public Token current_token;


        public Interpreter(string text)
        {
            this.pos = 0;
            this.text = text;
            
        }

        public void error()
        {
            Console.WriteLine("Error parsing input");
        }




        public Token get_next_token()
        {
            /* Lexical analyzer (also known as scanner or tokenizer)
            This method is responsible for breaking a sentence
            apart into tokens.One token at a time.
            */

            Token token = new Token();

            string text = this.text;

            /* is self.pos index past the end of the self.text ?
            #if so, then return EOF token because there is no more
            # input left to convert into tokens
            */

            if (this.pos > text.Length - 1)
            {
                token = new Token("EOF",null);
                return token; 
            }
            /*
            # get a character at the position self.pos and decide
            # what token to create based on the single character
            */
            string current_char = this.text.Substring(this.pos, 1);

            /*
            #if the character is a digit then convert it to
            # integer, create an INTEGER token, increment self.pos
            # index to point to the next character after the digit,
            # and return the INTEGER token
            */

            if (Regex.IsMatch(current_char, @"^\d{1}$"))
            {

                token = new Token("INTEGER", Convert.ToInt32(current_char));
                this.pos += 1;

                return token;
            }
            else if (current_char.Contains('+'))
            {

                token = new Token("PLUS", current_char);

                this.pos += 1;
                return token;
            }
            else error();

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
            this.eat("INTEGER");


            Token op = this.current_token;
            this.eat("PLUS");

            Token right = this.current_token;
            this.eat("INTEGER");
            int result = Convert.ToInt32(left.value) + Convert.ToInt32(right.value);

            return result;
        
        }

    }



}