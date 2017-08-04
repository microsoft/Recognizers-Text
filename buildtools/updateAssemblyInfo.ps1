Param ([string]$rootFolder = $(get-location).Path, 
       [string]$filePattern
)

Write-Host ("Root folder: " + $rootFolder)
Write-Host ("File pattern: " + $filePattern)

$replace = $true;

function UpdateAssemblyInfo()
{
    foreach ($file in $input) 
    {
        Write-Host ($file.FullName)
		
		if($replace)
		{
			$tmpFile = $file.FullName + ".tmp"
			$fileContent = Get-Content $file.FullName -encoding utf8
			
			$fileContent = TryRemove InternalsVisibleTo;
			$fileContent = AddCodeSign ;

			Set-Content $tmpFile -value $fileContent -encoding utf8
		
			Move-Item $tmpFile $file.FullName -force
		}
		
    }
}

function AddCodeSign()
{
	$fileContent = $fileContent + "`r`n[assembly: AssemblyKeyFileAttribute(@`"..\\..\\buildtools\\35MSSharedLib1024.snk`")] `r`n [assembly: AssemblyDelaySignAttribute(true)]"
	return $fileContent
}

function TryRemove($attributeName)
{
	$fileContent = $fileContent -replace ('\[assembly:\s*'+$attributeName +'\(".*"\)\s*\]'), ""
	return $fileContent
}

Write-Host ("Updating files...")
Get-Childitem -Path $rootFolder -recurse |? {$_.Name -like $filePattern} | UpdateAssemblyInfo; 