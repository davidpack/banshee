AC_DEFUN([BANSHEE_CHECK_EXTENSION_MINIMODE],
[
    AC_ARG_ENABLE(minimode, AC_HELP_STRING([--enable-minimode], [Build MiniMode]),, enable_minimode="yes")
    AM_CONDITIONAL(ENABLE_MINIMODE, test "x$enable_minimode" = "xyes")
])
