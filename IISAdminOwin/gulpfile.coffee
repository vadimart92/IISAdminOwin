gulp = require 'gulp'
less = require 'gulp-less'
coffee = require 'gulp-coffee'
gutil = require('gulp-util');
jshint = require('gulp-jshint');
path = require('path');

doSomethingOnThisFile = (target)-> 
	gulp.src target
    .pipe coffee bare: on 
    .on 'error', gutil.log
    .pipe do jshint
    .pipe gulp.dest  path.join 'resources/app/components', path.relative 'components', path.dirname target

gulp.task 'coffee', -> 
	gulp.src 'components/**/*.coffee'
		.on 'data', (file)->
            doSomethingOnThisFile(file.path);

gulp.task 'less', -> 
	gulp.src 'components/**/*.less'
		.pipe do less
		.pipe gulp.dest 'resources/app/components'

gulp.task 'watch', ->
	gulp.watch 'components/**/*.less', ['less']
	gulp.watch 'components/**/*.coffee', ['coffee']

gulp.task 'default', ['coffee', 'less' ,'watch']