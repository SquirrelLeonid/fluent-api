﻿using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using ObjectPrintingTask;
using ObjectPrintingTask.Extensions;
using ObjectPrintingTask.PrintingConfiguration;
using ObjectPrintingTaskTests.TestData;

namespace ObjectPrintingTaskTests
{
    [TestFixture]
    public class ObjectPrinterAcceptanceTests
    {
        private readonly Person person = Person.GetTestInstance();
        private Printer<Person> printer;

        [SetUp]
        public void SetUp()
        {
            printer = ObjectPrinter.For<Person>();
        }

        [Test]
        public void AcceptanceTest_ExcludeMembersAndTypes()
        {
            printer
                .Excluding<Guid>()
                .Excluding(p => p.Surname);

            Console.WriteLine(printer.PrintToString(person));
        }

        [Test]
        public void AcceptanceTest_SpecifyAlternateScenarios()
        {
            printer
                .PrintingType<Guid>().Using(g => g.ToString().Substring(0, 8))
                .PrintingMember(p => p.Height).Using(height => (height / 100).ToString("#.##m"));

            Console.WriteLine(printer.PrintToString(person));
        }

        [Test]
        public void AcceptanceTest_SpecifyCultureInfo()
        {
            printer
                .PrintingType<double>().Using(CultureInfo.GetCultureInfo("RU-ru"));

            Console.WriteLine(printer.PrintToString(person));
        }

        [Test]
        public void AcceptanceTest_SpecifyStringMembersLength()
        {
            printer
                .PrintingMember(p => p.Name).TrimmedToLength(3);

            Console.WriteLine(printer.PrintToString(person));
        }

        [Test]
        public void AcceptanceTest_PrintingPlainCollectionsOfPrimitive()
        {
            var plainList = new List<int>() { 1, 2, 3, 4, 5 };
            var plainDictionary = new Dictionary<int, string>()
            {
                {0, "zero" },
                {1, "one" },
                {2, "two" },
                {42, "forty two" }
            };
            Console.WriteLine(plainList.PrintToString());
            Console.WriteLine(plainDictionary.PrintToString());
        }

        [Test]
        public void AcceptanceTest_PrintingComposedCollectionsOfPrimitive()
        {
            var composedList = new List<List<int>>()
            {
                new List<int>(){1, 2, 3},
                new List<int>(){4, 5, 6},
                new List<int>(){7, 8, 9}
            };

            var composedDictionary = GetComposedDictionary();

            Console.WriteLine(composedList);
            Console.WriteLine(composedDictionary);
        }

        [Test]
        public void AcceptanceTest_CompositionOfCustomizations()
        {
            printer
                .Excluding<Guid>()
                .PrintingMember(p => p.Name).Using(name => name.ToUpper())
                .PrintingType<double>().Using(CultureInfo.GetCultureInfo("RU-ru"))
                .Excluding(p => p.Surname)
                .PrintingType<string>().TrimmedToLength(4);

            Console.WriteLine(printer.PrintToString(person));
        }

        [Test]
        public void AcceptanceTest_PrintingShouldBeExtensionForAnyObject()
        {
            var s1 = person.PrintToString();
            var s2 = person.PrintToString(print => print.Excluding<Guid>().Excluding<double>());

            Console.WriteLine(s1);
            Console.WriteLine(s2);
        }

        [Test]
        public void Demo()
        {          
            var config = ObjectPrinter.For<Person>()
                //1. Исключить из сериализации свойства определенного типа
                .Excluding<Guid>()
                //2. Указать альтернативный способ сериализации для определенного типа
                .PrintingType<int>().Using(i => i.ToString("X"))
                //3. Для числовых типов указать культуру
                .PrintingType<double>().Using(CultureInfo.InvariantCulture)
                //4. Настроить сериализацию конкретного свойства
                //5. Настроить обрезание строковых свойств (метод должен быть виден только для строковых свойств)
                .PrintingMember(p => p.Name).TrimmedToLength(10)
                //6. Исключить из сериализации конкретного свойства
                .Excluding(p => p.Age);

            string s1 = config.PrintToString(person);

            //7. Синтаксический сахар в виде метода расширения, сериализующего по-умолчанию
            string s2 = person.PrintToString();

            //8. ...с конфигурированием
            string s3 = person.PrintToString(s => s.Excluding(p => p.Age));
            Console.WriteLine(s1);
            Console.WriteLine(s2);
            Console.WriteLine(s3);
        }

        private Dictionary<string, Dictionary<string, int>> GetComposedDictionary()
        {
            var JohnContacts = new Dictionary<string, int>()
            {
                {"Joseph", 26090 },
                {"Brock", 45417 }
            };

            var BrockContacts = new Dictionary<string, int>()
            {
                {"John", 43908 },
                {"Mirlanda", 34998 }
            };

            var phoneBook = new Dictionary<string, Dictionary<string, int>>()
            {
                {"John", JohnContacts },
                {"Brock", BrockContacts }
            };

            return phoneBook;

        }
    }
}