# LogDLL

## *Install Instructions*
1. Click [**Clone or Download**](https://github.com/dtavana/LogDLL/archive/master.zip/) on the Repository Page
2. Extract the downloaded ZIP and open it
3. Navigate to your copy the **LogDLL.dll** file from the extracted folder to your **@Dayz_Epoch_Server** folder in your server root
4. Use the following format to log.
`"LogDLL" callExtension "FOLDERNAME/FULLPATH\~FILENAME\~LOGENTRY";`

Example 1:
`"LogDLL" callExtension "Logs~Coins~This is a test";`

This will create the follow file layout where your arma2oaserver.exe is located.
> PATHTOSERVEREXE\Logs\Coins\Coins_CURRENTDATE.log

Example 2:
`"LogDLL" callExtension "C:\Epoch_Server\Logs~Coins~This is a test";`

This will create the follow file layout.
> C:\Epoch_Server\Logs\Coins\Coins_CURRENTDATE.log

NOTE: The full path including all sub directories will always be created if they do not exist

If you would like more detailed error handling, simply set the result of the call to the DLL to a variable and log this result:
```
result = "LogDLL" callExtension "Logs~Coins~This is a test";
diag_log result;
```
This will log the output of the command to your Server RPT.

If you have any questions, please feel free to message me on Discord: TwiSt#7777.



