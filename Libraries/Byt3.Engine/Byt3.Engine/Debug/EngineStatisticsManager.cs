using System.Collections.Generic;
using System.IO;
using Byt3.DisposableManagement;
using Byt3.OpenCL;

namespace Byt3.Engine.Debug
{
    /// <summary>
    /// Class that keeps track of all possible statistics during the run of the engine
    /// </summary>
    public static class EngineStatisticsManager
    {
        public static int TotalUpdates { get; private set; }
        public static int TotalFrames { get; private set; }
        public static float BiggestUpdateTime { get; private set; }
        public static float SmallestUpdateTime { get; private set; }
        public static float BiggestRenderTime { get; private set; }
        public static float SmallestRenderTime { get; private set; }
        public static float TimeSinceStartup { get; private set; }
        public static float TotalRenderTime { get; private set; }

        public static int TotalGLObjectsCreated { get; private set; }

        internal static void Update(float deltaTime)
        {
            if (deltaTime > BiggestUpdateTime)
            {
                BiggestUpdateTime = deltaTime;
            }
            else if (deltaTime < SmallestUpdateTime)
            {
                SmallestUpdateTime = deltaTime;
            }

            TimeSinceStartup += deltaTime;
            TotalUpdates++;
        }

        internal static void Render(float renderTime)
        {
            if (renderTime > BiggestRenderTime)
            {
                BiggestRenderTime = renderTime;
            }
            else if (renderTime < SmallestRenderTime)
            {
                SmallestRenderTime = renderTime;
            }

            TotalFrames++;
            TotalRenderTime += renderTime;
        }

        internal static void GlObjectCreated(DisposableObjectBase bytes)
        {
            TotalGLObjectsCreated++;
            objects.Add(bytes);
        }

        internal static void GlObjectDestroyed(DisposableObjectBase bytes)
        {
            objects.Remove(bytes);
        }

        private static readonly List<DisposableObjectBase> objects = new List<DisposableObjectBase>();


        public static void DisposeAllHandles()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].Dispose();
            }

            objects.Clear();
        }

        public static void WriteStatistics(string file)
        {
            Stream s = File.Create(file);
            TextWriter tw = new StreamWriter(s);
            tw.WriteLine("Total Updates:" + TotalUpdates);
            tw.WriteLine("\tMin Update Time:" + SmallestUpdateTime);
            tw.WriteLine("\tMax Update Time:" + BiggestUpdateTime);
            tw.WriteLine("Total Frames:" + TotalFrames);
            tw.WriteLine("Total Render Time:" + TotalRenderTime);
            tw.WriteLine("\tMin Render Time:" + SmallestRenderTime);
            tw.WriteLine("\tMax Render Time:" + BiggestRenderTime);
            tw.WriteLine("OpenGL Stats:");
            tw.WriteLine("Total Objects Created:" + TotalGLObjectsCreated);
            tw.WriteLine("\tUndisposed Objects: " + objects.Count);
            for (int i = 0; i < objects.Count; i++)
            {
                tw.WriteLine($"\t\tObject {i} : " + objects[i].HandleIdentifier);
            }


            tw.Write(HandleBase.WriteStatistics());

            tw.Close();
            s.Close();
        }
    }
}