using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Refma.Controllers;
using Refma.Models;
using System.Collections.Generic;

namespace RefmaUnitTestProject
{
    [TestClass]
    public class ArticleDecoratorTest
    {

        [TestMethod]
        public void Test_GetAllViewElements()
        {

            WebArticle article = new WebArticle();
            article.ID = 1;
            article.LangId = 2;
            article.PlainText = "Hi, my name is Test, how is your name?";
            ArticleDecorator decorator = new ArticleDecorator(article, false); // dont access database

            decorator.Dic.Add("Hi", new LangElement() { Value = "Hi" });

            List<ViewArticleElement> viewElements = decorator.GetAllViewElements();

            Assert.AreEqual(20, viewElements.Count, "wront element-count");

            Assert.AreEqual("Hi", viewElements[0].Value);
            Assert.AreEqual(false, viewElements[0].IsNotAWord, "declared as non-word");

            Assert.AreEqual(",", viewElements[1].Value);
            Assert.AreEqual(true, viewElements[1].IsNotAWord);
            
            Assert.AreEqual(" ", viewElements[2].Value, "no space");
            Assert.AreEqual("my", viewElements[3].Value);
            Assert.AreEqual(" ", viewElements[4].Value);
            Assert.AreEqual("name", viewElements[5].Value);
            Assert.AreEqual(" ", viewElements[6].Value);
            Assert.AreEqual("is", viewElements[7].Value);
            Assert.AreEqual(" ", viewElements[8].Value);
            Assert.AreEqual("Test", viewElements[9].Value);
            Assert.AreEqual(",", viewElements[10].Value);
            Assert.AreEqual(" ", viewElements[11].Value);
            Assert.AreEqual("how", viewElements[12].Value);
            Assert.AreEqual(" ", viewElements[13].Value);
            Assert.AreEqual("is", viewElements[14].Value);
            Assert.AreEqual(" ", viewElements[15].Value);
            Assert.AreEqual("your", viewElements[16].Value);
            Assert.AreEqual(" ", viewElements[17].Value);
            Assert.AreEqual("name", viewElements[18].Value);
            Assert.AreEqual("?", viewElements[19].Value);

        
            
        }
    }
}
