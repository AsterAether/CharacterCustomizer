@echo on

IF NOT EXIST "%1%2\build\" (
    mkdir "%1%2\build\"
)

IF NOT "%RoR2Location%" == "" (
    copy /Y "%1%2\bin\netstandard2.0\%3.dll" "%RoR2Location%BepInEx\plugins\%3.dll";
)

copy /Y "%1%2\bin\netstandard2.0\%3.dll" "%1%2\build\%3.dll";

copy /Y "%1%2\manifest.json" "%1%2\build\manifest.json";

copy /Y "%1%2\icon.png" "%1%2\build\icon.png";

copy /Y "%1\README.md" "%1%2\build\README.md";

copy /Y "%1\LICENSE.txt" "%1%2\build\LICENSE.txt";

IF NOT "%SevZipLocation%" == "" (
    "%SevZipLocation%7z.exe" a "%1%2\build\build.zip" "%1%2\build\*" -x!build.zip
)

