<!-- ABOUT THE PROJECT -->
## About The Project

[![Product Name Screen Shot][product-screenshot]](https://example.com)

Here's a blank template to get started. To avoid retyping too much info, do a search and replace with your text editor for the following: `github_username`, `repo_name`, `twitter_handle`, `linkedin_username`, `email_client`, `email`, `project_title`, `project_description`, `project_license`

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Built With


<a href="https://learn.microsoft.com/en-us/dotnet/core/install/windows">
  <img width="100" height="100" alt="Asp.Net" src="https://github.com/user-attachments/assets/1161ed10-b863-4e42-a2d1-c3f758e118d3" />
<a/>

[![React][React.js]][React-url]

# Getting Started

This is an example of how you may give instructions on setting up your project locally.
To get a local copy up and running follow these simple example steps.

## Prerequisites

The easiest way of running this app is by building and running both the api and react app with docker.
That way there are less dependencies, but you still need Docker.

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
* Node/NPM
  ```sh
  winget install -e --id OpenJS.NodeJS
  ```

## Installation

### Docker 
1. Open up a terminal in main folder
2. Build React app docker image
   ```sh
   docker build -f squares.react/Dockerfile -t squares-react .   
   ```
3. Build Api docker image
   ```sh
   docker build -f Squares.Api/Dockerfile -t squares-api .
   ```
4. Run React app docker container
   ```js
   docker run -p 3000:3000 squares-react
   ```
5. Run Api docker container
   ```sh
   docker run -p 7280:7280 --name squares squares-api  
   ```
6. Open browser [http://localhost:3000)](http://localhost:3000)

### Non Docker
1. Open up a terminal in main folder
2. Build Api 
   ```sh
   dotnet build   
   ```
3. Run Api
   ```sh
   dotnet run
   ```
4. Go to folder /squares.react
5. Build React app
   ```sh
   npm install
   ```
6. Run React app
   ```sh
   npm run dev
   ```
7. Open browser [http://localhost:3000)](http://localhost:3000)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[Asp.net]: https://www.pngmart.com/files/23/Net-Logo-PNG-Image.png
[Asp.net-url]: https://dotnet.microsoft.com/en-us/apps/aspnet
