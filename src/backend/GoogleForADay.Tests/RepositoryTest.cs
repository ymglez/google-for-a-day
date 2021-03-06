﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleForADay.Core.Abstractions.Store;
using GoogleForADay.Core.Model;
using GoogleForADay.Core.Model.Store;
using GoogleForADay.Infrastructure.Store.LightningDB;
using GoogleForADay.Services.Business.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GoogleForADay.Tests
{
    public class RepositoryTest : BaseTest
    {
        private readonly IKeyValueRepository<Keyword> _repo = ServiceProvider.GetService<IKeyValueRepository<Keyword>>();

        [Fact]
        public void TestUpsert()
        {
            var rnd = new Random();
            
            for (int i = 2; i < 200000; i++)
           
            {

                var data = new Keyword
                {
                    Term = GenerateWord(i % 8 + 2),
                    References = new List<Reference>
                    {
                        new Reference
                        {
                            Occurrences = rnd.Next(5000),
                            Tittle = "this is the first test",
                            Url = "http://www.test.first"
                        },
                        new Reference
                        {
                            Occurrences = rnd.Next(5000),
                            Tittle = "hello world",
                            Url = "http://www.hello.com"
                        }
                    }
                };

                _repo.Upsert(data.Term, data);
            }

            _repo.SaveChanges();

        }

        [Fact]
        public void TestOverride()
        {
            
            _repo.Upsert("github", null);
            //_repo.SaveChanges();

            var data = _repo.Get("github");
            Assert.True(data == null);


            _repo.Upsert("github", new Keyword
            {
                Term = "github",
                References = new List<Reference>
                {
                    new Reference
                    {
                        Occurrences = 2,
                        Tittle = "this is the first test",
                        Url = "http://www.test.first"
                    },
                    new Reference
                    {
                        Occurrences = 8,
                        Tittle = "hello world",
                        Url = "http://www.hello.com"
                    }
                }
            });
            
            data = _repo.Get("github");
            Assert.True(data.Term == "github" && data.References.Count == 2);

            _repo.SaveChanges();
        }

        [Fact]
        public void TestGet()
        {
            var cant = ((LightningRepository<Keyword>) _repo).Count();
            
            var words = new List<string>
            {
                "cuba", "software","icrt"
            };

            foreach (var word in words)
            {
                var data = _repo.Get(word);
                Assert.True(data.Term == word);
            }
        }


        [Fact]
        public void TestCleanup()
        {
            Assert.True(_repo.Clear());
        }


        string GenerateWord(int length)
        {
            Random rand = new Random();
            string[] cons = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
            string[] vowel = { "a", "e", "i", "o", "u" };

            if (length < 1) // do not allow words of zero length
                throw new ArgumentException("Length must be greater than 0");

            string word = string.Empty;

            if (rand.Next() % 2 == 0) // randomly choose a vowel or consonant to start the word
                word += cons[rand.Next(0, 20)];
            else
                word += vowel[rand.Next(0, 4)];

            for (int i = 1; i < length; i += 2) // the counter starts at 1 to account for the initial letter
            { // and increments by two since we append two characters per pass
                char c = cons[rand.Next(0, 20)].First();
                char v = vowel[rand.Next(0, 4)].First();

                if (c == 'q') // append qu if the random consonant is a q
                    word += "qu";
                else // otherwise just append a random consant and vowel
                    word += c + v;
            }

            // the word may be short a letter because of the way the for loop above is constructed
            if (word.Length < length) // we'll just append a random consonant if that's the case
                word += cons[rand.Next(0, 20)];

            return word;
        }
    }
}
