﻿using System.Collections.Generic;
using System.IO;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.ExtPP.Tests
{
    [TestClass]
    public  class PreProcessorTests
    {
        private static List<AbstractPlugin> Plugins
        {
            get
            {
                IncludePlugin inc = new IncludePlugin
                {
                    IncludeInlineKeyword = "pp_includeinl:", IncludeKeyword = "pp_include:"
                };
                ConditionalPlugin cond = new ConditionalPlugin
                {
                    StartCondition = "pp_if:",
                    ElseIfCondition = "pp_elseif:",
                    ElseCondition = "pp_else:",
                    EndCondition = "pp_endif:",
                    DefineKeyword = "pp_define:",
                    UndefineKeyword = "pp_undefine:"
                };

                return new List<AbstractPlugin>
                {
                    new FakeGenericsPlugin(),
                    inc,
                    cond,
                    new ExceptionPlugin(),
                    new MultiLinePlugin()
                };
            }
        }
        private static string ResourceFolder { get; } = TestHelper.ResourceFolder;

        [TestInitialize]
        public  void SetUp()
        {
            TestHelper.SetupPath();
        }

        [TestMethod]
        public  void ExtPP_PreProcessor_FilterRun_Test()
        {
            Directory.SetCurrentDirectory(ResourceFolder);
            string[] files = Directory.GetFiles("filter/tests/", "*.fl");
            foreach (string file in files)
            {
                string dir = Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(ResourceFolder);
                PreProcessor pp = new PreProcessor();
                pp.SetFileProcessingChain(Plugins);
                pp.Run(new[] { file }, new Definitions());
            }

        }
    }
}
