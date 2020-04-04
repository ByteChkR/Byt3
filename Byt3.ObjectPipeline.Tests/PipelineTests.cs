using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.ObjectPipeline.Tests
{
    [TestClass]
    public class PipelineTests
    {
        #region Example Pipeline Stages

        private class InterceptFileReadStage : PipelineStage<string, byte[]>
        {
            public override byte[] Process(string input)
            {
                if (input == "C:\\TestFile") return Encoding.ASCII.GetBytes("Hello World!");
                if (input == "C:\\InterceptedFile") return Encoding.ASCII.GetBytes("!dlroW olleH");
                return File.ReadAllBytes(input);
            }
        }

        private class InterceptFilePathStage : PipelineStage<string, string>
        {
            private string FileToIntercept = "C:\\TestFile";
            public override string Process(string input)
            {
                return Path.GetFullPath(input) == FileToIntercept ? "C:\\InterceptedFile" : input;
            }
        }

        private class ToLowerStage : PipelineStage<string, string>
        {
            public override string Process(string input)
            {
                return input.ToLower();
            }
        }

        private class BytesToTextStage : PipelineStage<byte[], string>
        {
            public override string Process(byte[] input)
            {
                return Encoding.ASCII.GetString(input);
            }
        }

        #endregion

        [TestMethod]
        public void Pipeline_InvalidStatesTest()
        {

            //Argument null not allowed.
            Assert.ThrowsException<ArgumentNullException>(() =>
                new DelegatePipelineStage<string, string>(null));

            Pipeline<string, byte[]> loadFilePipeline = new Pipeline<string, byte[]>();

            //Null not allowed
            Assert.ThrowsException<ArgumentNullException>(() => loadFilePipeline.AddSubStage(null));

            //Null not allowed
            Assert.ThrowsException<ArgumentNullException>(() => loadFilePipeline.Process(null));

            //pipeline in and stage in do not match
            Assert.ThrowsException<PipelineNotValidException>(() => loadFilePipeline.AddSubStage(new BytesToTextStage()));

            //Adding a valid stage
            loadFilePipeline.AddSubStage(new InterceptFileReadStage());

            //prev stage out and to tolower stage in does not match
            Assert.ThrowsException<PipelineNotValidException>(() => loadFilePipeline.AddSubStage(new ToLowerStage()));

            //Adding a valid State but with incompatible outtype to complete the pipeline
            loadFilePipeline.AddSubStage(new BytesToTextStage()); //Works but the pipline is incomplete because pipline out != last item out

            //Verification Fails
            Assert.IsFalse(loadFilePipeline.Verify());

            //Implementation Throws Exception
            Assert.ThrowsException<PipelineNotValidException>(() => loadFilePipeline.Process("Test"));
        }

        [TestMethod]
        public void Pipeline_ValidStatesTest()
        {
            InterceptFilePathStage interceptFilePathExample = new InterceptFilePathStage();
            InterceptFileReadStage interceptReadStage = new InterceptFileReadStage();

            //Delegate Pipeline requires no creation of classes.
            //This implements the same logic as the ToLowerStage class.
            DelegatePipelineStage<string, string> processTextExample = new DelegatePipelineStage<string, string>((input) => input.ToLower());

            BytesToTextStage decodeTextExample = new BytesToTextStage();


            //Loads Files from disk
            Pipeline<string, byte[]> loadFilePipeline = new Pipeline<string, byte[]>();

            //Intercepts filenames and changes them for specific files
            loadFilePipeline.AddSubStage(interceptFilePathExample);

            //Intercepts File Read calls and returns predefined arrays for specific files
            loadFilePipeline.AddSubStage(interceptReadStage);

            Assert.IsTrue(loadFilePipeline.Verify());

            //Loads Text Files from disk
            Pipeline<string, string> loadTextPipeline = new Pipeline<string, string>();

            //Uses the load file pipeline and adds it as a substage
            loadTextPipeline.AddSubStage(loadFilePipeline);

            //Decodes the Text
            loadTextPipeline.AddSubStage(decodeTextExample);

            //Turns the text to lower.
            loadTextPipeline.AddSubStage(processTextExample);

            Assert.IsTrue(loadTextPipeline.Verify());
        }

        [TestMethod]
        public void Pipeline_UsageTest()
        {
            //Set up the pipelines as in TestValidStates
            Pipeline<string, byte[]> loadFilePipeline = new Pipeline<string, byte[]>();
            loadFilePipeline.AddSubStage(new InterceptFilePathStage()); //Base
            loadFilePipeline.AddSubStage(new InterceptFileReadStage());

            Assert.IsTrue(loadFilePipeline.Verify());

            Pipeline<string, string> loadTextPipeline = new Pipeline<string, string>();
            loadTextPipeline.AddSubStage(loadFilePipeline);
            loadTextPipeline.AddSubStage(new BytesToTextStage());
            loadTextPipeline.AddSubStage(new ToLowerStage());

            Assert.IsTrue(loadFilePipeline.Verify());

            //Try to load Intercepted File 1. Will Get intercepted by the InterceptFilePathStage and changed to C:\\InterceptedFile
            //and after that it gets intercepted again by InterceptFileReadStage
            string testFile = loadTextPipeline.Process("C:\\TestFile");
            //Try to load Intercepted File 2. Will Get intercepted by the InterceptFileReadStage.
            string interceptedFile = loadTextPipeline.Process("C:\\InterceptedFile");

            Assert.IsTrue(testFile == "!dlrow olleh"); //To Lower
            Assert.IsTrue(testFile == interceptedFile); //Check if the "Different Files match"
        }
    }
}
