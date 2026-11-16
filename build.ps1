param([Parameter(Mandatory = $true)][string]$GamePath)
$ErrorActionPreference = 'Stop'
$dotnet = (Get-Command dotnet).Source
$dotnetRoot = Split-Path $dotnet
$sdk = Get-ChildItem (Join-Path $dotnetRoot 'sdk') -Directory |
    Sort-Object { [version]$_.Name } -Descending | Select-Object -First 1
$runtime = Get-ChildItem (Join-Path $dotnetRoot 'shared\Microsoft.NETCore.App') -Directory |
    Where-Object { $_.Name -like '10.*' } |
    Sort-Object { [version]$_.Name } -Descending | Select-Object -First 1
if (!$sdk -or !$runtime) { throw 'A .NET SDK and .NET 10 runtime are required.' }
$compiler = Join-Path $sdk.FullName 'Roslyn\bincore\csc.dll'
$source = Join-Path $PSScriptRoot 'source'
$output = Join-Path $source 'HidePieces.dll'
$references = @(Get-ChildItem $runtime.FullName -Filter 'System*.dll' | Where-Object { $_.Name -notlike '*.Native.dll' } | ForEach-Object FullName)
$references += Join-Path $runtime.FullName 'netstandard.dll'
$references += Join-Path $GamePath 'VintagestoryAPI.dll'
$references += Join-Path $GamePath 'Mods\VSSurvivalMod.dll'
$references += Join-Path $GamePath 'Lib\Newtonsoft.Json.dll'
$arguments = @('/nologo', '/noconfig', '/nostdlib+', '/target:library', '/langversion:latest',
    '/optimize+', '/deterministic+', "/out:$output")
$arguments += $references | ForEach-Object { "/reference:$_" }
$arguments += Join-Path $PSScriptRoot 'code\HidePieces.cs'
& $dotnet $compiler @arguments
if ($LASTEXITCODE -ne 0) { throw 'Compilation failed.' }

$version = (Get-Content (Join-Path $source 'modinfo.json') -Raw | ConvertFrom-Json).version
$dist = Join-Path $PSScriptRoot 'dist'
New-Item -ItemType Directory -Path $dist -Force | Out-Null
$zip = Join-Path $dist "hidepieces_$version.zip"
if (Test-Path -LiteralPath $zip) { throw "Archive already exists: $zip" }
Add-Type -AssemblyName System.IO.Compression.FileSystem
[IO.Compression.ZipFile]::CreateFromDirectory($source, $zip)
$archive = [IO.Compression.ZipFile]::Open($zip, [IO.Compression.ZipArchiveMode]::Update)
try {
    [IO.Compression.ZipFileExtensions]::CreateEntryFromFile($archive,
        (Join-Path $PSScriptRoot 'LICENSE'), 'LICENSE') | Out-Null
} finally {
    $archive.Dispose()
}
$hash = (Get-FileHash -LiteralPath $zip -Algorithm SHA256).Hash.ToLowerInvariant()
[IO.File]::WriteAllText((Join-Path $dist "hidepieces_$version.sha256"),
    "$hash  hidepieces_$version.zip`n", [Text.UTF8Encoding]::new($false))
Write-Output "Built $zip"
