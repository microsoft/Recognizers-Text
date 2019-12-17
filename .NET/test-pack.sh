#!/bin/bash

nugetExe=$1
version=$2
echo "Version: $version"

targetDir="./test-pack"

config="release;basic=$version;number=$version;numberWithUnit=$version"

$nugetExe pack ./Microsoft.Recognizers.Text/Microsoft.Recognizers.Text.nuspec -NonInteractive -OutputDirectory $targetDir -Properties Configuration=$config -Symbols -version "$version" -Verbosity Detailed
$nugetExe pack ./Microsoft.Recognizers.Text.Choice/Microsoft.Recognizers.Text.Choice.nuspec -NonInteractive -OutputDirectory $targetDir -Properties Configuration=$config -Symbols -version "$version" -Verbosity Detailed
$nugetExe pack ./Microsoft.Recognizers.Text.Sequence/Microsoft.Recognizers.Text.Sequence.nuspec -NonInteractive -OutputDirectory $targetDir -Properties Configuration=$config -Symbols -version "$version" -Verbosity Detailed
$nugetExe pack ./Microsoft.Recognizers.Text.Number/Microsoft.Recognizers.Text.Number.nuspec -NonInteractive -OutputDirectory $targetDir -Properties Configuration=$config -Symbols -version "$version" -Verbosity Detailed
$nugetExe pack ./Microsoft.Recognizers.Text.NumberWithUnit/Microsoft.Recognizers.Text.NumberWithUnit.nuspec -NonInteractive -OutputDirectory $targetDir -Properties Configuration=$config -Symbols -version "$version" -Verbosity Detailed
$nugetExe pack ./Microsoft.Recognizers.Text.DateTime/Microsoft.Recognizers.Text.DateTime.nuspec -NonInteractive -OutputDirectory $targetDir -Properties Configuration=$config -Symbols -version "$version" -Verbosity Detailed
$nugetExe pack ./Microsoft.Recognizers.Text.DataTypes.TimexExpression/Microsoft.Recognizers.Text.DataTypes.TimexExpression.nuspec -NonInteractive -OutputDirectory $targetDir -Properties Configuration=$config -Symbols -version "$version" -Verbosity Detailed

