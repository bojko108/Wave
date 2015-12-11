powershell.exe -nologo -noprofile -command "& { If (Test-Path '.\Sample Data\Geodatabases'){ Remove-Item '.\Sample Data\Geodatabases' -Force -Recurse}; Add-Type -A 'System.IO.Compression.FileSystem'; [IO.Compression.ZipFile]::ExtractToDirectory('.\Sample Data\Geodatabase_931SP1.zip', '.\Sample Data'); }"
powershell.exe -nologo -noprofile -command "& { If (Test-Path '.\Sample Data\Databases'){ Remove-Item '.\Sample Data\Databases' -Force -Recurse}; Add-Type -A 'System.IO.Compression.FileSystem'; [IO.Compression.ZipFile]::ExtractToDirectory('.\Sample Data\Database_931SP1.zip', '.\Sample Data'); }"