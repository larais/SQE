param(
	[String]$AntlrJar="./antlr-4.7-complete.jar",
	[String]$Grammar="SQE.g4",
	[String]$Language="CSharp",
	[String]$Package="SQE",
	[String]$Output="../SQE.CSharp/Generated"
)
Write-Output "Start of build.ps1"

if (-Not (Test-Path($AntlrJar)))
{
	Write-Output "Antlr runtime is missing. Jar File is being downloaded."
	Invoke-WebRequest http://www.antlr.org/download/$AntlrJar -OutFile $AntlrJar
}

try
{
	Write-Output "Building Antlr files and generating $Language classes."
	java -jar $AntlrJar -Dlanguage="$Language" $Grammar -visitor -no-listener -package $Package -o $Output
}
catch {
	Write-Error "Could not generate the $Language files, please verify that you have java installed on your machine."
}
Write-Output "End of build.ps1"