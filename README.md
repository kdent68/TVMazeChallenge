# TVMazeChallenge
## Project Title
TVMazeChallenge
## Technology/Framework Used
C#, NUnit
## Features
This project will help automate the testing of TV Maze endpoints for shows and episodes.
## Installation
No installation needed. Make sure you have Visual Studio installed and available.
## API Reference
[TVMaze] (https://www.tvmaze.com/api)
## How to use
Open solution. Make sure Nuget Packages are restored/installed. Make sure Test Explorer window is visible (to make visible go to View menu at top of window, click Test Explorer). Then click run all test.
## Issues
Nunit Runner gave me issues installing, would've been able to run from command line.  RestSharp gave me deserialization issues, so scraped that.
Tried a new logger, configuration for export isn't corrcet though.

## Notes
First time focusing on API tetsing in VS, usually done in Postman. Our VS test projects are hooked to Jenkins and that's where we use plug ins to print reports etc.
So I hope I was on the right track with what I was attempting to do lol. 
