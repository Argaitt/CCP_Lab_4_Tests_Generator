using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using CCP_Lab_4_Tests_Generator;


namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GeneralTestMethod1()
        {
            String folderPath = @"D:\Bsuir\SPP\CCP_Lab_4_Tests_Generator\files_for_tests";
            string[] file_list = Directory.GetFiles(folderPath, "*.cs");
            DataflowProducerCosumer d = new DataflowProducerCosumer();
            d.Run(file_list);
            var expected = true;
            var actual = System.IO.Directory.GetFiles(folderPath).Length > 0;
            Assert.AreEqual(expected, actual);
        }
    }
}
