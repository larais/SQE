﻿using Antlr4.Runtime;
using System;

namespace SQE.CSharp.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter expression:" + Environment.NewLine);

                string input = input = Console.ReadLine();
                if (input == "exit")
                    break;


                AntlrInputStream inputStream = new AntlrInputStream(input);
                //var lexer = new SearchLexer(inputStream);
               // CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                //var parser = new SearchParser(commonTokenStream);

                //parser.RemoveErrorListeners();
                //parser.AddErrorListener(new ErrorListener());

                try
                {
                    //SearchParser.ExpressionContext expressionContext = parser.expression();
                    //Console.WriteLine("Tree expression context: " + expressionContext.ToStringTree());

                    //var visitor = new Visitor();
                    //Console.WriteLine("Visiting tree: " + visitor.Visit(expressionContext) + Environment.NewLine);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
