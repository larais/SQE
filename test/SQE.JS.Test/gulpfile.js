var gulp = require('gulp');

gulp.task("copy-antlr", function (done) {
    gulp.src([
        "node_modules/antlr4/**/*"
    ]).pipe(gulp.dest("wwwroot/antlr4"));
    done();
});


gulp.task('release', gulp.series(["copy-antlr"]));


gulp.task("publish-docs", function (done) {
    gulp.src("wwwroot/**/*").pipe(gulp.dest("../../docs"));
    done();
});