using System;
using System.Diagnostics;
using System.Linq;
using LanguageExt;
using LanguageExt.ClassInstances;
using static LanguageExt.Prelude;

namespace PracticalCSharp.Validation.Examples

{
    public class Demo
    {
        public Func<string, string> ToUpper => x => x.ToUpper();
        public async System.Threading.Tasks.Task Run()
        {  
            Either<TestError, string> notEmpty(string str) =>
            !string.IsNullOrEmpty(str)
            ? Right<TestError, string>(str)
            : Left<TestError, string>(new TestError("must not be empty"));


            Either<TestError, string> isDigit(string str) =>
                str.ForAll(Char.IsDigit)
            ? Right<TestError, string>(str)
            : Left<TestError, string>(new TestError("must be a valid number"));

            Func<string, Either<TestError, string>> maxStrLength(int maxlenght) => str =>
               str.Length < maxlenght
            ? Right<TestError, string>(str)
            : Left<TestError, string>(new TestError("failed max length"));


            var res = maxStrLength(3)("aaa").Bind(s => isDigit(s));
         
            res.Match(
                Right: _ =>
               {
                   Console.WriteLine("no errors");

               },
                Left: error =>
                {
                    Console.WriteLine(error);

                });

        }
        public void ExpiredAndInValidCreditCardNumberTest()
        {
            Validation<TestError, string> notEmpty(string str) =>
            !string.IsNullOrEmpty(str)
            ? Success<TestError, string>(str)
            : Fail<TestError, string>(TestError.New("must not be empty"));


            Validation<TestError, string> isDigit(string str) =>
                str.ForAll(Char.IsDigit)
              ? Success<TestError, string>(str)

              : Fail<TestError, string>(TestError.New("failed isDigit"));

            Func<string, Validation<TestError, string>> maxStrLength(int maxlenght) => str =>
               str.Length < maxlenght
              ? Success<TestError, string>(str)
              : Fail<TestError, string>(TestError.New("failed maxlenght"));


            var res = maxStrLength(3)("aaa").Bind(s => isDigit(s));
       
            res.Match(
                Succ: _ =>
                {
                    Console.WriteLine("no errors");
                },
                Fail: errors =>
                {
                    errors.ToList().ForEach(error => Console.WriteLine(error));
                });

            var bothFailed = maxStrLength(3)("aadda") | isDigit("1e12");
            bothFailed.Match(
              Succ: _ =>
              {
                  Console.WriteLine("no errors");
              },
              Fail: errors =>
              {
                  errors.ToList().ForEach(error => Console.WriteLine(error));
              });


            var errorList = bothFailed.FailToArray();

            var toEither = bothFailed.ToEither();
        }
   
        public class TestError : NewType<TestError, string>
        {
            public TestError(string e) : base(e) { }
        }
        

    }

}

