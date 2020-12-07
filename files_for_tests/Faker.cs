using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using UsersClasses;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fakernamespace
{
    
    public class Faker
    {
        Dictionary<Type, FakeGenerator> FakeGeneratorDictionary;
        public Faker() 
        {
            FakeGeneratorDictionary = new Dictionary<Type, FakeGenerator>();
            FakeGeneratorDictionary.Add(typeof(int), new IntFakeGenerator());
            FakeGeneratorDictionary.Add(typeof(long), new LongFakeGenerator());
            FakeGeneratorDictionary.Add(typeof(string), new StringFakeGenerator());
            FakeGeneratorDictionary.Add(typeof(double), new DoubleFakeGenerator());
            FakeGeneratorDictionary.Add(typeof(float), new FloatFakeGenerator());
            FakeGeneratorDictionary.Add(typeof(DateTime), new DateTimeFakeGenerator());
            FakeGeneratorDictionary.Add(typeof(List<int>), new ArrIntFakeGenerator());
            FakeGeneratorDictionary.Add(typeof(List<string>), new ArrStringFakeGenerator());
            FakeGeneratorDictionary.Add(typeof(List<double>), new ArrDoubleFakeGenerator());
        }
        public T Create<T>()
        {
            var obj = (T)Activator.CreateInstance(typeof(T),true);
            var type = obj.GetType();
            FieldInfo[] myFields = type.GetFields();
            foreach (var item in myFields)
            {
                if (FakeGeneratorDictionary.TryGetValue(item.FieldType, out var generator))
                    item.SetValue(obj, generator.Generate());
                else if(item.FieldType.Name == "Foo")
                {
                    Faker faker = new Faker();
                    item.SetValue(obj, faker.Create<Foo>());
                }
                else
                {
                    item.SetValue(obj, null);
                }
            }
            return obj;
        }
    }
    interface IFakeGenerator
    {
        object Generate();
    }
    class FakeGenerator : IFakeGenerator
    {
        public virtual object Generate()
        {
            object obj = 0;
            return obj;
        }
    }
    class IntFakeGenerator : FakeGenerator
    {
        public override object Generate()
        {
            Random rnd = new Random();
            return (int)rnd.Next();
        }
    }
    class LongFakeGenerator : FakeGenerator
    {
        public override object Generate()
        {
            Random rnd = new Random();
            return (long)rnd.Next();
        }
    }
    class StringFakeGenerator : FakeGenerator
    {
        public override object Generate()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 30)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
    class FloatFakeGenerator : FakeGenerator
    {
        public override object Generate()
        {
            Random rnd = new Random();
            return (float)rnd.Next();
        }
    }
    class DateTimeFakeGenerator : FakeGenerator
    {
        public override object Generate()
        {
            return DateTime.Now;
        }
    }
    class DoubleFakeGenerator : FakeGenerator
    {
        public override object Generate()
        {
            Random rnd = new Random();
            return (double)rnd.Next();
        }
    }
    class ArrIntFakeGenerator : FakeGenerator
    {
        public override object Generate()
        {
            Random rnd = new Random();
            List<int> list = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(rnd.Next());
            }
            return list;
        }
    }
    class ArrDoubleFakeGenerator : FakeGenerator
    {
        public override object Generate()
        {
            Random rnd = new Random();
            List<double> list = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(rnd.Next());
            }
            return list;
        }
    }
    class ArrStringFakeGenerator : FakeGenerator
    {
        public override object Generate()
        {
            Random random = new Random();
            //List<String> strings = new String();
            for (int i = 0; i < 10; i++)
            {

            }
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 30)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
