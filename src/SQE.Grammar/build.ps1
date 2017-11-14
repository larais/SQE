param(
	[String]$AntlrJar="./antlr-4.7-complete.jar",
	[String]$Grammar="SQE.g4",
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
	Write-Output "Building Antlr files and generating C# classes."
	java -jar $AntlrJar -Dlanguage=CSharp $Grammar -visitor -no-listener -package $Package -o $Output
}
catch {
	Write-Error "Could not generate the C# files, please verify that you have java installed on your machine."
}
Write-Output "End of build.ps1"