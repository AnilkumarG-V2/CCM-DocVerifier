Before running the solution just modify the configuration in the V2.DocVerifier.API project
File V2.DocVerifier.API --> appsettings.json
Change the value of the following configuration
------------ APIKey : Change the value to the Gemini API key generated for your Gemini Account
------------ ImagePath : Physical path where the API can store the images generated from PDF pages for the extraction purpose.


In case if V2.DocVerifier.API is running on any other hostname or port number then do modify the configuration file for V2.DocVerifier.Web
File V2.DocVerifier.Web --> appsettings.json
Change the value of the following configuration
------------ DocVerifierBaseURL : Hostname or URL for the V2.DocVerifier.API
