grammar SQE;
expression : mainExpr EOF;

mainExpr          : '(' mainExpr ')'                    #parenthesisExp
                    | mainExpr AND mainExpr             #andExp
                    | mainExpr OR mainExpr              #orExp
                    | PROPERTY OPERATOR NUMBER          #compareNumberExp
                    | PROPERTY OPERATOR ESCAPEDSTRING   #compareStringExp
                    ;

fragment LETTER     : [a-zA-Z] ;
fragment DIGIT      : [0-9] ;

AND                 : 'and' ;
OR                  : 'or' ;

fragment EQUALS              : '=' ;
fragment NOTEQUALS           : '!=' ;
fragment GREATER             : '>' ;
fragment LESS                : '<' ;

PROPERTY         	: LETTER+ ;
OPERATOR            : (EQUALS|NOTEQUALS|GREATER|LESS);

ESCAPEDSTRING       : '"' ('\\"'|.)*? '"'
                    | '\'' ('\\\''|.)*? '\''
                    ;

NUMBER              : DIGIT+ ('.' DIGIT+)? ;

WHITESPACE          : ' ' -> skip;
NEWLINE             : '\r'? '\n' -> skip;