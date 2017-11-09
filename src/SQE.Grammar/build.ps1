param(
	[String]$AntlrJar="./antlr-4.7-complete.jar",
	[String]$Grammar="SQE.g4",
	[String]$Package="SQE",
	[String]$Output="../SQE.CSharp/Generated"
)

if (-Not (Test-Path($AntlrJar)))
{
	Invoke-WebRequest http://www.antlr.org/download/$AntlrJar -OutFile $AntlrJar
}

try
{
	java -jar $AntlrJar -Dlanguage=CSharp $Grammar -visitor -no-listener -package $Package -o $Output
}
catch {
	Write-Error "Could not generate the C# files, please verify that you have java installed on your machine."
}
