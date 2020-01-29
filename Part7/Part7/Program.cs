using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace binop
{




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

            while (this.current_char != null)
            {
                // Console.WriteLine(this.pos);
                if (string.IsNullOrWhiteSpace(this.current_char))
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
                    token = new Token("RPAREN", "(");
                    return token;
                }
                else error();
            }
            return token;

        }
    }




    class Program
    {
        public const string INTEGER = "INTEGER";
        public const string MUL = "MUL";
        public const string PLUS = "PLUS";

        static void Main(string[] args)
        {
                       
            //input string
            String text = "2+7*3";
            Lexer lexer = new Lexer(text);
            Parser parser = new Parser(lexer);
            Interpreter interpreter = new Interpreter(parser);

            
            int result = interpreter.interpret();
                      
            Console.WriteLine(result);


        }
    }

    public class Interpreter
    {
        //BinOp node;
        private Parser parser;

        public Interpreter(Parser tmp)
        {
            this.parser = tmp;

        }

        public int visit(BinOp node)
        {
            if (node.op != null)
                return visit_BinOp(node);
            else
                return visit_Num(node);

        }
        public int visit_BinOp(BinOp node)
        {
            if (node.op.type == "PLUS")
            {
                // Console.WriteLine("ddd");
                return visit(node.left) + visit(node.right);
            }
            else if (node.op.type == "MINUS") return visit(node.left) - visit(node.right);
            else if (node.op.type == "MUL")
            {
                // Console.WriteLine("eee");
                return visit(node.left) * visit(node.right);

            }
            else if (node.op.type == "DIV") return visit(node.left) / visit(node.right);
            return 0;
        }

        public int visit_Num(BinOp node)
        {
            //Console.WriteLine(node.num.value);
            return Convert.ToInt32(node.num.value);
        }

        public int interpret()
        {
            BinOp tree = this.parser.parse();
            return visit(tree);
        }

    }


    public class Num
    {
        public Token token;
        public string value;

        public Num(Token token)
        {
            this.token = token;
            this.value = token.value.ToString();

        }
    }


    public class BinOp
    {
        public Num num;
        public Token op;
        public BinOp left, right;

        public BinOp(Num num)
        {
            this.num = num;
        }
        public BinOp(Token op)
        {
            this.op = op;
        }
        // similar to add_node
        public BinOp(BinOp left, Token op, BinOp right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }
        public void add_node(BinOp left, BinOp right)
        {
            this.left = left;
            this.right = right;

        }

        /* Given a binary tree, print 
           its nodes in preorder*/
        public void travsalDF(BinOp root)
        {
            // depth-first using a stack
            Stack<BinOp> s = new Stack<BinOp>();
            s.Push(root);
            while (s.Count > 0)
            {
                BinOp n = s.Pop();
                if (n.num != null) Console.WriteLine(n.num.value);
                if (n.op != null) Console.WriteLine(n.op.__repr__());
                if (n.left != null)
                    s.Push(n.left);
                if (n.right != null)
                    s.Push(n.right);
            }
        }



    }

    

    public class Parser
    {
        Lexer lexer;
        Token current_token;

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
            this.current_token = this.lexer.get_next_token();
        }
        public void error()
        {

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
                this.current_token = this.lexer.get_next_token();
            }
            else error();


        }

        public BinOp factor()
        {

            //"""factor : INTEGER | LPAREN expr RPAREN"""
            Token token = this.current_token;
            if (token.type == "INTEGER") {
                this.eat("INTEGER");
                //return Num(token);
                return new BinOp(new Num(token));
            } else if (token.type == "LPAREN") {
                this.eat("LPAREN");
                BinOp node = this.expr();
                this.eat("RPAREN");
                return node;
            }
            return new BinOp(new Num(new Token("Interger", 0)));

        }

        public BinOp term()
        {
            //this.current_token = this.lexer.get_next_token();

            BinOp node = this.factor();

            while (current_token.type == "MUL" || current_token.type == "DIV")
            {

                Token token = current_token;
                if (token.type == "MUL")
                {
                    eat("MUL");
                    //BinOp result = result * Factor();
                }
                else if (token.type == "DIV")
                {
                    eat("DIV");
                    //BinOp result = result / Factor();
                }
                node = new BinOp(node, token, this.factor());
            }


            return node;

        }

        public BinOp expr()
        {
            //this.current_token = this.lexer.get_next_token();

            BinOp node = this.term();

            while (current_token.type == "PLUS" || current_token.type == "MINUS")
            {

                Token token = current_token;
                if (token.type == "PLUS")
                {
                    eat("PLUS");
                    //result = result + term();
                }
                else if (token.type == "MINUS")
                {
                    eat("MINUS");
                    //result = result - term();
                }
                node = new BinOp(node, token, this.term());
            }


            return node;

        }

        public BinOp parse() {
            return this.expr();
        }

    }




}