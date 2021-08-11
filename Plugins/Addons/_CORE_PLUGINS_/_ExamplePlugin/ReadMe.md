# EXAMPLE SYSTEM PLUGIN

The Example Plugin serves as a template to copy and create your own plugins.

[STEP1] Copy the ExamplePlugin folder and rename it to match your plugin
[STEP2] Firstly, rename all of the scripts to match your plugin starting with the _interface folder then Messages > MessageHandlers > Config > HelperMethods loosely in that order.
[STEP3] Rename the PluginCore (file name and class name)
[STEP4] Move through each code file in your new plugin folder and perform renames where appropriate.
[STEP5] Generally you want to create a Transaction between the client and server - What the flow will look like across the network (WAN) is: 

*initialization*
Client and Server both register Message Handlers to NetworkClient and NetworkServer respectively. OnServerResponseXXX or OnClientRequestXXX are executed when the appropriate message is received to handle the message.

*network flow*
* Client sends ClientExampleRequest
* Server receives ClientExampleRequest and processes it with OnClientExampleRequest.
* OnClientExampleRequest creates a new ServerExampleResponse and sends it back to the client with whatever data the client requested (or with no data in some cases). Every message will have a success bool that tells the client if the server was able to fulfill the request.
* Client receives ServerExampleResponse and handles it in OnServerExampleResponse.
* In our example a simple "Hello World!" popup will appear when the Client receives the Server's Response to it's Request.