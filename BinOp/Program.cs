using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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



    class Program
    {
        public const string INTEGER = "INTEGER";
        public const string MUL = "MUL";
        public const string PLUS = "PLUS";

        static void Main(string[] args)
        {

            BinOp left = new BinOp(new Num(new Token(INTEGER, 2)));
            BinOp right = new BinOp(new Num(new Token(INTEGER, 7)));

            BinOp mul_node = new BinOp(new Token(MUL, "*"));

            mul_node.add_node(left, right);
            //Console.WriteLine(mul_node.op);

            BinOp plus_node = new BinOp(new Token(PLUS, "+"));

            BinOp num2 = new BinOp(new Num(new Token(INTEGER, 5)));

            //mul_node.printPreorder(mul_node);

            plus_node.add_node(mul_node, num2);

            //plus_node.travsalDF(plus_node);

            Console.WriteLine(plus_node.visit(plus_node));


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
                if (n.num != null ) Console.WriteLine(n.num.value);
                if (n.op != null ) Console.WriteLine(n.op.__repr__());
                if (n.left != null)
                    s.Push(n.left);
                if (n.right != null)
                    s.Push(n.right);
            }
        }

        public int visit(BinOp node)
        {
            if (node.op != null) 
                return visit_BinOp(node);
            else 
                return visit_Num(node);
    
        }
        public int visit_BinOp(BinOp node) {
           if (node.op.type == "PLUS"){
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




    }

}