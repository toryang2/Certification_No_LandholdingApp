# Paths to projects
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$solutionDir = Split-Path $scriptDir -Parent   # Assuming the layout you showed

# Paths to relevant files
$manifestPath = Join-Path $solutionDir "AssessorCertification.Package\Package.appxmanifest"
$assemblyInfoPath = Join-Path $scriptDir "Properties\AssemblyInfo.cs"

# Check existence
if (-Not (Test-Path $manifestPath)) {
    Write-Host "❌ Package.appxmanifest not found in AssessorCertification.Package"
    exit 1
}

if (-Not (Test-Path $assemblyInfoPath)) {
    Write-Host "❌ AssemblyInfo.cs not found in NoLandholdingApp\Properties"
    exit 1
}

# Read and parse Package.appxmanifest
[xml]$manifest = Get-Content $manifestPath -Encoding UTF8

# Extract version from manifest
$version = $manifest.Package.Identity.Version
if ([string]::IsNullOrWhiteSpace($version)) {
    Write-Host "⚠️ No version found in Package.appxmanifest. Defaulting to 0.0.0.0"
    $version = "0.0.0.0"
} else {
    Write-Host "✅ Found version: $version"
}

# Read AssemblyInfo.cs
$content = Get-Content $assemblyInfoPath -Raw

# Update AssemblyVersion and AssemblyFileVersion
$content = $content -replace 'AssemblyVersion\(".*?"\)', "AssemblyVersion(`"$version`")"
$content = $content -replace 'AssemblyFileVersion\(".*?"\)', "AssemblyFileVersion(`"$version`")"

# Save back to AssemblyInfo.cs
Set-Content $assemblyInfoPath -Value $content -Encoding UTF8

Write-Host "✅ AssemblyInfo.cs updated to version $version for NoLandholdingApp"
