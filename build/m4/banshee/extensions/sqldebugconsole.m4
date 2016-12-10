AC_DEFUN([BANSHEE_CHECK_EXTENSION_SQLDEBUGCONSOLE],
[
    AC_ARG_ENABLE(sqldebugconsole, AC_HELP_STRING([--enable-sqldebugconsole], [Build SqlDebugConsole]),, enable_sqldebugconsole="yes")
    AM_CONDITIONAL(ENABLE_SQLDEBUGCONSOLE, test "x$enable_sqldebugconsole" = "xyes")
])
