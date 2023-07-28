### Prerequisites

To run Dynamo graphs in the cloud, it is necessary to have a basic understanding on how Autodesk Design Automation works. Please check out the [documentation for Design Automation](https://aps.autodesk.com/en/docs/design-automation/v3/developers_guide/overview/) for get an overview. If you are new to this we recommend starting with the [step-by-step tutorials](https://aps.autodesk.com/en/docs/design-automation/v3/tutorials/revit/about_this_tutorial/).


### Making your owl Revit Design Automation Dynamo Implementation

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
- 
