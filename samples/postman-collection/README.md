# Postman collection

Import the postman collection and global variables into a new postman workspace.

Before starting, the `client_id` and `client_secret` 
global variables as to be set to match the Forge application you want to use.

To let the postman collection know which Revit file and which input.zip to use. Copy the **complete URL** of the file you want to use and paste it into the `D4DABim360Upload` collection variable. Do the same with your input.zip file on BIM360, and paste it to the `inputFileBim360Url` collection variable. 

The postman tasks are organized sequentially, so you can run them one by one, starting from the top. 'Get an Access Token' → 'Get Revit Model Storage Id' → etc... After you run 'Create WorkItem' you need to run 'Check Status of a  WorkItem' until the status is 'Succeeded'.