using System;
using System.IO;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using DesignAutomationFramework;
using RDADHelper;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExampleImplementation
{
    [Transaction(TransactionMode.Manual),
     Regeneration(RegenerationOption.Manual)]
    public class Example : IExternalDBApplication
    {
        public ExternalDBApplicationResult OnShutdown(ControlledApplication application)
        {
            return ExternalDBApplicationResult.Succeeded;
        }

        public ExternalDBApplicationResult OnStartup(ControlledApplication application)
        {
            Console.WriteLine("<<!>> Example implementation started.");
            DesignAutomationBridge.DesignAutomationReadyEvent += HandleDesignAutomationReadyEvent;
            Actions.OnGraphResultReady += ProcessResult;


            return ExternalDBApplicationResult.Succeeded;
        }

        private string fileName = "";
        public void HandleDesignAutomationReadyEvent(object sender, DesignAutomationReadyEventArgs e)
        {
            Console.WriteLine("<<!>> Got called by DA event.");

            //optionally do some pre-execution setup
            var fp = e.DesignAutomationData.FilePath;
            var workDir = Path.GetDirectoryName(fp);
            fileName = Path.GetFileNameWithoutExtension(fp);
            
            var inputData = File.ReadAllText(Path.Combine(workDir, "input.json"));
            var graphsToRun = JsonConvert.DeserializeObject<List<RunGraphArgs>>(inputData);
            foreach (var graphArgs in graphsToRun)
            {
                Actions.RunDynamoGraph(graphArgs);
            }

            e.Succeeded = true;
        }

        private int n = 1;
        private void ProcessResult(GraphResultArgs args)
        {
            //optionally do some post-execution cleanup and management

            //make sure you save your graph data for review
            var graphData = JsonConvert.SerializeObject(args, Formatting.Indented);
            File.WriteAllText($"{args.WorkItemResultFolder}/{fileName}_{args.GraphName}_{n++}.json", graphData);
        }
    }
}
