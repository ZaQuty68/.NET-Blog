using Projekt;
using NUnit.Framework;
using System;
using Projekt.Data;
using Projekt.Models;

namespace UnitTestProject1
{
    [TestFixture]
    public class UnitTest1
    {
        private ProjektContext _sut;
        [SetUp]
        public void SetUp()
        {
            _sut = new ProjektContext();
            var Post1 = new Post { Id = 1, Title = "PostTest", Content = "Post for testing" };
            _sut.Posts.Add(Post1);
        }
    }
}
