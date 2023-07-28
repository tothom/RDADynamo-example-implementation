### Prerequisites

To run Dynamo graphs in the cloud, it is necessary to have a basic understanding on how Autodesk Design Automation works. Please check out the [documentation for Design Automation](https://aps.autodesk.com/en/docs/design-automation/v3/developers_guide/overview/) for get an overview. If you are new to this we recommend starting with the [step-by-step tutorials](https://aps.autodesk.com/en/docs/design-automation/v3/tutorials/revit/about_this_tutorial/).


### Making your own Revit Design Automation Dynamo Implementation

To run Dynamo graphs on your Revit model in Design Automation you need to reference *RDADHelper.dll* in your DB app. RDADHelper stands for *Revit Design Automation Dynamo Helper* and works as an intermediate layer between your DB app bundle and Dynamo. Refer to the files included in this implementation on the specifics.

The APS activity for this implementation looks similar to the below:
```json
{
    "commandLine": [
        "$(engine.path)\\\\revitcoreconsole.exe /i \"$(args[rvtFile].path)\" /al \"$(appbundles[D4DA].path)\""
    ],
    "parameters": {
        "rvtFile": {
            "zip": false,
            "ondemand": false,
            "verb": "get",
            "description": "Input Revit model",
            "required": true
        },
        "input": {
            "zip": true,
            "ondemand": false,
            "verb": "get",
            "description": "Input Dynamo graph(s) and supporting files",
            "required": true,
            "localName": "input.zip"
        },
        "result": {
            "zip": true,
            "ondemand": false,
            "verb": "put",
            "description": "graph result",
            "required": true,
            "localName": "result"
        }
    },
    "engine": "Autodesk.Revit+2024",
    "appbundles": [
        "{{dasNickName}}.D4DA+default"
    ],
    "description": "Run dynamo in the cloud"
}
```

The APS work item for this implementation looks similar to the below:
```json
{
  "activityId": "{{dasNickName}}.D4DA_Activity+default",
  "arguments": {
    "rvtFile": {
      "url": "{{ossDownloadURLForRvt}}"
    },
    "input": {
      "url": "{{ossDownloadURLForInput}}"
    },
    "result": {
      "verb": "put",
      "url": "{{ossUploadURL}}"
    }
  }
}
```

The activity expects a signed download link to a Revit file (either uploaded to the OSS storage or from Bim360), that will be downloaded and opened in Revit by Design Automation. The second input is a signed download link to a zip that will include the dynamo graph(s) and all of the supporting files.

The current implementation expects an `input.json` file that contains a list of RDADHelper `RunGraphArgs`. Each `RunGraphArgs` item in the list has the following properties:
- GraphName: The name of the Dynamo graph to run inside the zip file.
- InputFolder: the local name of the input zip as defined in your APS activity. You have to remember that RDADynamo does not have any knowledge of your implementation specifics and needs this property to find the dynamo graph.
- ResultFolder: this is an optional input that is recommended. If you specify a result folder, RDADynamo will move your graph there before executing it. That way all files saved by the graph will be placed in that result folder and it can later be returned by APS as a single link to a zip, instead of multiple ones for each file. If you do not want to use this feature, you have to make sure that you handle the created files manually.
- InputFileNames: if your graph has any file input nodes, Dynamo will look for the file relative to the folder of the graph. If you've specified a 'ResultFolder', you will need to list these files here, so that RDADynamo can also move them over to the result folder prior to executing the graph. That way you can simplify your input paths inside your graph.
- Packages: list of folders where Dynamo should look for custom packages. An easy way to implement this is to put all of your custom packages in a single reference folder. Alternatively, you can provide each folder separately.
- NodeInput: a list of `NodeValues` to modify the graph nodes with. Only valid input nodes can be modified. In Dynamo, input nodes have an "Is Input" attribute in the right-click menu. 'Number' node or 'String' node are examples of input nodes. 'Add' node or 'Python Script' node are examples of nodes that cannot be modified. Each `NodeValue` has the following properties:
  - Id: the unique identifier of the node. This can be found in the Dynamo graph file. You can open the graph file in a text editor and search for the node name to find the Id.
  - Name: The name of the input parameter to modify. This is not the name of the node. Usually it should be `Value`.
  - Value: the value to set the input parameter to. This can be a string or a number.

> 💡**Hint**: Set your input nodes in your graph to `Is Input` by right clicking on them. The input nodes will then appear under `Inputs` when you open the graph in a text editor, so you can easily find them.

The input.json in this case could look something like this:
```json
[
    {
        "GraphName": "graphA.dyn",
        "InputFolder": "input.zip",
        "ResultFolder": "result",
        "InputFileNames": [
            "some_graph_input_file.csv"
        ],
        "Packages": [
            "packages"
        ],
        "NodeInput": [
            {
                "Id": "a1b2c3d4-1234-5678-9abc-def012345678",
                "Name": "Value",
                "Value": "42"
            },
            {
                "Id": "a1b2c3d4-1234-5678-9abc-def012345678",
                "Name": "Value",
                "Value": "Hello World"
            }
        ]
    },
    {
        "GraphName": "graphB.dyn",
        "InputFolder": "input.zip",
        "ResultFolder": "result",
        "InputFileNames": [
        ],
        "Packages": [
            "packages"
        ],
        "NodeInput": [
            {
              "Id": "a1b2c3d4-1234-5678-9abc-def012345678",
              "Name": "Value",
              "Value": "42"
            }
        ]
    }
]
```

Based on that, your input.zip file would need to have a folder structure similar to this:

### input.zip:
- input.json
- packages:
  - package A folder
  - package B folder
- graphA.dyn
- graphB.dyn
- some_graph_input_file.csv
- python-3.9.12-embed-amd64.zip (for when you need to run cPython scripts)

## Current Limitations
Please refer to the [list of limitations](https://github.com/tothom/RDADynamo-example-implementation/blob/main/docs/UnsupportedNodes.md) for more info and suggested workaround.

## Building RDA Dynamo
Please refer to the [following steps](https://github.com/tothom/RDADynamo-example-implementation/blob/main/docs/Develop.md) to build and develop RDADynamo further.
