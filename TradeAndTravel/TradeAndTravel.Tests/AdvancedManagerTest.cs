using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TradeAndTravel.Engine;
using System.Collections.Generic;

namespace TradeAndTravel.Tests
{
    [TestClass]
    public class AdvancedManagerTest
    {
        private Engine.Engine engine;

        [TestInitialize]
        public void Initialize()
        {
            var manager = new AdvancedInteractionManager();
            this.engine = new Engine.Engine(manager);
        }

        [TestMethod]
        public void TestSampleInput()
        {
            var input = ReadAllLines("test.000.in.txt");
            var expectedOutput = ReadAllLines("test.000.out.txt");

            var output = engine.Start(input);
            CompareOutput(expectedOutput, output);
        }

        [TestMethod]
        public void TestInput1()
        {
            var input = ReadAllLines("test.001.in.txt");
            var expectedOutput = ReadAllLines("test.001.out.txt");

            var output = engine.Start(input);
            CompareOutput(expectedOutput, output);
        }

        private void CompareOutput(string[] expectedOutput, string[] output)
        {
            Assert.AreNotEqual(expectedOutput, null);
            Assert.AreNotEqual(output, null);

            Assert.AreEqual(expectedOutput.Length, output.Length);

            for (int i = 0; i < expectedOutput.Length; i++)
            {
                Assert.AreEqual(expectedOutput[i], output[i]);
            }
        }

        private string[] ReadAllLines(string fileName)
        {
            var debugDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            var baseDirectory = debugDirectory.Parent.Parent.FullName;

            var fullPath = Path.Combine(baseDirectory, "Files", fileName);
            var data = File.ReadAllLines(fullPath);
            return data;
        }
    }
}
