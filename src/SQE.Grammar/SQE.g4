grammar SQE;

@parser::header {#pragma warning disable 3021}
@lexer::header {#pragma warning disable 3021}

expression : mainExpr EOF;

mainExpr          : '(' mainExpr ')'                    #parenthesisExp
                    | mainExpr AND mainExpr             #andExp
                    | mainExpr OR mainExpr              #orExp
                    | PROPERTY OPERATOR NUMBER          #compareNumberExp
                    | PROPERTY OPERATOR ESCAPEDSTRING   #compareStringExp
                    ;

AND                 : 'and' ;
OR                  : 'or' ;
OPERATOR            : (EQUALS|NOTEQUALS|GREATER|LESS);

NUMBER              : (DIGIT)+ ;
PROPERTY         	: LETTER (LETTER | DIGIT)* ;

fragment LETTER     : [a-zA-Z] ;
fragment DIGIT      : [0-9] ;
fragment LETTERNUM  : [a-zA-Z0-9] ;

fragment EQUALS              : '=' ;
fragment NOTEQUALS           : '!=' ;
fragment GREATER             : '>' ;
fragment LESS                : '<' ;

ESCAPEDSTRING       : '"' ('\\"'|.)*? '"'
                    | '\'' ('\\\''|.)*? '\''
                    ;

WHITESPACE          : ' ' -> skip;
NEWLINE             : '\r'? '\n' -> skip;