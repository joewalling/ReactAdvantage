@echo off
dotnet build server\ReactAdvantage.Api
dotnet build server\ReactAdvantage.IdentityServer
start dotnet run --project server\ReactAdvantage.Api
start dotnet run --project server\ReactAdvantage.IdentityServer
echo waiting for projects to start before opening the browser
timeout 10

start https://localhost:44398
rem //no need to open identity server directly
rem start https://localhost:44338