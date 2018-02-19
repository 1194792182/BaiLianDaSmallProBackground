﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BaseDatabase;
using BaseDatabase.AutoMaps;
using System.Data.Entity.Infrastructure;

namespace BaseDataBaseTest
{
    /// <summary>
    /// CreateScriptStrUnitTest 的摘要说明
    /// </summary>
    [TestClass]
    public class CreateScriptStrUnitTest
    {
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            BaseDatabaseConfig.SetDbInfo();
            AutoMapConfig.CreateMaps();
        }

        [TestMethod]
        public void TestMethod1()
        {
            var context = new BaseDatabaseContext();

            var script = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();

            Assert.IsTrue(!string.IsNullOrWhiteSpace(script));
        }
    }
}