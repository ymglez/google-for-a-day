#Google for a day

# Overview

Simple search engine to rank web pages based on the number of occurrences of certain words.

## Build run && usage
	Requeriments:
	- dotnet 2.2.300 or higher

	- For test environment:
	```sh
		$ cd src/backend/GoogleForADay.Services.Api/
		$ dotnet build
		$ cd /bin/Debug/netcoreapp2.2
		$ dotnet GoogleForADay.Services.Api.dll  --urls=http://localhost:5000/
	```
	
	- Usage:
		- Open prefered REST client
		- serach API: GET http://localhost:5000/api/engine/search/wordtofind
		- index API: POST http://localhost:5000/api/engine/index
						{
							"Url": "https://github.com/ymglez/google-for-a-day",
							"Depth": 2
						}
	

