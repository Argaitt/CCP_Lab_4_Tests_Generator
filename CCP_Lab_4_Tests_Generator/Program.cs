using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
using System.Text;

namespace CCP_Lab_4_Tests_Generator
{
    public class DataflowProducerCosumer
    {
        BlockingCollection<string> q = new BlockingCollection<string>();

        public void Run(string[] fileList)
        {
            var threads = new[] { new Thread(Consumer), new Thread(Consumer), new Thread(Consumer), new Thread(Consumer) };
            foreach (var t in threads)
            {
                t.Start();
            }
            foreach (var file in fileList)
            {
                StreamReader streamReader = new StreamReader(file);
                String buffer = streamReader.ReadToEnd();
                q.Add(buffer);
            }
            q.CompleteAdding();
            foreach (var t in threads)
                t.Join();
        }
        void Consumer()
        {
            foreach (var s in q.GetConsumingEnumerable())
            {
                SyntaxTree tree = CSharpSyntaxTree.ParseText(s);
                var methods = tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>();
                int count = 0;
                foreach (var method in methods)
                {
                    var modificator = method.Modifiers;
                    if (modificator.ToString().Contains("public"))
                    {
                        count++;
                    }
                }
                StringBuilder sb = new StringBuilder("using System;\n " +
                    "using Microsoft.VisualStudio.TestTools.UnitTesting;\n" +
                    "namespace UnitTestProject1\n" +
                    "{\n"+
                    "   [TestClass]\n"+
                    "   public class UnitTest1\n" +
                    "   {");
                for (int i = 0; i < count; i++)
                {
                    sb.Append("     [TestMethod]\n" +
                              "     public void TestMethod" + count + "()\n" +
                              "     {\n" +
                              "         }\n"
                    );
                }
                sb.Append("     }\n" +
                         "}\n"
                );
                string pathfile = @"D:\Bsuir\SPP\CCP_Lab_4_Tests_Generator\folder_for_result\UnitTest" + Thread.CurrentThread.ManagedThreadId + ".cs";
                StreamWriter sw = new StreamWriter(pathfile);
                sw.Write(sb);
                sw.Close();
            }
            
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            String folderPath = @"D:\Bsuir\SPP\CCP_Lab_4_Tests_Generator\files_for_tests";
            string[] file_list = Directory.GetFiles(folderPath, "*.cs");
            DataflowProducerCosumer d = new DataflowProducerCosumer();
            d.Run(file_list);
        }
    }
}
