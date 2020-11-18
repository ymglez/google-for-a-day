## Google for a day
===================
# Overview
Simple search engine to rank web pages based on the number of occurrences of certain words.

# Build run && usage
Requeriments:
    - dotnet 2.2.300 or higher

- For test environment:
    ```sh
    $ cd src/backend/GoogleForADay.Services.Api/
    $ dotnet build
    $ dotnet run GoogleForADay.Services.Api.csproj  --urls=http://localhost:5000/
    ```
	
- Usage:
    - Open prefered REST client
     - SET headers ```{ "ApiKey": "5f1as6df51asd5f16ad5f16a5df" }```
    - search API: `GET` http://localhost:5000/api/engine/search/wordtofind
    - index API: `POST` http://localhost:5000/api/engine/index
       Body
        ```json
        {
        	"Url": "https://github.com/ymglez/google-for-a-day",
        	"Depth": 2
        }
        ```
    - clear API: https://localhost:44341/api/engine/clear
        Body
         ```json
        {}
        ```
	
