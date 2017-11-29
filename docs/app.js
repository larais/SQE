var antlr4 = require('antlr4/index');
var SQELexer = require('SQELexer');
var SQEParser = require('SQEParser');

var errors = [];
var lastInput = "";

var ErrorListener = function (errors) {
    antlr4.error.ErrorListener.call(this);
    this.errors = errors;
    return this;
};

ErrorListener.prototype = Object.create(antlr4.error.ErrorListener.prototype);
ErrorListener.prototype.constructor = ErrorListener;
ErrorListener.prototype.syntaxError = function (rec, sym, line, col, msg, e) {
    console.log("syntaxError");
    console.log("msg" + msg);
    this.errors.push(msg);
};

function checkInput() {
    var input = $("#expressionInput").val();
    if (input === lastInput) {
        return;
    }

    lastInput = input;

    console.log("Input: " + input);

    $("#expressionInput").parent().removeClass("error");
    errors = [];

    $("#errorOutput").hide();
    $("#output").hide();

    var listener = new ErrorListener(errors);
    
    var chars = new antlr4.InputStream(input);
    var lexer = new SQELexer.SQELexer(chars);

    lexer.removeErrorListeners();
    lexer.addErrorListener(listener);


    var tokens = new antlr4.CommonTokenStream(lexer);
    var parser = new SQEParser.SQEParser(tokens);

    parser.removeErrorListeners();
    parser.addErrorListener(listener);

    var tree = parser.expression();
    console.log("Parsed: " + tree.toStringTree());

    $("#output > p").text(tree.toStringTree());

    $("#output").show();

    if (errors.length > 0) {
        $("#expressionInput").parent().addClass("error");
        $("#errorOutput").html(errors[0]);
        for (var i = 1; i < errors.length; i++) {
            $("#errorOutput").html($("#errorOutput").html() + "<br>" + errors[i]);
        }
        $("#errorOutput").show();
    }
}