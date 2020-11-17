#Google for a day

## Overview

Simple search engine to rank web pages based on the number of occurrences of certain words.

## Build and run 
	# Requeriments
	 - dotnet 2.2.300 or higher
	
	# backend
	$ cd src/backend/
	$ dotnet publish --configuration Release
	$ cd GoogleForADay.Services.Api/bin/Release/netcoreapp2.2/publish/
	$ dotnet GoogleForADay.Services.Api.dll
	
	#usage
	- Open prefered REST client
	- perform http GET to
		http://localhost:5000/api/engine/search/wordtofind
	

