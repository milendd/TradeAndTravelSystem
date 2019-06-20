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

        [TestMethod]
        public void TestInput2()
        {
            var input = ReadAllLines("test.002.in.txt");
            var expectedOutput = ReadAllLines("test.002.out.txt");

            var output = engine.Start(input);
            CompareOutput(expectedOutput, output);
        }

        [TestMethod]
        public void TestInput3()
        {
            var input = ReadAllLines("test.003.in.txt");
            var expectedOutput = ReadAllLines("test.003.out.txt");

            var output = engine.Start(input);
            CompareOutput(expectedOutput, output);
        }

        [TestMethod]
        public void TestInput4()
        {
            var input = ReadAllLines("test.004.in.txt");
            var expectedOutput = ReadAllLines("test.004.out.txt");

            var output = engine.Start(input);
            CompareOutput(expectedOutput, output);
        }

        [TestMethod]
        public void TestInput5()
        {
            var input = ReadAllLines("test.005.in.txt");
            var expectedOutput = ReadAllLines("test.005.out.txt");

            var output = engine.Start(input);
            CompareOutput(expectedOutput, output);
        }

        [TestMethod]
        public void TestInput6()
        {
            var input = ReadAllLines("test.006.in.txt");
            var expectedOutput = ReadAllLines("test.006.out.txt");

            var output = engine.Start(input);
            CompareOutput(expectedOutput, output);
        }

        [TestMethod]
        public void TestInput7()
        {
            var input = ReadAllLines("test.007.in.txt");
            var expectedOutput = ReadAllLines("test.007.out.txt");

            var output = engine.Start(input);
            CompareOutput(expectedOutput, output);
        }

        [TestMethod]
        public void TestInput8()
        {
            var input = ReadAllLines("test.008.in.txt");
            var expectedOutput = ReadAllLines("test.008.out.txt");

            var output = engine.Start(input);
            CompareOutput(expectedOutput, output);
        }

        [TestMethod]
        public void TestInput9()
        {
            var input = ReadAllLines("test.009.in.txt");
            var expectedOutput = ReadAllLines("test.009.out.txt");

            var output = engine.Start(input);
            CompareOutput(expectedOutput, output);
        }

        [TestMethod]
        public void TestInput10()
        {
            var input = ReadAllLines("test.010.in.txt");
            var expectedOutput = ReadAllLines("test.010.out.txt");

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
