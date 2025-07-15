Interact with the file system through crud operations. 

it is needed to run the server in development mode since that is how it works. which means you should have installed asp.net core on your machine to run the dotnet command.
otherwise, to run the app in production, it should be modified the path of the folder in where the app search the files and that's it.

to use the app it is recommended to use another tool like postman to make the http requests.

"POST /files/{filename}" : creates a new file
"GET /files/{filename}" : reads specific file
"PUT /files/{filename}" : updates a file
"DELETE /files/{filename}" : deletes a file
"GET /files" : list all files
