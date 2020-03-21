if "%APPVEYOR_REPO_TAG%" == "true" (

echo Publishing...

nuget source Add -Name "GitHub" -Source "https://nuget.pkg.github.com/JohnnyCrazy/index.json" -UserName JohnnyCrazy -Password %GH_TOKEN%

cd ./SpotifyAPI.Web
dotnet pack -c Release SpotifyAPI.Web.csproj -p:PackageVersion=%APPVEYOR_REPO_TAG_NAME%
nuget push ./bin/Release/SpotifyAPI.Web.%APPVEYOR_REPO_TAG_NAME%.nupkg -ApiKey %NUGET_TOKEN% -NonInteractive -Source https://www.nuget.org/api/v2/package
nuget push ./bin/Release/SpotifyAPI.Web.%APPVEYOR_REPO_TAG_NAME%.nupkg -NonInteractive -Source GitHub

cd ../SpotifyAPI.Web.Auth
dotnet pack -c Release SpotifyAPI.Web.Auth.csproj -p:PackageVersion=%APPVEYOR_REPO_TAG_NAME%
nuget push ./bin/Release/SpotifyAPI.Web.Auth.%APPVEYOR_REPO_TAG_NAME%.nupkg -ApiKey %NUGET_TOKEN% -NonInteractive -Source https://www.nuget.org/api/v2/package
nuget push ./bin/Release/SpotifyAPI.Web.Auth.%APPVEYOR_REPO_TAG_NAME%.nupkg -NonInteractive -Source GitHub

cd ..

) else (
  echo Skipping Publishing
)
