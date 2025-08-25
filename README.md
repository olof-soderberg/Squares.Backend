<!-- ABOUT THE PROJECT -->
## About The Project

This is the Api for the application Squares.
<br>
Squares is a project driven by passion for the creation of potentially an infinite amount of squares, each with a different color from the last!
Functionality includes, but might not be limited to, adding and clearing all squares.

<img width="381" height="356" alt="image" src="https://github.com/user-attachments/assets/d1f5c9ac-e368-4e7e-a55a-47bfd088157b" />




### Built With

<a href="https://learn.microsoft.com/en-us/dotnet/core/install/windows">
  <img width="100" height="100" alt="Asp.Net" src="https://github.com/user-attachments/assets/1161ed10-b863-4e42-a2d1-c3f758e118d3" />
<a/>

# Getting Started

Download the code and follow installation steps below. 
<br>
**Obs**. This guide is for Windows only at the moment.

## Prerequisites

The easiest way of running this app is by building and running both the api and react app with docker.
That way there are less dependencies, but you still need Docker in some form to build the images and run the containers.


### Docker
* Docker 
  ```sh
  winget install -e --id Docker.DockerDesktop
  ```
### Non Docker
* Asp.Net
  ```sh
  winget install -e --id Microsoft.DotNet.AspNetCore.9
  ```

## Installation

### Docker 
1. Open up a terminal in main folder
2. Build Api docker image
   ```sh
   docker build -f Squares.Api/Dockerfile -t squares-api .
   ```
3. Run Api docker container
   ```sh
   docker run -p 7280:7280 --name squares squares-api  
   ```
### Non Docker
1. Open up a terminal in main folder
2. Build Api 
   ```sh
   dotnet build   
   ```
3. Run Api
   ```sh
   dotnet run --project .\Squares.Api\Squares.Api.csproj --urls http://localhost:7280
   ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Considerations
### These are some, but not all, considerations in general and about what could have been added or changed 

- There is an endpoint that allows streaming the data from the json file, thus minimizing memory allocation. As of now, this has not been implemented in the frontend.
- There could have been CI/CD pipeline setup with GitHub Actions, to run the tests, etc. May be added in the future.
- The two repos could have been merged for a unified and clearer installation. I like to keep them seperated though.


[Asp.net]: https://www.pngmart.com/files/23/Net-Logo-PNG-Image.png
[Asp.net-url]: https://dotnet.microsoft.com/en-us/apps/aspnet
