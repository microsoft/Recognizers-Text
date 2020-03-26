try
{
	$ttFile = $args[0]

	$result = $true

	$ttExtension = ".tt"
	$codeExtension = ".cs"
	$patternsExtension = ".yaml"

	$inputFile = Split-Path $ttFile -Leaf
	$codeFile = $ttfile.Replace($ttExtension, $codeExtension)

	$isBase = $inputFile.Contains("Base")

	if (-Not $isBase)
	{
		$language = Split-Path $ttFile -Parent
		$language = Split-Path $language -Leaf
		$type = $inputFile.Replace("Definitions.tt", "")
	}
	else
	{
		$language = "Base"
		$type = $inputFile.Replace("Base", "").Replace(".tt", "")
	}

	#Write-Host $language $type

	$codeSubPath = ".NET\Microsoft.Recognizers.Definitions.Common"
	$patternSubPath = "Patterns"

	$rootPath = $ttFile.Substring(0, $ttFile.IndexOf($codeSubPath))

	if (-not $isBase)
	{
		$patternFile = [IO.Path]::Combine($rootPath, $patternSubPath, $language, $language + "-" + $type + $patternsExtension)
	}
	else
	{
		$patternFile = [IO.Path]::Combine($rootPath, $patternSubPath, $language + "-" + $type + $patternsExtension)
	}

	# If code file is older than patterns file (time difference less than 0), we need to re-gen
	$val = [datetime](Get-ItemProperty -Path $codeFile -Name LastWriteTime).lastwritetime -[datetime](Get-ItemProperty -Path $patternFile -Name LastWriteTime).lastwritetime
	$result = $val -lt 0

	#Write-Host
	#Write-Host "TT:" $ttFile
	#Write-Host "YAML:" $patternFile
	#Write-Host $result

	if ($result) 
	{
		#Write-Host "Re-gen for:" $patternFile
		Write-Host 1
		exit 1
	}
	else
	{
		Write-Host 0
		exit 0
	}
}
catch
{
        #Make sure any error causes a re-gen to happen
		Write-Host 1
		exit 1
}