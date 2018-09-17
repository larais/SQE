﻿var gulp = require('gulp');

gulp.task("copy-antlr", function () {
    gulp.src([
        "node_modules/antlr4/**/*"
    ])
        .pipe(gulp.dest("wwwroot/antlr4"));
});

gulp.task('release', gulp.series(["copy-antlr"], function () {
    gulp.src([
        "node_modules/jquery/dist/jquery.min.js",
        "semantic/dist/semantic.min.css",
        "semantic/dist/semantic.min.js"
    ])
        .pipe(gulp.dest("wwwroot/lib"));
}));


gulp.task("publish-docs", function () {
    gulp.src("wwwroot/**/*")
        .pipe(gulp.dest("../../docs"));
});