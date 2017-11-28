$Component = (Get-Item *.nuspec).Name
Write-Output "Component is ${Component}"

$Version = (select-string -Path '*.nuspec' -Pattern '<Version>\b[0-9.]+\b</Version>' -AllMatches | % { $_.Matches } | % { $_.Value }).ToLower().Replace("<version>", "").Replace("</version>", "")
Write-Output "Version is ${Version}"
