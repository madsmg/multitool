$NewSolutionName = (Get-Item * | Select Parent).Parent.Name
Write-Host "Gathered new solution name from parent folder: $NewSolutionName."

If (Test-Path ".git")
{
    Write-Host "Remove .git path"
    Remove-Item -Recurse -Force ".git"
}

If (Test-Path ".vs")
{
    Write-Host "Remove .vs path"
    Remove-Item -Recurse -Force ".vs"
}

If (Test-Path "src\MultiTool\bin")
{
    Write-Host "Remove path src\MultiTool\bin"
    Remove-Item -Recurse -Force "src\MultiTool\bin"
}

If (Test-Path "src\MultiTool\obj")
{
    Write-Host "Remove path src\MultiTool\obj"
    Remove-Item -Recurse -Force "src\MultiTool\obj"
}

Rename-Item -NewName "$NewSolutionName.sln" -Path MultiTool.sln -Force
Rename-Item -NewName "$NewSolutionName.csproj" -Path src\MultiTool\MultiTool.csproj -Force
Rename-Item -NewName "$NewSolutionName" -Path src\MultiTool -Force

 Write-Host "Replace MultiTool in files"
Get-ChildItem * -recurse  | Where-Object {$_.Attributes -ne "Directory"} | ForEach-Object {  (Get-Content $_.FullName) -replace "MultiTool","$NewSolutionName" | Set-Content -path $_.FullName }
