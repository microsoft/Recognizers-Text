Param ([string]$rootFolder = (get-item $(Get-Location).Path).parent.FullName)

$SpecsFile = Join-Path -Path $rootFolder -ChildPath "Specs"
$filePattern = "*.json"
# Write-Host ("Root folder: " + $rootFolder)
# Write-Host ("Specs location folder: " + $SpecsFile)

$global:totalEmpty = 0
$global:totalDuplicate = 0

function SpecInfo()
{
    foreach ($file in $input) 
    {   
        $parentFolder = Split-Path -parent $file.FullName
        $parentName = Split-Path -leaf $parentFolder
        Write-Host ($parentName + "`t" + $file.Name)
		$ret = Get-Content $file.FullName -encoding utf8 | ConvertFrom-Json
        CheckSpec -spec $ret
    }
}

function CheckSpec($spec)
{
    $emptyNum = 0
    $duplicateNum = 0
    $casesDict= New-Object System.Collections.Hashtable

    foreach ($line in $spec.Input)
    {
        if ($line -eq "")
        {
            $emptyNum += 1
        }
        else
        {
            if($casesDict.ContainsKey($line))
            {
                write-host "Duplicate case: $($line)"
                $duplicateNum += 1
            }
            else
            {
                $casesDict.Add($line,$line)
            }
        }
    }
    
    Write-Host ("Empty number:" + $emptyNum + "`tDuplicate number:" + $duplicateNum)
    $global:totalEmpty = $global:totalEmpty + $emptyNum
    $global:totalDuplicate = $global:totalDuplicate + $duplicateNum
}

Get-Childitem -Path $SpecsFile -recurse |? {$_.Name -like $filePattern} | SpecInfo; 
Write-Host ("Total empty input:`t" + $global:totalEmpty)
Write-Host ("Total duplicate input:`t" + $global:totalDuplicate)
