# Check for empty and duplicate inputs in Specs
Param ([string]$rootFolder = (get-item $(Get-Location).Path).parent.FullName)

$SpecsFile = Join-Path -Path $rootFolder -ChildPath "Specs"
$filePattern = "*.json"

$global:totalEmpty = 0
$global:totalDuplicate = 0
$global:emptyFileList = New-Object System.Collections.Generic.List[string]
$global:duplicateFileList = New-Object System.Collections.Hashtable

function SpecInfo()
{
    foreach ($file in $input) 
    {   
        $parentName = $file.FullName | Split-Path -parent | Split-Path -leaf
        $typeFolder = $file.FullName | Split-Path -parent | Split-Path -parent | Split-Path -leaf
		$ret = Get-Content $file.FullName -encoding utf8 | ConvertFrom-Json
        CheckSpec -spec $ret -type $typeFolder -parent $parentName -name $file.Name
    }

    Write-Host("Total empty input:`t" + $global:totalEmpty)
    Write-Host("Total duplicate input:`t" + $global:totalDuplicate)

    if ($global:totalEmpty)
    {
        Write-Host("Contain empty input `"`" in these files")
        foreach ($case in $global:emptyFileList)
        {
            Write-Host($case)
        }
    }

    if ($global:totalDuplicate)
    {
        Write-Host("Contain duplicate input in these files")
        foreach ($key in $global:duplicateFileList.keys)
        {
            Write-Host("$($key)")
            foreach ($case in $global:duplicateFileList[$key])
            {
                Write-Host($case)
            }
        }
    }

    if ($global:totalEmpty -OR $global:totalDuplicate)
    {
        if ($global:totalEmpty) { exit 1 }
        elseif ($global:totalDuplicate) { exit 2}
        else { exit 3} 
    }
}

function CheckSpec($spec, $type, $parent, $name)
{
    $emptyNum = 0
    $duplicateNum = 0
    $inputDict = New-Object System.Collections.Hashtable
    $duplicateList = New-Object System.Collections.Generic.List[string]
    $isDateTime = $false
    $fileName = "$($type)/$($parent)/$($name)"

    if ($type -eq "DateTime")
    {
        $isDateTime = $true
    }

    foreach ($line in $spec)
    {
        if ($line.Input -eq "")
        {
            $emptyNum += 1
        }
        else
        {
            if ($inputDict.ContainsKey($line.Input))
            {
                if ($isDateTime -And $line.Context.ReferenceDateTime) 
                {   
                    $timeDict = $inputDict.($line.Input)
                    if ($line.Context.ReferenceDateTime -in $timeDict)
                    {
                        $duplicateList.Add("Input: $($line.Input)`tReferenceDateTime: $($line.Context.ReferenceDateTime)")
                        $duplicateNum += 1
                    }
                    else
                    {
                        $timeDict.Add($line.Context.ReferenceDateTime)
                    }
                }
                else
                {
                    $duplicateList.Add("Input: $($line.Input)")
                    $duplicateNum += 1
                }
            }
            else
            {
                # If it's a DateTime spec and contains "referencedatetime", we need to check the pair "input" and "referencedatetime"
                if ($isDateTime -And $line.Context.ReferenceDateTime)
                {
                    $referenceDict = New-Object System.Collections.Generic.List[string]
                    $referenceDict.Add($line.Context.ReferenceDateTime)
                    $inputDict.Add($line.Input, $referenceDict)
                }
                else
                {
                    $inputDict.Add($line.Input, $line.Input)
                }
            }
        }
    }
    
    if ($emptyNum)
    {
        $global:emptyFileList.Add($fileName)
        $global:totalEmpty += $emptyNum
    }

    if ($duplicateNum)
    {
        $global:duplicateFileList.Add($fileName, $duplicateList)
        $global:totalDuplicate += $duplicateNum
    }

}

Get-Childitem -Path $SpecsFile -recurse |? {$_.Name -like $filePattern} | SpecInfo;