$files = Get-ChildItem -Path Src -Filter *.cs -Recurse
$typeMap = @{}
foreach ($file in $files) {
    $content = Get-Content $file.FullName
    foreach ($line in $content) {
        if ($line -match "\b(class|interface|enum|struct|record)\s+([a-zA-Z0-9_]+)") {
            $typeName = $Matches[2]
            if (-not $typeMap.ContainsKey($typeName)) {
                $typeMap[$typeName] = New-Object System.Collections.Generic.List[string]
            }
            $typeMap[$typeName].Add($file.FullName)
        }
    }
}

foreach ($typeName in $typeMap.Keys) {
    if ($typeMap[$typeName].Count -gt 1) {
        Write-Output "Duplicate Type: $typeName"
        foreach ($path in $typeMap[$typeName]) {
            Write-Output "  $path"
        }
    }
}
