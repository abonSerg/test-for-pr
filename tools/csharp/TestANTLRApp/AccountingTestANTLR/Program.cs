using System;
using System.Reflection.PortableExecutable;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace AccountingTestANTLR
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = "Artem TOOK 5 USD \n Paul TOOK 3 USD \n Brian GAVE 2 USD \n Olga TOOK 1 USD \n";
            
            AntlrInputStream inputStream = new AntlrInputStream(text);
            var lexer = new AccountingLexer(inputStream);
            
            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new AccountingParser(commonTokenStream);
            var context = parser.accounting();
            var visitor = new AccountingVisitor();        
            visitor.Visit(context);
        }
    }
    
   
    public class AccountingVisitor : AccountingBaseVisitor<object>
    {
        //public List<SpeakLine> Lines = new List<SpeakLine>();

        private static void PrintChildValue(IParseTree tree)
        {
            if (tree.ChildCount > 0)
            {
                var empContext = tree as AccountingParser.EmployeeContext;

                if (empContext == null)
                {
                    var amountContext = tree as AccountingParser.AmountContext;
                    
                    foreach (var child in amountContext.children)
                    {
                        PrintChildValue(child);
                    }
                    
                    //PrintChildValue(amountContext);
                }
                else
                {
                    foreach (var child in empContext.children)
                    {
                        PrintChildValue(child);
                    }
                    
                }
                
                return;
            }

            
            
            var text = ((TerminalNodeImpl)tree).Payload.Text;

            if (text == "\n")
            {
                Console.WriteLine("\\n");
            }
            else
            {
                Console.WriteLine(text);
            }
        }

        private static void PrintNode(IParseTree tree)
        {
            var empContext = tree as AccountingParser.EmployeeContext;
            
            if (empContext != null)
            {
                Console.WriteLine("employee");
            }

            var amountContext = tree as AccountingParser.AmountContext;
            if (amountContext != null)
            {
                Console.WriteLine("amount");
                
            }
            
            var accountContext = tree as AccountingParser.AccountingContext;
            if (accountContext != null)
            {
                Console.WriteLine("accounting");
            }
            
            var opContext = tree as AccountingParser.OperationContext;
            if (opContext != null)
            {
                Console.WriteLine("operation");
            }
        }


        public override object VisitAccounting(AccountingParser.AccountingContext context)
        {            
            PrintNode(context);
            var operations = context.operation();

            //VisitChildren(context);

            foreach (var operation in operations)
            {
                
                PrintNode(operation);
         
                if (operation.children.Count > 0)
                {
                    foreach (var child in operation.children)
                    {
                        PrintNode(child);
                        PrintChildValue(child);
                    }
                }

                var employee = operation.employee();
                var amount = operation.amount();


                
            }
            
            // var employee = operation.;
            // SpeakLine line = new SpeakLine() { Person = name.GetText(), Text = opinion.GetText().Trim('"') };
            // Lines.Add(line);
            return "";
        }
    }
}