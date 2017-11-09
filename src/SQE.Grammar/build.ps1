$AntlrJar = "./antlr-4.7-complete.jar"
if (-Not (Test-Path($AntlrJar)))
{
	Invoke-WebRequest http://www.antlr.org/download/$AntlrJar -OutFile $AntlrJar
}

java -jar $AntlrJar -Dlanguage=CSharp SQE.g4 -visitor -no-listener -package SQE