$files = Get-ChildItem -Path Src -Filter *.cs -Recurse
foreach ($file in $files) {
    $content = Get-Content $file.FullName
    $srcPath = (Get-Item -Path Src).FullName
    $relativePath = $file.FullName.Substring($srcPath.Length + 1)
    $folderPath = Split-Path -Path $relativePath
    if ($folderPath -eq "") {
        $newNamespace = "U_VoluntApp_Backend.Src"
    } else {
        $newNamespace = "U_VoluntApp_Backend.Src." + $folderPath.Replace("\", ".")
    }
    
    foreach ($line in $content) {
        if ($line -match "\b(class|interface|enum|struct|record)\s+([a-zA-Z0-9_]+)") {
            $typeName = $Matches[2]
            Write-Output "$typeName|$newNamespace|$($file.FullName)"
        }
    }
}
