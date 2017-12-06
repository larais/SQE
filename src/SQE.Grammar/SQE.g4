grammar SQE;

expression : mainExpr EOF;

mainExpr          : '(' mainExpr ')'                    #parenthesisExp
                    | mainExpr AND mainExpr             #andExp
                    | mainExpr OR mainExpr              #orExp
                    | PROPERTY OPERATOR NUMBER          #compareNumberExp
                    | PROPERTY OPERATOR ESCAPEDSTRING   #compareStringExp
                    ;

AND                 : A N D ;
OR                  : O R ;
OPERATOR            : (EQUALS|NOTEQUALS|GREATER|LESS);

fragment A          : 'a' | 'A' ;
fragment D			: 'd' | 'D' ;
fragment N			: 'n' | 'N' ;
fragment O			: 'o' | 'O' ;
fragment R			: 'r' | 'R' ;

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