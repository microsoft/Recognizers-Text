# Check for empty and duplicate inputs in Specs
Param ([string]$rootFolder = (get-item $(Get-Location).Path).parent.FullName)

$SpecsFile = Join-Path -Path $rootFolder -ChildPath "Specs"
$filePattern = "*.json"

$global:totalEmpty = 0
$global:totalDuplicate = 0
$global:emptyFileList = New-Object System.Collections.Generic.List[string]
$global:duplicateFileDict = New-Object System.Collections.Hashtable

function SpecInfo()
{
    foreach ($file in $input) 
    {   
        $parentName = $file.FullName | Split-Path -parent | Split-Path -leaf
        $typeFolder = $file.FullName | Split-Path -parent | Split-Path -parent | Split-Path -leaf
        $contents = Get-Content $file.FullName -encoding utf8 | ConvertFrom-Json
        CheckSpec -spec $contents -type $typeFolder -parent $parentName -name $file.Name
    }

    Write-Host("Total empty input:`t" + $global:totalEmpty)
    Write-Host("Total duplicate input:`t" + $global:totalDuplicate)

    if ($global:totalEmpty)
    {
        Write-Host("# Contain empty input in these files")
        foreach ($case in $global:emptyFileList)
        {
            Write-Host($case)
        }
    }

    if ($global:totalDuplicate)
    {
        Write-Host("# Contain duplicate input in these files")
        foreach ($file in $global:duplicateFileDict.keys)
        {
            Write-Host("$($file)")
            foreach ($cases in $global:duplicateFileDict[$file])
            {
                foreach ($case in $cases.keys)
                {
                    Write-Host("Duplicate $($cases[$case]) times: $($case)")
                }
            }
        }
    }

    if ($global:totalEmpty -OR $global:totalDuplicate)
    {
        exit 2
    }
}

function CheckSpec($spec, $type, $parent, $name)
{
    $emptyNum = 0
    $duplicateNum = 0
    $inputDict = New-Object System.Collections.Hashtable
    $duplicateDict = New-Object System.Collections.Hashtable
    $isDateTime = $false
    $fileName = "$($type)/$($parent)/$($name)"

    if ($type -eq "DateTime")
    {
        $isDateTime = $true
    }

    foreach ($testCase in $spec)
    {
        if (!$testCase.Input -OR ($testCase.Input -eq ""))
        {
            $emptyNum += 1
        }
        else
        {
            if (!$inputDict.ContainsKey($testCase.Input))
            {
                # If it's a DateTime spec and contains "referencedatetime", we need to check the pair "input" and "referencedatetime"
                if ($isDateTime -And $testCase.Context.ReferenceDateTime)
                {
                    $referenceTimeList = New-Object System.Collections.Generic.List[string]
                    $referenceTimeList.Add($testCase.Context.ReferenceDateTime)
                    $inputDict.Add($testCase.Input, $referenceTimeList)
                }
                else
                {
                    $inputDict.Add($testCase.Input, $testCase.Input)
                }
            }
            else
            {
                if ($isDateTime -And $testCase.Context.ReferenceDateTime)
                {   
                    $referenceTimeList = $inputDict[$testCase.Input]
                    if ($testCase.Context.ReferenceDateTime -in $referenceTimeList)
                    {
                        $duplicateCase = "Input: $($testCase.Input)`tReferenceDateTime: $($testCase.Context.ReferenceDateTime)"
                        if ($duplicateDict.ContainsKey($duplicateCase))
                        {
                            $duplicateDict[$duplicateCase] += 1
                        }
                        else
                        {
                            $duplicateDict.Add($duplicateCase, 2)
                            $duplicateNum += 1
                        }
                    }
                    else
                    {
                        $referenceTimeList.Add($testCase.Context.ReferenceDateTime)
                    }
                    
                }
                else
                {
                    $duplicateCase = "Input: $($testCase.Input)"
                    if ($duplicateDict.ContainsKey($duplicateCase))
                    {
                        $duplicateDict[$duplicateCase] += 1
                    }
                    else
                    {
                        $duplicateDict.Add($duplicateCase, 2)
                        $duplicateNum += 1
                    }
                    
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
        $global:duplicateFileDict.Add($fileName, $duplicateDict)
        $global:totalDuplicate += $duplicateNum
    }

}

Get-Childitem -Path $SpecsFile -recurse |? {$_.Name -like $filePattern} | SpecInfo;