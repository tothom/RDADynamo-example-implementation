# RDADynamo-example-implementation

## Installation
1. Clone this git repository
2. Restore the nuget packages
3A. When available, get the latest version of the dll _RDADHelper.dll_ and copy it to the `./dist/` folder
3B. Alternatively, use the existing file in the `./dist/` folder
4. Now all the references should be ok
5. The next step includes setting up an Autodesk Platform Service (APS) application, creating a AppBundle and a related activity. Please have a look below on directions on how to do this.

## How to run Dynamo graphs in Autodesk Design Automation

### Prerequisites

Do run Dynamo graphs in the cloud it is necessary to have a basic understanding on how Autodesk Design Automation works. Please check out the [documentation for Design Automation](https://aps.autodesk.com/en/docs/design-automation/v3/developers_guide/overview/) to get an overview. If you are new to this we recommend starting with the [step-by-step tutorials](https://aps.autodesk.com/en/docs/design-automation/v3/tutorials/revit/about_this_tutorial/).

The example apps also contains example setup for an Design Automation [activity](https://aps.autodesk.com/en/docs/design-automation/v3/reference/http/activities-POST/) to give you a head start.

### Setup your DB app to work with Dynamo

To run Dynamo graphs on your Revit model in Design Automation you need to reference *RDADHelper.dll* in your DB app. RDADHelper stands for *Revit Design Automation Dynamo Helper* and works as a intermediate layer between your DB app and Dynamo. RDADHelper works similarly to Autodesk DesignAutomationBridge. 

The steps are as follows:
1. Subscribe to the `RDADHelper.Actions.OnGraphResultReady` event with your own event handler.
2. Prepare to run your Dynamo graph by making a new `RunGraphArgs` object.
3. 

### Dynamo nodes which do not work in the cloud

When running an app in the cloud there’s no possibility of user interaction. Therefore Dynamo nodes with user interface will work when running Dynamo in Design Automation.

These nodes will definitely not work in the cloud:
- Select Model Element By Category
- Select Model Elements By Category

After trying to run a graph, the `OnGraphResultReady` action will return an object of type `GraphResultArgs`. In that object there is a list called `MissingNodes`, which contains all of the nodes which could not be found. This list will also contain nodes which will not work when running in the cloud, but also nodes from custom packages which are missing. Use this list to troubleshoot your graph nodes.



Other nodes with user interface could fail as well.

### Using custom packages

Custom packages needs to be uploaded with your AppBundle to work. 

Alternatively you can also pass custom packages through the WorkItem.

### How to set Dynamo node values from DB app

1. To set a value of an input node, you need to get this information aobut the node:
    
    | Guid | The unique identifier of the node |
    | --- | --- |
    | Name | The name of the property that you would like to change (usually Value) |
    | Value | The new value you want the node to have |
	
2. This information is located in the .dyn file of your Dynamo graph.
3. Open the graph in a text editor like Notepad in Windows.