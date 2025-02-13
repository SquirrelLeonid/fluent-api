﻿using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using ObjectPrintingTask;
using ObjectPrintingTask.PrintingConfiguration;
using ObjectPrintingTaskTests.TestData;
using System;
using System.Text.RegularExpressions;

namespace ObjectPrintingTaskTests
{
    public class ObjectPrintingExcludingTests
    {
        private Person person = Person.GetTestInstance();
        private Printer<Person> printer;

        [SetUp]
        public void SetUp()
        {
            printer = ObjectPrinter.For<Person>();
        }

        [Test]
        public void ObjPrinter_ShouldExcludeSpecificType()
        {
            printer.Excluding<Guid>();

            var result = printer.PrintToString(person);

            var regex = new Regex(@"\s*Id\s*=\s*[\d\w\-]+[\d\w]{1}");
            regex.Match(result).Success.Should().BeFalse();
        }

        [Test]
        public void ObjPrinter_ShouldExcludeSpecificProperty()
        {
            printer.Excluding(p => p.Age);

            var result = printer.PrintToString(person);

            var regex = new Regex(@"\s*Age\s*=\s*\d+");
            regex.Match(result).Success.Should().BeFalse();
        }

        [Test]
        public void ObjPrinter_ShouldExcludeSpecificField()
        {
            printer.Excluding(p => p.Weight);

            var result = printer.PrintToString(person);

            var regex = new Regex(@"\s*Weight\s*=\d+}");
            regex.Match(result).Success.Should().BeFalse();
        }
    }
}
