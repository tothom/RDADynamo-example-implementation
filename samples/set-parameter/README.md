Dynamo graph which writes a combined mark parameter value to all doors in the model. The graph gets all doors, finds out which room they belong. The room number is combined with the door mark and written to a parameter in the door. The revit model is saved as a new file with name `result.rvt`, which needs to be uploaded after the the Design Automation has completed.

The name of the combined mark parameter can be changed like this in the `input.json` file:
```json
[
  {
    "GraphName": "Mark Doors.dyn",
    "InputFolder": "input.zip",
    "ResultFolder": "result",
    "InputFileNames": [
    ],
    "Packages": [
    ],
    "NodeInput": [
      {
        "Id": "0c302edfd35c494d823990676f5f4aa0",
        "Name": "Value",
        "Value": "CombinedMark"
      },
    ]
  }
]
```