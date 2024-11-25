## Usage

### Build solution
- `dotnet build`

### Generate file
- To check options use: `dotnet run -- generate --help`
- To generate file with random lines `dotnet run -- generate`

### Sort file
- To check options use: `dotnet run -- sort --help`
- To generate file with random lines `dotnet run -- sort -i <your-file>`

##  Observations
Due to huge amount of I/O operations async approach seems to be slower.
Very likely due to tasks based context switching. I have decided to keep async implementation
in case the application would be used by multiple users, and non-blocking async approach is necessary.

##  Test results

I could not achieve expected results 1GB/1min for files with line length lower than 2000 characters.
Tested on SSD: ~30 seconds (line length ~2000 characters)
Tested on HDD: ~1 minute (line length ~2000 characters)