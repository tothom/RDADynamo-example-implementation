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
## Get the Bim360/ACC file storage id for use in Design Automation

<aside>
☝ The `ProjectId` must be prefixed with `b.` That is b with a dot afterwards
Example:

`5ce30d0c-002d-42bb-8c00-661c0689ec41`

becomes

`b.5ce30d0c-002d-42bb-8c00-661c0689ec41`

</aside>

1. Get item **URN**
    1. The URN is just the `id` when querying the items in a folder
    2. You can also get it directly from the URL in BIM360 Docs. For example, it’s the last part of the URL when viewing a model
        
        ```html
        https://docs.b360.autodesk.com/projects/5ce30d0c-002d-42bb-8c00-661c0689ec41/folders/urn:adsk.wipprod:fs.folder:co.I_OxfDFsTFazBTR9Gz7mzQ/detail/viewer/items/***urn:adsk.wipprod:dm.lineage:Gxrge9LfQXS0LFrW2sq8_Q***
        ```
        
        `urn:adsk.wipprod:dm.lineage:Gxrge9LfQXS0LFrW2sq8_Q`
        
2. Get the **Storage Id**
    1. This Id is located in the response body of a query to
        1. https://developer.api.autodesk.com/data/v1/projects/`projectID`/items/`itemUrn`/tip
        2. *Tip* means the latest version
    2. The Storage Id is located here in the response body:
        1. `responseBody.data.relationships.storage.data.id`
